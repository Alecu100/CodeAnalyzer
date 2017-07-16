using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeEvaluator.Workflows;

namespace CodeEvaluator.Demos.Demo3
{
    public class MyClass1
    {
        public MyClass1()
        {
            
        }

        public void Method1()
        {
            WorkflowEvaluator.AddProcess("Method1 Process1", "Method1 Process1");
        }

        public void Method2()
        {
            WorkflowEvaluator.AddProcess("Method2 Process1", "Method2 Process1");
        }
    }
}
