using System.Reflection.Metadata.Ecma335;
using Banks.Accounts;
using Banks.Client;
using Banks.Exceptions;
using Banks.Time;
using Banks.Transactions;

namespace Banks.Banks;

// This class has to be a singletone, so we need to use static here.
public class CentralBank
{
    private const int MinId = 0;

    private static CentralBank _instance = new CentralBank();

    private int _bankId = MinId;
    private List<Bank> _banksList = new List<Bank>();

    private CentralBank() { }

    public IReadOnlyCollection<Bank> BanksList => _banksList;

    public static CentralBank GetInstance()
    {
        if (_instance is null)
            _instance = new CentralBank();

        return _instance;
    }

    public Bank? FindBankById(int bankId) => _banksList.SingleOrDefault(bank => bank.Id == bankId);

    public Bank GetBankById(int bankId) => _banksList.Single(bank => bank.Id == bankId);

    public IAccount? FindAccountById(int bankId, int accountId)
    {
        Bank? bank = FindBankById(bankId);
        if (bank is null)
            return null;
        return bank.FindAccountById(accountId);
    }

    public IAccount GetAccountById(int bankId, int accountId)
    {
        Bank bank = GetBankById(bankId);
        return bank.GetAccountById(accountId);
    }

    public IReadOnlyCollection<ClientAccount> GetClientAccounts()
    {
        var list = new List<ClientAccount>();
        foreach (var bank in _banksList)
        {
            list.AddRange(bank.ClientAccountsList);
        }

        return list;
    }

    public IReadOnlyCollection<IAccount> GetAccounts()
    {
        var list = new List<IAccount>();
        foreach (var bank in _banksList)
        {
            list.AddRange(bank.AccountsList);
        }

        return list;
    }

    public Bank CreateBank(string name, decimal debitInterestRate, decimal commission, decimal creditLimit, DepositInterestRateConditions depositCoditions, decimal suspiciousClientMoneyLimit)
    {
        var newBank = new Bank(_bankId, name, debitInterestRate, commission, creditLimit, depositCoditions, suspiciousClientMoneyLimit);

        _banksList.Add(newBank);
        _bankId++;

        return newBank;
    }

    public void NotifyBanksToAccumulateInterestRate()
    {
        foreach (var bank in _banksList)
        {
            bank.AccumulateInterestRate();
        }
    }

    public void TransferMoneyBetweenBanks(IMoneyTransaction transactionFrom)
    {
        if (transactionFrom is null)
            throw new BanksException("Null transactionFrom while creating Bank to Bank transfer.");
        if (!(transactionFrom is TransferTransaction))
            throw new BanksException("Wrong transaction while creating Bank to Bank transfer.");

        IAccount accountFrom = ((TransferTransaction)transactionFrom).AccountFrom;
        if (!accountFrom.CanWithdrawMoney(transactionFrom.TransactionConditions.AmountOfMoney))
        {
            throw new BanksException("Can't withdraw money, not enought money in account.");
        }

        transactionFrom.CreateTransaction();
        Bank bankTo = ((TransferTransaction)transactionFrom).AccountTo.ClientAccount.Bank;
        bankTo.AddTransaction(transactionFrom);
        Bank bankFrom = ((TransferTransaction)transactionFrom).AccountFrom.ClientAccount.Bank;
        bankFrom.AddTransaction(transactionFrom);
    }

    public void RollBackMoneyBetweenBanks(Guid transactionToRollbackId, RollBackTransaction rollbackTransaction)
    {
        if (rollbackTransaction is null)
            throw new BanksException("Null rollbackTransaction while creating Bank to Bank Rollback.");

        if (rollbackTransaction.Transaction is RollBackTransaction)
            throw new BanksException("Not BankToBank transaction rollback while creating Bank to Bank Rollback.");

        Bank bankTo = ((TransferTransaction)rollbackTransaction.Transaction).AccountTo.ClientAccount.Bank;
        Bank bankFrom = ((TransferTransaction)rollbackTransaction.Transaction).AccountFrom.ClientAccount.Bank;

        if (!bankTo.HasTransactionById(transactionToRollbackId))
            throw new BanksException("No transaction to RollBack in BankTo.");

        if (!bankFrom.HasTransactionById(transactionToRollbackId))
            throw new BanksException("No transaction to RollBack in BankFrom.");

        rollbackTransaction.CreateTransaction();
        bankTo.AddTransaction(rollbackTransaction);
        bankFrom.AddTransaction(rollbackTransaction);
    }

    public void SpeedUpTime(int days)
    {
        OwnTime time = OwnTime.GetInstance();
        for (int i = 0; i < days; i++)
        {
            time.SpeedUp();
            NotifyBanksToAccumulateInterestRate();
        }
    }
}