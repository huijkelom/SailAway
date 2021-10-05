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
    }

    IEnumerator instanceBoats()
    {
        AudioManager.Instance.Play("ScoreGrow");

        float xaxis = 1;
        float back = -5.5f;
        for (int i = 0; i< __Score;i++)
        {
            float length = back +(i * 0.5f);
            GameObject boat;
            boat = Instantiate(_boat,new Vector3(length,xaxis,0),Quaternion.identity,gameObject.transform);
            if(i>=starsMin)
            {
                boat.GetComponent<Image>().sprite = redboat;
            }
            if(i>88)
            {
                GameObject _arrow = Instantiate(arrow, new Vector3((length + 1), xaxis, 0), Quaternion.identity, gameObject.transform);
                break;
            }
            else if(i>67)
            {
                xaxis = -2;
                back = -40f;
            }
            else if (i > 44)
            {
                xaxis = -1;
                back = -28.5f;
            }
            else if(i>21)
            {
                xaxis = -0;
                back = -17f;
            }
            yield return new WaitForSeconds(0.05f);
        }
        LanguageController.LanguageChangedEvent += GetTextFromLanguageControllerAndPlaceItOnLabel;
        if (!LanguageController.LanguageLoaded.Equals(string.Empty))
        {
            GetTextFromLanguageControllerAndPlaceItOnLabel();
        }
    }

    private void GetTextFromLanguageControllerAndPlaceItOnLabel()
    {
        _Text.gameObject.SetActive(true);
        if (starsMin != __Score)
        {
            _Text.text = LanguageController.GetText(1) + __Score.ToString() + LanguageController.GetText(2) + starsMin + "!";
        }
        else
        {
            _Text.text = LanguageController.GetText(3) + starsMin + "!";
        }
    }
}

