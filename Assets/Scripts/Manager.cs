﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    [SerializeField] GameObject Boat;
    [SerializeField] Levels[] Levels;
    Levels Level;
    [SerializeField] Text _Text;
    [SerializeField] Text LevelNameText;
    [SerializeField] Sprite LongSprite;
    [SerializeField] Sprite LongSpriteGlow;
    [SerializeField] Sprite MainBoatSprite;
    [SerializeField] GameObject icon;
    [SerializeField] private Transform Flipper;
    public int LevelName;
    public static int _LevelName;
    public static int off_LEvelName; 
    public static int minimaleZetten;
    public int Score;
    public int _Difficulty =20;

    private void Start()
    {
        string Difficulty = GlobalGameSettings.GetSetting("Difficulty");
        switch (Difficulty)
        {
            case "Beginner":
                _Difficulty = 0;
                break;
            case "Easy":
                _Difficulty = 8;
                break;
            case "Medium":
                _Difficulty = 16;
                break;
            case "Hard":
                _Difficulty = 24;
                break;
            case "Expert":
                _Difficulty = 32;
                break;
        }
        int Level;
        int.TryParse(GlobalGameSettings.GetSetting("Level"), out Level);
        foreach (GameObject AllLevels in GameObject.FindGameObjectsWithTag("level"))
        {
            AllLevels.GetComponent<Menu>().levelName = AllLevels.GetComponent<Menu>().level + _Difficulty;//+1;
            int LevelAmount = AllLevels.GetComponent<Menu>().levelName = AllLevels.GetComponent<Menu>().level + _Difficulty;
            AllLevels.GetComponentInChildren<Text>().text = "Level " + LevelAmount;
            //AllLevels.GetComponent<Image>().sprite = unUsingSprite;
            //if (AllLevels.GetComponent<Menu>()._Difficulty == Controller.LevelName)
            //{
            //    AllLevels.GetComponent<Image>().sprite = AllLevels.GetComponent<Menu>().UsingSprite;
            //}
            StartCoroutine(AllLevels.GetComponent<Menu>().Working());
        }
        foreach (GameObject AllLevels in GameObject.FindGameObjectsWithTag("niveau"))
        {
            //AllLevels.GetComponent<Image>().sprite = unUsingSprite;
            StartCoroutine(AllLevels.GetComponent<Menu>().Working());
        }
        foreach (GameObject AllGrids in GameObject.FindGameObjectsWithTag("point"))
        {
            AllGrids.GetComponent<Pointbehavoir>().Image.enabled = false;
            AllGrids.GetComponent<Pointbehavoir>().CanInteract = false;
        }
        if (off_LEvelName == 0)
        {
            StartLevel((Level-1 )+ _Difficulty);
            LevelName = ((Level) + _Difficulty);
            LevelNameText.text = Difficulty +":" + Level;
            minimaleZetten = Levels[LevelName - 1].minimaleZetten;

        }
        else
        {
            int _lev = Level;
            StartLevel(_LevelName );
            LevelName = (_LevelName);
            int _switch =0;
            _lev = (LevelName + 1);
            Debug.Log(_lev);
            while(_lev >0)
            {
                _lev = _lev- 8;
                _switch++;
            }
            switch (_switch)
            {
                case 1:
                    Difficulty = "Beginner";
                    break;
                case 2:
                    Difficulty = "Easy";
                    break;
                case 3:
                    Difficulty = "Medium";
                    break;
                case 4:
                    Difficulty = "Hard";
                    break;
                case 5:
                    Difficulty = "Expert";
                    break;
            }
            Debug.Log(_lev);
            _lev = _lev+8;
            LevelNameText.text = Difficulty + ":" + _lev;
            minimaleZetten = Levels[LevelName].minimaleZetten;


        }
    }

    // Use this for initialization
    public void StartLevel(int LevelName)
    {
        Level = Levels[LevelName];
        _Text.text = ": 0";
        _Text.GetComponentInChildren<SpriteRenderer>().enabled = true;
        icon.SetActive(true);
        Score = 0;
        for (int i = 0; i < Level.Boat.Length; i++)
        {
            Spawn(Level.Boat[i].Pos, Quaternion.Euler(Level.Boat[i].Rotation), Level.Boat[i].Horizontal, Level.Boat[i].longBoat, Level.Boat[i].MainBoat);
        }

        Flipper.localScale = new Vector3(-1, 1, 1);
    }
    public void Spawn(Vector2 place, Quaternion rotation, bool Horizontal, bool LongBoat, bool MainBoat)
    {
        if (Horizontal == true)
        {
            place = new Vector2(place.x - 2, place.y - 3.5f);
        }
        else
        {
            place = new Vector2(place.x - 2.5f, place.y - 3);
        }

        GameObject newboat = Instantiate(Boat, place, rotation, Flipper);

        //newboat.transform.SetParent(Boat.transform.parent);
        newboat.transform.tag = "boat";
        newboat.transform.localScale = new Vector3(1, 1, 1);

        if (LongBoat == true)
        {
            GameObject BoatImg = newboat.transform.GetChild(0).gameObject;
            BoatImg.GetComponent<Image>().sprite = LongSprite;
            newboat.GetComponent<Image>().sprite = LongSpriteGlow;
            newboat.GetComponent<BoxCollider2D>().size = new Vector2(119, 359);
            newboat.GetComponent<RectTransform>().sizeDelta = new Vector2(170, 370);
            newboat.transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector2(0,180.1f);
            newboat.transform.GetChild(2).GetComponent<RectTransform>().localPosition = new Vector2(0, -180.1f);
            BoatImg.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 320);

            BoatBehavoir boat = newboat.GetComponent<BoatBehavoir>();
            boat.TotalDistance = 1;
            boat.SelectBoatType(BoatType.Big);
        }
        else if (MainBoat == true)
        {
            GameObject BoatImg = newboat.transform.GetChild(0).gameObject;
            BoatImg.GetComponent<Image>().sprite = MainBoatSprite;

            BoatBehavoir boat = newboat.GetComponent<BoatBehavoir>();
            boat.SelectBoatType(BoatType.Player);
        }
        else
        {
            BoatBehavoir boat = newboat.GetComponent<BoatBehavoir>();
            boat.SelectBoatType(BoatType.Small);
        }
    }
}
