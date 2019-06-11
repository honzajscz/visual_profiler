// Guids.cs
// MUST match guids.h

using System;

namespace VisualProfilerVSPackage.PackageFiles
{
    static class GuidList
    {
        public const string guidVisualProfilerVSPackagePkgString = "e6705835-5f02-45ac-924b-537b24bc8175";
        public const string guidVisualProfilerVSPackageCmdSetString = "17b494fb-0fb9-4263-8b54-b27d2b8ef391";

        public static readonly Guid guidVisualProfilerVSPackageCmdSet = new Guid(guidVisualProfilerVSPackageCmdSetString);
    };
}