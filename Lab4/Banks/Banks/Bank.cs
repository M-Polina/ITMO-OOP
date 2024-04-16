using System.Transactions;
using Banks.Accounts;
using Banks.Client;
using Banks.Exceptions;
using Banks.MessageHandlers;
using Banks.Transactions;

namespace Banks.Banks;

public class Bank
{
    private const int MinId = 0;

    private int _accountId = MinId;
    private List<IAccount> _accountsList = new List<IAccount>();
    private List<ClientAccount> _clientAccountsList = new List<ClientAccount>();
    private List<IMoneyTransaction> _transactionsList = new List<IMoneyTransaction>();

    public Bank(int id, string name, decimal debitInterestRate, decimal commission, decimal creditLimit, DepositInterestRateConditions depositCoditions, decimal suspiciousClientMoneyLimit)
    {
        if (id < MinId)
            throw new BanksException("Id <0 while creating Bank.");
        if (string.IsNullOrWhiteSpace(name))
            throw new BanksException("Name is incorrect while creating Bank.");

        Id = id;
        Name = name;
        Conditions = new BankConditions(debitInterestRate, commission, creditLimit, depositCoditions, suspiciousClientMoneyLimit);
    }

    public int Id { get; }
    public string Name { get; }
    public BankConditions Conditions { get; private set; }

    public IReadOnlyCollection<IAccount> AccountsList => _accountsList;
    public List<IMoneyTransaction> TransactionsList => _transactionsList;
    public IReadOnlyCollection<ClientAccount> ClientAccountsList => _clientAccountsList;
    public IMoneyTransaction LastTransaction => _transactionsList.Last();
    public Guid LastTransactionId => LastTransaction.Id;

    public ClientAccount? FindClientAccountById(Guid clientAccountId) =>
        _clientAccountsList.SingleOrDefault(acc => acc.Id == clientAccountId);

    public ClientAccount GetClientAccountById(Guid clientAccountId) =>
        _clientAccountsList.Single(acc => acc.Id == clientAccountId);

    public IAccount? FindAccountById(int accountId) => _accountsList.SingleOrDefault(acc => acc.Id == accountId);

    public IAccount GetAccountById(int accountId) => _accountsList.Single(acc => acc.Id == accountId);

    public IMoneyTransaction GetTransactionById(Guid transactionId) =>
        _transactionsList.Single(tr => tr.Id == transactionId);

    public IMoneyTransaction? FindTransactionById(Guid transactionId) =>
        _transactionsList.Single(tr => tr.Id == transactionId);

    public IMoneyTransaction? FindTransactionRollBackById(Guid transactionId)
    {
        foreach (IMoneyTransaction tr in _transactionsList)
        {
            if (tr is RollBackTransaction)
            {
                if (((RollBackTransaction)tr).Transaction.Id == transactionId)
                    return tr;
            }
        }

        return null;
    }

    public bool ClientPassportExists(int id)
    {
        foreach (var cl in _clientAccountsList)
        {
            if (cl.Passport is not null)
            {
                if (cl.Passport.Id == id)
                    return true;
            }
        }

        return false;
    }

    public bool HasTransactionById(Guid transactionId)
    {
        var foundTransaction = FindTransactionById(transactionId);
        if (foundTransaction is null)
            return false;
        return true;
    }

    public bool HasTransactionRollBackById(Guid transactionId)
    {
        var foundTransaction = FindTransactionRollBackById(transactionId);
        if (foundTransaction is null)
            return false;
        return true;
    }

    public bool HasClient(ClientAccount client)
    {
        if (client is null)
            return false;
        if (client.Bank.Id != Id)
            return false;
        return true;
    }

    public bool HasAccount(IAccount account)
    {
        if (account is null)
            return false;
        if (account.ClientAccount.Bank.Id != Id)
            return false;
        return true;
    }

    public void ChangeDebitInterestRate(decimal newRate)
    {
        Conditions.ChangeDebitInterestRate(newRate);
        ChangeAllAccountsConditions();
    }

    public void ChangeCreditComission(decimal commission)
    {
        Conditions.ChangeCreditComission(commission);
        ChangeAllAccountsConditions();
    }

    public void ChangeDepositConditions(DepositInterestRateConditions coditions)
    {
        Conditions.ChangeDepositConditions(coditions);
        ChangeAllAccountsConditions();
    }

    public void ChangeCreditLimit(decimal creditLimit)
    {
        Conditions.ChangeCreditLimit(creditLimit);
        ChangeAllAccountsConditions();
    }

    public void ChangeSuspiciousClientMoneyLimit(decimal newLimit)
    {
        Conditions.ChangeSuspiciousClientMoneyLimit(newLimit);
        ChangeAllAccountsConditions();
    }

    public void AddClientAccount(ClientAccount client)
    {
        if (client is null)
            throw new BanksException("Null client while adding it to bank");
        _clientAccountsList.Add(client);
    }

    public DebitAccount CreateDebitAccount(ClientAccount client)
    {
        if (client is null)
        {
            throw new BanksException("No such client, so can't create Debit account");
        }

        if (client.Bank.Id != Id)
        {
            throw new BanksException("No such client, so can't create Debit account");
        }

        var newAccount = new DebitAccount(_accountId, client);

        client.AddAccount(newAccount);
        _accountsList.Add(newAccount);
        _accountId++;

        return newAccount;
    }

