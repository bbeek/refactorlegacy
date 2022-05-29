# Lift pass pricing

This application solves the problem of calculating the pricing for ski lift passes.
There's some intricate logic linked to what kind of lift pass you want, your age and the specific date at which you'd like to ski. 
Plus if you book on the same day for which you request a ticket, 
you might be eligible for either a "early bird" or a "end of day" discount depending on the booking time and your age

There's a new feature request, be able to get the price for several lift passes, not just one. 
Currently the pricing for a single lift pass is implemented, unfortunately the code as it is designed is ***not reusable***.<br/>
You are going to **break dependencies** to add *characterization tests* in place that enables further refactoring 
so that the new feature requires minimum effort to implement.

## Steps

### 1. Breaking dependencies

#### 1.1 Extract and encapsulate SQL queries
- Remove unwanted side-effects from the constructor, extract `connection`.<br/>

	The `connection` instance variable needs to be broken in order to get the code into a test harness.

	See if you can extract the code for creating the `connection`.<br/>

	As the current state does not bring up the **Extract method** option in the quick actions menu, 
	start with *extracting* the `SqlConnection` creation by changing `connection` into a local variable by adding `var` keyword.
	```c#
	public PricesController()
    {
        var connection = new SqlConnection(@"Database=lift_pass;Data Source=localhost;Trusted_Connection=true");
        connection.Open();
    }
	```

	And after the `Open` statement, assign the local variable `connection` to the instance variable `connection`.
	```c#
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

- Extract holiday calculation<br/>

	Next, extract the holiday calculation into a new method. The holiday calculation is the using block below the comment
	```c#
	// Calculate if selected date is a holiday
	```
	Use **Slide statement** refactoring to move the `isHoliday` variable declaration closer to the first using below the comment above. <br/>
	Apply **Extract method** on the holiday calculation part. Let's call the new method `IsHoliday` and delete the comment as the <br/>
	This new method is now a *seam*, that we can use later to modify behavior under test.<br/>

- Extract base price retrieval<br/>

	Due to the `using` scope, we cannot simply extract the base price retrieval. The base price retrieval is the very first using and
	ends after the `ExecuteScalarAsync` method following the comment `// Retrieve the base price`<br/>
	First, change the `using` scope so that it ends after `double result = (int)(await costCmd.ExecuteScalarAsync());`.<br/>
	You'll see that the `result` variable is used throughout the rest of the method.<br/>
	Meaning that we will need to split the variable declaration from the variable assignment.<br/>
	
	Change into
	```c#
	using (var costCmd = new SqlCommand(@"SELECT cost FROM base_price WHERE type = @type", connection))
    {
        costCmd.Parameters.Add(new SqlParameter("@type", this.Request.Query["type"].ToString()) { DbType = DbType.String, Size = 255 });
		costCmd.Prepare();
		double result;
		// Retrieve the base price
		result = (int)(await costCmd.ExecuteScalarAsync());
	}
	```
	And using the **Slide statement** refactoring, move the variable declaration out of the now much shorter `using` block.

	Apply **Extract method** on the `using` block, including the `double result` variable declaration.
	Give this new method a descriptive name. This is now our second *seam*.

Your `PricesController` class should now look like: `Day2\solutions\1.1 Extracted SQL\PricesController.cs`

With all of the SQL dependencies out of the way, we move on to the next dependency, namely the `this.Request.Query`.<br/>
By making use of `this.Request.Query`, it is extremely hard to get the method to run in our test harness as we would need to sub the `Request` and the `Query` properties.

#### 1.2 Parameterize the `GetAsync` method
* Copy the `GetAsync` method below the existing variant.
* Add `int? age` as parameter to the copied method and remove the `int? age` variable declaration line.
* From the original `GetAsync` method, remove all lines apart from `int? age = this.Request.Query["age"] != StringValues.Empty ? Int32.Parse(this.Request.Query["age"]) : null;` and call copied method, as such `return await GetAsync(age);`

* Repeat adding parameters for `Query["type"]` and `Query["date"]`.

	Here, also add parameters to the extracted `RetrieveBasePrice` and `IsHoliday` methods
	For the latter ensure that you use the correct type declaration as the existence to the "date" query string is also checked.

