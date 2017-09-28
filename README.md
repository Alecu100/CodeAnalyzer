# CodeAnalyzer
A sort of dynamic code analyzer that simulates the actual C# code execution

This currently works only for Visual Studio 2015 as a package. 

To run this just set the CodeEvaluator.Packages.Vs2015 as the startup project and press F5 to start in debug mode or without debug mode if you prefer. A new instance of Visual Studio will be launched.

This actually is a partially implemented C# interpreter. It takes and evaluates each line of code. It builds a table of all the types included in the code files and in compiled assemblies.

Then using the type information it starts evaluating the code calling methods, creating new objects and doing various other operations.

Some demos are included. When you run the project with the Visual Studio package mentioned earlier open a solution found in CodeAnalyzer/CodeEvaluator.Demos/. Then in the Visual Studio menu select Code Analyzer and the Generate Workflow Diagram from the dropdown menu.

A new Visual Studio pane will appear from which you can select the projects and start method to begin simulating the execution. On the right side there is a blank canvas in which a workflow diagram will be generated. 

To generate a workflow diagram, the code execution simulator scans for specific methods in the code and for each method call, adds a new element on the diagram with the name and description specified in the method call.

The possible method calls are: "WorkflowEvaluator.AddDecision", "WorkflowEvaluator.BeginWorkflow", "WorkflowEvaluator.EndWorkflow", "WorkflowEvaluator.AddProcess" and "WorkflowEvaluator.AddDecision".

I added a quick video demo at the following address: https://www.youtube.com/watch?v=F3pjRgD0D6c
