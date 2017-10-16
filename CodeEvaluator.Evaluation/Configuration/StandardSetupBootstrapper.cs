using CodeEvaluator.Evaluation.Common;
using CodeEvaluator.Evaluation.Interfaces;
using CodeEvaluator.Evaluation.Members;
using CodeEvaluator.Evaluation.Members.Finalizers;
using StructureMap;

namespace CodeEvaluator.Evaluation.Configuration
{
    public static class StandardSetupBootstrapper
    {
        public static void RegisterStandardComponents()
        {
            ObjectFactory.Configure(config => config.For<IParsedSourceFilesCache>().Use(new ParsedSourceFilesCache()));
            ObjectFactory.Configure(config => config.SetAllProperties(x => x.OfType<IParsedSourceFilesCache>()));

            ObjectFactory.Configure(
                config => config.For<ISyntaxNodeEvaluatorFactory>().Use(new SyntaxNodeEvaluatorFactory()));
            ObjectFactory.Configure(config => config.SetAllProperties(x => x.OfType<ISyntaxNodeEvaluatorFactory>()));

            ObjectFactory.Configure(
                config => config.For<ISyntaxNodeNamespaceProvider>().Use(() => new SyntaxNodeNamespaceProvider()));
            ObjectFactory.Configure(config => config.SetAllProperties(x => x.OfType<ISyntaxNodeNamespaceProvider>()));

            ObjectFactory.Configure(config => config.For<ICodeEvaluator>().Use(() => new Common.CodeEvaluator()));

            ObjectFactory.Configure(config => config.For<IParsedSourceFilesCache>().Use(new ParsedSourceFilesCache()));
            ObjectFactory.Configure(config => config.SetAllProperties(x => x.OfType<IParsedSourceFilesCache>()));

            ObjectFactory.Configure(
                config => config.For<IKeywordToTypeInfoRemapper>().Use(new KeywordToTypeInfoRemapper()));
            ObjectFactory.Configure(config => config.SetAllProperties(x => x.OfType<IKeywordToTypeInfoRemapper>()));

            ObjectFactory.Configure(config => config.For<IEvaluatedTypesInfoTable>()
                .Use(new EvaluatedTypesInfoTable()));
            ObjectFactory.Configure(config => config.SetAllProperties(x => x.OfType<IEvaluatedTypesInfoTable>()));
            ObjectFactory.Configure(
                config => config.For<IEvaluatedObjectAllocator>().Use(new EvaluatedObjectAllocator()));

            ObjectFactory.Configure(config => config.For<IEvaluatedObjectsHeap>().Use(new EvaluatedObjectsHeap()));
            ObjectFactory.Configure(config => config.SetAllProperties(x => x.OfType<IEvaluatedObjectsHeap>()));
            ObjectFactory.Configure(
                config => config.For<IEvaluatorExecutionFrameFactory>().Use(new EvaluatorExecutionFrameFactory()));
            ObjectFactory.Configure(config => config.SetAllProperties(x => x.OfType<IEvaluatedObjectAllocator>()));

            ObjectFactory.Configure(config => config.For<IMethodSignatureComparer>()
                .Use(new MethodSignatureComparer()));
            ObjectFactory.Configure(config => config.SetAllProperties(x => x.OfType<IMethodSignatureComparer>()));

            ObjectFactory.Configure(
                config => config.For<IInheritanceChainResolver>().Use(new InheritanceChainResolver()));
            ObjectFactory.Configure(config => config.SetAllProperties(x => x.OfType<IInheritanceChainResolver>()));

            ObjectFactory.Configure(
                config => config.For<IMethodInvocationResolver>().Use(new MethodInvocationResolver()));
            ObjectFactory.Configure(config => config.SetAllProperties(x => x.OfType<IMethodInvocationResolver>()));

            ObjectFactory.Configure(config => config.For<IEvaluatedTypeInfoFinalizer>()
                .AddInstances(a => a.Object(new AddDefaultConstructorFinalizer())));
        }
    }
}