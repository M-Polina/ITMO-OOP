using Banks.Exceptions;

namespace Banks.Banks;

public class DepositConditions : IObservable
{
    private List<ISubscriber> _subscribersList = new List<ISubscriber>();

    public DepositConditions(DepositInterestRateConditions conditions)
    {
        if (conditions is null)
            throw new BanksException("Coditions is incorrect while creating Bank.");

        DepositInterestRateConditions = conditions;
    }

    public DepositInterestRateConditions DepositInterestRateConditions { get; private set; }

    public IReadOnlyCollection<ISubscriber> SubscribersList => _subscribersList;

    public void AddSubscriber(ISubscriber subscriber)
    {
        if (subscriber is null)
            throw new BanksException("Null Subscriber while Adding DepositConditions Subscriber.");
        if (!_subscribersList.Contains(subscriber))
            _subscribersList.Add(subscriber);
    }

    public void RemoveSubscriber(ISubscriber subscriber)
    {
        if (subscriber is null)
            throw new BanksException("Null Subscriber while removing DebitInterestRate Subscriber.");
        if (!HasSubscriber(subscriber))
            throw new BanksException("Subscriber doesn't exists, so it can't be added to DepositConditions Subscribers.");

        _subscribersList.Remove(subscriber);
    }

    public void NotifySubscribers()
    {
        foreach (ISubscriber sr in _subscribersList)
        {
            sr.Update(this);
        }
    }

    public void ChangeDepositConditions(DepositInterestRateConditions coditions)
    {
        if (coditions is null)
            throw new BanksException("DepositCoditions is incorrect while changing.");

        DepositInterestRateConditions = coditions;
        NotifySubscribers();
    }

    private bool HasSubscriber(ISubscriber subscriber)
    {
        if (subscriber is null)
            return false;
        return _subscribersList.SingleOrDefault(sb => sb.Id == subscriber.Id) is not null;
    }
}