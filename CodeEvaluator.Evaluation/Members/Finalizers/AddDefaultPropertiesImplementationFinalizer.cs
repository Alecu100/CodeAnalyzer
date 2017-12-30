using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeEvaluator.Evaluation.Members.Finalizers
{
    public class AddDefaultPropertiesImplementationFinalizer : EvaluatedTypeInfoFinalizer
    {
        private const string BackingFieldFormat = @"namespace {0} 
            {{
                partial {2} {1} 
                {{
                    private {3} _{4}_k__BackingField;
                }}
            }}";

        private const string GetterFormat = @"namespace {0} 
            {{
                partial {2} {1} 
                {{
                    public {3} {4}
                    {{
                        get
                        {{
                            return _{4}_k__BackingField;
                        }}
                    }}
                }}
            }}";

        private const string SetterFormat = @"namespace {0} 
            {{
                partial {2} {1} 
                {{
                    public {3} {4}
                    {{
                        set
                        {{
                            _{4}_k__BackingField = value;
                        }}
                    }}
                }}
            }}";

        public override void FinalizeTypeInfo(EvaluatedTypeInfo evaluatedTypeInfo)
        {
            foreach (var accesibleProperty in evaluatedTypeInfo.AccesibleProperties)
                if (accesibleProperty.IsAutoProperty && !accesibleProperty.IsFinalized)
                {
                    GenerateBackingField(evaluatedTypeInfo, accesibleProperty);

                    if (accesibleProperty.PropertyGetAccessor != null)
                        GenerateGetter(evaluatedTypeInfo, accesibleProperty);

                    if (accesibleProperty.PropertyGetAccessor != null)
                        GenerateSetter(evaluatedTypeInfo, accesibleProperty);
                }
        }

        private void GenerateSetter(EvaluatedTypeInfo evaluatedTypeInfo, EvaluatedProperty accesibleProperty)
        {
            var typeKind = GetTypeKind(evaluatedTypeInfo);

            var generatedSetterCode = string.Format(SetterFormat,
                GetNamespace(evaluatedTypeInfo.IdentifierText, evaluatedTypeInfo.FullIdentifierText),
                evaluatedTypeInfo.IdentifierText,
                typeKind,
                accesibleProperty.TypeInfo.FullIdentifierText,
                accesibleProperty.IdentifierText);


            var syntaxTree = CSharpSyntaxTree.ParseText(generatedSetterCode);
            var rootNode = syntaxTree.GetRoot();
            var namespaceDeclaration = (NamespaceDeclarationSyntax) rootNode.ChildNodes().First();
            var typeDeclaration = (BaseTypeDeclarationSyntax) namespaceDeclaration.ChildNodes().Skip(1).First();

            var propertyDeclarationSyntax =
                (PropertyDeclarationSyntax) typeDeclaration.ChildNodes().First();

            accesibleProperty.PropertySetAccessor.Declaration =
                propertyDeclarationSyntax.AccessorList.Accessors.First();
        }

        private void GenerateGetter(EvaluatedTypeInfo evaluatedTypeInfo, EvaluatedProperty accesibleProperty)
        {
            var typeKind = GetTypeKind(evaluatedTypeInfo);

            var generatedGetterCode = string.Format(GetterFormat,
                GetNamespace(evaluatedTypeInfo.IdentifierText, evaluatedTypeInfo.FullIdentifierText),
                evaluatedTypeInfo.IdentifierText,
                typeKind,
                accesibleProperty.TypeInfo.FullIdentifierText,
                accesibleProperty.IdentifierText);


            var syntaxTree = CSharpSyntaxTree.ParseText(generatedGetterCode);
            var rootNode = syntaxTree.GetRoot();
            var namespaceDeclaration = (NamespaceDeclarationSyntax) rootNode.ChildNodes().First();
            var typeDeclaration = (BaseTypeDeclarationSyntax) namespaceDeclaration.ChildNodes().Skip(1).First();

            var propertyDeclarationSyntax =
                (PropertyDeclarationSyntax) typeDeclaration.ChildNodes().First();

            accesibleProperty.PropertyGetAccessor.Declaration =
                propertyDeclarationSyntax.AccessorList.Accessors.First();
        }

        private void GenerateBackingField(EvaluatedTypeInfo evaluatedTypeInfo, EvaluatedProperty accesibleProperty)
        {
            var typeKind = GetTypeKind(evaluatedTypeInfo);
            var generatedBackingFieldCode = string.Format(BackingFieldFormat,
                GetNamespace(evaluatedTypeInfo.IdentifierText, evaluatedTypeInfo.FullIdentifierText),
                evaluatedTypeInfo.IdentifierText,
                typeKind,
                accesibleProperty.TypeInfo.FullIdentifierText,
                accesibleProperty.IdentifierText);


            var syntaxTree = CSharpSyntaxTree.ParseText(generatedBackingFieldCode);
            var rootNode = syntaxTree.GetRoot();
            var namespaceDeclaration = (NamespaceDeclarationSyntax) rootNode.ChildNodes().First();
            var typeDeclaration = (BaseTypeDeclarationSyntax) namespaceDeclaration.ChildNodes().Skip(1).First();

            var fieldDeclarationSyntax =
                (FieldDeclarationSyntax) typeDeclaration.ChildNodes().First();
            var variableDeclaratorSyntax = fieldDeclarationSyntax.Declaration.Variables.First();
            var evaluatedField = new EvaluatedField();

            evaluatedField.Declaration = fieldDeclarationSyntax;

            evaluatedField.Identifier = variableDeclaratorSyntax.Identifier;
            evaluatedField.IdentifierText = variableDeclaratorSyntax.Identifier.ValueText;
            evaluatedField.FullIdentifierText =
                evaluatedTypeInfo.FullIdentifierText + "." + evaluatedField.IdentifierText;
            evaluatedField.MemberFlags = EMemberFlags.Private;

            evaluatedTypeInfo.SpecificFields.Add(evaluatedField);
            evaluatedTypeInfo.AccesibleFields.Add(evaluatedField);
        }
    }
}