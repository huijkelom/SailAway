using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Mode {
    ARDUINO, BLOB, NONE
}

public class ConnectionFeedback : MonoBehaviour {

    public Mode Hardware;
    public GameObject[] FeedbackObjects;
    bool _WasOn = true;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        switch (Hardware)
        {
            case Mode.ARDUINO:
                //List<string> arduinos = ArduinoControl.GetConnectedArduinos();
                //if ((arduinos.Count != 0 || ArduinoControl.QueueForArduino.Count == 0) && _WasOn)
                //{
                //    foreach (GameObject gb in FeedbackObjects)
                //    {
                //        gb.SetActive(false);
                //    }
                //    _WasOn = false;
                //}
                //else if (arduinos.Count == 0 && ArduinoControl.QueueForArduino.Count != 0 && !_WasOn)
                //{
                //    foreach (GameObject gb in FeedbackObjects)
                //    {
                //        gb.SetActive(true);
                //    }
                //    _WasOn = true;
                //}
                //ConnectionLog.ClearConnections();
                //foreach (string s in arduinos)
                //{
                //    ConnectionLog.RegisterConnection(s);
                //}
                //break;
            case Mode.BLOB:
                if (BlobTracking.Connected && _WasOn)
                {
                    foreach (GameObject gb in FeedbackObjects)
                    {
                        gb.SetActive(false);
                    }
                    _WasOn = false;
                }
                else if (!BlobTracking.Connected && !_WasOn)
                {
                    foreach (GameObject gb in FeedbackObjects)
                    {
                        gb.SetActive(true);
                    }
                    _WasOn = true;
                }
                break;
        }
    }
}