using System;

namespace CodeEvaluator.Workflows.Enums
{
    [Serializable]
    public enum EWorkflowStepType
    {
        None,

        Decision,

        Process,

        Start,

        Stop
    }
}