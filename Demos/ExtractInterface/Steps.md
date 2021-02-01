Change the dependency from an object to an interface.
This gives us a great range of flexibility within our testharness without great impact on the other consumers of the target class.

## Why:
- Safe, *Lean on compiler* to show mistakes
- Minimal impact
- Breaks hard dependency on implementation
- Creates dependency inversion

## How:

1. Create the interface using Visual Studio automated refactoring, "Extract Interface".
Either:
-	Open the class you want to extract the interface on.<br/>
	In menu bar go to *`Edit`* -> *`Refactor`* -> *`Extract Interface...`*<br/>
	**Optional:** select the methods to include, most times the default selection of all methods is good enough.<br/>
	**Optional:** choose name of interface and where to create. (Depends on code guidelines)<br/>
	Press OK

or

-	On the `class` declaration line, place the caret on the classname, bring up the *quick actions* context menu (default shortcut `Ctrl + .`)<br/>
	Select the option *`Extract Interface...`*<br/>
	Optional, adjust the configuration just as above.<br/>
	Press OK<br/>


2. Change the place where you want to use the object so that is uses the new interface.

3. Compile, checking that this change does not result in build errors

## Disadvantage
Only works on classes you can edit (obviously), meaning that this does not directly work on hard dependencies outside of you control.
Think external libraries. (Use *Skin and Wrap* for these hard dependencies). Out-of-scope for these labs
Does slightly increase build time as it creates more files to compile.


For demo:
extract interface

Adjust constructor and readonly property to interface

Change unittest to mock:
`var checkout = new Checkout(Mock.Of<IReceiptRepository>());`