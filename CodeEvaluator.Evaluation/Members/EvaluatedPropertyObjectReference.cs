using System.Collections.Generic;
using CodeEvaluator.Evaluation.Extensions;
using CodeEvaluator.Evaluation.Interfaces;
using StructureMap;

namespace CodeEvaluator.Evaluation.Members
{
    public class EvaluatedPropertyObjectReference : EvaluatedObjectReference
    {
        public override IReadOnlyList<EvaluatedObject> EvaluatedObjects
        {
            get
            {
                var evaluatedObjects = new List<EvaluatedObject>();

                foreach (var evaluatedPropertyObject in EvaluatedPropertyObjects)
                {
                    var objects = CallGetValue(evaluatedPropertyObject);

                    if (objects.IsNotNull())
                        evaluatedObjects.AddRange(objects.EvaluatedObjects);
                }

                return evaluatedObjects;
            }
        }

        public List<EvaluatedPropertyObject> EvaluatedPropertyObjects { get; } = new List<EvaluatedPropertyObject>();

        private EvaluatedObjectReference CallGetValue(EvaluatedPropertyObject evaluatedPropertyObject)
        {
            var methodInvocationResolver = ObjectFactory.GetInstance<IMethodInvocationResolver>();
            var syntaxNodeEvaluatorFactory = ObjectFactory.GetInstance<ISyntaxNodeEvaluatorFactory>();

            var methodInvocationResolverResult = methodInvocationResolver.ResolveMethodInvocation(
                new List<EvaluatedMethodBase> {evaluatedPropertyObject.TargetProperty.PropertyGetAccessor},
                evaluatedPropertyObject.TargetObject, evaluatedPropertyObject.TypeInfo,
                new List<EvaluatedObjectReference>(), new Dictionary<string, EvaluatedObjectReference>());

            if (methodInvocationResolverResult.CanInvokeMethod)
            {
                var previousPassedArguments = new Dictionary<int, EvaluatedObjectReference>();

                var passedMethodParameters = evaluatedPropertyObject.CodeEvaluatorExecutionStack.CurrentExecutionFrame
                    .PassedMethodParameters;

                foreach (var evaluatedObjectReference in passedMethodParameters)
                    previousPassedArguments[evaluatedObjectReference.Key] = evaluatedObjectReference.Value;

                passedMethodParameters.Clear();

                foreach (var evaluatedObjectReference in methodInvocationResolverResult.ResolvedPassedParameters)
                    evaluatedPropertyObject.CodeEvaluatorExecutionStack.CurrentExecutionFrame.PassedMethodParameters[
                        evaluatedObjectReference.Key] = evaluatedObjectReference.Value;

                var methodEvaluator =
                    syntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(
                        methodInvocationResolverResult.ResolvedMethod.Declaration,
                        EEvaluatorActions.None);

                if (methodEvaluator != null)
                    methodEvaluator.EvaluateSyntaxNode(
                        methodInvocationResolverResult.ResolvedMethod.Declaration,
                        evaluatedPropertyObject.CodeEvaluatorExecutionStack);

                passedMethodParameters.Clear();

                foreach (var evaluatedObjectReference in previousPassedArguments)
                    passedMethodParameters[evaluatedObjectReference.Key] = evaluatedObjectReference.Value;

                return evaluatedPropertyObject.CodeEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference;
            }

            return null;
        }

        public override void AssignEvaluatedObject(EvaluatedObject evaluatedObject)
        {
            foreach (var evaluatedPropertyObject in EvaluatedPropertyObjects)
                CallSetValue(evaluatedPropertyObject, evaluatedObject);
        }

        private void CallSetValue(EvaluatedPropertyObject evaluatedPropertyObject, EvaluatedObject evaluatedObject)
        {
            var methodInvocationResolver = ObjectFactory.GetInstance<IMethodInvocationResolver>();
            var syntaxNodeEvaluatorFactory = ObjectFactory.GetInstance<ISyntaxNodeEvaluatorFactory>();

            var methodInvocationResolverResult = methodInvocationResolver.ResolveMethodInvocation(
                new List<EvaluatedMethodBase> {evaluatedPropertyObject.TargetProperty.PropertySetAccessor},
                evaluatedPropertyObject.TargetObject, evaluatedPropertyObject.TypeInfo,
                new List<EvaluatedObjectReference> {new EvaluatedObjectDirectReference(evaluatedObject)},
                new Dictionary<string, EvaluatedObjectReference>());

            if (methodInvocationResolverResult.CanInvokeMethod)
            {
                var previousPassedArguments = new Dictionary<int, EvaluatedObjectReference>();

                var passedMethodParameters = evaluatedPropertyObject.CodeEvaluatorExecutionStack.CurrentExecutionFrame
                    .PassedMethodParameters;

                foreach (var evaluatedObjectReference in passedMethodParameters)
                    previousPassedArguments[evaluatedObjectReference.Key] = evaluatedObjectReference.Value;

                passedMethodParameters.Clear();

                foreach (var evaluatedObjectReference in methodInvocationResolverResult.ResolvedPassedParameters)
                    evaluatedPropertyObject.CodeEvaluatorExecutionStack.CurrentExecutionFrame.PassedMethodParameters[
                        evaluatedObjectReference.Key] = evaluatedObjectReference.Value;

                var methodEvaluator =
                    syntaxNodeEvaluatorFactory.GetSyntaxNodeEvaluator(
                        methodInvocationResolverResult.ResolvedMethod.Declaration,
                        EEvaluatorActions.None);

                if (methodEvaluator != null)
                    methodEvaluator.EvaluateSyntaxNode(
                        methodInvocationResolverResult.ResolvedMethod.Declaration,
                        evaluatedPropertyObject.CodeEvaluatorExecutionStack);

                passedMethodParameters.Clear();

                foreach (var evaluatedObjectReference in previousPassedArguments)
                    passedMethodParameters[evaluatedObjectReference.Key] = evaluatedObjectReference.Value;
            }
        }

        public override void AssignEvaluatedObject(EvaluatedObjectReference evaluatedObjectReference)
        {
            foreach (var evaluatedObject in evaluatedObjectReference.EvaluatedObjects)
                AssignEvaluatedObject(evaluatedObject);
        }

        public override void AssignEvaluatedObjects(IEnumerable<EvaluatedObject> evaluatedObjects)
        {
            foreach (var evaluatedObject in evaluatedObjects)
                AssignEvaluatedObject(evaluatedObject);
        }

        public void AssignEvaluatedProperty(EvaluatedPropertyObject evaluatedPropertyObject)
        {
            EvaluatedPropertyObjects.Add(evaluatedPropertyObject);
        }
    }
}