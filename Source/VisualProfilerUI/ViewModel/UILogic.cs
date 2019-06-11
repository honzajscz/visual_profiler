using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media;
using VisualProfilerUI.Model.Criteria;
using VisualProfilerUI.Model.CriteriaContexts;
using VisualProfilerUI.Model.Methods;
using VisualProfilerUI.Model.Values;
using VisualProfilerUI.View;

namespace VisualProfilerUI.ViewModel
{
    public class UILogic
    {
        public MethodViewModel ActiveMethodVM { get; set; }

        public MethodViewModel HighlightedMethodVM { get; set; }

        public DetailViewModel Detail { get; set; }

        public Dictionary<uint, MethodViewModel> MethodVMByIdDict { get; set; }

        public Dictionary<uint, Method> MethodModelByIdDict { get; set; }

        public Criterion ActiveCriterion { get; set; }

        public ICriteriaContext CriteriaContext { get; set; }

        public CriterionSwitchViewModel[] CriterionSwitchVMs { get; set; }

        public ObservableCollection<MethodViewModel> SortedMethodVMs { get; set; }

        public event Action<MethodViewModel> MethodClick;

        public void OnMethodClick(MethodViewModel methodViewModel)
        {
            Action<MethodViewModel> handler = MethodClick;
            if (handler != null)
            {
                handler(methodViewModel);
            }
        }

        private void ShowMethodInDetail(uint methodId)
        {
            Method method = MethodModelByIdDict[methodId];
            Detail.MethodName = method.Name;
            IValue value = method.GetValueFor(ActiveCriterion);
            Detail.Metrics = value.GetAsString(ActiveCriterion.Divider) + " " + ActiveCriterion.Unit;
            Detail.Class = method.ClassFullName;
            Detail.Source = Path.GetFileName(method.SourceFile) + ", line " + method.FirstLineNumber; ;
        }

        private void ClearDetail()
        {
            Detail.MethodName = string.Empty;
            Detail.Metrics = string.Empty;
        }

        public void MethodActivate(MethodViewModel methodVM)
        {
            if (ActiveMethodVM != null && ActiveMethodVM.IsActive)
            {
                MethodDeactivated(ActiveMethodVM);
            }

            ActiveMethodVM = methodVM;
            methodVM.IsActive = true;
            methodVM.BorderBrush = MethodView.ActiveMethodBorderColor.ToBrush();
            ActiveMethodVM.OpacityTemp = ActiveMethodVM.Opacity;
            ActiveMethodVM.Opacity = 1;
            ShowMethodInDetail(methodVM.Id);
        }

        public void MethodDeactivated(MethodViewModel methodVM)
        {
            if (ActiveMethodVM == null) return;
            Contract.Assume(ActiveMethodVM.Id == methodVM.Id);
            ActiveMethodVM.Opacity = ActiveMethodVM.OpacityTemp;
            ActiveMethodVM = null;
            methodVM.BorderBrush = MethodView.MethodBorderColor.ToBrush();
            methodVM.IsActive = false;
        }

        public void InitAllMethodViewModels()
        {
            foreach (var kvp in MethodVMByIdDict)
            {
                MethodViewModel methodViewModel = kvp.Value;
                methodViewModel.Activate += MethodActivate;
                methodViewModel.Deactivate += MethodDeactivated;
                methodViewModel.Highlight += OnMethodClick;
            }
        }

        public void ActivateCriterion(Criterion criterion)
        {
            if (MethodVMByIdDict == null)
                return;
            ActiveCriterion = criterion;
            foreach (var kvp in MethodVMByIdDict)
            {
                uint methodId = kvp.Key;
                MethodViewModel methodViewModel = kvp.Value;

                Method method = MethodModelByIdDict[methodId];
                Contract.Assume(method.Id == methodViewModel.Id);
                IValue activeValue = method.GetValueFor(criterion);
                IValue maxValue = CriteriaContext.GetMaxValueFor(criterion);

                methodViewModel.Opacity = activeValue.ConvertToZeroOneScale(maxValue) * 0.8 + 0.1;
                methodViewModel.ActiveValue = activeValue;

            }

            var newSortedMethodVMs = SortedMethodVMs.OrderByDescending(mvm => mvm.ActiveValue).ToArray();
            SortedMethodVMs.Clear();
            foreach (var methodVM in newSortedMethodVMs)
            {
                SortedMethodVMs.Add(methodVM);
            }
           
        }

        public void MethodHighlighted(uint methodId)
        {
            //not implemented in this version
        }

        public void MethodSuppress(uint methodId)
        {
            //not implemented in this version 
        }

        public string GetSourceFilePathForMethod(MethodViewModel methodViewModel)
        {
            Method method = MethodModelByIdDict[methodViewModel.Id];
            string sourceFile = method.SourceFile;
            return sourceFile;
        }

        public int GetFirstLineOfMethod(MethodViewModel methodViewModel)
        {
            Method method = MethodModelByIdDict[methodViewModel.Id];
            var firstLineNumber = method.FirstLineNumber;
            return firstLineNumber;
        }
    }
}