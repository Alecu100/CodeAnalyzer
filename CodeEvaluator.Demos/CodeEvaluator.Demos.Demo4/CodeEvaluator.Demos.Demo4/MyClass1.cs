using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeEvaluator.Workflows;

namespace CodeEvaluator.Demos.Demo4
{
    public class MyClass1
    {
        public MyClass1()
        {

        }

        public void Method1(string parameter1, string parameters = null)
        {
            WorkflowEvaluator.AddProcess("Method1 Process1", "Method1 Process1");
        }

        public void Method2(int parameter1, string parameter2 = null)
        {
            WorkflowEvaluator.AddProcess("Method2 Process1", "Method2 Process1");
        }
    }
}
