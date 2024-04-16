using Banks.Exceptions;

namespace Banks.MessageHandlers;

public class InMemoryMessageHandler : IMessageHandler
{
    private List<string> _messages = new List<string>();

    public IReadOnlyCollection<string> Messages => _messages;

    public void SendMessage(IMessage message)
    {
        if (message is null)
            throw new BanksException("NullMessage while sending message in ConsoleMessageHandler.");

        _messages.Add(message.GetStringMessage());
    }
}