Your `PricesController` class should now look something like: `Day2\solutions\1.2 Parameterized method\PricesController.cs` 

Although we are now able to get the parameterized method in our test harness, let's break the last dependency that blocks us from fully testing all characteristics of the GetAsync method.

#### 1.3 Breaking global dependency
Apply the dependency breaking technique **Replace global reference with getter** on `DateTime.Now.Hour` calls in `GetAsync(..)`.
> Even though `IsHoliday` method also contains a reference to the global `DateTime.Now` instance, we do not need to remove that dependency as we can already override the `IsHoliday` method.
> Making this less of an issue at this point in our refactoring journey.

* Extract the global reference into an new method using the **Extract method** automated refactoring.
* Change the accessibility from `private static` to `protected virtual`
* Replace other references with the extracted method.

Your `PricesController` class should now look something like: `Day2\solutions\1.3 Replace global reference with getter\PricesController.cs` 

### 2. Apply characterization testing
With all dependencies, that impact our ability to fully test the code we want to test, broken, let's apply ***Characterization testing*** to capture the existing behaviour.

But first, we need to apply **subclass and override method** technique on the extracted methods.
* Apply step 1-3 of the **subclass and override method** on `IsHoliday` and `RetrieveBasePrice` methods
* For applying step 4, create a new testing *subclass* named `TestingPricesController` in the `Day2Tests` project and override the 3 methods.

In order to help write readable tests, a *Builder pattern* class has already been provided.
Ensure that the `TestingPricesController` can be created from `PricesControllerBuilder` with the correct behaviour by ensuring that the `Build` method returns a `TestingPricesController`.
Or look into the provided solution for the suggested setup. 

In the class `PricesTests`, there are already a few suggestions for *characterization tests* defined.
Let's start with the first suggestion `GetAsync_price_for_child_nightticket` using *characterization test* approach:
* Get the code in test harness.<br/>

	For this we need to arrange an int constant that is within the age range for a child ticket, so `age < 6`. let's take 5 as our magic child value.<br/>
	And a "night" ticket type.<br/>
	Next, using the `PricesControllerBuilder`, build a `sut` with the correct ticket type defined.

	Using the subject under test (`sut`), call and await the parameterized `GetAsync` method.

```c#
        public async Task GetAsync_price_for_child_nightticket()
        {
            // Arrange
            const int child = 5;
            const string nightTicket = "night";
            var sut = new PricesControllerBuilder().WithNightTicket().Build();

            // Act
            var price = await sut.GetAsync(child, nightTicket, null);

            // Assert
            // TODO: Verify actual behaviour
        }
```

* Write an assertion that fails<br/>
	For example: `price.Should().Be("");`

* Let the failure tell what the behaviour is.<br/>
	For this, simply run the test.

* Change the assertion to the expected value.<br/>

	As this currently still returns an `OkObjectResult`, cast the `price` to the correct type and evaluate the value using the actual returned value.
	```c#
	((OkObjectResult)price).Value.Should().Be("{ \"Cost\": 0}");
	```
	Run the test again the check that it is green.

	As a best practice, change the name of the test to also incorporate the should part of the test.<br/>
	So for this test, the correct name would be: `GetAsync_price_for_child_nightticket_should_be_free`

* Repeat<br/>
	(We will do this later, please read on)
	
Before filling in the other suggested tests, let's first look at the ***actual*** behaviour of the "end of day" discount for an adult.<br/>
Start by creating a new test that tests what the behaviour is if we book a ticket for an adult for tomorrow after the "end of day" discount time.<br/>
As we do not know the actual outcome, start by describing the setup: `GetAsync_price_for_adult_dayticket_after_endofday_time_for_tomorrow`<br/>
Then:
* Get the code in test harness.<br/>
	> Hint, use the `.WithDayTicket().WithEndOfDayDiscountTime()` methods to configure the `PricesController` correctly

* Write an assertion that fails.<br/>

	For example: `price.Should().Be("");`

