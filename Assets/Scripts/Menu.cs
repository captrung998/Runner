using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using JetBrains.Annotations;
using Unity.VisualScripting;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject[] characters;
    public int characterIndex;
    public int characterIndex1 = 1;
    private static Menu instance;
    private AudioSource audioSourceClick;
    [SerializeField] private AudioClip click;

    public static Menu Instance { get => instance; }
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        audioSourceClick = GetComponent<AudioSource>();
        audioSourceClick.clip = click;
    }



    private void Update()
    {
        characterIndex1 = characterIndex;

    }
    public void ChangeCharacter(int index)
    {
        characterIndex += index;
        if (characterIndex >= characters.Length)
        {
            characterIndex = 0;

        }
        else if (characterIndex < 0)
        {
            characterIndex = characters.Length - 1;
        }

        for (int i = 0; i < characters.Length; i++)
        {
            if (i == characterIndex)
            {
                characters[i].SetActive(true);
            }
            else
                characters[i].SetActive(false);
        }
        audioSourceClick.Play();
    }
    public void StartRun()
    {
        PlayerPrefs.SetInt("characterIndex", characterIndex);
        PlayerPrefs.Save();
        GameManager.Instance.StartRun(characterIndex);
        audioSourceClick.Play();

    }
    public void StartAgain()
    {
        GameManager.Instance.StartRun(characterIndex);
        audioSourceClick.Play();
    }

    public void EndRun()
    {
        GameManager.Instance.EndRun();
        audioSourceClick.Play();
    }
}
