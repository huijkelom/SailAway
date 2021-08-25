using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSoundsManager : MonoBehaviour
{
    private static ScoreSoundsManager _Instance;
    public AudioSource WinSound;
    public AudioSource ScoreSound;

    private void Awake()
    {
        _Instance = this;
    }


    private void _PlayWinSound()
    {
        if (WinSound != null)
        {
            if (!WinSound.isPlaying)
            {
                WinSound.Play();
            }
        }
        else
        {
            Debug.LogError("ScoreSoundsManager | PlayWinSound | Missing link to audio source.");
        }
    }

    private void _PlayScoreSound()
    {
        if (ScoreSound != null)
        {
            if (!ScoreSound.isPlaying)
            {
                ScoreSound.Play();
            }
        }
        else
        {
            Debug.LogError("ScoreSoundsManager | PlayScoreSound | Missing link to audio source.");
        }
    }
}
