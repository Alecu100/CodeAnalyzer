using CodeEvaluator.Workflows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeEvaluator.Demos.Demo7
{
    class Program
    {
        static void Main(string[] args)
        {
            WorkflowEvaluator.BeginWorkflow();

            WorkflowEvaluator.AddProcess("Main Process", "Main Process");

            var myClassWithProperty = new MyClassWithProperty();

            myClassWithProperty.MyClassForProperty1 = new MyClassForProperty1();
            myClassWithProperty.MyClassForProperty2 = new MyClassForProperty2();

            myClassWithProperty.MyClassForProperty1.MyMethod1();
            myClassWithProperty.MyClassForProperty2.MyMethod2();

            WorkflowEvaluator.StopWorkflow();
        }
    }
}
