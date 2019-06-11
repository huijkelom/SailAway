using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugObject : Interactable
{
    protected override void Click(Vector3 clickposition)
    {
        //print("You clicked " + name);
        ReloadScene();
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(Application.loadedLevel);
    }
}