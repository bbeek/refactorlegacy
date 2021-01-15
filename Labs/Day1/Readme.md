# Refactoring "TheatricalPlays"
Background: 

This project contains a bill generator for a theatrical company.
This company charges their customers based on the size of the audience and the type of play.
Currently they are using a .NET 5 console application to generate a plain text bill.

Beside providing a bill, it also outputs the customers "volume credits", which is used to calculate a discount for repeat customers.
Every 10 credits earns a customer 1 percentage point up to a maximum discount of 30% of the total bill.

The plays this company can perform are stored in a simple JSON file.
In another JSON file are the performances stored that were performed for a customer containing the playid and the audience.

## Target Environment
* .NET Core 5.0
* This lab expects the default keybindings. If you're using a different keymap, please look up the keyboard shortcuts used in this lab.

### Build
* Visual Studio 2019 v16.8+ 

# Goal
The company wants to perform more types of plays. 
They hope to perform: historical, pastoral, pastoral-comical, historical-pastoral, tragical-comical-historical-pastoral and poem unlimited
Although the pricing structure and the volume credit calculations are not fully worked out, it appears that this will be under subject of change in the near future.

Furthermore, the company wants to send out HTML formatted statements to some customers.

In order to do so, you would either have to copy-paste the current `BillGenerator.Statement` implementation into a HTML statement generator with all code duplicated.

Or, first refactor the current implementation to a improved design more suitable for the upcoming features/requests.

In this lab we are going to perform the steps needed for the latter (of course :))

# First steps
As a first step, let's begin by extracting functionality from the current long `Statement` function.
The `switch` statement related to calculating the charge of a performance is a good start as it is performs 1 thing that we can return in a method.

So we are going to perform a "Extract Method" on this statement.
Select line 22 till 40 in `BillGenerator.cs` and apply the "Extract method" refactoring from Visual Studio (either via Edit > Refactor > Extract Method or using the Ctrl+. "quick actions" > Extract )
And name the newly extracted function `AmountFor`. (We can always rename it later to a more suitable name).
We finish the first refactoring by running our unittests to verify that we did not break anything and committing the first change

Next, apply the "Rename variable" refactoring from Visual Studio on the `thisAmount` variable in the `AmountFor` method and rename it to `result`
Again compile-test-commit.

Apply the same "Rename variable" refactoring on the `perf` argument of `AmountFor`. Rename it to `performance`, test and commit.

