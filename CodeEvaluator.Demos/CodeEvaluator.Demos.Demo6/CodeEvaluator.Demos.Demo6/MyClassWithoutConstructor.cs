using CodeEvaluator.Workflows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeEvaluator.Demos.Demo6
{
    public class MyClassWithoutConstructor
    {
        public void MyMethod()
        {
            WorkflowEvaluator.AddProcess("Process default contructor", "Process default contructor");
        }
    }
}