* Let the failure tell what the behaviour is.<br/>

	For this, simply run the test.<br/>
	And here we will find out that there is a difference between the described behaviour (a price of 35) and the actual behaviour (a price of 34).<br/>
	As we are still in the refactoring phase, we will *not* fix this bug as we do not want to mix refactoring with adding features<br/>
	> **Note** If you find such a bug in a real refactoring, you can fix it *only if and when* you know its an previously unfound bug 
	> and do this, of course, after refactoring. Proceed with caution however as other code or workflows might depend on this "undocumented feature".

* Change the assertion to the expected value.<br/>
	Again, as we do not want to mix refactoring with adding features/bugfixing, document the actual incorrect for now in the assertion:
	```c#
	((OkObjectResult)price).Value.Should().Be("{ \"Cost\": 34}");
	```
	Plus update the name of the test to its current form, `GetAsync_price_for_adult_dayticket_after_endofday_time_for_tomorrow_should_return_discounted_price`


If you want to pratice a bit more, repeat this process for the suggested tests.
Or if you think you've practiced enough and got the hang of it (or finished all of the behaviours), check `Labs\Day2\solutions\2 Characterization tests` for the finished Day2Tests project state
In case you did not document all behaviours, copy the `PricesTest` from the solution to your own project as a safety net for future refactoring.

### 3. Extract logic
With our code now no longer being "legacy" code, as it is covered by unittests, let's move to the fourth step of *Refactoring legacy code approach*: refactor.

Our goal is to move the logic out of the controller, however here return type of the already extracted `GetAsync` method would drag in unwanted references.

Thus let's introduce a new `response` variable via 'string response;', return this at the end of the method as `return Ok(response);` and replace all existing returns to `response` assignments

After all `return`s have been replaced, this allows us the apply the **Extract method** refactoring for a `CalculatePrice` method.<br/>
Run tests to verify.

#### 3.1 Next, encapsulate the SQL queries:
* Create a new folder repositories, add new class PriceRepository. Using the **Move Method**, move `RetrieveBasePrice` into this new class named `PriceRepository`.<br/>
	*Note:* Copy the `GetConnection` method as well.<br/>
	Change the accessibility modifiers to a `public` method. <br/>
	Keep the source method with a call to the target class:
	```c#
		protected virtual async Task<double> RetrieveBasePrice(string liftPassType)
		{
			return await new PriceRepository().RetrieveBasePrice(liftPassType);
		}
	```

	Run all tests
* Using the **Move Method**, move `IsHoliday` into the `PriceRepository`. Keep the source method with a call to the target class:
	```c#
        protected virtual async Task<bool> IsHoliday(DateTime? skiDate)
        {
            return await new PriceRepository().IsHoliday(skiDate);
        }
	```

* Apply the **Extract interface** refactoring on `PriceRepository`. Hint: use the quick actions menu on `PriceRepository`<br/>
	Create a new `private readonly IPriceRepository repository;` in `PricesController` and use the quick actions menu to add it to the constructor.

	Add `services.AddTransient<IPriceRepository, PriceRepository>();` to `Startup.ConfigureServices`

* Replace all usages of `new PriceRepository()` with `repository`.

* Change the `TestingPricesController` to inject a stub of the `IPriceRepository`. Do this by changing the constructor to:
	```c#
		public TestingPricesController(int hour, bool holiday, double cost, IPriceRepository repository) : base(repository)
	```

	And changing the `PricesController.Build` method to:

	```c#
		internal PricesController Build()
        {
            var stub = new Mock<IPriceRepository>();
			stub.Setup(f => f.IsHoliday(It.IsAny<DateTime?>())).ReturnsAsync(holiday);
			stub.Setup(f => f.RetrieveBasePrice(It.IsAny<string>())).ReturnsAsync(cost);
            return new TestingPricesController(hour, holiday, cost, stub.Object);
        }
	```
	And run all tests.

* Inline and remove the `IsHoliday` and `RetrieveBasePrice` in `PricesController`. Plus remove the temporary override methods and fields from `TestingPricesController`

