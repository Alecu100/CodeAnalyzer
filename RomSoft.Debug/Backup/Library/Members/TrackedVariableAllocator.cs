//  Project              : GLP
//  Module               : RomSoft.Client.Debug.dll
//  File                 : TrackedVariableAllocator.cs
//  Author               : Alecsandru
//  Last Updated         : 18/12/2015 at 23:08
//  
// 
//  Contains             : Implementation of the TrackedVariableAllocator.cs class.
//  Classes              : TrackedVariableAllocator.cs
// 
//  
//  ----------------------------------------------------------------------- 
//   <copyright file="TrackedVariableAllocator.cs" company="Sysmex"> 
//       Copyright (c) Sysmex. All rights reserved. 
//   </copyright> 
//  -----------------------------------------------------------------------

namespace RomSoft.Client.Debug.Library.Members
{
    #region Using

    using RomSoft.Client.Debug.Library.Interfaces;

    #endregion

    public class TrackedVariableAllocator : ITrackedVariableAllocator
    {
        #region Public Methods and Operators

        /// <summary>
        ///     Expands the variable type information.
        /// </summary>
        /// <param name="typeInfo">The type information.</param>
        /// <returns></returns>
        public TrackedVariable AllocateVariable(TrackedTypeInfo typeInfo)
        {
            var trackedVariable = new TrackedVariable();

            foreach (var trackedField in typeInfo.AllFields)
            {
                var trackedVariableReference = new TrackedVariableReference();
                trackedVariableReference.Declaration = trackedField.Declaration;
                trackedVariableReference.TypeInfo = trackedField.TypeInfo;
                trackedVariableReference.Identifier = trackedField.Identifier;
                trackedVariableReference.IdentifierText = trackedField.IdentifierText;
                trackedVariableReference.FullIdentifierText = trackedField.FullIdentifierText;
                trackedVariable.Fields.Add(trackedVariableReference);
            }

            foreach (var trackedProperty in typeInfo.AllProperties)
            {
                if (trackedProperty.IsAutoProperty)
                {
                    var trackedVariableReference = new TrackedVariableReference();
                    trackedVariableReference.Declaration = trackedProperty.Declaration;
                    trackedVariableReference.TypeInfo = trackedProperty.TypeInfo;
                    trackedVariableReference.IdentifierText = "<" + trackedProperty.IdentifierText + ">";
                    trackedVariable.Fields.Add(trackedVariableReference);
                }
            }

            return trackedVariable;
        }

        #endregion
    }
}