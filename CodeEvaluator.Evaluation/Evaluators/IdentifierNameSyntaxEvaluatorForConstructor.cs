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
        protected override void EvaluateSyntaxNodeInternal(SyntaxNode syntaxNode, CodeEvaluatorExecutionStack workflowEvaluatorExecutionStack)
        {
            var identifierNameSyntax = (IdentifierNameSyntax)syntaxNode;
            var evaluatedTypesInfoTable = ObjectFactory.GetInstance<IEvaluatedTypesInfoTable>();

            var evaluatedTypeInfo = evaluatedTypesInfoTable.GetTypeInfo(identifierNameSyntax.Identifier.ValueText,
                workflowEvaluatorExecutionStack.CurrentExecutionFrame.ThisReference.TypeInfo);

            var reference = new EvaluatedObjectDirectReference();

            foreach (var evaluatedConstructor in evaluatedTypeInfo.Constructors)
            {
                var evaluatedDelegate =
                    new EvaluatedDelegate(
                        evaluatedTypeInfo, evaluatedConstructor);
                reference.AssignEvaluatedObject(evaluatedDelegate);
            }

            workflowEvaluatorExecutionStack.CurrentExecutionFrame.MemberAccessReference = reference;
        }
    }
}
