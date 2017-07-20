namespace CodeEvaluator.Evaluation.Configuration
{
    using CodeEvaluator.Evaluation.Common;
    using CodeEvaluator.Evaluation.Interfaces;
    using CodeEvaluator.Evaluation.Members;

    using StructureMap;

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
            ObjectFactory.Configure(
                config => config.For<ICodeEvaluator>().Use(() => new Common.CodeEvaluator()));
            ObjectFactory.Configure(config => config.For<IParsedSourceFilesCache>().Use(new ParsedSourceFilesCache()));
            ObjectFactory.Configure(config => config.SetAllProperties(x => x.OfType<IParsedSourceFilesCache>()));
            ObjectFactory.Configure(
                config => config.For<IEvaluatedTypesInfoTable>().Use(new EvaluatedTypesInfoTable()));
            ObjectFactory.Configure(config => config.SetAllProperties(x => x.OfType<IEvaluatedTypesInfoTable>()));
            ObjectFactory.Configure(
                config => config.For<IEvaluatedObjectAllocator>().Use(new EvaluatedObjectAllocator()));
            ObjectFactory.Configure(config => config.For<IEvaluatedObjectsHeap>().Use(new EvaluatedObjectsHeap()));
            ObjectFactory.Configure(config => config.SetAllProperties(x => x.OfType<IEvaluatedObjectsHeap>()));
            ObjectFactory.Configure(
                config =>
                    config.For<IEvaluatorExecutionFrameFactory>()
                        .Use(new EvaluatorExecutionFrameFactory()));

            ObjectFactory.Configure(config => config.SetAllProperties(x => x.OfType<IEvaluatedObjectAllocator>()));
        }
    }
}