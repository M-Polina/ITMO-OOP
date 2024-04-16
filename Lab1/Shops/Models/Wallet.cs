using Isu.Exceptions;

namespace Shops.Models;

public class Wallet
{
    private const int _minAmountOfMoney = 0;

    private int _amountOfMoney = 0;

    public Wallet(int initialAmountOfMoney)
    {
        if (initialAmountOfMoney < _minAmountOfMoney)
        {
            throw new ArgumentException("Initial amount of money can't be < 0.");
        }

        _amountOfMoney = initialAmountOfMoney;
    }

    public int AmountOfMoney => _amountOfMoney;

    public void WriteOffMoney(int debitedAmountOfMoney)
    {
        if (debitedAmountOfMoney > _amountOfMoney)
        {
            throw new DebitingMoneyException($" Not enought money ({_amountOfMoney}) to wright-off {debitedAmountOfMoney}.");
        }

        _amountOfMoney -= debitedAmountOfMoney;
    }

    public void DipositMoney(int dipositedAmountOfMoney)
    {
        if (dipositedAmountOfMoney < _minAmountOfMoney)
        {
            throw new ArgumentException($" Deposited amount of money can't be < 0.");
        }

        _amountOfMoney += dipositedAmountOfMoney;
    }
}