1. Create a new variable for the loop collection

```
            var loopItems = lines;
            foreach (var line in loopItems)
```

2. Starting from the top, take each bit of behaviour in the loop and replace it with a collection pipeline operation

The first part of the loop is skipping the first line.
Let's replace it with a slice action (`Skip` in Linq)

```
            var loopItems = lines
                .Skip(1);
            foreach (var line in loopItems)
            {
                if (string.IsNullOrEmpty(line.Trim()))
```

And test!

Bonus, we can remove the unused `firstLine` variable. Alway nice!

(using Ctrl + .)

3.1 Next behaviour is skipping the blank lines.

Let's filter out this behaviour using the Where clause.
Note that we do need to invert the check and let's use the `string.IsNullOrWhiteSpace` feature.

```
            var loopItems = lines
                .Skip(1)
                .Where(line => !string.IsNullOrWhiteSpace(line));
            foreach (var line in loopItems)
            {
                var record = line.Split(",");
```

And test!

3.2 Next behaviour is converting the line into an array.
We can do this with a map command, in C# this is the `Select` function

```
            var loopItems = lines
                .Skip(1)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(line => line.Split(","));
            foreach (var line in loopItems)
            {
                var record = line;
```

And test!

3.3 Then the filter on "Norway"

```
            var loopItems = lines
                .Skip(1)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(line => line.Split(","))
                .Where(record => record[1].Trim().Equals("Norway"));
            foreach (var line in loopItems)
            {
                var record = line;
                result.Add(new Office { City = record[0].Trim(), Phone = record[2].Trim(), Address = record[3].Trim() });
            }
```

And test!

3.4 Then the mapping to output record:

```

            var loopItems = lines
                .Skip(1)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(line => line.Split(","))
                .Where(record => record[1].Trim().Equals("Norway"))
                .Select(record => new Office { City = record[0].Trim(), Phone = record[2].Trim(), Address = record[3].Trim() });
            foreach (var line in loopItems)
            {
                var record = line;
                result.Add(line);
            }
```

And test!

4. Removed the loop

```
            var result = lines
                .Skip(1)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(line => line.Split(","))
                .Where(record => record[1].Trim().Equals("Norway"))
                .Select(record => new Office { City = record[0].Trim(), Phone = record[2].Trim(), Address = record[3].Trim() });
            return result;
```

And test!
Test breaks as the "List" was apparently also a behaviour.
So let's add that behaviour

```
            var result = lines
                .Skip(1)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(line => line.Split(","))
                .Where(record => record[1].Trim().Equals("Norway"))
                .Select(record => new Office { City = record[0].Trim(), Phone = record[2].Trim(), Address = record[3].Trim() })
                .ToList();
            return result;
```

And test again!

4.1 Cleaning up, remove the temporary variable

```
        public IEnumerable<Office> GetNorwayOffices(string input)
        {
            var lines = input.Split("\n");
            return lines
                .Skip(1)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(line => line.Split(","))
                .Where(record => record[1].Trim().Equals("Norway"))
                .Select(record => new Office { City = record[0].Trim(), Phone = record[2].Trim(), Address = record[3].Trim() })
                .ToList();
        }
```

(Possibly end with comparison of end result via Git)