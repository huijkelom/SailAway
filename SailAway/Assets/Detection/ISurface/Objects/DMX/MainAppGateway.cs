using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class MainAppGateway : MonoBehaviour {

    private Socket _MainApp;
    private bool _BuisySending = false;
    private Queue<string> _Queue = new Queue<string>();

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(this);
        IPEndPoint ipe = new IPEndPoint(IPAddress.Loopback, 2248);
        _MainApp = new Socket(ipe.AddressFamily, SocketType.Stream,ProtocolType.Tcp);
        _MainApp.BeginConnect(ipe, new AsyncCallback(Connected), _MainApp);
    }

    void Connected(IAsyncResult ar)
    {
        if (_Queue.Count > 0)
        {
            SendMessageToMain(_Queue.Dequeue());
        }
    }

    private void OnApplicationQuit()
    {
        _MainApp.Disconnect(true);
    }

    public bool IsConnected()
    {
        if (_MainApp != null)
        {
            return _MainApp.Connected;
        }
        else
        {
            return false;
        }
    }

    public void SendMessageToMain(string msg)
    {
        if (IsConnected() && !_BuisySending)
        {
            byte[] data = Encoding.UTF8.GetBytes(msg);
            _MainApp.BeginSend(data,0,data.Length, SocketFlags.None, new AsyncCallback(MessageSend), _MainApp);
            _BuisySending = true;
        }
        else
        {
            _Queue.Enqueue(msg);
        }
    }

    public void MessageSend(IAsyncResult ar)
    {
        _BuisySending = false;
        if(_Queue.Count > 0)
        {
            SendMessageToMain(_Queue.Dequeue());
        }
    }
}
