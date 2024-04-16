﻿using Banks.Accounts;
using Banks.Banks;

namespace Banks.Console.Handler;

public class AddMoneyHandler : AbstractHandler
{
    private const decimal MinNumber = 0;
    public override void Handle(string? request)
    {
        if (string.IsNullOrEmpty(request))
            return;
        if (request.Equals("Add"))
            DoBusinessLogic();
        else if (NextHendler != null)
            NextHendler.Handle(request);
    }

    private void DoBusinessLogic()
    {
        System.Console.WriteLine("Set information about account to add to:");
        Bank bank = GetBankId();
        IAccount account = bank.GetAccountById(GetAccountId(bank));

        decimal amount = GetAmountOfMoney();
        bank.AddMoney(account.Id, amount);
    }

    private decimal GetAmountOfMoney()
    {
        System.Console.WriteLine("Set  amount of money to add:");
        decimal number;
        string? str = System.Console.ReadLine();
        while (!decimal.TryParse(str, out number))
        {
            System.Console.WriteLine("Set  amount of money to add:");
            str = System.Console.ReadLine();
        }

        if (number <= MinNumber)
            number = GetAmountOfMoney();

        return number;
    }

    private int GetAccountId(Bank bank)
    {
        System.Console.WriteLine("Set account id:");
        int number;
        string? str = System.Console.ReadLine();
        while (!int.TryParse(str, out number))
        {
            System.Console.WriteLine("Set account id:");
            str = System.Console.ReadLine();
        }

        return number;
    }

    private Bank GetBankId()
    {
        System.Console.WriteLine("Set bank id:");
        int number;
        string? str = System.Console.ReadLine();
        while (!int.TryParse(str, out number))
        {
            System.Console.WriteLine("Set bank id:");
            str = System.Console.ReadLine();
        }

        CentralBank centralBank = CentralBank.GetInstance();

        return centralBank.GetBankById(number);
    }
}