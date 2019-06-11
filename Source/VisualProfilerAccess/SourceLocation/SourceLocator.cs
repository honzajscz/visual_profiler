using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using Microsoft.Cci;

namespace VisualProfilerAccess.SourceLocation
{
    public class SourceLocator : ISourceLocator, IDisposable
    {
        private const int MaxSourceLineLength = 20000;

        public SourceLocator(string modulePath)
        {
            LocationsByToken = new Dictionary<uint, IEnumerable<CciMethodLine>>();
            PdbReader = new PdbReader(modulePath);
            PopulateSourceLocations();
        }

        private PdbReader PdbReader { get; set; }
        private Dictionary<uint, IEnumerable<CciMethodLine>> LocationsByToken { get; set; }

        #region ISourceLocator Members

        public IEnumerable<IMethodLine> GetMethodLines(uint methodMdToken)
        {
            IEnumerable<CciMethodLine> methodLines = LocationsByToken[methodMdToken];
            return methodLines;
        }

        public string GetSourceFilePath(uint methodMdToken)
        {
            Contract.Ensures(!string.IsNullOrEmpty(Contract.Result<string>()));
            Contract.Ensures(!string.IsNullOrWhiteSpace(Contract.Result<string>()));

            string filePath = LocationsByToken[methodMdToken].First().SourceLocation.PrimarySourceDocument.Location;
            return filePath;
        }

        public void Dispose()
        {
            PdbReader.Dispose();
        }

        #endregion

        private void PopulateSourceLocations()
        {
            IModule module = PdbReader.Module;
            foreach (INamedTypeDefinition namedTypeDefinition in module.GetAllTypes())
            {
                foreach (IMethodDefinition methodDefinition in namedTypeDefinition.Methods)
                {
                    PropertyInfo propertyInfo = methodDefinition.GetType().GetProperty("TokenValue",
                                                                                       BindingFlags.NonPublic |
                                                                                       BindingFlags.Instance);
                    var methodMdToken = (uint) propertyInfo.GetValue(methodDefinition, null);
                    var primarySourceLocations = new List<IPrimarySourceLocation>();
                    foreach (ILocation location in methodDefinition.Locations)
                    {
                        IEnumerable<IPrimarySourceLocation> notNullLocations =
                            PdbReader.GetAllPrimarySourceLocationsFor(location).Where(sl => sl != null);
                        primarySourceLocations.AddRange(notNullLocations);
                    }

                    IEnumerable<CciMethodLine> cciMethodLines =
                        primarySourceLocations.Where(sl => sl.StartLine < MaxSourceLineLength).Select(sl => new CciMethodLine(sl));
                    LocationsByToken[methodMdToken] = cciMethodLines;
                }
            }
        }
    }
}