using CodeEvaluator.Workflows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeEvaluator.Demos.Demo4
{
    public static class StaticMyClass
    {
        public static void StaticMyMethod()
        {
            WorkflowEvaluator.AddProcess("StaticClass Process1", "StaticClass Process1", "StaticClass Process1");
        }
    }
}
