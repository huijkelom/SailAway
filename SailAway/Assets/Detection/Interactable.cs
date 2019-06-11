using UnityEngine;
using System.Collections;

public abstract class Interactable : MonoBehaviour
{
    [HideInInspector]
    public bool Clicked = false;
    public float Cooldown = 20; //The amount of frames the interactable should not be hit again
    // Prevents double inputs

    public void Interact(Vector2 clickposition)
    {
        if (!Clicked)
        {
            StartCoroutine(_clicked());
            Click(clickposition);
        }
    }

    protected abstract void Click(Vector3 clickposition);

    IEnumerator _clicked()
    {
        Clicked = true;
        float framecount = Cooldown;
        while (framecount >= 0)
        {
            framecount--;
            yield return null;
        }

        Clicked = false;
    }
}