using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour, I_SmartwallInteractable
{
    void I_SmartwallInteractable.Hit(Vector3 hitPosition)
    {
        Clicking();
    }
    [SerializeField] public int levelName;
    [SerializeField] public int level;
    [SerializeField] Manager Controller;
    [SerializeField] GameObject _UI;
    [SerializeField] Sprite UsingSprite;
    [SerializeField] Sprite unUsingSprite;
    bool usable = true;
    // Use this for initialization

    public void Clicking()
    {
        if (gameObject.transform.tag == "level")
        {
            Click();
        }
        else
        {
            method();
        }
    }

    public void Click()
    {
        if (usable == true)
        {
            GetComponent<AudioSource>().Play();
            Controller.Score = 0;
            foreach (GameObject AllBoats in GameObject.FindGameObjectsWithTag("boat"))
            {
                GameObject.Destroy(AllBoats);
            }
            foreach (GameObject AllGrids in GameObject.FindGameObjectsWithTag("point"))
            {
                AllGrids.GetComponent<Pointbehavoir>().Image.enabled = false;
                AllGrids.GetComponent<Pointbehavoir>().CanInteract = false;
            }
            Controller.StartLevel(levelName);
            foreach (GameObject AllLevels in GameObject.FindGameObjectsWithTag("level"))
            {
                AllLevels.GetComponent<Image>().sprite = unUsingSprite;
                StartCoroutine(AllLevels.GetComponent<Menu>().Working());
            }
            foreach (GameObject AllLevels in GameObject.FindGameObjectsWithTag("niveau"))
            {
                StartCoroutine(AllLevels.GetComponent<Menu>().Working());
            }
            GetComponent<Image>().sprite = UsingSprite;
            Controller.LevelName = levelName;
        }
    }
    public void Niveau()
    {
        if (usable == true)
        {
            GetComponent<AudioSource>().Play();
            foreach (GameObject AllLevels in GameObject.FindGameObjectsWithTag("level"))
            {
                AllLevels.GetComponent<Menu>().levelName = AllLevels.GetComponent<Menu>().level + levelName;
                int LevelAmount = AllLevels.GetComponent<Menu>().levelName = AllLevels.GetComponent<Menu>().level + levelName + 1;
                AllLevels.GetComponentInChildren<Text>().text = "Level " + LevelAmount;
                AllLevels.GetComponent<Image>().sprite = unUsingSprite;
                if (AllLevels.GetComponent<Menu>().levelName == Controller.LevelName)
                {
                    AllLevels.GetComponent<Image>().sprite = AllLevels.GetComponent<Menu>().UsingSprite;
                }
                StartCoroutine(AllLevels.GetComponent<Menu>().Working());
            }
            foreach (GameObject AllLevels in GameObject.FindGameObjectsWithTag("niveau"))
            {
                AllLevels.GetComponent<Image>().sprite = unUsingSprite;
                StartCoroutine(AllLevels.GetComponent<Menu>().Working());
            }
            GetComponent<Image>().sprite = UsingSprite;
        }
    }
    public IEnumerator Working()
    {
        usable = false;
        yield return new WaitForSeconds(0.3f);
        usable = true;
    }
    public int _Difficulty;
    public void method()
    {
        string Difficulty = GlobalGameSettings.GetSetting("Difficulty");
        switch (Difficulty)
        {
            case "Beginner":
                _Difficulty = 0;
                break;
            case "Intermediate":
                _Difficulty = 10;
                break;
            case "Advanced":
                _Difficulty = 20;
                break;
            case "Expert":
                _Difficulty = 30;
                break;
        }
        foreach (GameObject AllLevels in GameObject.FindGameObjectsWithTag("level"))
        {
            AllLevels.GetComponent<Menu>().levelName = AllLevels.GetComponent<Menu>().level + _Difficulty;
            int LevelAmount = AllLevels.GetComponent<Menu>().levelName = AllLevels.GetComponent<Menu>().level + levelName + 1;
            AllLevels.GetComponentInChildren<Text>().text = "Level " + LevelAmount;
            AllLevels.GetComponent<Image>().sprite = unUsingSprite;
            if (AllLevels.GetComponent<Menu>()._Difficulty == Controller.LevelName)
            {
                AllLevels.GetComponent<Image>().sprite = AllLevels.GetComponent<Menu>().UsingSprite;
            }
            StartCoroutine(AllLevels.GetComponent<Menu>().Working());
        }
        foreach (GameObject AllLevels in GameObject.FindGameObjectsWithTag("niveau"))
        {
            AllLevels.GetComponent<Image>().sprite = unUsingSprite;
            StartCoroutine(AllLevels.GetComponent<Menu>().Working());
        }
    }

}
