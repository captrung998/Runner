using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour
{
    [SerializeField] private GameObject[] clouds;
    private float speedClound = 3f;
    private List<GameObject> listCloud = new List<GameObject>();

    private static CloudManager instance;
    public static CloudManager Instance { get => instance; }


    private void Awake()
    {
        if (Instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        AddList();
    }
    private void Update()
    {
        Move();
    }


    private void AddList()
    {
        for (int i = 0; i < clouds.Length; i++)
        {
            GameObject newCloud = Instantiate(clouds[Random.Range(0, clouds.Length)]);
            newCloud.SetActive(true);
            listCloud.Add(newCloud);

        }
    }
    public void Move()
    {
        foreach (GameObject cloud in listCloud)
        {
            cloud.transform.Translate(Vector3.left * Random.Range(0, speedClound) * Time.deltaTime);

            if (cloud.transform.position.x <= -21f)
            {
                cloud.transform.position = new Vector3(30f, cloud.transform.position.y, cloud.transform.position.z);
            }
        }
    }
}
