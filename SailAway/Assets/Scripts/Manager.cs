using System.Collections;
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
            case "Starter":
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
            LevelNameText.text = "Level: " + (LevelName).ToString();

        }
        else
        {
            StartLevel(_LevelName );
            LevelName = (_LevelName);
            LevelNameText.text = "Level: " + (LevelName+1).ToString();

        }
        minimaleZetten = Levels[LevelName].minimaleZetten;
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
    }
    public void Spawn(Vector2 place, Quaternion rotatio, bool Horizontal, bool LongBoat, bool MainBoat)
    {
        if (Horizontal == true)
        {
            place = new Vector2(place.x - 2, place.y - 3.5f);
        }
        else
        {
            place = new Vector2(place.x - 2.5f, place.y - 3);
        }
        GameObject newboat = Instantiate(Boat, place, rotatio);
        newboat.transform.SetParent(Boat.transform.parent);
        newboat.transform.tag = "boat";
        newboat.transform.localScale = new Vector2(1, 1);
        if (LongBoat == true)
        {
            GameObject BoatImg = newboat.transform.GetChild(0).gameObject;
            BoatImg.GetComponent<Image>().sprite = LongSprite;
            newboat.GetComponent<Image>().sprite = LongSpriteGlow;
            newboat.GetComponent<BoxCollider2D>().size = new Vector2(119, 369);
            newboat.GetComponent<RectTransform>().sizeDelta = new Vector2(170, 370);
            newboat.transform.GetChild(1).GetComponent<RectTransform>().localPosition = new Vector2(0,180f);
            newboat.transform.GetChild(2).GetComponent<RectTransform>().localPosition = new Vector2(0, -180f);
            BoatImg.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 320);
            newboat.GetComponent<BoatBehavoir>().TotalDistance = 1f;
        }
        if (MainBoat == true)
        {
            GameObject BoatImg = newboat.transform.GetChild(0).gameObject;
            BoatImg.GetComponent<Image>().sprite = MainBoatSprite;
        }
    }
}
