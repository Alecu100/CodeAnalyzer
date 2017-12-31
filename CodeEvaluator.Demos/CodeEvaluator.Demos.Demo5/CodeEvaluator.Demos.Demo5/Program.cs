using CodeEvaluator.Workflows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeEvaluator.Demos.Demo5
{
    public class Program
    {
        static void Main(string[] args)
        {
            WorkflowEvaluator.Initialize();

            WorkflowEvaluator.BeginWorkflow();

            MyClassBase myClass = GetMyClass();

            WorkflowEvaluator.AddDecision("Decission1", "Decission1", "Decission1");

            WorkflowEvaluator.BeginWorkflow();

            WorkflowEvaluator.AddProcess("Process3", "Process3", "Process3");

            WorkflowEvaluator.EndWorkflow();

            WorkflowEvaluator.BeginWorkflow();

            WorkflowEvaluator.AddProcess("Process55", "Process55", "Process55");

            WorkflowEvaluator.EndWorkflow();

            WorkflowEvaluator.AddProcess("Process4345", "Process4345", "Process4345");

            myClass = GetMyClass3();

            myClass.MyMethod();

            WorkflowEvaluator.StopWorkflow();
        }

        private static MyClassBase GetMyClass()
        {
            if (true)
            {
                return new MyClass1();
            }

            return new MyClass2();
        }

        private static MyClassBase GetMyClass3()
        {
            return new MyClass3();
        }
    }
}
