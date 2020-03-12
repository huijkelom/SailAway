using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floating : MonoBehaviour {
    float time;
    [SerializeField] Transform waterPlane;
    Mesh planeMesh;
    [SerializeField] int distanceIndex;
    // Use this for initialization
    void Start () {
        
        time = Random.Range(0.1f, 0.8f);
        StartCoroutine(Bigger());
	}

    // Update is called once per frame

    IEnumerator Bigger()
    {
        while (transform.localScale.x <= 1.05f)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(1.027f, 1.027f), Time.deltaTime * time);
            yield return null;
        }
        StartCoroutine(Smaller());
    }

    IEnumerator Smaller()
    {
        while (transform.localScale.x >= 0.95f)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(0.974f, 0.974f), Time.deltaTime * time);
            yield return null;
        }
        StartCoroutine(Bigger());
    }
}
