using CodeEvaluator.Workflows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeEvaluator.Demos.Demo5
{
    public class MyClass1 : MyClassBase
    {
        public override void MyMethod()
        {
            WorkflowEvaluator.AddProcess("MyClass1 Method1", "MyClass1 Method1", "MyClass1 Method1");
        }

        public MyClass1()
        {

        }
    }
}
