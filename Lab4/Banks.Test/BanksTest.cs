using Banks.Accounts;
using Banks.Banks;
using Banks.Client;
using Banks.Exceptions;
using Banks.MessageHandlers;
using Banks.Time;
using Banks.Transactions;

namespace Banks.Test;

using Xunit;

public class BanksTest
{
    private OwnTime time;
    private CentralBank centralBank;

    public BanksTest()
    {
        time = OwnTime.GetInstance();
        centralBank = CentralBank.GetInstance();
    }

    [Fact]
    public void AddMoney_MoneyAreInAccount()
    {
        DepositInterestRateConditions deposintConditions = new DepositInterestRateConditions(0.01m);
        Bank bank1 = centralBank.CreateBank("Tinkoff", 0.01m, 1000m, -100000.0m, deposintConditions, 1000.0m);

        IBuilder builder1 = new ClientBuilder(bank1, "Den", "Milik");
        builder1.SetAddress("Gavai");
        builder1.SetPassport(1);
        ClientAccount client1 = builder1.CreateAndGetClient();
        bank1.AddClientAccount(client1);

        DebitAccount debit1 = bank1.CreateDebitAccount(client1);
        CreditAccount credit1 = bank1.CreateCreditAccount(client1);

        DepositAccount deposit1 = bank1.CreateDepositAccount(client1, 1000, 12);

        bank1.AddMoney(debit1.Id, 1000.1m);

        bank1.AddMoney(credit1.Id, 1000.1m);

        bank1.AddMoney(deposit1.Id, 1000.1m);

        Assert.Equal(1000.1m, debit1.Money);
        Assert.Equal(1000.1m, credit1.Money);
        Assert.Equal(2000.1m, deposit1.Money);
    }

    [Fact]
    public void WithdrawMoney_CorrectAmountOfMoneyInAccount()
    {
        DepositInterestRateConditions deposintConditions = new DepositInterestRateConditions(0.01m);
        Bank bank1 = centralBank.CreateBank("Tinkoff", 0.01m, 100, -100000, deposintConditions, 1000);

        IBuilder builder1 = new ClientBuilder(bank1, "Den", "Milik");
        builder1.SetAddress("Gavai");
        builder1.SetPassport(1);
        ClientAccount client1 = builder1.CreateAndGetClient();
        bank1.AddClientAccount(client1);

        DebitAccount debit1 = bank1.CreateDebitAccount(client1);
        CreditAccount credit1 = bank1.CreateCreditAccount(client1);

        bank1.AddMoney(debit1.Id, 1000.1m);
        bank1.WithdrawMoney(debit1.Id, 500);

        bank1.AddMoney(credit1.Id, 1000.1m);
        bank1.WithdrawMoney(credit1.Id, 500);

        Assert.Equal(500.1m, debit1.Money);
        Assert.Equal(500.1m, credit1.Money);

        bank1.WithdrawMoney(credit1.Id, 1000.1m);
        Assert.Equal(-600, credit1.Money);
    }

    [Fact]
    public void TransferMoney_CorrectAmountOfMoneyInAccounts()
    {
        DepositInterestRateConditions deposintConditions = new DepositInterestRateConditions(0.01m);
        Bank bank1 = centralBank.CreateBank("Tinkoff", 0.01m, 100, -100000, deposintConditions, 1000);

        IBuilder builder1 = new ClientBuilder(bank1, "Den", "Milik");
        builder1.SetAddress("Gavai");
        builder1.SetPassport(1);
        ClientAccount client1 = builder1.CreateAndGetClient();
        bank1.AddClientAccount(client1);

        DebitAccount debit1 = bank1.CreateDebitAccount(client1);
        CreditAccount credit1 = bank1.CreateCreditAccount(client1);

        bank1.AddMoney(debit1.Id, 1000);
        bank1.TransferMoney(debit1, credit1, 500);

        Assert.Equal(500, debit1.Money);
        Assert.Equal(500, credit1.Money);
    }

