1. Copy the loop in Customer.cs

```
            // determine amounts for each line
            foreach (var rental in Rentals)
            {
                decimal thisAmount = rental.Movie.Price;

                // Calculate amount due
                if (rental.DaysRented > 2)
                {
                    thisAmount += (rental.DaysRented - 2) * 1.5m;
                }

                // frequent renter points
                frequentRenterPoints++;

                // add bonus for a two day new release rental
                if (rental.Movie.IsNewRelease && rental.DaysRented > 1)
                {
                    frequentRenterPoints++;
                }

                // show figures for this rental
                result.AppendLine("\t" + rental.Movie.Title + "\t" + thisAmount.ToString());
                totalAmount += thisAmount;
            }
            
            // determine amounts for each line
            foreach (var rental in Rentals)
            {
                decimal thisAmount = rental.Movie.Price;

                // Calculate amount due
                if (rental.DaysRented > 2)
                {
                    thisAmount += (rental.DaysRented - 2) * 1.5m;
                }

                // frequent renter points
                frequentRenterPoints++;

                // add bonus for a two day new release rental
                if (rental.Movie.IsNewRelease && rental.DaysRented > 1)
                {
                    frequentRenterPoints++;
                }

                // show figures for this rental
                result.AppendLine("\t" + rental.Movie.Title + "\t" + thisAmount.ToString());
                totalAmount += thisAmount;
            }
```

2. Identify and remove duplicate side effects
Using our unittests, we can identify all side-effect. As added benefit, this gives us the opportunity to verify our unittests

Now the frequent renter points and the total amount is calculated twice.
So let's decide that the first loop will calculate the frequent renter points and the second loop will calculate the total amount.

Thus remove the duplicate side effects from the first and second loop:

```
            foreach (var rental in Rentals)
            {
                // frequent renter points
                frequentRenterPoints++;

                // add bonus for a two day new release rental
                if (rental.Movie.IsNewRelease && rental.DaysRented > 1)
                {
                    frequentRenterPoints++;
                }
            }

            // determine amounts for each line
            foreach (var rental in Rentals)
            {
                decimal thisAmount = rental.Movie.Price;

                // Calculate amount due
                if (rental.DaysRented > 2)
                {
                    thisAmount += (rental.DaysRented - 2) * 1.5m;
                }

                // show figures for this rental
                result.AppendLine("\t" + rental.Movie.Title + "\t" + thisAmount.ToString());
                totalAmount += thisAmount;
            }
```

3. Test
And we are done with this first refactoring.
(Depending on time, show when less then 1 hour elapsed)
However while identify the side-effects we also saw a third side-effect, namely `result.AppendLine("\t" + rental.Movie.Title + "\t" + thisAmount.ToString());`

Let's again apply the Split Loop refactoring

```
            foreach (var rental in Rentals)
            {
                decimal thisAmount = rental.Movie.Price;

                // Calculate amount due
                if (rental.DaysRented > 2)
                {
                    thisAmount += (rental.DaysRented - 2) * 1.5m;
                }

                totalAmount += thisAmount;
            }

            foreach (var rental in Rentals)
            {
                decimal thisAmount = rental.Movie.Price;
                
                // Calculate amount due
                if (rental.DaysRented > 2)
                {
                    thisAmount += (rental.DaysRented - 2) * 1.5m;
                }

                // show figures for this rental
                result.AppendLine("\t" + rental.Movie.Title + "\t" + thisAmount.ToString());
            }
```

This does however duplicate some code for calculating the amount due.
So let's use the `extract method` refactoring.


```
            foreach (var rental in Rentals)
            {
                decimal thisAmount = CalculateAmountDue(rental);
                
                // show figures for this rental
                result.AppendLine("\t" + rental.Movie.Title + "\t" + thisAmount.ToString());
            }
```

