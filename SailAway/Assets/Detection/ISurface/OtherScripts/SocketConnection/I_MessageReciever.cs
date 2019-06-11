public interface I_MessageReciever
{
    void RecieveMessage(byte[] message);
    void SetConnected(bool connected);
}