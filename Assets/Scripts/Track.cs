using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Track : MonoBehaviour
{
    public GameObject[] obstacles;
    public Vector2 numberOfObstacles;
    public GameObject coin;
    public Vector2 numberOfCoins;

    public List<GameObject> newObstacles;
    public List<GameObject> newCoins;

    void Start()
    {
        AddObstacles();

        int newNumberCoins = (int)Random.Range(numberOfCoins.x, numberOfCoins.y);
        for (int i = 0; i < newNumberCoins; i++)
        {
            newCoins.Add(Instantiate(coin, transform));
            newCoins[i].SetActive(false);
        }

        PositionateObstacles();
        PositionateCoins();
    }
    private void AddObstacles()
    {
        int obstaclesSpawm = 10;
            for (int i = 0; i < obstaclesSpawm; i++)
        {
            newObstacles.Add(Instantiate(obstacles[Random.Range(0, obstacles.Length)], transform));
            newObstacles[i].SetActive(false);
        }
    }



    private void PositionateObstacles()
    {
        for (int i = 0; i < newObstacles.Count; i++)
        {
            float posZMin = (291f / newObstacles.Count) + (291f / newObstacles.Count) * i;
            float posZMax = (291f / newObstacles.Count) + (291f / newObstacles.Count) * i + 1;
            newObstacles[i].transform.localPosition = new Vector3(0, 0, Random.Range(posZMin, posZMax));
            newObstacles[i].SetActive(true);
            if (newObstacles[i].GetComponent<ChangeLane>() != null)
                newObstacles[i].GetComponent<ChangeLane>().PositionLane();
        }
    }

    private void PositionateCoins()
    {
        float minZPos = 10f;
        for (int i = 0; i < newCoins.Count; i++)
        {
            float maxZPos = minZPos + 5f;
            float randomZPos = Random.Range(minZPos, maxZPos);
            newCoins[i].transform.localPosition = new Vector3(transform.position.x, transform.position.y, randomZPos);
            newCoins[i].SetActive(true);
            newCoins[i].GetComponent<ChangeLane>().PositionLane();
            minZPos = randomZPos + 1;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().IncreaseSpeed();
            transform.position = new Vector3(0, 0, transform.position.z + 291 * 2);
            PositionateObstacles();
            PositionateCoins();
        }
    }
}


