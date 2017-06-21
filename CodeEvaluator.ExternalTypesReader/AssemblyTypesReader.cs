using System;
using System.Collections.Generic;
using System.Reflection;
using CodeEvaluator.Bootstrapper;
using CodeEvaluator.Dto;

namespace CodeEvaluator.ExternalTypesReader
{
    public class AssemblyTypesReader : MarshalByRefObject, IAssemblyTypesReader
    {
        public List<EvaluatedTypeInfoDto> ReadTypeInfos(List<string> assemblyFileNames)
        {
            var domaininfo = new AppDomainSetup();
            var assemblyLocation =
                typeof(AssemblyBootstrapper).Assembly.Location.Replace(
                    typeof(AssemblyBootstrapper).Assembly.GetName().Name + ".dll", "");

            domaininfo.ApplicationBase = assemblyLocation;
            domaininfo.ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;

            var assembliesToLoad = new List<string>();

            var referencedAssemblies = GetType().Assembly.GetReferencedAssemblies();

            foreach (var referencedAssembly in referencedAssemblies)
            {
                assembliesToLoad.Add(referencedAssembly.Name);
            }

            referencedAssemblies = typeof(AssemblyTypesReader).Assembly.GetReferencedAssemblies();

            foreach (var referencedAssembly in referencedAssemblies)
            {
                assembliesToLoad.Add(referencedAssembly.Name);
            }

            var adEvidence = AppDomain.CurrentDomain.Evidence;

            var remoteAppDomain = AppDomain.CreateDomain(Guid.NewGuid().ToString(), adEvidence, domaininfo);

            try
            {
                remoteAppDomain.Load(typeof(AssemblyBootstrapper).Assembly.GetName());
            }
            catch (Exception)
            {
            }

            AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolver.AppDomain_AssemblyResolve;

            try
            {
                var remoteAssemblyResolver =
                    (AssemblyBootstrapper)
                        remoteAppDomain.CreateInstanceAndUnwrap(typeof(AssemblyBootstrapper).Assembly.FullName,
                            typeof(AssemblyBootstrapper).FullName);

                remoteAssemblyResolver.LoadAssemblies(
                    new List<string> {assemblyLocation, AppDomain.CurrentDomain.BaseDirectory},
                    assembliesToLoad);

                var instanceFrom =
                    (AssemblyTypesReader)
                        remoteAppDomain.CreateInstanceAndUnwrap(GetType().Assembly.GetName().Name, GetType().FullName);

                var readTypeInfosRemote = instanceFrom.ReadTypeInfosRemote(
                    assemblyFileNames);

                return readTypeInfosRemote;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                AppDomain.Unload(remoteAppDomain);

                AppDomain.CurrentDomain.AssemblyResolve -= AssemblyResolver.AppDomain_AssemblyResolve;
            }
        }

        public
            List<EvaluatedTypeInfoDto> ReadTypeInfosRemote
            (List<string>
                assemblyFileNames)
        {
            var evaluatedTypeInfoLinks = new Dictionary<Type, EvaluatedTypeInfoDto>();
            var evaluatedTypeInfos = new List<EvaluatedTypeInfoDto>();

            var assemblyBootstrapper = new AssemblyBootstrapper();

            var assemblyDirectories = new List<string>();

            foreach (var assemblyFileName in assemblyFileNames)
            {
                var nameParts = assemblyFileName.Split('\\', '/');

                var assemblyDirectory = assemblyFileName.Replace(nameParts[nameParts.Length - 1], "");

                assemblyDirectories.Add(assemblyDirectory);
            }

            assemblyBootstrapper.LoadAssemblies(assemblyDirectories, assemblyFileNames);

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            try
            {
                foreach (var assembly in assemblies)
                {
                    LoadTypesFromAssembly(assembly, evaluatedTypeInfos, evaluatedTypeInfoLinks);

                    LinkTypes(evaluatedTypeInfos, evaluatedTypeInfoLinks);
                }
            }
            catch (Exception)
            {
                throw;
            }


            return evaluatedTypeInfos;
        }

