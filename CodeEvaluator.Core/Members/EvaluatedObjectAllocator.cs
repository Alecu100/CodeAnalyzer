//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : EvaluatedObjectAllocator.cs
//  Author               : Alecsandru
//  Last Updated         : 18/12/2015 at 23:08
//  
// 
//  Contains             : Implementation of the EvaluatedObjectAllocator.cs class.
//  Classes              : EvaluatedObjectAllocator.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="EvaluatedObjectAllocator.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using CodeAnalysis.Core.Extensions;
using CodeAnalysis.Core.Interfaces;

namespace CodeAnalysis.Core.Members
{

    #region Using

    #endregion

    public class EvaluatedObjectAllocator : IEvaluatedObjectAllocator
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Expands the variable type information.
        /// </summary>
        /// <param name="typeInfo">The type information.</param>
        /// <returns></returns>
        public EvaluatedObject AllocateVariable(EvaluatedTypeInfo typeInfo)
        {
            var fields = new List<EvaluatedObjectReference>();

            foreach (
                var trackedField in typeInfo.AllFields.Where(field => !field.IsStatic()))
            {
                var trackedVariableReference = new EvaluatedObjectReference();
                trackedVariableReference.Declaration = trackedField.Declaration;
                trackedVariableReference.TypeInfo = trackedField.TypeInfo;
                trackedVariableReference.Identifier = trackedField.Identifier;
                trackedVariableReference.IdentifierText = trackedField.IdentifierText;
                trackedVariableReference.FullIdentifierText = trackedField.FullIdentifierText;
                fields.Add(trackedVariableReference);
            }

            foreach (
                var trackedProperty in
                    typeInfo.AllProperties.Where(prop => !prop.IsStatic()))
            {
                if (trackedProperty.IsAutoProperty)
                {
                    var trackedVariableReference = new EvaluatedObjectReference();
                    trackedVariableReference.Declaration = trackedProperty.Declaration;
                    trackedVariableReference.TypeInfo = trackedProperty.TypeInfo;
                    trackedVariableReference.IdentifierText = "<" + trackedProperty.IdentifierText + ">";
                    fields.Add(trackedVariableReference);
                }
            }

            var trackedVariable = new EvaluatedObject(typeInfo, fields);

            return trackedVariable;
        }

        #endregion
    }
}