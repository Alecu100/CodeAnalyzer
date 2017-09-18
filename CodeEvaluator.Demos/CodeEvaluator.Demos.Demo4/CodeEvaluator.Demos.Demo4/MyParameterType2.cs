using CodeEvaluator.Workflows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeEvaluator.Demos.Demo4
{
    public class MyParameterType2 : IMyParameterType2
    {
        public void Method()
        {
            WorkflowEvaluator.AddProcess("MyParameterType2 Process1", "MyParameterType2 Process1");
        }

        public MyParameterType2()
        {

        }
    }
}
