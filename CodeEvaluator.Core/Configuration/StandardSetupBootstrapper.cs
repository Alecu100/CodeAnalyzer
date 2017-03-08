using CodeAnalysis.Core.Common;
using CodeAnalysis.Core.Interfaces;
using CodeAnalysis.Core.Members;
using StructureMap;

namespace CodeAnalysis.Core.Configuration
{
    public static class StandardSetupBootstrapper
    {
        public static void RegisterStandardComponents()
        {
            ObjectFactory.Configure(config => config.For<IParsedSourceFilesCache>().Use(new ParsedSourceFilesCache()));
            ObjectFactory.Configure(config => config.SetAllProperties(x => x.OfType<IParsedSourceFilesCache>()));
            ObjectFactory.Configure(config => config.For<IProjectFilesProvider>().Use(() => new ProjectFilesProvider()));
            ObjectFactory.Configure(config => config.SetAllProperties(x => x.OfType<IProjectFilesProvider>()));
            ObjectFactory.Configure(
                config => config.For<ISyntaxNodeEvaluatorFactory>().Use(new SyntaxNodeEvaluatorFactory()));
            ObjectFactory.Configure(config => config.SetAllProperties(x => x.OfType<ISyntaxNodeEvaluatorFactory>()));
            ObjectFactory.Configure(
                config => config.For<ISyntaxNodeNamespaceProvider>().Use(() => new SyntaxNodeNamespaceProvider()));
            ObjectFactory.Configure(config => config.SetAllProperties(x => x.OfType<ISyntaxNodeNamespaceProvider>()));
            ObjectFactory.Configure(
                config => config.For<ICodeEvaluator>().Use(() => new CodeEvaluator()));
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
            ObjectFactory.Configure(config => config.For<ISystemSettings>().Use(new SystemSettings()));
            ObjectFactory.Configure(config => config.SetAllProperties(x => x.OfType<ISystemSettings>()));
        }
    }
}