﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishBehaviour : MonoBehaviour
{
    int spawnplace;
    public Vector3 place;
    public Vector3 movingTowards;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("_Wait");
    }

    IEnumerator _Wait()
    {
        yield return new WaitForSeconds(Random.Range(5, 10));
        spawnplace = Random.Range(0, 4);
        switch (spawnplace)
        {
            case 0:
                place = new Vector3(8, Random.Range(5, -5), 10);
                transform.eulerAngles = new Vector3(180,90,90);
                movingTowards = new Vector3(0.02f, 0, 0);
                break;
            case 1:
                place = new Vector3(Random.Range(6, -6), 7, 10);
                transform.eulerAngles = new Vector3(90, 90, 90);
                movingTowards = new Vector3(0, 0.02f, 0);

                break;
            case 2:
                place = new Vector3(-8, Random.Range(5, -5), 10);
                transform.eulerAngles = new Vector3(00, 90, 90);
                movingTowards = new Vector3(-0.02f, 0, 0);
                break;
            case 3:
                place = new Vector3(Random.Range(6, -6), -7, 10);
                transform.eulerAngles = new Vector3(270, 90, 90);
                movingTowards = new Vector3(0, -0.02f, 0);
                break;
        }

        StartCoroutine(FishEnter());
    }

    IEnumerator FishEnter()
    {
        transform.position = place;
        while(Vector3.Distance(transform.position,place)<20)
        {
            transform.position -= movingTowards * 50 * Time.deltaTime;
            yield return null;
        }

        StartCoroutine("_Wait");
    }
}