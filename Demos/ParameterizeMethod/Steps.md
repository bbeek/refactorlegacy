Introduce additional parameter to method, moving the creation of an object outside.

## Why:
When we need the internal created object to sense or separate

## How:

1. Copy the existing method

2. Add a parameter to the method that you want to pass in, replace the object creation with parameter assignment

3. In the copied method, delete the body and make a call to the parameterize method, using object creation from the original

