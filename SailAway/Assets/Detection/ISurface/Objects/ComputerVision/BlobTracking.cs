using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

/// <summary>
/// Contains the Blobs detected by CCV. Is updated once per frame.
/// </summary>
public class BlobTracking : MonoBehaviour, I_MessageReciever {

    public static BlobTracking Instance;
    public static bool Connected = false;
    public static List<Blob> Blobs = new List<Blob>();
    public SocketMessageProcessor processor;
    private bool DbgBlobs = false;

    private void Awake()
    {
        if(Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            //Debug.Log("You cannot have more then one Blob Tracker");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        processor.SetReciever(this);
        Action method = ToggleDbgBlobs; 
        GameConsole.RegisterCommand(method, "msg");
    }

    public void ToggleDbgBlobs()
    {
        DbgBlobs = !DbgBlobs;
    }

    public void SetConnected(bool connected)
    {
        Connected = connected;
        if (connected)
        {
            ConnectionLog.RegisterConnection("I-Wall");
            GameConsole.Log("I-Wall");
        }
        else
        {
            ConnectionLog.UnregisterConnection("I-Wall");
            GameConsole.Log("no I-Wall");
        }
    }

    public void Reconnect() {
        processor.Reconnect();
    }

    public void RecieveMessage(byte[] message)
    {
        string smessage = Encoding.ASCII.GetString(message);
        string[] values = smessage.Split('!');
        Blobs.Clear();
        int count = int.Parse(values[0]); //number of blobs that were detected this frame.
        int i = 0;
        //Debug.Log(count.ToString());
        while (i < count)
        {
            Blob b = new Blob(int.Parse(values[i*8+1]), float.Parse(values[i * 8 + 2]),  1 - float.Parse(values[i * 8 + 3]),
                int.Parse(values[i * 8 + 4]), int.Parse(values[i * 8 + 5]), int.Parse(values[i * 8 + 6]), int.Parse(values[i * 8 + 7]), int.Parse(values[i * 8 + 8]));
            //Debug.Log(b.XPosition + " " + b.YPosition);
            Blobs.Add(b);
            i++;
        }
    }
}
