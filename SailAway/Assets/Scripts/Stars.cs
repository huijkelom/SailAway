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
    public Sprite redboat;
    public GameObject arrow;
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
        float xaxis =1;
        float back = -5.5f;
        audi.Play();
        for (int i = 0; i< __Score;i++)
        {
            float length = back +(i * 0.5f);
            GameObject boat;
            boat = Instantiate(_boat,new Vector3(length,xaxis,0),Quaternion.identity,gameObject.transform);
            if(i>=starsMin)
            {
                boat.GetComponent<Image>().sprite = redboat;
            }
            if(i>21)
            {
                xaxis = -0;
                back = -17f;
            }
            else if(i>43)
            {
                xaxis = -1;
                back = -29f;
            }
            else if (i > 65)
            {
                xaxis = -2;
                back = -41f;
            }
            else if(i>82)
            {
                GameObject _arrow = Instantiate(arrow, new Vector3((length+1), xaxis, 0), Quaternion.identity, gameObject.transform);
                break;
            }
            yield return new WaitForSeconds(0.05f);
        }
        _Text.gameObject.SetActive(true);
        if (starsMin != __Score)
        {
            _Text.text = "Jou aantal zetten zijn " + __Score + ". De minste aantal zetten zijn " + starsMin + "!";
        }
        else
        {
            _Text.text = "Je hebt de minst aantal zetten met in totaal " + starsMin + "!";
        }
    }
}

