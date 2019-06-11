using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using System.Windows;
using EnvDTE;
using EnvDTE80;
using Microsoft.Win32;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using VisualProfilerAccess.ProfilingData;
using VisualProfilerUI;
using VisualProfilerUI.ViewModel;
using VisualProfilerVSPackage.PackageFiles;
using VisualProfilerVSPackage.View;

namespace JanVratislav.VisualProfilerVSPackage
{

    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof(VisualProfilerToolWindow))]
    [Guid(GuidList.guidVisualProfilerVSPackagePkgString)]
    public sealed class VisualProfilerVSPackagePackage : Package
    {
        private DTE2 _dte;
        private VisualProfilerToolWindow _window;

        public VisualProfilerVSPackagePackage()
        {
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this.ToString()));
            
        }

        protected override void Initialize()
        {
            Trace.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this.ToString()));
            base.Initialize();

            OleMenuCommandService mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (null != mcs)
            {
                CommandID menuCommandIDTracing = new CommandID(GuidList.guidVisualProfilerVSPackageCmdSet, (int)PkgCmdIDList.cmdidStartVisualProfilerTracingMode);
                MenuCommand menuItemTracing = new MenuCommand(TracingCallback, menuCommandIDTracing);
                mcs.AddCommand(menuItemTracing);

                CommandID menuCommandIDSampling = new CommandID(GuidList.guidVisualProfilerVSPackageCmdSet, (int)PkgCmdIDList.cmdidStartVisualProfilerSamplingMode);
                MenuCommand menuItemSampling = new MenuCommand(SamplingCallback, menuCommandIDSampling);
                mcs.AddCommand(menuItemSampling);

                CommandID menuCommandIDStop = new CommandID(GuidList.guidVisualProfilerVSPackageCmdSet, (int)PkgCmdIDList.cmdidStopVisualProfiler);
                MenuCommand menuItemStop = new MenuCommand(StopProfilerCallback, menuCommandIDStop);
                mcs.AddCommand(menuItemStop);
            }

            _dte = GetService(typeof(SDTE)) as DTE2;
        }

        private void TracingCallback(object sender, EventArgs e)
        {
            StartProfiler(ProfilerTypes.TracingProfiler);
        }

        private void SamplingCallback(object sender, EventArgs e)
        {
           StartProfiler(ProfilerTypes.SamplingProfiler);
        }

        private void StopProfilerCallback(object sender, EventArgs e)
        {
            var messageBoxResult = MessageBox.Show("Are you sure that you want to stop the profiling session?", "Visual Profiler", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            if(messageBoxResult ==MessageBoxResult.Yes)
            {
                ContainingUnitView.RemoveAllContainingUnits();
                if (_window != null && _window.VisualProfilerUIView != null)
                {
                    _window.VisualProfilerUIView.CloseProfileeProcess();
                }
            }
        }

        private void StartProfiler(ProfilerTypes profilerType)
        {
            try
            {
                bool buildSuccedded = StartBuild();
                if (!buildSuccedded) throw new Exception("Could not build the solution.");

                Project startUpProject = GetStartUpProject();
                if (startUpProject == null) throw new Exception("Could not locate a startUp project.");
              
                string assemblyPath = GetOutputAssemblyPath(startUpProject);
                if (string.IsNullOrEmpty(assemblyPath)) throw new Exception("Could not locate the output assembly.");
                if (Path.GetExtension(assemblyPath) != ".exe") throw new Exception("The output of the startUp project is not an executable.");
                if (!File.Exists(assemblyPath)) throw new Exception("The output executable file could not be found.");

                _window = this.FindToolWindow(typeof(VisualProfilerToolWindow), 0, true) as VisualProfilerToolWindow;
                if ((null == _window) || (null == _window.Frame)) throw new NotSupportedException("Cannot create a window.");

                UILogic uiLogic = _window.VisualProfilerUIView.UILogic;
                _window.VisualProfilerUIView.UILogic.MethodClick += mvm => OnMethodClick(uiLogic, mvm);
                _window.VisualProfilerUIView.Profile(profilerType, assemblyPath);
                _window.VisualProfilerUIView.DataUpdate += ContainingUnitView.UpdateDataOfContainingUnits;
                _window.Caption = string.Format("Visual Profiler - {0} Mode", GetModeString(profilerType));
                IVsWindowFrame windowFrame = (IVsWindowFrame)_window.Frame;
                ErrorHandler.ThrowOnFailure(windowFrame.Show());

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Profiler initialization failed.", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnMethodClick(UILogic uiLogic, MethodViewModel mvm)
        {
            string sourcePath = uiLogic.GetSourceFilePathForMethod(mvm);
            _dte.ItemOperations.OpenFile(sourcePath);
            

            //TODO when the adornment layer is stable, enable this line moving.
            //var firstLineOfMethod = uiLogic.GetFirstLineOfMethod(mvm);
            //var commandName = "Edit.GoTo " + firstLineOfMethod;
            //_dte.ExecuteCommand(commandName);
        }


        private string GetModeString(ProfilerTypes profilerType)
        {
            string profilerTypeName = profilerType.ToString();
            int indexOfProfiler = profilerTypeName.IndexOf("Profiler");
            string modeString = profilerTypeName.Substring(0, profilerTypeName.Length - indexOfProfiler - 1);
            return modeString;
        }

        private bool StartBuild()
        {
            _dte.Solution.SolutionBuild.Build(true);
            int numberOfFailures = _dte.Solution.SolutionBuild.LastBuildInfo;
            bool buildSucceeded = numberOfFailures == 0;
            return buildSucceeded;
        }

        private Project GetStartUpProject()
        {
           Array startupProjects = _dte.Solution.SolutionBuild.StartupProjects as Array;
            string value = startupProjects.GetValue(0) as string;

            Project startUpProject = null;
            foreach (var project in _dte.Solution.Projects)
            {
                var project1 = project as Project;
                if (project1.FullName.EndsWith(value))
                {
                    startUpProject = project1;
                }
            }

            return startUpProject;
        }

        private string GetOutputAssemblyPath(EnvDTE.Project vsProject)
        {
            string fullPath = vsProject.Properties.Item("FullPath").Value.ToString();
            string outputPath = vsProject.ConfigurationManager.ActiveConfiguration.Properties.Item("OutputPath").Value.ToString();
            string outputDir = Path.Combine(fullPath, outputPath);
            string outputFileName = vsProject.Properties.Item("OutputFileName").Value.ToString();
            string assemblyPath = Path.Combine(outputDir, outputFileName);
            return assemblyPath;
        }


    }
}
