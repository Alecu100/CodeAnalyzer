using CodeEvaluator.Workflows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeEvaluator.Demos.Demo4
{
    class Program
    {
        private static void Main(string[] args)
        {
            WorkflowEvaluator.Initialize();

            WorkflowEvaluator.BeginWorkflow();

            Process1();

            Process2();

            WorkflowEvaluator.AddDecision("Decission1", "Decission1");

            WorkflowEvaluator.BeginWorkflow();

            WorkflowEvaluator.AddProcess("Process3", "Process3");

            WorkflowEvaluator.EndWorkflow();

            WorkflowEvaluator.BeginWorkflow();

            WorkflowEvaluator.AddProcess("Process55", "Process55");

            var myClass1 = new MyClass1();

            myClass1.Method1();

            myClass1.Method2();

            WorkflowEvaluator.EndWorkflow();

            WorkflowEvaluator.AddProcess("Process4345", "Process4345");
        

            IMyParameterType1 type1 = new MyParameterType1();
            IMyParameterType2 type2 = new MyParameterType2();

            myClass1.MethodOverloaded(type1);

            myClass1.MethodOverloaded(type2);

            WorkflowEvaluator.StopWorkflow();
        }

        private static void Process2()
        {
            WorkflowEvaluator.AddProcess("Process2", "Process2");
        }

        private static void Process1()
        {
            WorkflowEvaluator.AddProcess("Process1", "Process1");
        }
    }
}