    [Fact]
    public void TransferMoneyBetweenBanks_CorrectAmountOfMoneyInAccounts()
    {
        DepositInterestRateConditions deposintConditions = new DepositInterestRateConditions(0.01m);
        Bank bank1 = centralBank.CreateBank("Tinkoff", 0.01m, 100, -100000, deposintConditions, 1000);

        IBuilder builder1 = new ClientBuilder(bank1, "Den", "Milik");
        builder1.SetAddress("Gavai");
        builder1.SetPassport(1);
        ClientAccount client1 = builder1.CreateAndGetClient();
        bank1.AddClientAccount(client1);

        Bank bank2 = centralBank.CreateBank("Sber", 0.01m, 100, -100000, deposintConditions, 1000);

        IBuilder builder2 = new ClientBuilder(bank2, "Den2", "Milik2");
        builder2.SetAddress("Gavai");
        builder2.SetPassport(1);
        ClientAccount client2 = builder2.CreateAndGetClient();
        bank2.AddClientAccount(client2);

        DebitAccount debit1 = bank1.CreateDebitAccount(client1);
        bank1.AddMoney(debit1.Id, 1000);
        DebitAccount debit2 = bank2.CreateDebitAccount(client2);

        bank1.TransferMoney(debit1, debit2, 600);

        Assert.Equal(400, debit1.Money);
        Assert.Equal(600, debit2.Money);
    }

    [Fact]
    public void RollBackSimple_CorrectAmountOfMoneyInAccounts()
    {
        DepositInterestRateConditions deposintConditions = new DepositInterestRateConditions(0.01m);
        Bank bank1 = centralBank.CreateBank("Tinkoff", 0.01m, 100, -100000, deposintConditions, 1000);

        IBuilder builder1 = new ClientBuilder(bank1, "Den", "Milik");
        builder1.SetAddress("Gavai");
        builder1.SetPassport(1);
        ClientAccount client1 = builder1.CreateAndGetClient();
        bank1.AddClientAccount(client1);

        DebitAccount debit1 = bank1.CreateDebitAccount(client1);

        bank1.AddMoney(debit1.Id, 1000);
        bank1.RollbackTrabsaction(bank1.TransactionsList.ElementAt(0).Id);

        Assert.Equal(0, debit1.Money);
    }

    [Fact]
    public void RollBack_CorrectAmountOfMoneyInAccounts()
    {
        DepositInterestRateConditions deposintConditions = new DepositInterestRateConditions(0.01m);
        Bank bank1 = centralBank.CreateBank("Tinkoff", 0.01m, 100, -100000, deposintConditions, 1000);

        IBuilder builder1 = new ClientBuilder(bank1, "Den", "Milik");
        builder1.SetAddress("Gavai");
        builder1.SetPassport(1);
        ClientAccount client1 = builder1.CreateAndGetClient();
        bank1.AddClientAccount(client1);

        DebitAccount debit1 = bank1.CreateDebitAccount(client1);
        CreditAccount credit1 = bank1.CreateCreditAccount(client1);

        bank1.AddMoney(debit1.Id, 1000);
        bank1.TransferMoney(debit1, credit1, 500);
        bank1.RollbackTrabsaction(bank1.TransactionsList.ElementAt(1).Id);

        Assert.Equal(1000, debit1.Money);
        Assert.Equal(0, credit1.Money);
    }

    [Fact]
    public void RollBackTransferBetweenBanks_CorrectAmountOfMoneyInAccounts()
    {
        DepositInterestRateConditions deposintConditions = new DepositInterestRateConditions(0.01m);
        Bank bank1 = centralBank.CreateBank("Tinkoff", 0.01m, 100, -100000, deposintConditions, 1000);
        IBuilder builder1 = new ClientBuilder(bank1, "Den", "Milik");
        builder1.SetAddress("Gavai");
        builder1.SetPassport(1);
        ClientAccount client1 = builder1.CreateAndGetClient();
        bank1.AddClientAccount(client1);

        Bank bank2 = centralBank.CreateBank("Sber", 0.01m, 100, -100000, deposintConditions, 1000);
        IBuilder builder2 = new ClientBuilder(bank2, "Den", "Milik");
        builder2.SetAddress("Gavai");
        builder2.SetPassport(1);
        ClientAccount client2 = builder2.CreateAndGetClient();
        bank2.AddClientAccount(client2);

        DebitAccount debit1 = bank1.CreateDebitAccount(client1);
        bank1.AddMoney(debit1.Id, 1000);
        CreditAccount credit2 = bank2.CreateCreditAccount(client2);
        bank1.TransferMoney(debit1, credit2, 600);

        bank1.RollbackTrabsaction(bank1.TransactionsList.ElementAt(1).Id);

        Assert.Equal(1000, debit1.Money);
        Assert.Equal(0, credit2.Money);

        bank2.TransferMoney(credit2, debit1, 600);

        bank2.RollbackTrabsaction(bank2.TransactionsList.ElementAt(2).Id);

        Assert.Equal(1000, debit1.Money);
        Assert.Equal(0, credit2.Money);
    }

