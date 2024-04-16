using Banks.Exceptions;

namespace Banks.Banks;

public class CreditCommission : IObservable
{
    private const decimal MinCommission = 0;

    private List<ISubscriber> _subscribersList = new List<ISubscriber>();

    public CreditCommission(decimal commission)
    {
        if (commission <= MinCommission)
            throw new BanksException("Commission is incorrect while creating Commission.");

        Commission = commission;
    }

    public IReadOnlyCollection<ISubscriber> SubscribersList => _subscribersList;
    public decimal Commission { get; private set; }

    public void AddSubscriber(ISubscriber subscriber)
    {
        if (subscriber is null)
            throw new BanksException("Null Subscriber while Adding Commission Subscriber.");
        if (!_subscribersList.Contains(subscriber))
            _subscribersList.Add(subscriber);
    }

    public void RemoveSubscriber(ISubscriber subscriber)
    {
        if (subscriber is null)
            throw new BanksException("Null Subscriber while removing Commission Subscriber.");
        if (!HasSubscriber(subscriber))
            throw new BanksException("Subscriber doesn't exists, so it can't be added to Commission Subscribers.");

        _subscribersList.Remove(subscriber);
    }

    public void NotifySubscribers()
    {
        foreach (ISubscriber sr in _subscribersList)
        {
            sr.Update(this);
        }
    }

    public void ChangeCreditComission(decimal commission)
    {
        if (commission <= MinCommission)
            throw new BanksException("Commission is incorrect while changing.");

        Commission = commission;
        NotifySubscribers();
    }

    private bool HasSubscriber(ISubscriber subscriber)
    {
        if (subscriber is null)
            return false;
        return _subscribersList.SingleOrDefault(sb => sb.Id == subscriber.Id) is not null;
    }
}