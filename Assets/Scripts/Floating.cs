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
        while (transform.localScale.x <= 0.919f)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(0.920f, 0.920f), Time.fixedDeltaTime * time);
            yield return new WaitForFixedUpdate();
        }

        StartCoroutine(Smaller());
    }

    IEnumerator Smaller()
    {
        while (transform.localScale.x >= 0.881f)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(0.880f, 0.880f), Time.fixedDeltaTime * time);
            yield return new WaitForFixedUpdate();
        }

        StartCoroutine(Bigger());
    }
}