    [Fact]
    public void AfterMothInterastRateCreates_CorrectAmountOfMoneyInAccount()
    {
        DepositInterestRateConditions deposintConditions = new DepositInterestRateConditions(0.012m);
        Bank bank1 = centralBank.CreateBank("Tinkoff", 0.012m, 100, -100000, deposintConditions, 1000);

        IBuilder builder = new ClientBuilder(bank1, "Den", "Milik");
        ClientAccount client1 = builder.CreateAndGetClient();
        bank1.AddClientAccount(client1);

        bank1.AddClientAdress(client1, "Gavai");
        bank1.AddClientPassport(client1, 1);
        DebitAccount debit1 = bank1.CreateDebitAccount(client1);
        bank1.AddMoney(debit1.Id, 1000);

        DepositAccount depos1 = bank1.CreateDepositAccount(client1, 1000, 5);
        centralBank.SpeedUpTime(40);

        Assert.Equal(1030, debit1.Money);
        Assert.Equal(1030, depos1.Money);
    }

    [Fact]
    public void UpdatinfConditions_ConditionsChangedAmmountOfMoneyCorrrect()
    {
        List<decimal> lms = new List<decimal>() { 10000, 100000, 5000000 };
        List<decimal> irs = new List<decimal>() { 0.012m, 0.02m, 0.025m, 0.03m };
        DepositInterestRateConditions deposintConditions = new DepositInterestRateConditions(lms, irs);
        Bank bank1 = centralBank.CreateBank("Tinkoff", 0.024m, 100, -100000, deposintConditions, 1000);

        IBuilder builder = new ClientBuilder(bank1, "Den", "Milik");
        ClientAccount client1 = builder.CreateAndGetClient();

        bank1.AddClientAccount(client1);
        bank1.AddClientAdress(client1, "Gavai");
        bank1.AddClientPassport(client1, 1);
        DebitAccount debit1 = bank1.CreateDebitAccount(client1);
        bank1.AddMoney(debit1.Id, 1000);

        centralBank.SpeedUpTime(30);

        Assert.Equal(1060, debit1.Money);

        bank1.ChangeDebitInterestRate(0.048m);

        centralBank.SpeedUpTime(30);
        Console.WriteLine(time.TimeNow);
        Assert.Equal(0.048m, debit1.InterestRate);
        Assert.Equal(1187.2m, debit1.Money);
    }

    [Fact]
    public void SuspiciousClient_CanNotWithdrawMoreThanAllaud()
    {
        DepositInterestRateConditions deposintConditions = new DepositInterestRateConditions(0.01m);
        Bank bank1 = centralBank.CreateBank("Tinkoff", 0.01m, 100, -100000, deposintConditions, 100);

        IBuilder builder = new ClientBuilder(bank1, "Den", "Milik");
        ClientAccount client1 = builder.CreateAndGetClient();
        bank1.AddClientAccount(client1);

        DebitAccount debit1 = bank1.CreateDebitAccount(client1);
        CreditAccount credit1 = bank1.CreateCreditAccount(client1);

        bank1.AddMoney(debit1.Id, 1000);
        Assert.Throws<BanksException>(() => bank1.WithdrawMoney(debit1.Id, 500));
    }

    [Fact]
    public void Subscribe_MessageHandlerRecivesNotifications()
    {
        DepositInterestRateConditions deposintConditions = new DepositInterestRateConditions(0.01m);
        Bank bank1 = centralBank.CreateBank("Tinkoff", 0.01m, 100, -100000, deposintConditions, 100);

        IBuilder builder = new ClientBuilder(bank1, "Den", "Milik");
        ClientAccount client1 = builder.CreateAndGetClient();
        bank1.AddClientAccount(client1);

        DebitAccount debit1 = bank1.CreateDebitAccount(client1);
        CreditAccount credit1 = bank1.CreateCreditAccount(client1);

        bank1.AddMoney(debit1.Id, 1000);

        InMemoryMessageHandler handler = new InMemoryMessageHandler();
        bank1.Subscribe(client1, handler, ObservablesNames.DebitInterestRate);
        bank1.ChangeDebitInterestRate(0.048m);

        Assert.Equal("Debit interest rate was changed to 0.048!", handler.Messages.ElementAt(0));
    }
}