# Lift pass pricing

This application solves the problem of calculating the pricing for ski lift passes.
There's some intricate logic linked to what kind of lift pass you want, your age and the specific date at which you'd like to ski. 
Plus if you book on the same day for which you request a ticket, 
you might be eligible for either a "early bird" or a "end of day" discount depending on the booking time and your age

There's a new feature request, be able to get the price for several lift passes, not just one. 
Currently the pricing for a single lift pass is implemented, unfortunately the code as it is designed is ***not reusable***.
You are going to ***break dependencies*** to add *characterization tests* in place that enables further refactoring 
so that the new feature requires minimum effort to implement.

## Steps

### 1. Breaking dependencies
#### Extract and encapsulate SQL queries
- Remove unwanted side-effects from the constructor, extract `connection`
	The `connection` instance variable needs to be broken in order to get the code into a test harness.

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
	> Or, of course, use `Find and Replace` to update the code.

- Extract holiday calculation
	Next, extract the holiday calculation into a new method.
	Use **Slide statement** refactoring to move the `isHoliday` variable declaration closer to the using. <br/>
	Apply **Extract method** on the holiday calculation part.<br/>
	This new method is now a *seam*, that we can use later to modify behavior under test.<br/>

- Extract cost retrieval
	Due to the `using` scope, we cannot simply extract the cost retrieval. <br/>
	First, change the `using` scope so that it ends after `double result = (int)(await costCmd.ExecuteScalarAsync());`.<br/>
	You'll see that the `result` variable is used throughout the rest of the method.<br/>
	Meaning that we will need to split the variable declaration from the variable assignment.<br/>
	Change into
	```
		double result;
		result = (int)(await costCmd.ExecuteScalarAsync());
	}
	```
	And using the **Slide statement** refactoring, move the variable declaration out of the now much shorter `using` block.

	Apply **Extract method** on the `using` block, including the `double result` variable declaration.
	Give this new method a descriptive name. This is now our second seam.

Your `PricesController` class should now look like: `Day2\solutions\1.1 Extracted SQL\PricesController.cs`

With all of the SQL dependencies out of the way, we move on to the next dependency, namely the `this.Request.Query`.<br/>
By making use of `this.Request.Query`, it is extremely hard to get the method to run in our test harness as we would need to sub the `Request` and the `Query` properties.

#### Parameterize the `GetAsync` method
* Copy the `GetAsync` method below the existing variant.
* Add `int? age` as parameter to the copied method and remove the `int? age` variable declaration line.
* From the original `GetAsync` method, remove all lines apart from `int? age = this.Request.Query["age"] != StringValues.Empty ? Int32.Parse(this.Request.Query["age"]) : null;` and call copied method, as such `return await GetAsync(age);`

* Repeat adding parameters for `Query["type"]` and `Query["date"]`. For the latter ensure that you use the correct type declaration as the existence to the `date` query string is also checked.

Your `PricesController` class should now look something like: `Day2\solutions\1.2 Parameterized method\PricesController.cs` 

Although we are now able to get the parameterized method in our test harness, let's break the last dependency that blocks us from fully testing all characteristics of the GetAsync method.

#### Breaking global dependency
Apply the dependency breaking technique **Replace global reference with getter** on `DateTime.Now.Hour` calls in `GetAsync(..)`.
> Even though `IsHoliday` method also contains a reference to the global `DateTime.Now` instance, we do not need to remove that dependency as we can already override the `IsHoliday` method.
> Making this less of an issue at this point in our refactoring journey.

* Extract the global reference into an new method using the **Extract method** automated refactoring.
* Change the accessibility from `private static` to `protected virtual`
* Replace other references with the extracted method.

Your `PricesController` class should now look something like: `Day2\solutions\1.3 Replace global reference with getter\PricesController.cs` 

### 2. Apply characterization testing
With all dependencies, that impact our ability to fully test the code we want to test, broken, let's apply ***Characterization testing** to capture the existing behaviour.

But first, we need to apply **subclass and override method** technique on the extracted methods.
* Apply step 1-3 on `IsHoliday` and `RetrieveCost`
* Apply step 4, create a new testing *subclass* named `TestingPricesController` in the `Day2Tests` project and override the 3 methods.

In order to help write readable tests, a *Builder pattern* class has already been provided.
Ensure that the `TestingPricesController` can be created from `PricesControllerBuilder` with the correct behaviour.




3. Extract logic

4. Encapsulate objects

5. Add new feature