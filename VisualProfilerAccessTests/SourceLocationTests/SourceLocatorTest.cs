using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using NUnit.Framework;
using VisualProfilerAccess.SourceLocation;

namespace VisualProfilerAccessTests.SourceLocationTests
{
    [TestFixture]
    public class SourceLocatorTest
    {
        private SourceLocatorFactory _sourceLocatorFactory;
        private MethodInfo _methodMethodInfo;
        private MethodInfo _methodWithCommentsMethodInfo;
        private MethodInfo _methodWithLambdaMethodInfo;
        private MethodInfo _lambdaMethod;

        private ISourceLocator _iSourceLocator;
        private ISourceLocatorFactory _iSourceLocatorFactory;
        private string _tempAssemblyFile;
        private string _tempSourceFile;
        private string _tempPdbFile;

        private const string Code =
            @"using System;

namespace VisualProfilerAccessTests.SourceLocationTests
{
    class SourceLocationDummyTestClass
    {
        public void Method()
        {
            DummyMethod();
            MethodWithComments();
            MethodWithLambda();
            new SourceLocationDummyTestClass();
        }

        public void MethodWithComments()
        {
            DummyMethod();
            DummyMethod();
          
            //Comment
            //Comment
            DummyMethod();
            DummyMethod();
        }

        public void MethodWithLambda()
        {
            DummyMethod();
            DummyMethod();
            Action action = () =>
            {
                MethodWithComments();
                MethodWithComments();
                MethodWithComments();
            };
            DummyMethod();
            DummyMethod();
        }

        private double DummyMethod()
        {
            var sqrt = Math.Sqrt(12);
            return sqrt;
        }
    }
}";

        private void CompileTestCode()
        {
            string assemblyFileName = Path.GetTempFileName();
            _tempAssemblyFile = assemblyFileName + ".dll";
            _tempSourceFile = assemblyFileName + ".cs";
            _tempPdbFile = assemblyFileName + ".pdb";

            var parameters = new CompilerParameters();
            CodeDomProvider codeProvider = CodeDomProvider.CreateProvider("CSharp");
            File.WriteAllText(_tempSourceFile, Code);
            parameters.GenerateExecutable = false;
            parameters.OutputAssembly = _tempAssemblyFile;
            parameters.CompilerOptions = "/debug:pdbonly /optimize-";
            CompilerResults results = codeProvider.CompileAssemblyFromFile(parameters, _tempSourceFile);
            Assert.AreEqual(0, results.Errors.Count);
        }

        private void LoadMethodInfos()
        {
            Assembly assembly = Assembly.ReflectionOnlyLoadFrom(_tempAssemblyFile);

            Type dummyClassType =
                assembly.GetType("VisualProfilerAccessTests.SourceLocationTests.SourceLocationDummyTestClass");

            _methodMethodInfo = dummyClassType.GetMethod("Method");
            _methodWithCommentsMethodInfo = dummyClassType.GetMethod("MethodWithComments");
            _methodWithLambdaMethodInfo = dummyClassType.GetMethod("MethodWithLambda");

            _lambdaMethod =
                dummyClassType.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly).
                    Where(
                        mi => mi.Name.StartsWith("<")).Single();
        }

        [TestFixtureSetUp]
        public void SetUp()
        {
            CompileTestCode();
            LoadMethodInfos();

            _sourceLocatorFactory = new SourceLocatorFactory();
            _iSourceLocatorFactory = _sourceLocatorFactory;
            _iSourceLocator = _iSourceLocatorFactory.GetSourceLocator(_tempAssemblyFile);
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            _iSourceLocator.Dispose();
            File.Delete(_tempSourceFile);
            File.Delete(_tempPdbFile);
            //The assembly file remains loaded in the current app domain and it is not possible to delete it. Only after the process ends. 
            DeleteFileAtNextSystemStart(_tempAssemblyFile);
        }

        //   [Test]
        public void GetSourceLocatorTest()
        {
            Assert.IsNotNull(_iSourceLocatorFactory);
            Assert.IsNotNull(_iSourceLocator);
        }

        private void AssertMethodLine(int expectedStartLine, int expectedStartColumn, int expectedStartIndex,
                                      int expectedEndLine, int expectedEndColumn, int expectedEndIndex,
                                      IMethodLine actualLine)
        {
            Assert.AreEqual(expectedStartLine, actualLine.StartLine);
            Assert.AreEqual(expectedStartColumn, actualLine.StartColumn);
            Assert.AreEqual(expectedStartIndex, actualLine.StartIndex);
            Assert.AreEqual(expectedEndColumn, actualLine.EndColumn);
            Assert.AreEqual(expectedEndIndex, actualLine.EndIndex);
        }

