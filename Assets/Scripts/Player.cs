using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody rb;
    private Animator anim;
    private BoxCollider boxCollider;
    public GameObject model;
    [SerializeField] private AudioClip coinAdd;
    [SerializeField] private AudioClip ouch;
    private AudioSource audioSource;

    private Vector3 verticalTargetPosition;
    private Vector3 boxColliderSize;
    private Vector2 startingTouch;

    [HideInInspector] public float score;
    [HideInInspector] public int coins = 0;

    private int currentLane = 1;
    private bool jumping = false;
    private bool sliding = false;
    private bool isSwiping = false;
    private bool invincible = false;
    private float jumpStart;
    private float slideStart;
    private int currentLife;
    private bool canMove;

    public float speed;
    public float laneSpeed;
    public float jumpLength;
    public float slideLength;
    [SerializeField] private float jumpHeight;
    public int maxLife = 3;
    public float minSpeed = 10f;
    public float maxSpeed = 30f;
    public float invicibleTime;

    static int blinkingValue;
    void Start()
    {
        canMove = false;
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        
        boxCollider = GetComponent<BoxCollider>();
        boxColliderSize = boxCollider.size;
        currentLife = maxLife;
        blinkingValue = Shader.PropertyToID("_blinkingValue");
        Invoke("StartRun", 0);
    }

    private void StartRun()
    {
        anim.Play("runStart");
        speed = minSpeed;
        canMove = true;
    }
    void Update()
    {
        if (!canMove)
            return;
        score += Time.deltaTime * speed;
        UIManager.Instance.UpdateScore((int)score);
        if (Input.GetKeyDown(KeyCode.D))
        {
            ChangeLane(1);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            ChangeLane(-1);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            Jump();
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Slide();
        }

        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                startingTouch = touch.position;
                isSwiping = true;
            }
            else if (touch.phase == TouchPhase.Ended && isSwiping)
            {
                Vector2 diff = touch.position - startingTouch;
                diff = new Vector2(diff.x / Screen.width, diff.y / Screen.width);

                if (diff.magnitude > 0.1f) // Check for significant swipe
                {
                    if (Mathf.Abs(diff.y) > Mathf.Abs(diff.x))
                    {
                        if (diff.y > 0)
                        {
                            Jump();
                        }
                        else
                        {
                            Slide();
                        }
                    }
                    else
                    {
                        if (diff.x > 0)
                        {
                            ChangeLane(1);
                        }
                        else
                        {
                            ChangeLane(-1);
                        }
                    }
                }

                isSwiping = false;
            }
        }

        if (jumping)
        {
            float ratio = (transform.position.z - jumpStart) / jumpLength;

            if (ratio >= 1f)
            {

                jumping = false;
                anim.SetBool("Jumping", false);
                boxCollider.size = boxColliderSize;
            }
            else
            {
                verticalTargetPosition.y = Mathf.Sin(ratio * Mathf.PI) * jumpHeight;

            }
        }
        else
        {
            verticalTargetPosition.y = Mathf.MoveTowards(verticalTargetPosition.y, 0, 5 * Time.deltaTime);
        }

        if (sliding)
        {
            float ratio = (transform.position.z - slideStart) / slideLength;
            if (ratio >= 1f)
            {
                sliding = false;
                anim.SetBool("Sliding", false);
                boxCollider.size = boxColliderSize;
            }
        }

        Vector3 targetPosition = new Vector3(verticalTargetPosition.x, verticalTargetPosition.y, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, laneSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        rb.velocity = Vector3.forward * speed;
    }
    void ChangeLane(int direction)
    {
        int targetLane = currentLane + direction;
        if (targetLane < 0 || targetLane > 2) return;

        currentLane = targetLane;
        verticalTargetPosition = new Vector3((currentLane - 1), 0, transform.position.z);
    }

    void Jump()
    {
        if (!jumping)
        {
            jumpStart = transform.position.z;
            anim.SetFloat("JumpSpeed", speed / jumpLength);
            Vector3 newSize = boxCollider.center;
            newSize.y = newSize.y + 1;
            boxCollider.size = newSize;
            anim.SetBool("Jumping", true);
            jumping = true;

        }
    }

    void Slide()
    {
        if (!jumping && !sliding)
        {
            slideStart = transform.position.z;
            anim.SetFloat("JumpSpeed", speed / slideLength);
            anim.SetBool("Sliding", true);
            Vector3 newSize = boxCollider.size;
            newSize.y = newSize.y - 0.5f;
            boxCollider.size = newSize;
            sliding = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            coins++;
            audioSource.clip = coinAdd;
            audioSource.Play();
            UIManager.Instance.UpdateCoin(coins);
            other.gameObject.SetActive(false);
        }
        if (invincible)
            return;

        if (other.CompareTag("Obstacle"))
        {
            currentLife--;
            UIManager.Instance.UpdateLives(currentLife);
            canMove = false;
            Invoke("CanMove", 0.6f);
            anim.SetTrigger("Hit");

            speed = 0;
            if (currentLife <= 0)
            {
                canMove = false;
                speed = 0;
                anim.SetBool("Dead", true);
                UIManager.Instance.GameOver();
            }
            else
            {
                StartCoroutine(Blinking(invicibleTime));
                audioSource.clip = ouch;
                audioSource.Play();
                Invoke("CanMove", 0.6f);
            }
        }
    }
    IEnumerator Blinking(float time)
    {
        invincible = true;
        float timer = 0;
        float lastBlinking = 0;
        float blinkPeriod = 0.6f;
        bool enabled = false;
        yield return new WaitForSeconds(1f);
        speed = minSpeed;

        while (timer < time && invincible)
        {
            model.SetActive(enabled);
            timer += Time.deltaTime;
            lastBlinking += Time.deltaTime;

            if (lastBlinking >= blinkPeriod)
            {
                lastBlinking = 0;
                enabled = !enabled;
            }
        }
        model.SetActive(true);
        invincible = false;
    }


    public void IncreaseSpeed()
    {
        speed *= 1.15f;
        if (speed > maxSpeed)
            speed = maxSpeed;
    }

    private void CanMove()
    {
        canMove = true;
    }


}
