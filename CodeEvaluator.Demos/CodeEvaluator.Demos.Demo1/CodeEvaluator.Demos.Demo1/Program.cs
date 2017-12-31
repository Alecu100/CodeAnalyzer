using CodeEvaluator.Workflows;

namespace CodeEvaluator.Demos.Demo1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            WorkflowEvaluator.Initialize();

            WorkflowEvaluator.BeginWorkflow();

            Process1();

            Process2();

            WorkflowEvaluator.EndWorkflow();
        }

        private static void Process2()
        {
            WorkflowEvaluator.AddProcess("Process2", "Process2", "Process2");
        }

        private static void Process1()
        {
            WorkflowEvaluator.AddProcess("Process1", "Process1", "Process1");
        }
    }
}