    public DepositAccount CreateDepositAccount(ClientAccount client, decimal amountOfMoney, int months)
    {
        if (client is null)
            throw new BanksException("No such client, so can't create Deposit account");

        if (client.Bank.Id != Id)
            throw new BanksException("No such client, so can't create Deposit account");

        var newAccount = new DepositAccount(_accountId, client, amountOfMoney, months);

        client.AddAccount(newAccount);
        _accountsList.Add(newAccount);
        _accountId++;

        return newAccount;
    }

    public CreditAccount CreateCreditAccount(ClientAccount client)
    {
        if (client is null)
            throw new BanksException("No such client, so can't create Credit account");
        if (client.Bank.Id != Id)
            throw new BanksException("No such client, so can't create Credit account");

        var newAccount = new CreditAccount(_accountId, client);
        client.AddAccount(newAccount);
        _accountsList.Add(newAccount);
        _accountId++;

        return newAccount;
    }

    public void AddClientAdress(ClientAccount account, string address)
    {
        if (account is null)
            throw new BanksException("Account is null while adding adress.");

        if (!HasClient(account))
            throw new BanksException("Account not in bank while adding Address.");

        account.SetAddress(new Address(address));
    }

    public void AddClientPassport(ClientAccount account, int passportId)
    {
        if (account is null)
            throw new BanksException("Account is null while adding passport.");

        if (!HasClient(account))
            throw new BanksException("Account not in bank while adding Passport.");

        account.SetPassport(new Passport(passportId));
    }

    public void AddMoney(int accountId, decimal amount)
    {
        var account = GetAccountById(accountId);
        var transaction = new AddTransaction(amount, account);
        transaction.CreateTransaction();

        _transactionsList.Add(transaction);
    }

    public void WithdrawMoney(int accountId, decimal amount)
    {
        var account = GetAccountById(accountId);
        var transaction = new WithdrawTransaction(amount, account);
        transaction.CreateTransaction();

        _transactionsList.Add(transaction);
    }

    public void TransferMoney(IAccount accountFrom, IAccount accountTo, decimal amount)
    {
        if (!HasAccount(accountFrom))
            throw new BanksException("Can't tansfer money, as Account is not in Bank.");
        if (HasAccount(accountFrom))
        {
            if (!accountFrom.CanWithdrawMoney(amount))
                throw new BanksException("Can't withdraw money, not enought money in account.");

            var transaction = new TransferTransaction(amount, accountFrom, accountTo);

            if (!HasAccount(accountTo))
            {
                CentralBank.GetInstance().TransferMoneyBetweenBanks(transaction);
            }
            else
            {
                transaction.CreateTransaction();
                _transactionsList.Add(transaction);
            }
        }
    }

    public void AddTransaction(IMoneyTransaction transaction)
    {
        if (transaction is null)
            throw new BanksException("Null transactian while adding it to Bank.");

        _transactionsList.Add(transaction);
    }

    public void RollbackTrabsaction(Guid transactionId)
    {
        IMoneyTransaction transaction = GetTransactionById(transactionId);

        if (transaction is RollBackTransaction)
            throw new BanksException("Can't Rollback Rollbacks.");

        if (HasTransactionRollBackById(transactionId))
            throw new BanksException("Rollback already exist, can't creat more.");

        RollBackTransaction rollbackTransaction = new RollBackTransaction(transaction);
        if (IsTransactionBetweenBanks(transactionId))
        {
            CentralBank.GetInstance().RollBackMoneyBetweenBanks(transactionId, rollbackTransaction);
        }
        else
        {
            rollbackTransaction.CreateTransaction();
            _transactionsList.Add(rollbackTransaction);
        }
    }

    public void AccumulateInterestRate()
    {
        foreach (var accaunt in _accountsList)
        {
            accaunt.AccumulateInterestRate();
        }
    }

    public void Subscribe(ClientAccount client, IMessageHandler handler, ObservablesNames observableName)
    {
        if (handler is null)
            throw new BanksException("Null handler while subscribing in bank.");

        if (client is null)
            throw new BanksException("No such client to subscribe.");
        if (client.Bank.Id != Id)
            throw new BanksException("No such client to subscribe.");

        IObservable observable;

        switch (observableName)
        {
            case ObservablesNames.CreditCommission:
                observable = Conditions.ComissionClass;
                break;
            case ObservablesNames.CreditLimit:
                observable = Conditions.CreditLimitClass;
                break;
            case ObservablesNames.DepositConditions:
                observable = Conditions.DepositConditionsClass;
                break;
            case ObservablesNames.DebitInterestRate:
                observable = Conditions.DebitInterestRateClass;
                break;
            case ObservablesNames.SuspiciousClientLimit:
                observable = Conditions.SuspiciousClientMoneyLimitClass;
                break;
            default:
                throw new BanksException("Wrong ObservablesName enum.");
        }

        client.AddHandler(handler);
        observable.AddSubscriber(client);
    }

    private bool IsTransactionBetweenBanks(Guid transactionId)
    {
        IMoneyTransaction? foundTransaction = FindTransactionById(transactionId);

        if (foundTransaction is null)
            throw new BanksException("Null transaction");

        if (!(foundTransaction is TransferTransaction))
        {
            return false;
        }

        return true;
    }

    private void ChangeAllAccountsConditions()
    {
        foreach (var account in _accountsList)
        {
            account.ChangeConditions();
        }
    }
}