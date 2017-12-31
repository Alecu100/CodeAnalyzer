using CodeEvaluator.Workflows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeEvaluator.Demos.Demo5
{
    public class MyClass3 : MyClassBase
    {
        public override void MyMethod()
        {
            WorkflowEvaluator.AddProcess("MyClass3 Method3", "MyClass3 Method3", "MyClass3 Method3");
        }

        public MyClass3()
        {

        }
    }
}
