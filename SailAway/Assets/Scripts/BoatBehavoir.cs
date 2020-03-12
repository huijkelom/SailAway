using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoatBehavoir : MonoBehaviour, I_SmartwallInteractable
{
    void I_SmartwallInteractable.Hit(Vector3 hitPosition)
    {
        points();
    }
    public LayerMask layer;
    int stop = 0;
    public bool Clicked = false;
    bool SetRight = false;
    public float TotalDistance = 0.5f;
    [SerializeField] GameObject pointOrginFront;
    [SerializeField] GameObject pointOrginBack;
    public bool CanUse;

    void SettingActive(Vector2 Direction,GameObject pointOrgin, bool frontOrBack)
    {
        if (GetComponent<RectTransform>().sizeDelta == new Vector2(100,300)&& SetRight == false)
        {
            pointOrginFront.transform.position += gameObject.transform.TransformDirection(Vector2.up / 2);
            pointOrginBack.transform.position += gameObject.transform.TransformDirection(Vector2.down / 2);
            SetRight = true;
        }
        Clicked = true;
        Vector2 fwd = transform.TransformDirection(Vector2.up);
        Ray LineForward = new Ray(pointOrgin.transform.position, fwd);
        Debug.DrawRay(LineForward.origin, LineForward.direction, Color.yellow, 5);
        foreach (RaycastHit2D hit in Physics2D.RaycastAll(pointOrgin.transform.position, Direction, Mathf.Infinity,layer))
        {
            if (stop <1)
            {
                Activate(hit, frontOrBack);
            }
            else
            {
                stop = 0;
                Clicked = false;
                break;
            }
        }
        Clicked = false;
    }
    void Activate(RaycastHit2D hit, bool _frontOrBack)
    {
        if (hit.transform.tag == "boat" && hit.transform.GetComponent<BoatBehavoir>().Clicked == false)
        {
            stop++;
            if (stop == 1)
            {
                return;
            }
        }
        else if (hit.transform.tag == "point")
        {
            hit.transform.GetComponent<Pointbehavoir>().Activate(gameObject,_frontOrBack);
        }
    }
    public void points()
    {
        if (CanUse != false)
        {
            StartCoroutine("Working");
            GetComponent<AudioSource>().Play();
            foreach (GameObject AllGrids in GameObject.FindGameObjectsWithTag("point"))
            {
                if (AllGrids.GetComponent<Pointbehavoir>().CanInteract == true)
                {
                    AllGrids.GetComponent<Pointbehavoir>().Image.enabled = false;
                    AllGrids.GetComponent<Pointbehavoir>().CanInteract = false;
                }
            }
            Vector2 fwd = transform.TransformDirection(Vector2.up);
            Vector2 bwd = transform.TransformDirection(Vector2.down);
            SettingActive(fwd, pointOrginFront, true);
            SettingActive(bwd, pointOrginBack, false);
            foreach (GameObject Allboeats in GameObject.FindGameObjectsWithTag("boat"))
            {
                StartCoroutine(Allboeats.GetComponent<BoatBehavoir>().Working());
                Allboeats.GetComponent<Image>().enabled = false;
            }
            GetComponent<Image>().enabled = true;
            StartCoroutine(hit());
        }
    }
    public IEnumerator Working()
    {
         CanUse = false;
         yield return new WaitForSeconds(0.3f);
         CanUse = true;
    }
    public void setfalse()
    {
        CanUse = false;
    }
    public IEnumerator hit()
    {
        Vector2 oSize = transform.localScale;
        transform.localScale = new Vector2(0.9f, 0.9f);
        yield return new WaitForSeconds(0.1f);
        transform.localScale = oSize;
    }
}
