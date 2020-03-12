using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] public int yRow;
    [SerializeField] public int xRow;
    [SerializeField] Vector2 StartingLoc;
    public List<Vector2> Coördinates = new List<Vector2>();
    [SerializeField] GameObject Boat;
    [SerializeField] GameObject Crate;
    [SerializeField] GameObject Point;
    [SerializeField] bool openFieldNeeded;
    // Use this for initialization
    void Makegrid()
    {
        MakeBorders(new Vector2(3.5f, -4.5f), new Vector2(-3.5f, -4.5f));
        for (int i = 0; i < yRow; i++)
        {
            for (int j = 0; j < xRow; j++)
            {
                Vector2 place = new Vector2(i - 2.5f, j - 3.5f);
                GameObject newboat = Instantiate(Boat, place, Boat.transform.rotation);
                newboat.transform.SetParent(Boat.transform.parent);
                newboat.transform.localScale = new Vector2(1, 1);
                Coördinates.Add(place);
            }
        }
        MakeBorders(new Vector2(3.5f, 2.5f), new Vector2(3.5f, -4.5f));
    }
    void MakeBorders(Vector2 x, Vector2 y)
    {
        for (int i = 0; i < xRow + 2; i++)
        {
            Vector2 place = y + new Vector2(0, i);
            GameObject newcrate = Instantiate(Crate, place, Boat.transform.rotation);
            newcrate.transform.SetParent(Boat.transform.parent);
            newcrate.transform.localScale = new Vector2(1, 1);
            if (openFieldNeeded == false && i == 3)
            {
                GameObject.Destroy(newcrate);
                GameObject finish = Instantiate(Point, place, Point.transform.rotation);
                finish.transform.SetParent(Boat.transform.parent);
                finish.transform.localScale = new Vector2(1, 1);
                finish.GetComponent<Pointbehavoir>().finishPoint = true;
                openFieldNeeded = true;
            }
        }
        for (int i = 0; i < yRow + 2; i++)
        {
            Vector2 place = x - new Vector2(i, 0);
            GameObject newcrate = Instantiate(Crate, place, Boat.transform.rotation);
            newcrate.transform.SetParent(Boat.transform.parent);
            newcrate.transform.localScale = new Vector2(1, 1);
        }
    }
    void Awake()
    {
        Makegrid();
    }
}
