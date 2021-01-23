
## Goal

Let's suppose that the change that we need to make to the code is to add a header row for the HTML table `QuarterlyReportGenerator` producing.

The header row should look something like this:

    <tr><td>Department</td><td>Manager</td><td>Profit</td><td>Expenses</td></tr>

## Steps

1. Identify where you need to make your code change.

2. Determine what local variables you need from the source method, and make them arguments to the classes' constructor.

3. Determine whether the sprouted class will need to return values to the source method.
If so, provide a method in the class that will supply those values, and add a call in the source method to receive those values.

4. Develop the sprout class

5. Call the sprout class in the source method and use its calls.

For demo:

New class `QuarterlyReporTableHeaderProducer`
```c#
namespace SproutClass
{
    internal class QuarterlyReporTableHeaderProducer
    {
        internal string MakeHeader()
        {
            return "<tr><td>Department</td><td>Manager</td><td>Profit</td><td>Expenses</td></tr>";
        }
    }
}
```


Add internalsvisibleto

`[assembly:InternalsVisibleTo("SproutClassTests")]`

Unittest.


Add new class + method to source
```

                var producer = new QuarterlyReporTableHeaderProducer();
                pageText += producer.MakeHeader();
```