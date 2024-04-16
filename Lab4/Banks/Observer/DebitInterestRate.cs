using Banks.Exceptions;

namespace Banks.Banks;

public class DebitInterestRate : IObservable
{
    private const decimal MinInterestRate = 0;

    private List<ISubscriber> _subscribersList = new List<ISubscriber>();

    public DebitInterestRate(decimal interestRate)
    {
        if (interestRate <= MinInterestRate)
            throw new BanksException("debitInterestRate is incorrect while creating DebitInterestRate.");

        InterestRate = interestRate;
    }

    public IReadOnlyCollection<ISubscriber> SubscribersList => _subscribersList;
    public decimal InterestRate { get; private set; }

    public void AddSubscriber(ISubscriber subscriber)
    {
        if (subscriber is null)
            throw new BanksException("Null Subscriber while Adding DebitInterestRate Subscriber.");
        if (!_subscribersList.Contains(subscriber))
            _subscribersList.Add(subscriber);
    }

    public void RemoveSubscriber(ISubscriber subscriber)
    {
        if (subscriber is null)
            throw new BanksException("Null Subscriber while removing DebitInterestRate Subscriber.");
        if (!HasSubscriber(subscriber))
            throw new BanksException("Subscriber doesn't exists, so it can't be added to DebitInterestRate Subscribers.");

        _subscribersList.Remove(subscriber);
    }

    public void NotifySubscribers()
    {
        foreach (ISubscriber sr in _subscribersList)
        {
            sr.Update(this);
        }
    }

    public void ChangeDebitInterestRate(decimal newRate)
    {
        if (newRate <= MinInterestRate)
            throw new BanksException("debitInterestRate is incorrect while changing.");

        InterestRate = newRate;
        NotifySubscribers();
    }

    private bool HasSubscriber(ISubscriber subscriber)
    {
        if (subscriber is null)
            return false;
        return _subscribersList.SingleOrDefault(sb => sb.Id == subscriber.Id) is not null;
    }
}