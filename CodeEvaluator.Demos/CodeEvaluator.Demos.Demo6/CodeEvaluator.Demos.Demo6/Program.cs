using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeEvaluator.Workflows;

namespace CodeEvaluator.Demos.Demo6
{
    class Program
    {
        static void Main(string[] args)
        {
            int integerValue = 16;

            WorkflowEvaluator.BeginWorkflow();

            WorkflowEvaluator.AddProcess("Main Process", "Main Process");

            var myClassInstance = new MyClassWithoutConstructor();

            myClassInstance.MyMethod();

            WorkflowEvaluator.EndWorkflow();
        }
    }
}
