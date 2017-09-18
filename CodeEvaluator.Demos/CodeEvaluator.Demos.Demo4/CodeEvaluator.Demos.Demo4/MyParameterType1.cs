using CodeEvaluator.Workflows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeEvaluator.Demos.Demo4
{
    public class MyParameterType1 : IMyParameterType1
    {
        public void Method()
        {
            WorkflowEvaluator.AddProcess("MyParameterType1 Process1", "MyParameterType1 Process1");
        }

        public MyParameterType1()
        {

        }
    }
}
