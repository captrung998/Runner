using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject[] players;

    void Awake()
    {
        int index = PlayerPrefs.GetInt("characterIndex");
        Instantiate(players[index], transform.position, Quaternion.identity);
    }
}