        private void LinkTypes(List<EvaluatedTypeInfoDto> evaluatedTypeInfos,
            Dictionary<Type, EvaluatedTypeInfoDto> evaluatedTypeInfoLinks)
        {
            var stillHasMissingTypes = true;

            while (stillHasMissingTypes)
            {
                stillHasMissingTypes = false;

                var evaluatedTypeInfoDtosClone = evaluatedTypeInfos.ToArray();

                foreach (var evaluatedTypeInfoDto in evaluatedTypeInfoDtosClone)
                {
                    var processedTypesToRemove = new List<Type>();

                    foreach (var originalBaseType in evaluatedTypeInfoDto.OriginalBaseTypes)
                    {
                        if (evaluatedTypeInfoLinks.ContainsKey(originalBaseType))
                        {
                            processedTypesToRemove.Add(originalBaseType);
                            evaluatedTypeInfoDto.BaseTypeInfos.Add(evaluatedTypeInfoLinks[originalBaseType]);
                        }
                        else
                        {
                            stillHasMissingTypes = true;
                            AddTypeInfoIfNeeded(evaluatedTypeInfos, evaluatedTypeInfoLinks, originalBaseType);
                        }
                    }

                    foreach (var processedType in processedTypesToRemove)
                    {
                        evaluatedTypeInfoDto.OriginalBaseTypes.Remove(processedType);
                    }
                }
            }

            foreach (var evaluatedTypeInfoDto in evaluatedTypeInfos)
            {
                foreach (var evaluatedMethodDto in evaluatedTypeInfoDto.Constructors)
                {
                    foreach (var evaluatedTypedMemberDto in evaluatedMethodDto.Parameters)
                    {
                        evaluatedTypedMemberDto.TypeInfo = evaluatedTypeInfoLinks[evaluatedTypedMemberDto.OriginalType];
                    }

                    evaluatedMethodDto.TypeInfo = evaluatedTypeInfoLinks[evaluatedMethodDto.OriginalType];
                }

                foreach (var evaluatedTypedMemberDto in evaluatedTypeInfoDto.Fields)
                {
                    evaluatedTypedMemberDto.TypeInfo = evaluatedTypeInfoLinks[evaluatedTypedMemberDto.OriginalType];
                }

                foreach (var evaluatedPropertyDto in evaluatedTypeInfoDto.Properties)
                {
                    evaluatedPropertyDto.Getter.TypeInfo =
                        evaluatedTypeInfoLinks[evaluatedPropertyDto.Getter.OriginalType];

                    evaluatedPropertyDto.Setter.TypeInfo =
                        evaluatedTypeInfoLinks[evaluatedPropertyDto.Setter.OriginalType];
                }

                foreach (var evaluatedMethodDto in evaluatedTypeInfoDto.Methods)
                {
                    evaluatedMethodDto.TypeInfo = evaluatedTypeInfoLinks[evaluatedMethodDto.OriginalType];

                    foreach (var evaluatedTypedMemberDto in evaluatedMethodDto.Parameters)
                    {
                        evaluatedTypedMemberDto.TypeInfo = evaluatedTypeInfoLinks[evaluatedTypedMemberDto.OriginalType];
                    }
                }
            }
        }

        private void LoadTypesFromAssembly(Assembly assembly, List<EvaluatedTypeInfoDto> evaluatedTypeInfos,
            Dictionary<Type, EvaluatedTypeInfoDto> evaluatedTypeInfoLinks)
        {
            foreach (var type in assembly.GetTypes())
            {
                AddTypeInfoIfNeeded(evaluatedTypeInfos, evaluatedTypeInfoLinks, type);
            }
        }

