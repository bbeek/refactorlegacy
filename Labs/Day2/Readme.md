# Lift pass pricing

This application solves the problem of calculating the pricing for ski lift passes.
There's some intricate logic linked to what kind of lift pass you want, your age
and the specific date at which you'd like to ski. 

There's a new feature request, be able to get the price for several lift passes, not just one. 
Currently the pricing for a single lift pass is implemented, unfortunately the code as it is designed
is ***not reusable***.
You could put some high level tests in place in order to do ***preparatory refactoring***
so that the new feature requires minimum effort to implement.

## Installation


## Steps

1. Breaking dependencies
- 	Extract and encapsulate SQL queries
	### Extract `connection`
	The `connection` instance variable needs to be broken in order to get the code into a testharness.

	See if you can extract the code for creating the `connection`.
	As the current state does not bring up the **Extract method** option in the quick actions menu, 
	start with *extracting* the `SqlConnection` creation by changing `connection` into a local variable by adding `var` keyword.
	```
	public PricesController()
    {
        var connection = new SqlConnection(@"Database=lift_pass;Data Source=localhost;Trusted_Connection=true");
        connection.Open();
    }
	```

	And after the `Open` statement, assign the local variable `connection` to the instance variable `connection`.
	```
	public PricesController()
        {
            var connection = new SqlConnection(@"Database=lift_pass;Data Source=localhost;Trusted_Connection=true");
            connection.Open();
            this.connection = connection;
        }
	```
	Now apply the **Extract method** refactoring to extract a `GetConnection` method that returns a `SqlConnection` object.
	
	Then, replace all usages of `connection` with `GetConnection` and remove the now useless instance variable `connection`.
	> Hint, when first removing the instance variable `connection` we can ***Lean on the compiler*** to see which instances we need to remove
	> Or use `Find and Replace`

	### Extract holiday calculation
	Next, extract the holiday calculation into a new method.
	Use **Slide statement** refactoring to move the `isHoliday` variable declaration closer to the using. <br/>
	Apply **Extract method** on the holiday calculation part.<br/>
	Ensure that the method is overridable in a subclass, as this new method is now a *seam* where we can modify behavior under test.<br/>

	### Extract cost retrieval
	Due to the `using` scope, we cannot simply extract the the cost retrieval.



	

- breaking global dependency


2. Apply characterization testing

Include templating code for getting into testharness using TestBuilder

3. Extract logic

4. Encapsulate objects

5. Add new feature