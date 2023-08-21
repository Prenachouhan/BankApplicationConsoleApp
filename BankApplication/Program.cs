using BankApplication.DBContext;
using BankApplication.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

internal class Program
{
    private static void Main(string[] args)
    {
        //Base method which ask user to enter any input
        BaseMethod();

    }

    private static void BaseMethod()
    {
        Console.WriteLine("Welcome to AwesomeGIC Bank! What would you like to do?");
        Console.WriteLine("[I]nput transactions ");
        Console.WriteLine("[D]efine interest rules");
        Console.WriteLine("[P]rint statement");
        Console.WriteLine("[Q]uit");

        string userInput = Console.ReadLine();

        switch (userInput.ToLower())
        {
            case "i": ProcessInput(); break;
            case "d": ProcessInterestRule(); break;
            case "p": ProcessPrintStatement(); break;
            case "q": ProcessQuit(); break;
            default: Console.WriteLine("Please enter a valid input"); Console.ReadKey(); break;
        }
    }

    private static void ProcessInput()
    
    
    {
        Console.WriteLine("Please enter transaction details in <Date>|<Account>|<Type>|<Amount> format \r\n(or enter blank to go back to main menu):");
        var userInputEntry = Console.ReadLine();
        try
        {
            if (userInputEntry != null)
            {
                var transactionDate = userInputEntry.Split("|")[0];
                var transactionAccount = userInputEntry.Split("|")[1];
                var transactionType = userInputEntry.Split("|")[2];
                var transactionAmount = userInputEntry.Split("|")[3];

                if (transactionDate != null && ValidateDate(transactionDate)
                    && (transactionType.ToLower() == "w" || transactionType.ToLower() == "d"))
                {
                    if (double.TryParse(transactionAmount, out double amount))
                    {
                        var _amount = amount;
                        if (_amount >= 0)
                        {
                            var _transactionDate = DateTime.ParseExact(transactionDate,
                                      "yyyyMMdd",
                                       CultureInfo.InvariantCulture);
                            using (var context = new BankDBContext())
                            {
                                var isExistingAccount = context.Account.Where(x => x.AccountInfo == transactionAccount).FirstOrDefault();
                                if (isExistingAccount == null)
                                {
                                    if (transactionType.ToLower() == "d")
                                    {
                                        var account = new Account()
                                        {
                                            AccountInfo = transactionAccount
                                        };
                                        context.Account.Add(account);
                                        context.SaveChanges();


                                        var transaction = new Transaction()
                                        {
                                            TransactionType = transactionType,
                                            Amount = _amount,
                                            TransactionDate = _transactionDate,
                                            AccountId = account.AccountId,
                                            Balance = _amount,
                                            Remarks = "User action"
                                        };
                                        context.Transaction.Add(transaction);
                                        context.SaveChanges();
                                        Console.WriteLine("Transaction Added Successfully");
                                    }
                                    else
                                    {
                                        Console.WriteLine("First transaction on Account cannot be withdrawl");

                                    }
                                }
                                else
                                {
                                    var lastTransactionBalance = context.Transaction.Where(x => x.AccountId == isExistingAccount.AccountId)
                                                        .OrderByDescending(x => x.TransactionId).Select(m => m.Balance).FirstOrDefault();
                                    double? _updateBalance = 0;
                                    if (transactionType.ToLower() == "d")
                                    {
                                        _updateBalance = lastTransactionBalance + _amount;
                                    }
                                    else
                                    {
                                        _updateBalance = lastTransactionBalance - _amount;
                                    }
                                    var transaction = new Transaction()
                                    {
                                        TransactionType = transactionType,
                                        Amount = _amount,
                                        TransactionDate = _transactionDate,
                                        AccountId = isExistingAccount.AccountId,
                                        Balance = _updateBalance,
                                        Remarks = "User action"
                                    };
                                    context.Transaction.Add(transaction);
                                    context.SaveChanges();
                                    Console.WriteLine("Transaction Added Successfully");

                                }

                            }
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Please enter in the correct format");
                BaseMethod();
            }
            Console.ReadKey();
            BaseMethod();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Please enter in the correct format");
            BaseMethod();
        }
    }

    private static bool ValidateDate(string inputDate)
    {
        try
        {
            if (inputDate.Length == 8)
            {
                if (int.TryParse(inputDate.Substring(4, 2), out int number))
                {
                    var month = number;
                    if (month >= 1 && month <= 12)
                    {
                        if (int.TryParse(inputDate.Substring(6, 2), out int dateNumber))
                        {
                            var date = dateNumber;
                            if (date >= 1 && date <= 31)
                                return true;
                            else { return false; }
                        }
                        else { return false; }
                    }
                    else { return false; }
                }
                else { return false; }
            }
            else
            {
                return false;
            }
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    private static void ProcessInterestRule()
    {
        Console.WriteLine("Please enter interesrules details in <Date>|<RuleId>|<Rate in %> format \r\n(or enter blank to go back to main menu):");
        var userInputEntry = Console.ReadLine();
        try
        {
            if (userInputEntry != null)
            {
                var ruleDate = userInputEntry.Split("|")[0];
                var ruleName = userInputEntry.Split("|")[1];
                var interest = userInputEntry.Split("|")[2];

                if (ruleDate != null && ValidateDate(ruleDate))
                {
                    if (double.TryParse(interest, out double rate))
                    {
                        var _rate = rate;
                        if (_rate >= 0 && _rate <= 100)
                        {
                            var _ruleDate = DateTime.ParseExact(ruleDate,
                                      "yyyyMMdd",
                                       CultureInfo.InvariantCulture);
                            using (var context = new BankDBContext())
                            {
                                var rule = new Rules()
                                {
                                    RuleDate = _ruleDate,
                                    RuleName = ruleName,
                                    Interest = _rate
                                };
                                context.Rules.Add(rule);
                                context.SaveChanges();
                                Console.WriteLine("Rule Added Successfully");
                            }
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Please enter in the correct format");
                BaseMethod();
            }

            Console.ReadKey();
            BaseMethod();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Please enter in the correct format");
            BaseMethod();
        }
    }
    private static void ProcessPrintStatement()
    {
        Console.WriteLine("Please enter account and month to generate the statement <Account>|<Month>\r\n(or enter blank to go back to main menu):");
        var userInputEntry = Console.ReadLine();
        try
        {
            if (userInputEntry != null)
            {
                var accountInfo = userInputEntry.Split("|")[0];
                var month = userInputEntry.Split("|")[1];

                if (month != null && month.Length == 2)
                {
                    if (int.TryParse(month, out int mnth))
                    {
                        var _month = mnth;
                        if (_month >= 0 && _month <= 12)
                        {
                            using (var context = new BankDBContext())
                            {
                                int accountId = context.Account.Where(x => x.AccountInfo == accountInfo).Select(y => y.AccountId).FirstOrDefault();

                                var result = (from tr in context.Transaction
                                              join ac in context.Account on tr.AccountId equals ac.AccountId
                                              where tr.TransactionDate.Month == _month && ac.AccountInfo == accountInfo
                                              select new
                                              {
                                                  tr.TransactionDate,
                                                  tr.TransactionId,
                                                  tr.TransactionType,
                                                  tr.Balance,
                                                  tr.Remarks
                                              }).ToList();
                                if (result != null && !result.Any(x => x.Remarks == "Interest Credit"))
                                {
                                    double? sum = 0;
                                    var rules = context.Rules.Where(x => x.RuleDate.Month <= _month).ToList();
                                    var isMultipleRule = rules.Where(x => x.RuleDate.Month == _month).ToList();
                                    if (isMultipleRule.Count() > 0)
                                    {
                                        var previousRule = rules.Where(x => x.RuleDate.Month < _month).OrderBy(x => x.RuleDate).FirstOrDefault();
                                        isMultipleRule.Insert(0, previousRule);

                                        int rulesLength = isMultipleRule.Count();
                                        for (int i = 0; i < rulesLength; i++)
                                        {

                                            DateTime previousTransactionDate = DateTime.Now;

                                            if (i != 0)
                                            {
                                                previousTransactionDate = isMultipleRule[i - 1].RuleDate == null ? DateTime.Now : isMultipleRule[i - 1].RuleDate;
                                            }
                                            var transactions = result.Where(x => x.TransactionDate <= isMultipleRule[i].RuleDate && x.TransactionDate >= previousTransactionDate && x.TransactionDate.Month == _month).ToList();

                                            foreach (var transaction in transactions)
                                            {
                                                var interest = (transaction.Balance * (isMultipleRule[i].Interest / 100) * 100) / 365;
                                                sum += interest;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        int previoiusDate = 0;
                                        var rule = rules.OrderByDescending(x => x.RuleDate).FirstOrDefault();
                                        foreach (var item in result.OrderBy(x => x.TransactionDate))
                                        {
                                            var date = Convert.ToInt32(item.TransactionDate.ToString("dd"));
                                            var interest = (item.Balance * (rule.Interest / 100) * (date - previoiusDate)) / 365;
                                            sum += interest;
                                        }
                                    }
                                    int days = DateTime.DaysInMonth(2023, _month);
                                    DateTime _transactionDate = new DateTime(2023, _month, days);
                                    var interestTransaction = new Transaction()
                                    {
                                        TransactionType = "D",
                                        Amount = Math.Round((double)sum, 2),
                                        TransactionDate = _transactionDate,
                                        AccountId = accountId,
                                        Remarks = "Interest Credit"
                                    };
                                    context.Transaction.Add(interestTransaction);
                                    context.SaveChanges();

                                }
                                    var finalResult = (from tr in context.Transaction
                                                       join ac in context.Account on tr.AccountId equals ac.AccountId
                                                       where tr.TransactionDate.Month == _month && ac.AccountInfo == accountInfo
                                                       select new
                                                       {
                                                           tr.TransactionDate,
                                                           tr.TransactionId,
                                                           tr.TransactionType,
                                                           tr.Amount
                                                       }).ToList();

                                    Console.WriteLine("Date | TransactionID | Type | Amount ");
                                    foreach (var item in finalResult)
                                    {
                                        Console.WriteLine(item.TransactionDate.ToShortDateString() + " | " + item.TransactionId + " | " + item.TransactionType + " | " + item.Amount);
                                    }
                            }
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Please enter in the correct format");
                BaseMethod();
            }

            Console.ReadKey();
            BaseMethod();
        }
        catch (Exception ex)
        {
            Console.WriteLine("Please enter in the correct format");
            BaseMethod();
        }
    }


    private static void ProcessQuit()
    {
        Console.WriteLine("Thank you for banking with AwesomeGIC Bank.\r\nHave a nice day!");
        Console.ReadKey();
        Environment.Exit(0);
    }
}