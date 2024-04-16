using Banks.Accounts;
using Banks.Banks;
using Banks.Client;
using Banks.Console.Handler;
using Banks.MessageHandlers;
using Banks.Time;
using Banks.Transactions;

OwnTime.GetInstance();
CentralBank.GetInstance();
System.Console.WriteLine("List of commands available:");
System.Console.WriteLine("-Help");
System.Console.WriteLine("-Create client");
System.Console.WriteLine("-Create account");
System.Console.WriteLine("-Set address");
System.Console.WriteLine("-Set passport");
System.Console.WriteLine("-Change debit interest rate");
System.Console.WriteLine("-Change deposit interest rate conditions");
System.Console.WriteLine("-Change credit commission");
System.Console.WriteLine("-Change credit limit");
System.Console.WriteLine("-Change deposit interest rate conditions");
System.Console.WriteLine("-Show accounts of client");
System.Console.WriteLine("-Show banks");
System.Console.WriteLine("-Show accounts");
System.Console.WriteLine("-Subscribe to notifications");
System.Console.WriteLine("-Speed up time");
System.Console.WriteLine("-Add");
System.Console.WriteLine("-Withdraw");
System.Console.WriteLine("-Transfer");
System.Console.WriteLine("-Rollback");

BankCreationHandler bankCreationHandler = new BankCreationHandler();
ClientCreationHandler clientCreationHandler = new ClientCreationHandler();
AccountCreationHandler accountCreationHandler = new AccountCreationHandler();
AdressAddingHandler adressAddingHandler = new AdressAddingHandler();
PassportAddingHandler passportAddingHandler = new PassportAddingHandler();
ChangeDebitInterestRateHandler changeDebitInterestRateHandler = new ChangeDebitInterestRateHandler();
ChangeDepositInterestRateHandler changeDepositInterestRateHandler = new ChangeDepositInterestRateHandler(); // l
ChangeCreditCommissionHandler changeCreditCommissionHandler = new ChangeCreditCommissionHandler();
ChangeCreditLimitHandler changeCreditLimitHandler = new ChangeCreditLimitHandler();
ChangeSuspiciousClientLimit changeSuspiciousClientLimit = new ChangeSuspiciousClientLimit();
AddMoneyHandler addMoneyHandler = new AddMoneyHandler();
WithdrawHandler withdrawHandler = new WithdrawHandler();
TransferHandler transferHandler = new TransferHandler();
RollBackHandler rollBackHandler = new RollBackHandler();
ShowAccountsHandler showAccountsHandler = new ShowAccountsHandler();
ShowCientsHandler showCientsHandler = new ShowCientsHandler();
ShowBanksHendler showBanksHendler = new ShowBanksHendler();
SpeedUpTimeHandler speedUpTimeHandler = new SpeedUpTimeHandler();
SubscribeHandler subscribeHandler = new SubscribeHandler();
ShowHelpHandler showHelpHandler = new ShowHelpHandler();

bankCreationHandler.SetNext(clientCreationHandler).SetNext(accountCreationHandler).SetNext(adressAddingHandler);
adressAddingHandler.SetNext(passportAddingHandler).SetNext(changeDebitInterestRateHandler).SetNext(changeDepositInterestRateHandler);
changeDepositInterestRateHandler.SetNext(changeCreditCommissionHandler).SetNext(changeCreditLimitHandler);
changeCreditLimitHandler.SetNext(changeSuspiciousClientLimit).SetNext(addMoneyHandler).SetNext(withdrawHandler);
withdrawHandler.SetNext(transferHandler).SetNext(rollBackHandler).SetNext(showAccountsHandler).SetNext(showCientsHandler);
showCientsHandler.SetNext(showBanksHendler).SetNext(speedUpTimeHandler).SetNext(subscribeHandler).SetNext(showHelpHandler);

string? requsetOrNull;
string request = string.Empty;

while (!request.Equals("exit"))
{
    requsetOrNull = Console.ReadLine();
    request = (requsetOrNull is not null) ? requsetOrNull : string.Empty;
    bankCreationHandler.Handle(request);
}