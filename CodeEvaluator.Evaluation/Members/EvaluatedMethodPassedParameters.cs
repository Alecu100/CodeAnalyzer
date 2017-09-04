using System.Collections.Generic;

namespace CodeEvaluator.Evaluation.Members
{
    public class EvaluatedMethodPassedParameters
    {
        public List<EvaluatedMethodPassedParameter> MandatoryMethodParameters { get; } =
            new List<EvaluatedMethodPassedParameter>();

        public List<EvaluatedMethodPassedParameter> OptionalMethodParameters { get; } =
            new List<EvaluatedMethodPassedParameter>();

        public EvaluatedMethodPassedParameter ThisReference { get; set; } = new EvaluatedMethodPassedParameter();

        public EvaluatedMethodPassedParameters Copy()
        {
            var evaluatedMethodPassedParameters = new EvaluatedMethodPassedParameters();

            foreach (var evaluatedMethodPassedParameter in MandatoryMethodParameters)
            {
                var methodPassedParameter = new EvaluatedMethodPassedParameter();
                methodPassedParameter.AssignEvaluatedObject(evaluatedMethodPassedParameter);

                evaluatedMethodPassedParameters.MandatoryMethodParameters.Add(methodPassedParameter);
            }

            foreach (var evaluatedMethodPassedParameter in OptionalMethodParameters)
            {
                var methodPassedParameter = new EvaluatedMethodPassedParameter();
                methodPassedParameter.AssignEvaluatedObject(evaluatedMethodPassedParameter);

                evaluatedMethodPassedParameters.OptionalMethodParameters.Add(methodPassedParameter);
            }

            return evaluatedMethodPassedParameters;
        }
    }
}