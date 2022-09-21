```cs
using System;

using BakedEnv.Environment;
using BakedEnv.Sources;

var function = new DelegateObject(delegate(object any) 
    { 
        Console.WriteLine(any.ToString()); 
    }
);

var environment = new BakedEnvironment();
environment.GlobalVariables["myFunc"] = function;

var source = new RawStringSource(Console.ReadLine()); 
// Assume the below example is the input
var session = environment.CreateSession(source);

session.Init();
session.ExecuteUntilTermination();

```
FunctionSample.cs

```
a = 500

myFunc("abc")
myFunc("trabajar")
myFunc(123 + 456)
myFunc(a * 0.5)
```
Example BakedEnv input

```sh
> abc
> trabajar
> 579
> 250
```
Expected output