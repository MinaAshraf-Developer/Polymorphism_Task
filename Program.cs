using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_polumorphism
{
    public class Account
    {
        public string Name { get; set; }
        public double Balance { get; set; }

        public Account(string name = "Unnamed Account", double balance = 0.0)
        {
            this.Name = name;
            this.Balance = balance;
        }

        public bool Deposit(double amount)
        {
            if (amount < 0)
                return false;
            else
            {
                Balance += amount;
                return true;
            }
        }

        public bool Withdraw(double amount)
        {
            if (Balance - amount >= 0)
            {
                Balance -= amount;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    class SavingsAccount : Account
    {
        double interestRate { get; set; }

        public SavingsAccount(double interestRate = 0.05, string name = "Unnamed Account", double balance = 0.0) : base(name, balance)
        {
            this.interestRate = interestRate;
        }

        public void CalcInterestRate()
        {
            Balance += Balance * interestRate;
        }
    }

    class Checking_Account : Account
    {
        private const double WithdrawalFee = 1.50;
        public Checking_Account(string name, double balance) :base(name, balance)
        {
            
        }

        public new bool withdraw(double amount)
        {
            if (Balance - amount - WithdrawalFee > 0)
            {
                Balance -= (amount + WithdrawalFee);
                return true;
            }
            return false;
        }
    }

    class TrustAccount : SavingsAccount
    {
        private int withdrawalCount;
        private const int MaxWithdrawalsPerYear = 3;
        private DateTime firstWithdrawalDate;
        public TrustAccount(string name, double balance, double interestRate) : base(balance, name, interestRate) 
        {
            withdrawalCount = 0;
            firstWithdrawalDate = DateTime.MinValue;
        }

        public new bool Deposit(double amount)
        {
            if (amount < 0)
                return false;
            else if(amount >= 5000)
            {
                Balance += (amount+50);
                return true;
            }
            else
            {
                Balance += amount;
                return true;
            }
        }

        public new bool Withdraw(double amount)
        {
            if(withdrawalCount  >= 3)
            {
                return false;
            }
            else if(amount >= (0.2 * Balance))
            {
                return false;
            }
            if (withdrawalCount == 1)
            {
                firstWithdrawalDate = DateTime.Now;
            }
            if ((DateTime.Now - firstWithdrawalDate).TotalDays > 365)
            {
                return false;
            }

              withdrawalCount ++;
             Balance -= amount;
             return true;

           
            
        }
    }

    public static class AccountUtil
    {
       // Generic Deposit Way
        public static void Deposit<T>(List<T> accounts, double amount) where T : Account
        {
            Console.WriteLine("\n=== Depositing to Accounts =================================");
            foreach (var acc in accounts)
            {
                if (acc.Deposit(amount))
                    Console.WriteLine($"Deposited {amount} to {acc}");
                else
                    Console.WriteLine($"Failed Deposit of {amount} to {acc}");
            }
        }

        // Generic Withdraw Way
        public static void Withdraw<T>(List<T> accounts, double amount) where T : Account
        {
            Console.WriteLine("\n=== Withdrawing from Accounts ==============================");
            foreach (var acc in accounts)
            {
                if (acc.Withdraw(amount))
                    Console.WriteLine($"Withdrew {amount} from {acc}");
                else
                    Console.WriteLine($"Failed Withdrawal of {amount} from {acc}");
            }
        }


        // Polymorphic Deposit Way
        public static void Deposit(List<Account> accounts, double amount)
        {
            Console.WriteLine("\n=== Depositing to Accounts =================================");
            foreach (var acc in accounts)
            {
                if (acc.Deposit(amount))
                    Console.WriteLine($"Deposited {amount} to {acc.Name}");
                else
                    Console.WriteLine($"Failed Deposit of {amount} to {acc.Name}");
            }
        }




        // Polymorphic Withdraw Way
        public static void Withdraw(List<Account> accounts, double amount)
        {
            Console.WriteLine("\n=== Withdrawing from Accounts ==============================");
            foreach (var acc in accounts)
            {
                if (acc.Withdraw(amount))
                    Console.WriteLine($"Withdrew {amount} from {acc.Name}");
                else
                    Console.WriteLine($"Failed Withdrawal of {amount} from {acc.Name}");
            }
        }

    }
    internal class Program
    {
        static void Main(string[] args)
        {
            // Accounts
            var accounts = new List<Account>();
            accounts.Add(new Account());
            accounts.Add(new Account("Larry"));
            accounts.Add(new Account("Moe", 2000));
            accounts.Add(new Account("Curly", 5000));

            AccountUtil.Deposit(accounts, 1000);
            AccountUtil.Withdraw(accounts, 2000);

            // Savings
            var savAccounts = new List<Account>();
            savAccounts.Add(new SavingsAccount());
            savAccounts.Add(new SavingsAccount("Superman"));
            savAccounts.Add(new SavingsAccount("Batman", 2000));
            savAccounts.Add(new SavingsAccount("Wonderwoman", 5000, 5.0));

            AccountUtil.DepositSavings(savAccounts, 1000);
            AccountUtil.WithdrawSavings(savAccounts, 2000);

            // Checking
            var checAccounts = new List<Account>();
            checAccounts.Add(new CheckingAccount());
            checAccounts.Add(new CheckingAccount("Larry2"));
            checAccounts.Add(new CheckingAccount("Moe2", 2000));
            checAccounts.Add(new CheckingAccount("Curly2", 5000));

            AccountUtil.DepositChecking(checAccounts, 1000);
            AccountUtil.WithdrawChecking(checAccounts, 2000);
            AccountUtil.WithdrawChecking(checAccounts, 2000);

            // Trust
            var trustAccounts = new List<Account>();
            trustAccounts.Add(new TrustAccount());
            trustAccounts.Add(new TrustAccount("Superman2"));
            trustAccounts.Add(new TrustAccount("Batman2", 2000));
            trustAccounts.Add(new TrustAccount("Wonderwoman2", 5000, 5.0));

            AccountUtil.DepositTrust(trustAccounts, 1000);
            AccountUtil.DepositTrust(trustAccounts, 6000);
            AccountUtil.WithdrawTrust(trustAccounts, 2000);
            AccountUtil.WithdrawTrust(trustAccounts, 3000);
            AccountUtil.WithdrawTrust(trustAccounts, 500);
        }
    }
}
