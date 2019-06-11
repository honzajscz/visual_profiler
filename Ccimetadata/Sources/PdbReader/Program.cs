//-----------------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.  All Rights Reserved.
// This code is licensed under the Microsoft Public License.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//-----------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.Cci;

using Microsoft.Cci.MetadataReader.ObjectModelImplementation;

namespace VisualProfiler.Code
{
    class Program
    {
        static void Main(string[] args)
        {


            PdbReader pdbReader = new PdbReader(@"D:\Honzik\Desktop\Mandelbrot\Mandelbrot\bin\Debug\Mandelbrot.exe");
            IModule module = pdbReader.Module;

            foreach (var namedTypeDefinition in module.GetAllTypes())
            {
                PropertyInfo propertyInfo2 = namedTypeDefinition.GetType().GetProperty("TokenValue", BindingFlags.NonPublic | BindingFlags.Instance);
                uint value2 = (uint)propertyInfo2.GetValue(namedTypeDefinition, null);

                foreach (var methodDefinition in namedTypeDefinition.Methods)
                {
                    Console.WriteLine(methodDefinition.Name);

                    PropertyInfo propertyInfo = methodDefinition.GetType().GetProperty("TokenValue", BindingFlags.NonPublic | BindingFlags.Instance);
                    uint value = (uint)propertyInfo.GetValue(methodDefinition, null);
                    foreach (var location in methodDefinition.Locations)
                    {
                        foreach (var primarySourceLocation in pdbReader.GetAllPrimarySourceLocationsFor(location))
                        {
                            
                            if (primarySourceLocation != null)
                            {
                                Console.WriteLine("line {0}, {1}:{2}", primarySourceLocation.StartLine,
                                                  primarySourceLocation.StartColumn, primarySourceLocation.EndColumn);

                            }
                        }
                    }
                }
            }
        }
    
    }

}