        private void AddTypeInfoIfNeeded(List<EvaluatedTypeInfoDto> evaluatedTypeInfos,
            Dictionary<Type, EvaluatedTypeInfoDto> evaluatedTypeInfoLinks, Type type)
        {
            if (!evaluatedTypeInfoLinks.ContainsKey(type))
            {
                var evaluatedTypeInfoDto = new EvaluatedTypeInfoDto();

                evaluatedTypeInfoDto.IdentifierText = type.Name;
                evaluatedTypeInfoDto.FullIdentifierText = type.FullName;

                AddMemberFlags(evaluatedTypeInfoDto, type);

                if (type.BaseType != null)
                {
                    var baseType = type.BaseType;

                    if (baseType.IsGenericType)
                    {
                        baseType = baseType.GetGenericTypeDefinition();
                    }

                    evaluatedTypeInfoDto.OriginalBaseTypes.Add(baseType);
                }

                foreach (var @interface in type.GetInterfaces())
                {
                    var baseType = @interface;

                    if (baseType.IsGenericType)
                    {
                        baseType = baseType.GetGenericTypeDefinition();
                    }

                    evaluatedTypeInfoDto.OriginalBaseTypes.Add(baseType);
                }

                foreach (var constructorInfo in type.GetConstructors(BindingFlags.NonPublic | BindingFlags.Public))
                {
                    var evaluatedConstructorDto = new EvaluatedMethodDto();

                    evaluatedConstructorDto.OriginalType = constructorInfo.DeclaringType;
                    evaluatedConstructorDto.IdentifierText = type.Name;
                    evaluatedConstructorDto.FullIdentifierText = type.FullName;

                    AddMemberFlags(evaluatedConstructorDto, constructorInfo);

                    foreach (var parameterInfo in constructorInfo.GetParameters())
                    {
                        var evaluatedTypedMemberDto = new EvaluatedTypedMemberDto();

                        evaluatedTypedMemberDto.OriginalType = parameterInfo.ParameterType;
                        evaluatedTypedMemberDto.IdentifierText = parameterInfo.Name;
                        evaluatedTypedMemberDto.FullIdentifierText = evaluatedConstructorDto.FullIdentifierText +
                                                                     "." +
                                                                     parameterInfo.Name;

                        AddMemberFlags(evaluatedTypedMemberDto, parameterInfo);

                        evaluatedConstructorDto.Parameters.Add(evaluatedTypedMemberDto);
                    }

                    evaluatedTypeInfoDto.Constructors.Add(evaluatedConstructorDto);
                }

                foreach (var fieldInfo in type.GetFields(BindingFlags.NonPublic | BindingFlags.Public))
                {
                    var evaluatedTypedMemberDto = new EvaluatedTypedMemberDto();

                    evaluatedTypedMemberDto.OriginalType = fieldInfo.FieldType;

                    evaluatedTypedMemberDto.IdentifierText = fieldInfo.Name;
                    evaluatedTypedMemberDto.FullIdentifierText = evaluatedTypedMemberDto.FullIdentifierText + "." +
                                                                 fieldInfo.Name;

                    AddMemberFlags(evaluatedTypedMemberDto, fieldInfo);

                    evaluatedTypeInfoDto.Fields.Add(evaluatedTypedMemberDto);
                }

                foreach (
                    var methodInfo in
                        type.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Public))
                {
                    var evaluatedMethodDto = new EvaluatedMethodDto();

                    evaluatedMethodDto.IdentifierText = methodInfo.Name;
                    evaluatedMethodDto.FullIdentifierText = type.FullName + "." + methodInfo.Name;
                    evaluatedMethodDto.OriginalType = methodInfo.ReturnType;

                    AddMemberFlags(evaluatedMethodDto, methodInfo);

                    foreach (var parameterInfo in methodInfo.GetParameters())
                    {
                        var evaluatedTypedMemberDto = new EvaluatedTypedMemberDto();

                        evaluatedTypedMemberDto.OriginalType = parameterInfo.ParameterType;
                        evaluatedTypedMemberDto.IdentifierText = parameterInfo.Name;
                        evaluatedTypedMemberDto.FullIdentifierText = evaluatedMethodDto.FullIdentifierText +
                                                                     "." +
                                                                     parameterInfo.Name;

                        AddMemberFlags(evaluatedTypedMemberDto, parameterInfo);

                        evaluatedMethodDto.Parameters.Add(evaluatedTypedMemberDto);
                    }

                    evaluatedTypeInfoDto.Methods.Add(evaluatedMethodDto);
                }

                foreach (var propertyInfo in type.GetProperties())
                {
                    var evaluatedPropertyDto = new EvaluatedPropertyDto();

                    if (propertyInfo.GetMethod != null)
                    {
                        evaluatedPropertyDto.OriginalType = propertyInfo.GetMethod.ReturnType;
                        evaluatedPropertyDto.Getter = new EvaluatedMethodDto();
                        evaluatedPropertyDto.Getter.OriginalType = propertyInfo.GetMethod.ReturnType;
                        evaluatedPropertyDto.IdentifierText = propertyInfo.Name;
                        evaluatedPropertyDto.FullIdentifierText = evaluatedTypeInfoDto.FullIdentifierText + "." +
                                                                  propertyInfo.Name;

                        foreach (var parameterInfo in propertyInfo.GetMethod.GetParameters())
                        {
                            var evaluatedTypedMemberDto = new EvaluatedTypedMemberDto();

                            evaluatedTypedMemberDto.OriginalType = parameterInfo.ParameterType;
                            evaluatedTypedMemberDto.IdentifierText = parameterInfo.Name;
                            evaluatedTypedMemberDto.FullIdentifierText = evaluatedPropertyDto.FullIdentifierText +
                                                                         "." +
                                                                         parameterInfo.Name;

                            AddMemberFlags(evaluatedTypedMemberDto, parameterInfo);

                            evaluatedPropertyDto.Getter.Parameters.Add(evaluatedTypedMemberDto);
                        }
                    }


                    if (propertyInfo.SetMethod != null)
                    {
                        evaluatedPropertyDto.OriginalType = propertyInfo.SetMethod.ReturnType;
                        evaluatedPropertyDto.Setter = new EvaluatedMethodDto();
                        evaluatedPropertyDto.Setter.OriginalType = propertyInfo.SetMethod.ReturnType;
                        evaluatedPropertyDto.IdentifierText = propertyInfo.Name;
                        evaluatedPropertyDto.FullIdentifierText = evaluatedTypeInfoDto.FullIdentifierText + "." +
                                                                  propertyInfo.Name;

                        foreach (var parameterInfo in propertyInfo.SetMethod.GetParameters())
                        {
                            var evaluatedTypedMemberDto = new EvaluatedTypedMemberDto();

                            evaluatedTypedMemberDto.OriginalType = parameterInfo.ParameterType;
                            evaluatedTypedMemberDto.IdentifierText = parameterInfo.Name;
                            evaluatedTypedMemberDto.FullIdentifierText = evaluatedPropertyDto.FullIdentifierText +
                                                                         "." +
                                                                         parameterInfo.Name;

                            AddMemberFlags(evaluatedTypedMemberDto, parameterInfo);

                            evaluatedPropertyDto.Setter.Parameters.Add(evaluatedTypedMemberDto);
                        }
                    }
                }

                evaluatedTypeInfoLinks[type] = evaluatedTypeInfoDto;

                evaluatedTypeInfos.Add(evaluatedTypeInfoDto);
            }
        }

