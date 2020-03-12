using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HighscoreContainer
{
    public int Highscore;
    public HighscoreContainer(int score)
    {
        Highscore = score;
    }
    public HighscoreContainer() { }
}

/// <summary>
/// This Class manages the score display scene. It is Important that the scene is still called "Scores".
/// You may wish to switch the replay button arrow, it is under C_UIRoot > P_Replay > BT_Replay
/// </summary>
public class ScoreScreenController : MonoBehaviour
{
    [SerializeField] Image LeftButton;
    [SerializeField] Image RightButton;
    [SerializeField] Text WarningText;
    private static List<int> Scores = new List<int>();
    /// <summary>
    /// Current highscore, publicly availible incase you want to use it for something. DOES NOT PERSIST!
    /// </summary>
    public static int Highscore { get { return _Highscore; } }
    private static int _Highscore = 0;

    public static int IndexOfSceneToMoveTo = 1;
    [HideInInspector]
    public float BarRiseAnimationTime = 0.7f;
    public GameObject P_Scoring;
    public GameObject ReplayButton;
    public GameObject ScoreBarBase;
    public bool _TimeGame;
    public int _Score;
    public Stars _stars;
    /// <summary>
    /// Moves to the scores scene to display the final scores and declare a winner and/or new highscore.
    /// Please set in the scene if you wish to use the continue or replay arrow on the button. The sceneIndex
    /// parameter is for determining what scen to move to after the scores have been shown.
    /// </summary>
    /// <param name="sceneIndex">Scene to move to from score scene, defaults to one.</param>
    public static void MoveToScores(List<int> scores, int sceneIndex = 1)
    {
        if (scores == null)
        {
            Debug.LogError("ScoreScreenController | MoveToScores | No scores have been stored in the scores list!");
        }
        else if (scores.Count == 0)
        {
            Debug.LogError("ScoreScreenController | MoveToScores | No scores have been stored in the scores list!");
        }
        IndexOfSceneToMoveTo = sceneIndex;
        Scores = scores;
        SceneManager.LoadScene("Scores");
    }
    private int i = 0;
    private void Awake()
    {
    }
    void Start()
    {
        _Score = Scores[0];
        _stars.AmountStars();
        i++;
        //load highscore from file
        if (GlobalGameSettings.GetSetting("Reset Highscore").Equals("No"))
        {
            LoadHighscore();
        }
        else if (GlobalGameSettings.GetSetting("Reset Highscore").Equals(string.Empty))
        {
            if (GlobalGameSettings.GetSetting("Reset HS").Equals("No"))
            {
                LoadHighscore();
            }
        }

        //check if we have all requirements linked
        if (ScoreBarBase == null) { Debug.LogError("ScoreScreenController | Start | Missing base object for score bars."); }
        if (P_Scoring == null) { Debug.LogError("ScoreScreenController | Start | Missing Link to perant panel."); }
        if (ReplayButton == null) { Debug.LogError("ScoreScreenController | Start | Missing Link to replay button."); }

        if (Scores == null)
        {
            Debug.LogError("ScoreScreenController | Start | No scores have been stored in the static Scores list!");
        }
        else
        {
            int numberOf0Scores = 0;
            int highestScore = 0;
            foreach (int score in Scores)
            {
                if (score == 0) { numberOf0Scores++; }
                if (score > highestScore) { highestScore = score; }
            }
            if (Scores.Count == 0)
            {
                Debug.LogError("ScoreScreenController | Start | No scores have been stored in the static Scores list!");
                return;
            }
            else if (Scores.Count - numberOf0Scores == 1)
            {
                //SetupSinglePlayer(Scores.IndexOf(highestScore), _TimeGame);
            }
            else if (Scores.Count - numberOf0Scores > 1)
            {
                //SetupMultiPlayer(highestScore);
            }
            if (Highscore == 0)
            {
                _Highscore = highestScore;
                SaveHighscore();
            }
            else if (highestScore < Highscore && _TimeGame == true)
            {
                _Highscore = highestScore;
                SaveHighscore();
            }
            else if (highestScore > Highscore && _TimeGame == false)
            {
                _Highscore = highestScore;
                SaveHighscore();
            }
        }
        Invoke("EnableReplay", BarRiseAnimationTime + 1f);
        if(Manager._LevelName > 38)
        {
            LeftButton.color = new Vector4(0.5f, 0.5f, 0.5f, 1);
            LeftButton.GetComponentInChildren<Image>().color = new Vector4(0.5f, 0.5f, 0.5f, 1);
            LeftButton.GetComponent<BoxCollider2D>().enabled = false;
            LeftButton.GetComponent<Button>().enabled = false;
        }
        if (Manager._LevelName < 1)
        {
            RightButton.color = new Vector4(0.5f, 0.5f, 0.5f, 1);
            RightButton.GetComponentInChildren<Image>().color = new Vector4(0.5f, 0.5f, 0.5f, 1);
            RightButton.GetComponent<BoxCollider2D>().enabled = false;
            RightButton.GetComponent<Button>().enabled = false;
        }
    }

