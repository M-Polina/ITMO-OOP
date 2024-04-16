namespace Banks.MessageHandlers;

public interface IMessageHandler
{
    void SendMessage(IMessage message);
}