        private void AddMemberFlags(EvaluatedMemberDto evaluatedMember, object memberInfo)
        {
            if (memberInfo is ParameterInfo)
            {
                var parameterInfo = (ParameterInfo) memberInfo;
            }
            else if (memberInfo is MethodBase)
            {
                var methodInfo = (MethodBase) memberInfo;

                var methodAttributes = methodInfo.Attributes;

                if ((methodAttributes & MethodAttributes.Abstract) != 0)
                {
                    evaluatedMember.MemberFlags |= 8;
                }

                if ((methodAttributes & MethodAttributes.Virtual) != 0)
                {
                    evaluatedMember.MemberFlags |= 2;
                }

                if ((methodAttributes & MethodAttributes.Private) != 0)
                {
                    evaluatedMember.MemberFlags |= 16;
                }

                if ((methodAttributes & MethodAttributes.Public) != 0)
                {
                    evaluatedMember.MemberFlags |= 64;
                }

                if ((methodAttributes & MethodAttributes.Static) != 0)
                {
                    evaluatedMember.MemberFlags |= 1;
                }

                if ((methodAttributes & MethodAttributes.Family) != 0)
                {
                    evaluatedMember.MemberFlags |= 32;
                }

                if ((methodAttributes & MethodAttributes.FamANDAssem) != 0)
                {
                    evaluatedMember.MemberFlags |= 1024;
                }

                if ((methodAttributes & MethodAttributes.FamORAssem) != 0)
                {
                    evaluatedMember.MemberFlags |= 32 + 128;
                }

                if ((methodAttributes & MethodAttributes.Final) != 0)
                {
                    evaluatedMember.MemberFlags = 2048;
                }
            }
            else if (memberInfo is Type)
            {
                var evaluatedTypeInfoDto = (EvaluatedTypeInfoDto) evaluatedMember;
                var type = (Type) memberInfo;

                var typeAttributes = type.Attributes;

                if ((typeAttributes & TypeAttributes.Abstract) != 0)
                {
                    evaluatedMember.MemberFlags |= 8;
                }


                if ((typeAttributes & TypeAttributes.NotPublic) != 0)
                {
                    evaluatedMember.MemberFlags |= 16;
                }

                if ((typeAttributes & TypeAttributes.Public) != 0)
                {
                    evaluatedMember.MemberFlags |= 64;
                }

                if ((typeAttributes & TypeAttributes.Abstract) != 0 && (typeAttributes & TypeAttributes.Sealed) != 0)
                {
                    evaluatedMember.MemberFlags |= 1;
                }

                if ((typeAttributes & TypeAttributes.Sealed) != 0)
                {
                    evaluatedMember.MemberFlags |= 2048;
                }


                if ((typeAttributes & TypeAttributes.Interface) != 0)
                {
                    evaluatedTypeInfoDto.IsInterfaceType = true;
                }
                else if ((typeAttributes & TypeAttributes.Class) != 0)
                {
                    evaluatedTypeInfoDto.IsReferenceType = true;
                }
                else
                {
                    evaluatedTypeInfoDto.IsValueType = true;
                }
            }
            else if (memberInfo is FieldInfo)
            {
                var fieldInfo = (FieldInfo) memberInfo;

                var fieldAttributes = fieldInfo.Attributes;

                if ((fieldAttributes & FieldAttributes.Assembly) != 0)
                {
                    evaluatedMember.MemberFlags |= 128;
                }

                if ((fieldAttributes & FieldAttributes.Private) != 0)
                {
                    evaluatedMember.MemberFlags |= 16;
                }

                if ((fieldAttributes & FieldAttributes.FamORAssem) != 0)
                {
                    evaluatedMember.MemberFlags |= 128 + 32;
                }

                if ((fieldAttributes & FieldAttributes.FamANDAssem) != 0)
                {
                    evaluatedMember.MemberFlags |= 1024;
                }

                if ((fieldAttributes & FieldAttributes.Static) != 0)
                {
                    evaluatedMember.MemberFlags |= 1;
                }

                if ((fieldAttributes & FieldAttributes.Public) != 0)
                {
                    evaluatedMember.MemberFlags |= 64;
                }

                if ((fieldAttributes & FieldAttributes.Assembly) != 0)
                {
                    evaluatedMember.MemberFlags |= 128;
                }
            }
        }
    }
}