    private void SetupSinglePlayer(int playerNr, bool _TimeGame)
    {
        int highestScore;
        if (_TimeGame == false)
        {
            if (Scores[playerNr] > Highscore)
            {
                highestScore = Scores[playerNr];
            }
            else
            {
                highestScore = Highscore;
            }
        }
        else if (_TimeGame == true && Highscore != 0)
        {
            if (Scores[playerNr] < Highscore)
            {
                highestScore = Scores[playerNr];
            }
            else
            {
                highestScore = Highscore;
            }
        }
        else
        {
            highestScore = Scores[playerNr];
        }
        ScoreBar temp = Instantiate(ScoreBarBase, P_Scoring.transform).GetComponent<ScoreBar>();
        temp.SetNewBarColour(PlayerColourContainer.GetPlayerColour(playerNr + 1));
        if (_TimeGame == true && Highscore != 0)
        {
            if (Scores[playerNr] < highestScore)
            {
                temp.Begin(Scores[playerNr], (float)Scores[playerNr] / (float)highestScore, BarRiseAnimationTime, Scores[playerNr] < Highscore, Scores[playerNr] < Highscore, 0.1f);
            }
            else
            {
                temp.Begin(Scores[playerNr], (float)highestScore / (float)Scores[playerNr], BarRiseAnimationTime, Scores[playerNr] < Highscore, Scores[playerNr] < Highscore, 0.1f);
            }
        }
        else if (highestScore != 0)
        {
            temp.Begin(Scores[playerNr], (float)Scores[playerNr] / (float)highestScore, BarRiseAnimationTime, Scores[playerNr] > Highscore, Scores[playerNr] > Highscore, 0.1f);
        }
        temp = Instantiate(ScoreBarBase, P_Scoring.transform).GetComponent<ScoreBar>();
        temp.SetNewBarColour(PlayerColourContainer.GetPlayerColour(0));
        if (_TimeGame == false && Highscore != 0)
        {
            temp.Begin(Highscore, (float)Highscore / (float)highestScore, BarRiseAnimationTime, false, false, 0.1f);
        }
        else if (Highscore != 0)
        {
            if (Scores[playerNr] < highestScore)
            {
                temp.Begin(Highscore, (float)Highscore / (float)highestScore, BarRiseAnimationTime, false, false, 0.1f);
            }
            else
            {
                temp.Begin(Highscore, (float)highestScore / (float)Highscore, BarRiseAnimationTime, false, false, 0.1f);
            }
        }
    }

    private void SetupMultiPlayer(int highestScore)
    {
        for (int i = 0; i < Scores.Count; i++)
        {
            if (Scores[i] > 0)
            {
                ScoreBar temp = Instantiate(ScoreBarBase, P_Scoring.transform).GetComponent<ScoreBar>();
                temp.SetNewBarColour(PlayerColourContainer.GetPlayerColour(i + 1));
                temp.Begin(Scores[i], (float)Scores[i] / (float)highestScore, BarRiseAnimationTime, Scores[i] > Highscore && Scores[i] == highestScore, Scores[i] == highestScore, 0.1f);
            }
        }
    }

    private void SaveHighscore()
    {
        XML_to_Class.SaveClassToXML(new HighscoreContainer(Highscore), "StreamingAssets" + Path.DirectorySeparatorChar + "HighScore");
    }

    private void LoadHighscore()
    {
        try
        {
            _Highscore = XML_to_Class.LoadClassFromXML<HighscoreContainer>("StreamingAssets" + Path.DirectorySeparatorChar + "HighScore").Highscore;
        }
        catch (NullReferenceException NREx)
        {
            //no HS file yet
        }
    }

    private void EnableReplay()
    {
        ReplayButton.SetActive(true);
    }

    public void BT_Replay_ClickedRestart()
    {
        Manager._LevelName = IndexOfSceneToMoveTo;
        SceneManager.LoadScene("SampleScene");
    }
    public void BT_Replay_ClickedPrevious()
    {
        if (Manager._LevelName > -1)
        {
            Manager._LevelName = IndexOfSceneToMoveTo - 1;
            SceneManager.LoadScene("SampleScene");
        }
        else
        {
            //Manager._LevelName = Manager;
        }
        //SceneManager.LoadScene("SampleScene");
    }
    public void BT_Replay_ClickedNext()
    {
        if (Manager._LevelName < 40)
        {
            Manager._LevelName = IndexOfSceneToMoveTo + 1;
            SceneManager.LoadScene("SampleScene");

        }
        else
        {
        //    Manager._LevelName = 0;
        }
        //SceneManager.LoadScene("SampleScene");
    }
}
