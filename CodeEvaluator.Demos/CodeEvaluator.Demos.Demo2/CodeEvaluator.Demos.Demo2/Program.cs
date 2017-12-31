using CodeEvaluator.Workflows;

namespace CodeEvaluator.Demos.Demo2
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            WorkflowEvaluator.Initialize();

            WorkflowEvaluator.BeginWorkflow();

            Process1();

            Process2();

            WorkflowEvaluator.AddDecision("Decission1", "Decission1", "Decission1");

            WorkflowEvaluator.BeginWorkflow();

            WorkflowEvaluator.AddProcess("Process3", "Process3", "Process3");

            WorkflowEvaluator.EndWorkflow();

            WorkflowEvaluator.BeginWorkflow();

            WorkflowEvaluator.AddProcess("Process55", "Process55", "Process55");

            WorkflowEvaluator.EndWorkflow();

            WorkflowEvaluator.AddProcess("Process4345", "Process4345", "Process4345");

            WorkflowEvaluator.StopWorkflow();
        }

        private static void Process2()
        {
            WorkflowEvaluator.AddProcess("Process2","Process2", "Process2");
        }

        private static void Process1()
        {
            WorkflowEvaluator.AddProcess("Process1", "Process1", "Process1");
        }
    }
}