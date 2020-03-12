using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Pointbehavoir : MonoBehaviour, I_SmartwallInteractable
{
    void I_SmartwallInteractable.Hit(Vector3 hitPosition)
    {
        MoveBoat();
    }
    [SerializeField] GameObject _Button;
    [SerializeField] public Image Image;
    [SerializeField] public Text _text;
    [SerializeField] Manager controller;
    public bool CanInteract = false;
    GameObject boat;
    bool overlapping;
    bool frontOrBack;
    public bool finishPoint = false;
    [SerializeField] ParticleSystem Particles;
    public void Activate(GameObject _Boat, bool _frontOrBack)
    {
        if (overlapping == false)
        {
            boat = _Boat;
            Image.enabled = true;
            CanInteract = true;
            frontOrBack = _frontOrBack;
        }
    }
    public void MoveBoat()
    {
        if (CanInteract == true)
        {
            boat.GetComponent<Image>().enabled = false;
            GetComponent<AudioSource>().Play();
            StartCoroutine(BoatMoving(boat.GetComponent<BoatBehavoir>().TotalDistance));
            foreach (GameObject AllGrids in GameObject.FindGameObjectsWithTag("point"))
            {
                if (AllGrids.GetComponent<Pointbehavoir>().CanInteract == true)
                {
                    AllGrids.GetComponent<Pointbehavoir>().Image.enabled = false;
                    AllGrids.GetComponent<Pointbehavoir>().CanInteract = false;
                }
            }
            Image.enabled = false;
            CanInteract = false;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        overlapping = true;
    }

    void OnTriggerExit(Collider other)
    {
        overlapping = false;
    }
    IEnumerator BoatMoving(float TotalDistance)
    {
        foreach (GameObject AllBoats in GameObject.FindGameObjectsWithTag("boat"))
        {
            AllBoats.GetComponent<BoatBehavoir>().CanUse = false;
        }
        Vector2 pointtopoint = boat.transform.position;
        controller.Score++;
        _text.GetComponentInChildren<SpriteRenderer>().enabled = true;
        _text.text = ": " + controller.Score;
        while (Vector2.Distance(gameObject.transform.position, pointtopoint) >= TotalDistance && boat !=null)
        {
            if (frontOrBack == true)
            {
                boat.transform.position += boat.transform.TransformDirection(Vector2.up / 30);
            }
            else
            {
                boat.transform.position -= boat.transform.TransformDirection(Vector2.up / 30);
            }
            pointtopoint = boat.transform.position;
            yield return null;
        }
        if (boat != null)
        {
            boat.GetComponent<BoatBehavoir>().CanUse = true;
        }
        foreach (GameObject Allboeats in GameObject.FindGameObjectsWithTag("boat"))
        {
            Allboeats.GetComponent<BoatBehavoir>().CanUse = true;
        }

        if (finishPoint == true)
        {
            Particles.Emit(100);
            _Button.transform.position = new Vector3(0, 0, 0);
            string HighScore = GlobalGameSettings.GetSetting("Reset_High_Score");
            if(PlayerPrefs.GetInt(controller.LevelName.ToString()) == 0)
            {
                PlayerPrefs.SetInt(controller.LevelName.ToString(), controller.Score);
            }
            if (controller.Score< PlayerPrefs.GetInt(controller.LevelName.ToString()))
            {
                PlayerPrefs.SetInt(controller.LevelName.ToString(), controller.Score);
            }
            if (HighScore == "Ja")
            {
                PlayerPrefs.DeleteKey(controller.LevelName.ToString());
                _Button.GetComponentInChildren<TextMesh>().text = "Beste score: " + controller.Score.ToString();
            }
            else
            {
                _Button.GetComponentInChildren<TextMesh>().text = "Beste score: " + PlayerPrefs.GetInt(controller.LevelName.ToString()).ToString();
            }
            _text.text = "";
            _text.GetComponentInChildren<SpriteRenderer>().enabled = false;
            controller.GetComponent<AudioSource>().Play();
            foreach (GameObject Allboeats in GameObject.FindGameObjectsWithTag("boat"))
            {
                Allboeats.GetComponent<BoatBehavoir>().CanUse = false;
            }
            for(int i =0; i<300; i++)
            {
                if (boat != null)
                {
                    boat.transform.position += boat.transform.TransformDirection(Vector2.up / 30);
                }
                else
                {
                    break;
                }
                yield return null;
            }
            Invoke("ToScores", 2.5f);
        }
    }
    private void ToScores()
    {
        ScoreScreenController.MoveToScores(new List<int>(new int[1] { controller.Score,}), controller.LevelName);
    }
}