#### 3.2 Introduce a `ReductionService`
Currently the only temporary override method still standing is the `GetHour` method. We could move this into a generic `DateTimeProvider` and be done with it.
However, there is more value in actually encapsulating what we use this global reference for.

And as you can see, it has to do with the potential reduction calculation based on age. Given that there is also a reduction calculation on date, let's combine these two.

Start by **Sliding statements** related to the "End of day" and "Early bird" discount based on age below the reduction calculation based on skiDate.

Run all tests to verify that nothing broke while moving the statements.

Next, **Slide statement** move the `int reduction = 0;` above the `bool isHoliday = await repository.IsHoliday(skiDate);`
Then apply **Extract method** on all reduction related statements. Name this new method `FindPotentialReduction`.

Run all tests.

Simplify the End of day discount calculation:
```c#

            if (age == null || age <= 64)
            {
                // End of day discount
                if (GetHour() > 15)
                {
                    reduction += 5;
                }
            }
            else
            {
                // Early bird discount
                if (GetHour() < 9)
                {
                    reduction += 15;
                }   
            }
```
Run all tests.

Extract the end of day check and the early bird check into 2 methods named `IsInEndOfDayPeriod` and `IsInEarlyBirdPeriod`.

Create a new folder `Logic` and in it a new class `ReductionService`. First copy `GetHour`, `IsInEndOfDayPeriod` and `IsInEarlyBirdPeriod` into this class, then copy `FindPotentialReduction` in it.
Make the latter public, and ensure that the `repository` is injected correctly. <br/>
Change `IsInEndOfDayPeriod` and `IsInEarlyBirdPeriod` into `protected virtual` methods so that we can apply **Subclass and Override**.<br/>
Inline and remove the `GetHour` in the `ReductionService`.

In the `Day2Tests` create a new class `FakeReductionService` that inherits from `ReductionService`.
```c#
using Day2.Logic;
using Day2.Repositories;

namespace Day2Tests
{
    internal class FakeReductionService : ReductionService
    {
        private readonly bool earlyBird;
        private readonly bool endOfDay;
        public FakeReductionService(IPriceRepository repository, bool earlyBird, bool endOfDay) : base(repository)
        {
            this.earlyBird = earlyBird;
            this.endOfDay = endOfDay;
        }

        protected override bool IsInEarlyBirdPeriod()
        {
            return earlyBird;
        }

        protected override bool IsInEndOfDayPeriod()
        {
            return endOfDay;
        }
    }
}
```

In the class `PricesController`, add `private readonly ReductionService reductionService;` and inject into constructor.<br/>
Change `await FindPotentialReduction(age, skiDate)` to `await reductionService.FindPotentialReduction(age, skiDate)` in `CalculatePrice`<br/>
Add `services.AddTransient<ReductionService>();` to `Startup.ConfigureServices`<br/>

In `PricesControllerBuilder`, add to instance variables, `private bool isEarlyBird = false;` and `private bool isEndOfDay = false;`.
Replace the magic hours with changing `isEarlyBird` and `isEndOfDay` to `true` in the 2 builder methods.

In the `Build` method create a `FakeReductionService` and pass it into the instantiated `PriceController`:
```c#
var fakeReductionService = new FakeReductionService(stub.Object, isEarlyBird, isEndOfDay);
return new PricesController(stub.Object, fakeReductionService);
```

Remove now unused the  `TestingPricesController`

Run all tests.

Remove unused methods `FindPotentialReduction`, `IsInEarlyBirdPeriod`, `IsInEndOfDayPeriod` and `GetHour`.


