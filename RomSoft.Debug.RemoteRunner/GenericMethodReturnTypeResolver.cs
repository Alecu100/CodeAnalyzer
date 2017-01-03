//  Project              : GLP
//  Module               : RomSoft.Debug.RemoteRunner.dll
//  File                 : GenericMethodReturnTypeResolver.cs
//  Author               : Alecsandru
//  Last Updated         : 02/12/2015 at 16:41
//  
// 
//  Contains             : Implementation of the GenericMethodReturnTypeResolver.cs class.
//  Classes              : GenericMethodReturnTypeResolver.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="GenericMethodReturnTypeResolver.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

namespace RomSoft.Debug.RemoteRunner
{
    #region Using

    using System;
    using System.Linq;
    using System.Reflection;

    #endregion

    public class GenericMethodReturnTypeResolver : MarshalByRefObject
    {
        #region Public Methods and Operators

        public int? GetReturnGenericParameterIndex(
            string assemblyPath,
            string namespaceName,
            string className,
            string methodName)
        {
            var loadedAssembly = Assembly.LoadFile(assemblyPath);

            foreach (var definedType in loadedAssembly.DefinedTypes)
            {
                if (definedType.Namespace == namespaceName && definedType.Name == className)
                {
                    var methodInfo = definedType.GetMethod(methodName);

                    if (methodInfo != null)
                    {
                        var returnType = methodInfo.ReturnType;

                        if (methodInfo.IsGenericMethodDefinition)
                        {
                            var genericArguments = methodInfo.GetGenericArguments().ToList();

                            for (var i = 0; i < genericArguments.Count; i++)
                            {
                                if (genericArguments[i] == returnType)
                                {
                                    return i;
                                }
                            }
                        }

                        if (definedType.IsGenericTypeDefinition)
                        {
                            var genericArguments = definedType.GetGenericArguments().ToList();

                            for (var i = 0; i < genericArguments.Count; i++)
                            {
                                if (genericArguments[i] == returnType)
                                {
                                    return i;
                                }
                            }
                        }
                    }
                }
            }

            return null;
        }

        #endregion
    }
}