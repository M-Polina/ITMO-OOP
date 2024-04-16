namespace Banks.Banks;

public interface IObservable
{
    void AddSubscriber(ISubscriber subscriber);

    void RemoveSubscriber(ISubscriber subscriber);

    void NotifySubscribers();
}