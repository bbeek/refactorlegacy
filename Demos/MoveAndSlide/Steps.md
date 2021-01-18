Demo move function.

Objective: move the overdraft calculation to AccountType.

1. Copy the `GetOverdraftCharge` function to AccountType class. Make it accessible by changing to a public method.
Make the copied method fit it's new context by removing the references to account type and adding a new DaysOverdrawn parameter.
Adding the last parameter with improper capitalization allows us to refactor to daysOverdrawn easily using the Rename functionality.

2. Call the target class in the original source function. 
We can now replace the body of GetOverdraftCharge in Account.cs with a call to AccountType.GetOverdraftCharge, with the correct parameters.
This results in:
```
        private decimal GetOverdraftCharge()
        {
            return accountType.GetOverdraftCharge(DaysOverdrawn);
        }
```

3. Test!
4. Consider inlining, by moving the `accountType.GetOverdraftCharge(DaysOverdrawn);` to line 32.