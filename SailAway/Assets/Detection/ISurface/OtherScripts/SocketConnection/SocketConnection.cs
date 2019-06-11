using System;
using System.Net;
using System.Net.Sockets;

public class SocketConnection {

    private string _IP;
    private int _Port;
    private SocketMessageProcessor _Processor;
    private Socket _Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    private byte[] _Buffer = new byte[8142];

    public SocketConnection(string ip, int port, SocketMessageProcessor processor)
    {
        _IP = ip;
        _Port = port;
        _Processor = processor;
    }

    public bool Connect()
    {
        try
        {
            _Socket.Connect(new IPEndPoint(IPAddress.Parse(_IP), _Port));
            _Socket.BeginReceive(_Buffer, 0, _Buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), null);
        }
        catch (SocketException)
        {
            GameConsole.Log("Failed connection");
            return false;
        }
        return true;
    }

    private void ReceiveCallback(IAsyncResult AR)
    {
        int recieved = _Socket.EndReceive(AR);
        if (recieved <= 0) { return; }

        byte[] recData = new byte[recieved];
        Buffer.BlockCopy(_Buffer, 0, recData, 0, recieved);

        _Processor.AddMessageToQueue(recData);

        _Socket.BeginReceive(_Buffer, 0, _Buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), null);
    }

    internal bool IsConnected()
    {
        if (_Socket != null)
        {
            return _Socket.Connected;
        }
        return false;
    }

    internal void Disconnect()
    {
        _Socket.Close();
    }
}
