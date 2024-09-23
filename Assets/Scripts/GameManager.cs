using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;


[Serializable]
public class PlayerData
{
    public int coins;
    public int[] max;
    public int[] progress;
    public int[] CurrentProgress;
    public int[] reward;
    public string[] missionType;
    public int[] characterCost;
}
public class GameManager : MonoBehaviour
{
    public int coins;
    private string filePath;
    public int[] characterCost;
    public int characterIndex;
    private AudioSource audioSourceMainLoop;
    [SerializeField] private AudioClip mainLoop;



    private static GameManager instance;
    public static GameManager Instance { get => instance; }
    private void Awake()
    {
        instance = this;

        filePath = Application.persistentDataPath + "/save.sav";
    }
    private void Start()
    {
        audioSourceMainLoop = GetComponent<AudioSource>();
        audioSourceMainLoop.clip = mainLoop;
        audioSourceMainLoop.Play();
    }
    public void StartRun(int charIndex)
    {
        SceneManager.LoadScene("Game");
        Time.timeScale = 1f;
    }
    public void EndRun()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1.0f;


    }
    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(filePath);

        PlayerData data = new PlayerData();

        data.coins = coins;

        data.max = new int[2];
        data.progress = new int[2];

        data.reward = new int[2];
        data.missionType = new string[2];
        data.characterCost = new int[characterCost.Length];


        for (int i = 0; i < characterCost.Length; i++)
        {
            data.characterCost[i] = characterCost[i];
        }

        bf.Serialize(file, data);
        file.Close();
    }
    void Load()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(filePath, FileMode.Open);

        PlayerData data = (PlayerData)bf.Deserialize(file);
        file.Close();

        for (int i = 0; i < data.characterCost.Length; i++)
        {
            characterCost[i] = data.characterCost[i];
        }
    }
}
