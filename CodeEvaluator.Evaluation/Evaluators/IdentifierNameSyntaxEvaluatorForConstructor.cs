using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeEvaluator.Evaluation.Common;
using CodeEvaluator.Evaluation.Interfaces;
using CodeEvaluator.Evaluation.Members;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using StructureMap;

namespace CodeEvaluator.Evaluation.Evaluators
{
    public class IdentifierNameSyntaxEvaluatorForConstructor : BaseMethodDeclarationSyntaxEvaluator
    {
        protected override void EvaluateSyntaxNodeInternal(SyntaxNode syntaxNode, CodeEvaluatorExecutionState workflowEvaluatorExecutionState)
        {
            var identifierNameSyntax = (IdentifierNameSyntax)syntaxNode;
            var evaluatedTypesInfoTable = ObjectFactory.GetInstance<IEvaluatedTypesInfoTable>();

            var evaluatedTypeInfo = evaluatedTypesInfoTable.GetTypeInfo(identifierNameSyntax.Identifier.ValueText,
                workflowEvaluatorExecutionState.CurrentExecutionFrame.ThisReference.TypeInfo);

            var reference = new EvaluatedObjectReference();

            foreach (var evaluatedConstructor in evaluatedTypeInfo.Constructors)
            {
                var evaluatedDelegate =
                    new EvaluatedDelegate(
                        evaluatedTypeInfo, evaluatedConstructor);
                reference.AssignEvaluatedObject(evaluatedDelegate);
            }

            workflowEvaluatorExecutionState.CurrentExecutionFrame.MemberAccessReference = reference;
        }
    }
}