Your solution should now look something like: `Day2\solutions\3.2 ReductionService\` 

### 4. Encapsulate primitive
Before extracting the `CalculatePrice` into a new class, we are first going to change the return type from a primitive `string` to an object.

* Add a folder `Model` and in it add a new class `Ticket`. This will become our return type. 
* Add a `get` only property `Cost` and add this initialization to the constructor using *Quick Actions context menu*.
* Apply **Extract variable** on `await repository.RetrieveBasePrice(liftPassType);` in `CalculatePrice`. Name the new variable `basePrice`.
* Change `double basePrice` into `Ticket` by wrapping the `await` part in the `Ticket` constructor. Return `basePrice.Cost` to `double result`
	```c#
	var basePrice = new Ticket(await repository.RetrieveBasePrice(liftPassType));
	double result = basePrice.Cost;
	```

* Apply **Inline variable** on `result` using *Quick Actions context menu*.
* Apply **Extract method** on `double cost = basePrice.Cost * (1 - reduction / 100.0);` and name the new method `ApplyReductionPercentage`
* Move the `ApplyReductionPercentage` to `Ticket` class, let it return `Ticket` and adjust it to fit the context (remove `basePrice` parameter)
	```c#
	public Ticket ApplyReductionPercentage(int reduction)
	{
		return new Ticket(this.Cost * (1 - reduction / 100.0));
	}
	```
* Replace all applications of the `(1 - reduction / 100.0)` with a call to `basePrice.ApplyReductionPercentage(reduction)`. Solve the compiler errors:
	```c#
	var cost = basePrice.ApplyReductionPercentage(reduction);
	response = "{ \"Cost\": " + (int)Math.Ceiling(cost.Cost) + "}";
	```
* Run all tests.


* For the senior discount for non night tickets, notice that we first apply a 25 percentage reduction and then a reduction.
	```c#
		var cost = basePrice
            .ApplyReduction(25)
            .ApplyReduction(reduction);
        response = "{ \"Cost\": " + (int)Math.Ceiling(cost.Cost) + "}";
	```
* Run tests to verify that this calculation is correct.

* Apply this same logic to the other reductions

* Extract `(int)Math.Ceiling(cost.Cost)` to a method named `RoundUp`
* Move `RoundUp` to `Ticket`, return `Ticket` as type and adjust it to fit the new context.
* Replace all usage of `(int)Math.Ceiling(cost.Cost)` with `.RoundUp()` pipeline. Solve compiler errors with `response = "{ \"Cost\": " + cost.Cost + "}"`

We are now almost ready to change from `string` to `Ticket`. Only the free tickets are blocking us.
Add a new static `Free` method to `Ticket` that returns a `Ticket` with cost 0.
Change the return type of the `CalculatePrice` method to `Ticket`, change `string response` to `Ticket response` and ensure that a `Ticket` object is always returned, making use of the `Ticket.Free()` method.

Run all tests. Unfortunately all will fail as they rely tightly on the previous internal behaviour.
However if we change the `string` value to a dynamic object with the same value and use the `BeEquivalentTo` function from *FluentAssertions*,
we can still use our previously gathered behaviour.
So `.Be("{ \"Cost\": 0}")` would become `.BeEquivalentTo(new { Cost = 0 })`

Check `Day2\solutions\4 Encapsulate primitive\test\Day2Tests\PricesTest.cs` for the adjusted tests.

Now, some cleaning up to do.
* First, change the return type for `IPriceRepository.RetrieveBasePrice` to `Task<Ticket>`. Update the implementation in `PriceRepository` and fix the compiler errors. Run tests
* Next, on `CalculatePrice` apply **Inline variable** refactoring on all `cost` variables. Run tests
* Then remove the repetition of the `RoundUp` call by only calling it once in `return Ok(response);` as `return Ok(response.RoundUp());`. Run tests
* In `CalculatePrice` remove the `else` leg of the `(age != null && age < 6)` check by directly returning `Ticket.Free()`.
* See if you can simplify the `if` checks, taking care to run tests after each change.

* Create a new `TicketService` in the folder `Logic` and move the `CalculatePrice` function into the new `TicketService`.
	Add a readonly instance variable of this type to `PricesController`, inject via constructor. (do not forget adding to `ConfigureServices`)
	Use this instance variable to call the moved function.
	Fix the compiler errors in the `PricesControllerBuilder`.

Run all tests

Check `Day2\solutions\4 Encapsulate primitive\` for a suggested end state.

## Conclusion
In this lab you've experience that by break the dependencies that either, cause the code to be hard to get into a test harness or, make it hard to run a method in a test harness.

As a result of those broken dependencies, we can apply *characterization testing* to document the **actual** behaviour, which allows us to safely refactor or change legacy code.