        public static void DeleteFileAtNextSystemStart(string filePath)
        {
            MoveFileEx(filePath, null, MoveFileFlags.MOVEFILE_DELAY_UNTIL_REBOOT);
        }

        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool MoveFileEx(string lpExistingFileName, string lpNewFileName,
                                              MoveFileFlags dwFlags);

        [Flags]
        private enum MoveFileFlags
        {
            MOVEFILE_REPLACE_EXISTING = 0x00000001,
            MOVEFILE_COPY_ALLOWED = 0x00000002,
            MOVEFILE_DELAY_UNTIL_REBOOT = 0x00000004,
            MOVEFILE_WRITE_THROUGH = 0x00000008,
            MOVEFILE_CREATE_HARDLINK = 0x00000010,
            MOVEFILE_FAIL_IF_NOT_TRACKABLE = 0x00000020
        }

        [Test]
        public void SourceFilePathTest()
        {
            string sourceFilePath = _iSourceLocator.GetSourceFilePath((uint) _methodMethodInfo.MetadataToken);
            Assert.AreEqual(_tempSourceFile.ToLower(), sourceFilePath.ToLower());
            sourceFilePath = _iSourceLocator.GetSourceFilePath((uint) _methodWithCommentsMethodInfo.MetadataToken);
            Assert.AreEqual(_tempSourceFile.ToLower(), sourceFilePath.ToLower());
            sourceFilePath = _iSourceLocator.GetSourceFilePath((uint) _methodWithLambdaMethodInfo.MetadataToken);
            Assert.AreEqual(_tempSourceFile.ToLower(), sourceFilePath.ToLower());
            sourceFilePath = _iSourceLocator.GetSourceFilePath((uint) _lambdaMethod.MetadataToken);
            Assert.AreEqual(_tempSourceFile.ToLower(), sourceFilePath.ToLower());
        }

        [Test]
        public void SourceLocatorGetMethodLinesLambdaMethodTest()
        {
            IMethodLine[] methodLines = _iSourceLocator.GetMethodLines((uint) _lambdaMethod.MetadataToken).ToArray();
            Assert.AreEqual(4, methodLines.Length);
        }

        [Test]
        public void SourceLocatorGetMethodLinesMethodWithCommentsTest()
        {
            IMethodLine[] methodLines =
                _iSourceLocator.GetMethodLines((uint) _methodWithCommentsMethodInfo.MetadataToken).ToArray();
            Assert.AreEqual(6, methodLines.Length);
            AssertMethodLine(16, 9, 373, 16, 10, 374, methodLines[0]);
            AssertMethodLine(17, 13, 388, 17, 27, 402, methodLines[1]);
            AssertMethodLine(18, 13, 416, 18, 27, 430, methodLines[2]);
            AssertMethodLine(22, 13, 502, 22, 27, 516, methodLines[3]);
            AssertMethodLine(23, 13, 530, 23, 27, 544, methodLines[4]);
            AssertMethodLine(24, 9, 554, 24, 10, 555, methodLines[5]);
        }

        [Test]
        public void SourceLocatorGetMethodLinesMethodWithLambdaTest()
        {
            IMethodLine[] methodLines =
                _iSourceLocator.GetMethodLines((uint) _methodWithLambdaMethodInfo.MetadataToken).ToArray();
            AssertMethodLine(30, 13, 678, 35, 15, 847, methodLines[3]);

            Assert.AreEqual(7, methodLines.Length);
        }

        [Test]
        public void SourceLocatorGetMethodLinesOrdinaryMethodTest()
        {
            IMethodLine[] methodLines = _iSourceLocator.GetMethodLines((uint) _methodMethodInfo.MetadataToken).ToArray();
            Assert.AreEqual(6, methodLines.Length);
            AssertMethodLine(8, 9, 162, 8, 10, 163, methodLines[0]);
            AssertMethodLine(9, 13, 177, 9, 27, 191, methodLines[1]);
            AssertMethodLine(10, 13, 205, 10, 34, 226, methodLines[2]);
            AssertMethodLine(11, 13, 240, 11, 32, 259, methodLines[3]);
            AssertMethodLine(12, 13, 273, 12, 48, 308, methodLines[4]);
            AssertMethodLine(13, 9, 318, 13, 10, 319, methodLines[5]);
        }
    }
}