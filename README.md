# CodeAnalyzer
A sort of dynamic code analyzer that simulates the actual C# code execution

This currently works only for Visual Studio 2015 as a package. 

To run this just set the CodeEvaluator.Packages.Vs2015 as the startup project and press F5 to start in debug mode or without debug mode if you prefer. A new instance of Visual Studio will be launched.

This actually is a partially implemented C# interpreter. It takes and evaluates each line of code. It builds a table of all the types included in the code files and in compiled assemblies.

Then using the type information it starts evaluating the code calling methods, creating new objecst and doing various other operations.
