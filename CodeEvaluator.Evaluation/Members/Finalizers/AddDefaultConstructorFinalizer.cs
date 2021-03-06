﻿using System.Linq;
using CodeEvaluator.Evaluation.Extensions;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeEvaluator.Evaluation.Members.Finalizers
{
    public class AddDefaultConstructorFinalizer : EvaluatedTypeInfoFinalizer
    {
        private const string ConstructorFormat = @"namespace {0} 
            {{
                partial {2} {1} 
                {{
                    public {1}() 
                    {{
                    }}
                }}
            }}";

        public override void FinalizeTypeInfo(EvaluatedTypeInfo evaluatedTypeInfo)
        {
            if (!evaluatedTypeInfo.Constructors.Any() && evaluatedTypeInfo.IsStatic() == false)
            {
                var typeKind = GetTypeKind(evaluatedTypeInfo);
                var generatedConstructorCode = string.Format(ConstructorFormat,
                    GetNamespace(evaluatedTypeInfo.IdentifierText, evaluatedTypeInfo.FullIdentifierText),
                    evaluatedTypeInfo.IdentifierText,
                    typeKind);


                var syntaxTree = CSharpSyntaxTree.ParseText(generatedConstructorCode);
                var rootNode = syntaxTree.GetRoot();
                var namespaceDeclaration = (NamespaceDeclarationSyntax) rootNode.ChildNodes().First();
                var typeDeclaration = (BaseTypeDeclarationSyntax) namespaceDeclaration.ChildNodes().Skip(1).First();
                var constructorDeclaration =
                    (ConstructorDeclarationSyntax) typeDeclaration.ChildNodes().First();

                var evaluatedConstructor = new EvaluatedConstructor();
                evaluatedConstructor.ReturnType = evaluatedTypeInfo;
                evaluatedConstructor.Declaration = constructorDeclaration;
                evaluatedConstructor.Identifier = constructorDeclaration.Identifier;
                evaluatedConstructor.IdentifierText = evaluatedTypeInfo.IdentifierText;
                evaluatedConstructor.FullIdentifierText = evaluatedTypeInfo.FullIdentifierText;

                evaluatedTypeInfo.Constructors.Add(evaluatedConstructor);
            }
        }

        public override int Priority
        {
            get { return 0; }
        }
    }
}