```
        private static decimal CalculateAmountDue(Rental rental)
        {
            decimal thisAmount = rental.Movie.Price;

            // Calculate amount due
            if (rental.DaysRented > 2)
            {
                thisAmount += (rental.DaysRented - 2) * 1.5m;
            }

            return thisAmount;
        }
```

and manually replace the second duplication:

```
            foreach (var rental in Rentals)
            {
                decimal thisAmount = CalculateAmountDue(rental);

                totalAmount += thisAmount;
            }
```



4. Optional: Extract function
First using slide statement, move the variable declarations closer to the loops:

```
            int frequentRenterPoints = 0;
            foreach (var rental in Rentals)
            {
                // frequent renter points
                frequentRenterPoints++;

                // add bonus for a two day new release rental
                if (rental.Movie.IsNewRelease && rental.DaysRented > 1)
                {
                    frequentRenterPoints++;
                }
            }

            // determine amounts for each line
            decimal totalAmount = 0;
            foreach (var rental in Rentals)
            {
                decimal thisAmount = CalculateAmountDue(rental);

                totalAmount += thisAmount;
            }

            foreach (var rental in Rentals)
            {
                decimal thisAmount = CalculateAmountDue(rental);
                
                // show figures for this rental
                result.AppendLine("\t" + rental.Movie.Title + "\t" + thisAmount.ToString());
            }
```

Then extract method on first loop:

```
            int frequentRenterPoints = GetFrequentRenterPoints();
```

```
        private int GetFrequentRenterPoints()
        {
            int frequentRenterPoints = 0;
            foreach (var rental in Rentals)
            {
                // frequent renter points
                frequentRenterPoints++;

                // add bonus for a two day new release rental
                if (rental.Movie.IsNewRelease && rental.DaysRented > 1)
                {
                    frequentRenterPoints++;
                }
            }

            return frequentRenterPoints;
        }
```

Consider inlining the method call (using the refactor action "Inline temporary variable")
```
        result.AppendFormat("You earned {0} frequent renter points", GetFrequentRenterPoints());
```

Test and extract method:

```
            decimal totalAmount = GetTotalAmountDue();
```
```
        private decimal GetTotalAmountDue()
        {
            decimal totalAmount = 0;
            foreach (var rental in Rentals)
            {
                decimal thisAmount = CalculateAmountDue(rental);

                totalAmount += thisAmount;
            }

            return totalAmount;
        }

```

And finishing up by inlining temporary variables, extracting methods, sorting method in order of when called and removing comments results in:

```
        public string Statement()
        {
            var result = new StringBuilder();
            result.AppendLine($"Rental Record for {this.Name}");

            foreach (var rental in Rentals)
            {
                result.AppendLine("\t" + rental.Movie.Title + "\t" + CalculateAmountDue(rental).ToString());
            }

            AddFooter(result);

            return result.ToString();
        }

        private void AddFooter(StringBuilder result)
        {
            result.AppendLine("Amount owed is " + GetTotalAmountDue().ToString(""));
            result.AppendFormat("You earned {0} frequent renter points", GetFrequentRenterPoints());
        }

        private static decimal CalculateAmountDue(Rental rental)
        {
            decimal thisAmount = rental.Movie.Price;

            if (rental.DaysRented > 2)
            {
                thisAmount += (rental.DaysRented - 2) * 1.5m;
            }

            return thisAmount;
        }

        private decimal GetTotalAmountDue()
        {
            decimal totalAmount = 0;
            foreach (var rental in Rentals)
            {
                decimal thisAmount = CalculateAmountDue(rental);

                totalAmount += thisAmount;
            }

            return totalAmount;
        }

        private int GetFrequentRenterPoints()
        {
            int frequentRenterPoints = 0;
            foreach (var rental in Rentals)
            {
                frequentRenterPoints++;

                if (HasNewReleaseBonus(rental))
                {
                    frequentRenterPoints++;
                }
            }

            return frequentRenterPoints;
        }

        private static bool HasNewReleaseBonus(Rental rental)
        {
            return rental.Movie.IsNewRelease && rental.DaysRented > 1;
        
```