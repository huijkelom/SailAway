using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stars : MonoBehaviour
{
    public GameObject _boat;
    public Sprite redboat;
    public GameObject arrow;

    [SerializeField] private Transform BoatHolder;
    [SerializeField] private ScoreScreenController ScoreChecker;
    [SerializeField] private Text _Text;

    private int MinimumMoves;
    private int Score;

    public void AmountStars()
    {
        MinimumMoves = Manager.minimaleZetten;
        Score = ScoreChecker._Score;
        Debug.Log(Score);
        Debug.Log(MinimumMoves);
        StartCoroutine(instanceBoats());
    }

    IEnumerator instanceBoats()
    {
        AudioManager.Instance.Play("ScoreGrow");

        float xaxis = 1;
        float back = -5.5f;
        for (int i = 0; i< Score;i++)
        {
            float length = back + (i * 0.5f);
            //GameObject boat = Instantiate(_boat, new Vector3(length,xaxis,0), Quaternion.identity, BoatHolder);
            GameObject boat = Instantiate(_boat, BoatHolder.position, Quaternion.Euler(Vector3.left * 45f), BoatHolder);
            if (i >= MinimumMoves)
            {
                boat.GetComponent<Image>().sprite = redboat;
            }

            if (i > 88)
            {
                GameObject _arrow = Instantiate(arrow, new Vector3((length + 1), xaxis, 0), Quaternion.identity, gameObject.transform);
                break;
            }
            else if (i > 67)
            {
                xaxis = -2;
                back = -40f;
            }
            else if (i > 44)
            {
                xaxis = -1;
                back = -28.5f;
            }
            else if (i > 21)
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
        if (MinimumMoves != Score)
        {
            _Text.text = LanguageController.GetText(1) + Score.ToString() + LanguageController.GetText(2) + MinimumMoves + "!";
        }
        else
        {
            _Text.text = LanguageController.GetText(3) + MinimumMoves + "!";
        }
    }
}

