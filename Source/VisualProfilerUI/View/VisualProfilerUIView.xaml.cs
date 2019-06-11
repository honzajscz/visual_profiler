using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using VisualProfilerAccess.ProfilingData;
using VisualProfilerAccess.ProfilingData.CallTrees;
using VisualProfilerUI.Model.CallTreeConvertors;
using VisualProfilerUI.Model.CallTreeConvertors.Sampling;
using VisualProfilerUI.Model.CallTreeConvertors.Tracing;
using VisualProfilerUI.Model.CriteriaContexts;
using VisualProfilerUI.ViewModel;

namespace VisualProfilerUI.View
{
    public partial class VisualProfilerUIView : UserControl
    {
     
        private readonly UILogic _uiLogic;
        readonly object _lockObject = new object();

        public VisualProfilerUIView()
        {
            if (Application.ResourceAssembly == null)
            {
                Application.ResourceAssembly = typeof (MainWindow).Assembly;
            }
            EnsureWPFToolKitDllLoads();
            InitializeComponent();
            _uiLogic = new UILogic();
        }

        /// <summary>
        /// This control uses a wpf dictionary ../Theme/ExpressionLight.xaml that needs the WPFToolkit dll to work.
        /// The WPFToolkit dll is loaded when baml (compiled xaml) is loaded. The problem is that its path is resolved based on
        /// the running application, which is in case of a VS extension the devenv.exe (Visual Studio) process. The default resolution 
        /// then fails, because the dll is hidden in an extension folder structire. We can either add the WPFToolkit to GAC or 
        /// load it explicitly before it is being referenced as we do here.
        /// </summary>
        private static void EnsureWPFToolKitDllLoads()
        {
            var assemblyDirectory = Path.GetDirectoryName(typeof (VisualProfilerUIView).Assembly.Location);
            string wpfToolKitAssemblyPath = Path.Combine(assemblyDirectory, "WPFToolkit.dll");
            Assembly.LoadFile(wpfToolKitAssemblyPath);
        }

        public UILogic UILogic
        {
            get { return _uiLogic; }
        }

        private Action _closeProfileeProcessAction;

        public void CloseProfileeProcess()
        {
            if(_closeProfileeProcessAction != null)
            {
                _closeProfileeProcessAction();
            }
        }

        public void Profile(ProfilerTypes profiler, string processPath)
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo { FileName = processPath };
            CriterionSwitchViewModel[] criterionSwitchVMs;

            if (profiler == ProfilerTypes.TracingProfiler)
            {
                var profilerAccess = new TracingProfilerAccess(
                    processStartInfo,
                    TimeSpan.FromMilliseconds(1000),
                    OnUpdateCallback);
                _closeProfileeProcessAction = profilerAccess.CloseProfileeProcess;
                profilerAccess.StartProfiler();
                _uiLogic.ActiveCriterion = TracingCriteriaContext.CallCountCriterion;

                criterionSwitchVMs = new[] {
                new CriterionSwitchViewModel(TracingCriteriaContext.CallCountCriterion){IsActive = true},
                new CriterionSwitchViewModel(TracingCriteriaContext.TimeActiveCriterion),
                new CriterionSwitchViewModel(TracingCriteriaContext.TimeWallClockCriterion)};
            }
            else
            {
                var profilerAccess = new SamplingProfilerAccess(
                    processStartInfo,
                    TimeSpan.FromMilliseconds(1000),
                    OnUpdateCallback);
                _closeProfileeProcessAction = profilerAccess.CloseProfileeProcess;
                profilerAccess.StartProfiler();
                _uiLogic.ActiveCriterion = SamplingCriteriaContext.TopStackOccurrenceCriterion;

                criterionSwitchVMs = new[] {
                new CriterionSwitchViewModel(SamplingCriteriaContext.TopStackOccurrenceCriterion){IsActive = true},
                new CriterionSwitchViewModel(SamplingCriteriaContext.DurationCriterion)};
            }

            foreach (var switchVM in criterionSwitchVMs)
            {
                switchVM.CriterionChanged += _uiLogic.ActivateCriterion;
            }

            criteriaSwitch.DataContext = criterionSwitchVMs;
            _uiLogic.CriterionSwitchVMs = criterionSwitchVMs;
           
        }

        private void OnUpdateCallback(object sender, ProfilingDataUpdateEventArgs<TracingCallTree> eventArgs)
        {
           //if (Interlocked.CompareExchange(ref _enter, 0, 1) == 1)
            {
                lock (_lockObject)
                {
                    CallTreeConvertor treeConvertor = new TracingCallTreeConvertor(eventArgs.CallTrees);
                    UpdateUI(treeConvertor);
                }
            }
        }

        private void OnUpdateCallback(object sender, ProfilingDataUpdateEventArgs<SamplingCallTree> eventArgs)
        {
           // if (Interlocked.CompareExchange(ref _enter, 0, 1) == 1)
            {
                lock (_lockObject)
                {
                    CallTreeConvertor treeConvertor = new SamplingCallTreeConvertor(eventArgs.CallTrees);
                    UpdateUI(treeConvertor);
                }
            }
        }

        private void UpdateUI(CallTreeConvertor treeConvertor)
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                var containingUnitViewModels = treeConvertor.SourceFiles.Select(sf =>
                {
                    var methodViewModels = sf.ContainedMethods.Select(cm => new MethodViewModel(cm));
                    var containingUnitViewModel = new ContainingUnitViewModel(sf.FullName);
                    containingUnitViewModel.Height = sf.Height;
                    containingUnitViewModel.MethodViewModels = methodViewModels.OrderBy(mvm => mvm.Top).ToArray();
                    return containingUnitViewModel;
                }).ToArray();


                List<MethodViewModel> allMethodViewModels = new List<MethodViewModel>();
                foreach (var containingUnitViewModel in containingUnitViewModels)
                {
                    allMethodViewModels.AddRange(containingUnitViewModel.MethodViewModels);
                }

                _uiLogic.CriteriaContext = treeConvertor.CriteriaContext;
                _uiLogic.MethodModelByIdDict = treeConvertor.MethodDictionary.ToDictionary(kvp => kvp.Key.Id,
                                                                                          kvp => kvp.Value);
                _uiLogic.MethodVMByIdDict = allMethodViewModels.ToDictionary(kvp => kvp.Id, kvp => kvp);
                var detailViewModel = new DetailViewModel();
                detail.DataContext = detailViewModel;
                _uiLogic.Detail = detailViewModel;
                _uiLogic.InitAllMethodViewModels();

                containingUnits.ItemsSource = containingUnitViewModels;
                containingUnits.DataContext = new { Height = treeConvertor.MaxEndLine + 20 };

                OnDataUpdate(containingUnitViewModels);

                var sortedMethodVMs = new ObservableCollection<MethodViewModel>(_uiLogic.MethodVMByIdDict.Values);
                sortedMethods.DataContext = sortedMethodVMs;
                _uiLogic.SortedMethodVMs = sortedMethodVMs;
                _uiLogic.ActivateCriterion(_uiLogic.ActiveCriterion);
            }), null);
        }

        public event Action<IEnumerable<ContainingUnitViewModel>> DataUpdate;

        public void OnDataUpdate(IEnumerable<ContainingUnitViewModel> data)
        {
            Action<IEnumerable<ContainingUnitViewModel>> handler = DataUpdate;
            if (handler != null) handler(data);
        }

        private int _enter = 0;

        //[Conditional("DEBUG")]
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            _enter = 1;
        }
    }


}
