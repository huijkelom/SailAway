using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstancePlane : MonoBehaviour
{
    public static InstancePlane Instance { get; private set; }
    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
