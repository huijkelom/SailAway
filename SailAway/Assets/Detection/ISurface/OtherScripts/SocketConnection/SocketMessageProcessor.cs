using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

enum State
{
    CONNECTING, CONNECTED
}

public class SocketMessageProcessor : MonoBehaviour {

    private List<byte[]> MessageQueue = new List<byte[]>();
    private SocketConnection _Connection;
    private State _ConnectionState;

    public I_MessageReciever Reciever;
    public string IP = "127.0.0.1";
    public int Port = 3000;
    public bool OnlyLastMessage = true;

    private void Awake()
    {
        GameConsole.Log("awake");
        _Connection = new SocketConnection(IP, Port, this);
        GameConsole.Log("awake2");
    }

    // Use this for initialization
    void Start () {
        //Connect();

        UnityAction method = Reconnect;
        GameConsole.RegisterCommand(method, "TCPconn");
	}

    public void SetReciever(I_MessageReciever reciever)
    {
        Reciever = reciever;
        Connect();
    }

    private void Connect()
    {
        _ConnectionState = State.CONNECTING;
        GameConsole.Log("Connecting to TCP Server");
        if (_Connection.Connect())
        {
            _ConnectionState = State.CONNECTED;
            GameConsole.Log("Connected to TCP Server");
            Reciever.SetConnected(true);
        }
        else
        {
            GameConsole.Log("Failed to connect, retrying in background.");
            StartCoroutine(BackgroundConnecting());
        }
    }

    private void Disconnect()
    {
        _Connection.Disconnect();
        if(Reciever != null)
        Reciever.SetConnected(false);
    }

    public void Reconnect()
    {
        Disconnect();
        Connect();
    }

    private void OnDestroy()
    {
        Disconnect();
    }

    // Once per frame we process the messages in the Queue, this way we return to the main thread.
    void Update () {
        if (MessageQueue.Count > 0)
        {
            if (OnlyLastMessage)
            {
                lock (MessageQueue)
                {
                    Reciever.RecieveMessage(MessageQueue[MessageQueue.Count - 1]);
                    MessageQueue.Clear();
                }
            }
            else
            {
                lock (MessageQueue)
                {
                    foreach (byte[] msg in MessageQueue)
                    {
                        Reciever.RecieveMessage(msg);
                    }
                    MessageQueue.Clear();
                }
            }
        }
        else
        {
            if (!_Connection.IsConnected() && _ConnectionState == State.CONNECTED)
            {
                GameConsole.Log("TCP socket lost connection");
                Reciever.SetConnected(false);
                _ConnectionState = State.CONNECTING;
            }
        }
	}

    public void AddMessageToQueue(byte[] msg)
    {
        MessageQueue.Add(msg);
    }

    IEnumerator BackgroundConnecting()
    {
        bool connected = false;
        int atempts = 0;
        while (!connected)
        {
            connected = _Connection.Connect();
            atempts++;
            if(atempts > 9)
            {
                break;
            }
            yield return new WaitForSeconds(5);
        }
        if (connected)
        {
            _ConnectionState = State.CONNECTED;
            Reciever.SetConnected(true);
        }
    }
}
