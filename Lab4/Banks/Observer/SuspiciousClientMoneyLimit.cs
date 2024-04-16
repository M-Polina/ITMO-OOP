using Banks.Exceptions;

namespace Banks.Banks;

public class SuspiciousClientMoneyLimit : IObservable
{
    private const decimal MinMoneyLimit = 0;

    private List<ISubscriber> _subscribersList = new List<ISubscriber>();

    public SuspiciousClientMoneyLimit(decimal limit)
    {
        if (limit < MinMoneyLimit)
            throw new BanksException("suspiciousClientMoneyLimit is incorrect while creating it.");

        MoneyLimit = limit;
    }

    public IReadOnlyCollection<ISubscriber> SubscribersList => _subscribersList;
    public decimal MoneyLimit { get; private set; }

    public void AddSubscriber(ISubscriber subscriber)
    {
        if (subscriber is null)
            throw new BanksException("Null Subscriber while Adding suspiciousClientMoneyLimit Subscriber.");
        if (!_subscribersList.Contains(subscriber))
            _subscribersList.Add(subscriber);
    }

    public void RemoveSubscriber(ISubscriber subscriber)
    {
        if (subscriber is null)
            throw new BanksException("Null Subscriber while removing suspiciousClientMoneyLimit Subscriber.");
        if (!HasSubscriber(subscriber))
            throw new BanksException("Subscriber doesn't exists, so it can't be added to suspiciousClientMoneyLimit Subscribers.");

        _subscribersList.Remove(subscriber);
    }

    public void NotifySubscribers()
    {
        foreach (ISubscriber sr in _subscribersList)
        {
            sr.Update(this);
        }
    }

    public void ChangeSuspiciousClientMoneyLimit(decimal newMoneyLimit)
    {
        if (newMoneyLimit < MinMoneyLimit)
            throw new BanksException("SuspiciousClientMoneyLimit is incorrect while changing.");

        MoneyLimit = newMoneyLimit;
        NotifySubscribers();
    }

    private bool HasSubscriber(ISubscriber subscriber)
    {
        if (subscriber is null)
            return false;
        return _subscribersList.SingleOrDefault(sb => sb.Id == subscriber.Id) is not null;
    }
}