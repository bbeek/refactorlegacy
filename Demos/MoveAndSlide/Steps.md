Demo slide statements, extract method and move function.

Objective: move the overdraft calculation to AccountType.

First, let's ensure we can extract the overdraft calculation by using the slide statement refactoring.

We begin by move base charge of 4.5m from line 45 to the initial result declaration and Test!
Then merge declaration and assignment into one line. This makes the else part from line 55 - 59 obsolete. So let's remove that and test!

Next, we slide line 42 to the beginning of the function.

Due to the modification of the `result` variable on line 48 and line 52, we can not yet cleanly extract the method for overdraft calculation.
So let's introduce a new temporary overdraft variable on line 42: `var overdraftSurcharge = 0m;`
and let's add this overdraft to the result outside of the if statement on line 56: `result += overdraftSurcharge;` and test!
Then change the assignment from `result` to `overdraftSurcharge` on line 48 and test.
Repeat for line 52.

Now we can cleanly extract the `GetOverdraftCharge` method using the Extract function feature.

After extracting the `GetOverdraftCharge`, let's remove the temporary variable `overdraftSurcharge` using inline temporary variable

Next moving functions:

3. Copy the `GetOverdraftCharge` function to AccountType class. Make it accessible by changing to a public method.
Make the copied method fit it's new context by removing the references to account type and adding a new DaysOverdrawn parameter.
Adding the last parameter with improper capitalization allows us to refactor to daysOverdrawn easily using the Rename functionality.

4. Call the target class in the original source function. 
We can now replace the body of GetOverdraftCharge in Account.cs with a call to AccountType.GetOverdraftCharge, with the correct parameters.
This results in:
```
        private decimal GetOverdraftCharge()
        {
            return accountType.GetOverdraftCharge(DaysOverdrawn);
        }
```

5. Test!
6. Consider inlining, by moving the `accountType.GetOverdraftCharge(DaysOverdrawn);` to line 32.