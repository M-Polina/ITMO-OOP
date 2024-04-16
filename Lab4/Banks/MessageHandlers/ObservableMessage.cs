using Banks.Banks;
using Banks.Exceptions;

namespace Banks.MessageHandlers;

public class ObservableMessage : IMessage
{
    public ObservableMessage(IObservable observable)
    {
        if (observable is null)
            throw new BanksException("Null IObservable while creating IObservableMessage");

        Observable = observable;
    }

    public IObservable Observable { get; }

    public string GetStringMessage()
    {
        string stringMessage = string.Empty;
        if (Observable is CreditCommission)
            stringMessage = StringComissionMessage();
        if (Observable is CreditLimit)
            stringMessage = StringCreditLimitMessage();
        if (Observable is DebitInterestRate)
            stringMessage = StringDebitInterestRateMessage();
        if (Observable is SuspiciousClientMoneyLimit)
            stringMessage = StringSuspiciousClientMessage();
        if (Observable is DepositConditions)
            stringMessage = StringDepositConditionsMessage();
        return stringMessage;
    }

    private string StringComissionMessage()
    {
        return $"Credit comission was changed to {((CreditCommission)Observable).Commission}!";
    }

    private string StringCreditLimitMessage()
    {
        return $"Credit limit was changed to {((CreditLimit)Observable).Limit}!";
    }

    private string StringDebitInterestRateMessage()
    {
        return $"Debit interest rate was changed to {((DebitInterestRate)Observable).InterestRate}!";
    }

    private string StringDepositConditionsMessage()
    {
        return "Deposit Conditions were changed!";
    }

    private string StringSuspiciousClientMessage()
    {
        return "Suspicious Client Money Limit was changed!";
    }
}