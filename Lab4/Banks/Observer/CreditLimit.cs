using Banks.Exceptions;

namespace Banks.Banks;

public class CreditLimit : IObservable
{
    private const decimal MaxCreditLimit = 0;

    private List<ISubscriber> _subscribersList = new List<ISubscriber>();

    public CreditLimit(decimal limit)
    {
        if (limit >= MaxCreditLimit)
            throw new BanksException("CreditLimit is incorrect while creating CreditLimit.");

        Limit = limit;
    }

    public IReadOnlyCollection<ISubscriber> SubscribersList => _subscribersList;
    public decimal Limit { get; private set; }

    public void AddSubscriber(ISubscriber subscriber)
    {
        if (subscriber is null)
            throw new BanksException("Null Subscriber while Adding CreditLimit Subscriber.");
        if (!_subscribersList.Contains(subscriber))
            _subscribersList.Add(subscriber);
    }

    public void RemoveSubscriber(ISubscriber subscriber)
    {
        if (subscriber is null)
            throw new BanksException("Null Subscriber while removing CreditLimit Subscriber.");
        if (!HasSubscriber(subscriber))
            throw new BanksException("Subscriber doesn't exists, so it can't be added to CreditLimit Subscribers.");

        _subscribersList.Remove(subscriber);
    }

    public void NotifySubscribers()
    {
        foreach (ISubscriber sr in _subscribersList)
        {
            sr.Update(this);
        }
    }

    public void ChangeCreditLimit(decimal creditLimit)
    {
        if (creditLimit >= MaxCreditLimit)
            throw new BanksException("CreditLimit is incorrect while changing.");

        Limit = creditLimit;
        NotifySubscribers();
    }

    private bool HasSubscriber(ISubscriber subscriber)
    {
        if (subscriber is null)
            return false;
        return _subscribersList.SingleOrDefault(sb => sb.Id == subscriber.Id) is not null;
    }
}