1. Copy the loop

```
            // determine amounts for each line
            foreach (var rental in Rentals)
            {
                decimal thisAmount = rental.Movie.Price;

                // Calculate 
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

                // Calculate 
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
Now the frequent renter points and the total amount is calculated twice.
So let's decide that the first loop will calculate the frequent renter points and the second loop will calcalute the total amount.

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

                // Calculate 
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
And we are done with this refactoring.

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
                decimal thisAmount = rental.Movie.Price;

                // Calculate 
                if (rental.DaysRented > 2)
                {
                    thisAmount += (rental.DaysRented - 2) * 1.5m;
                }

                // show figures for this rental
                result.AppendLine("\t" + rental.Movie.Title + "\t" + thisAmount.ToString());
                totalAmount += thisAmount;
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

For the second loop, notice that it still does 2 things, namely calculating the total amount by adding up each movie amount and listing the movie with amount.
So, let's first extract the method for calculating the movie amount:

```
            foreach (var rental in Rentals)
            {
                decimal thisAmount = CalculateAmountDue(rental);

                // show figures for this rental
                result.AppendLine("\t" + rental.Movie.Title + "\t" + thisAmount.ToString());
                totalAmount += thisAmount;
            }
```

```
    
        private static decimal CalculateAmountDue(Rental rental)
        {
            decimal thisAmount = rental.Movie.Price;

            // Calculate 
            if (rental.DaysRented > 2)
            {
                thisAmount += (rental.DaysRented - 2) * 1.5m;
            }

            return thisAmount;
        }
```

Then apply split loop on the loop:

From:
```
            decimal totalAmount = 0;
            foreach (var rental in Rentals)
            {
                decimal thisAmount = CalculateAmountDue(rental);

                // show figures for this rental
                result.AppendLine("\t" + rental.Movie.Title + "\t" + thisAmount.ToString());
                totalAmount += thisAmount;
            }
```

To:
```         
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