using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stars : MonoBehaviour
{
    [SerializeField] GameObject[] _Stars;
    int starsMin;
    int __Score;
    [SerializeField] ScoreScreenController ScoreChecker;
    [SerializeField] Text _Text;
    public GameObject _boat;
    GameObject SpawnPlace;
    public AudioSource audi;
    // Start is called before the first frame update
    public void AmountStars()
    {
        starsMin = Manager.minimaleZetten;
        __Score = ScoreChecker._Score;
        Debug.Log(__Score);
        Debug.Log(starsMin);
        StartCoroutine(instanceBoats());
        if (__Score < starsMin + 1)
        {
        //    StartCoroutine(stars3());
        }
        else if (__Score <= starsMin + 2)
        {
        //    StartCoroutine(stars2());
        }
        else
        {
        //    StartCoroutine(stars1());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator instanceBoats()
    {
        float xaxis =0;
        audi.Play();
        for (int i = 0; i< __Score;i++)
        {
            float length = -5f+(i * 0.5f);
            GameObject boat;
            boat = Instantiate(_boat,new Vector3(length,xaxis,0),Quaternion.identity,gameObject.transform);
            if(i>=starsMin)
            {
                boat.GetComponent<Image>().color = Color.red;
            }
            if(i>24)
            {
                xaxis = -1;
            }
            else if(i>49)
            {
                xaxis = -2;
            }
            yield return new WaitForSeconds(0.05f);
        }
        _Text.gameObject.SetActive(true);
        if (starsMin != __Score)
        {
            _Text.text = "Jou score is " + __Score + ". De beste score is " + starsMin + "!";
        }
        else
        {
            _Text.text = "Je hebt de beste score met " + starsMin + " zetten!";
        }
    }
}

