using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevel : MonoBehaviour, I_SmartwallInteractable
{
    [SerializeField] Manager Controller;
    [SerializeField] Sprite Pushed;
    [SerializeField] Sprite NotPushed;
    bool usable = true;
    public void Hit(Vector3 hitPosition)
    {
        StartCoroutine("Wait");
    }
    IEnumerator Wait()
    {
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = Pushed;
        yield return new WaitForSeconds(0.4f);
        _NextLevel();
    }
    void _NextLevel()
    {
        
        if (usable == true)
        {
            
            GetComponent<AudioSource>().Play();
            Controller.Score = 0;
            foreach (GameObject AllBoats in GameObject.FindGameObjectsWithTag("boat"))
            {
                GameObject.Destroy(AllBoats);
            }
            foreach (GameObject AllGrids in GameObject.FindGameObjectsWithTag("point"))
            {
                AllGrids.GetComponent<Pointbehavoir>().Image.enabled = false;
                AllGrids.GetComponent<Pointbehavoir>().CanInteract = false;
            }
            foreach (GameObject AllLevels in GameObject.FindGameObjectsWithTag("level"))
            {
                //AllLevels.GetComponent<Image>().sprite = unUsingSprite;
                StartCoroutine(AllLevels.GetComponent<Menu>().Working());
            }
            Controller.StartLevel(Controller.LevelName + 1);
            foreach (GameObject AllLevels in GameObject.FindGameObjectsWithTag("niveau"))
            {
                StartCoroutine(AllLevels.GetComponent<Menu>().Working());
            }
            transform.position = new Vector2(100, 100);
            GetComponent<SpriteRenderer>().sprite = NotPushed;
            Controller.LevelName = Controller.LevelName + 1;
            GetComponent<BoxCollider2D>().enabled = true;
            Debug.Log(Controller.LevelName);
        }
    }
}
