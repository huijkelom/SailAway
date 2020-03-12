using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("_Wait");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator _Wait()
    {
        yield return new WaitForSeconds(Random.Range(20, 60));
        StartCoroutine("FishEnter");
        StartCoroutine("FishEnter277");
    }
    IEnumerator FishEnter()
    {
        transform.position = new Vector3(8, Random.Range(5, -5), 0);
        while(transform.position.x >-10)
        {
            transform.position -= new Vector3(0.02f, 0, 0);
            //transform.
            yield return null; 
        }
        StartCoroutine("_Wait");
    }
    float ObjectMove1;
    float ObjectMove2;
    IEnumerator FishEnter2()
    {
        if (transform.rotation.y > 0)
        {
            ObjectMove1 -= 0.0004f;
        }
        else
        {
            ObjectMove1 += 0.0004f;
        }
        transform.Rotate(Vector3.forward * ObjectMove1);
        if (transform.position.x > 0)
        {
            ObjectMove2 -= 0.00004f;
        }
        else
        {
            ObjectMove2 += 0.00004f;
        }
        transform.Translate(Vector3.right * (ObjectMove2));
        yield return null;
    }
}
