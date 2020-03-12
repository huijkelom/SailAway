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
    // Start is called before the first frame update
    public void AmountStars()
    {
        starsMin = Manager.minimaleZetten;
        __Score = ScoreChecker._Score;
        Debug.Log(__Score);
        if (__Score < starsMin + 1)
        {
            StartCoroutine(stars3());
        }
        else if (__Score <= starsMin + 2)
        {
            StartCoroutine(stars2());
        }
        else
        {
            StartCoroutine(stars1());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator stars3()
    {
        foreach (GameObject obj in _Stars)
        {
            yield return new WaitForSeconds(1);
            obj.SetActive(true);
        }
        StartCoroutine(_text());
    }
    IEnumerator stars2()
    {
        for (int i = 0; i < 2; i++)
        {
            yield return new WaitForSeconds(1);
            _Stars[i].SetActive(true);
        }
        StartCoroutine(_text());
    }
    IEnumerator stars1()
    {
        yield return new WaitForSeconds(1);
        _Stars[0].SetActive(true);
        StartCoroutine(_text());
    }

    IEnumerator _text()
    {
        yield return new WaitForSeconds(1);
        _Text.gameObject.SetActive(true);
        if (starsMin != __Score)
        {
            _Text.text = "Jou score is " + __Score + ". De beste score is " + starsMin + "!";
        }
        else
        {
            _Text.text = "Je hebt de beste score met " + starsMin  + " zetten!";
        }
    }
}

