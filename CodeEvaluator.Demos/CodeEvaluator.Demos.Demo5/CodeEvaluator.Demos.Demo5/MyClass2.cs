using CodeEvaluator.Workflows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeEvaluator.Demos.Demo5
{
    public class MyClass2 : MyClassBase
    {
        public override void MyMethod()
        {
            WorkflowEvaluator.AddProcess("MyClass2 Method2", "MyClass2 Method2");
        }

        public MyClass2()
        {

        }
    }
}
