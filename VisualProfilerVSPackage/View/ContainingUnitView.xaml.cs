using System;
using System.Collections.Generic;
using System.Windows.Controls;
using VisualProfilerUI.ViewModel;

namespace VisualProfilerVSPackage.View
{
    /// <summary>
    /// Interaction logic for ContainingUnitView.xaml
    /// </summary>
    public partial class ContainingUnitView : UserControl
    {
        private static readonly Dictionary<string, ContainingUnitView>  ContainingUnitsDict = new Dictionary<string, ContainingUnitView>();

        public ContainingUnitView()
        {
            InitializeComponent();
        }

        public IEnumerable<MethodViewModel> ContainedMethods { get; set; }

        public static ContainingUnitView GetContainingUnitViewByName(string name)
        {
            string nameLowerCase = name.ToLower();
            ContainingUnitView cuv;
            bool found = ContainingUnitsDict.TryGetValue(nameLowerCase, out cuv);
            if(!found)
            {
                cuv = new ContainingUnitView();
                ContainingUnitsDict[nameLowerCase] = cuv;
            }

            return cuv;
        }

        public static void UpdateDataOfContainingUnits(IEnumerable<ContainingUnitViewModel> cuvModels)
        {
            foreach (var cuvModel in cuvModels)
            {
                ContainingUnitView containingUnitView = GetContainingUnitViewByName(cuvModel.Name);
                containingUnitView.DataContext = cuvModel;
            }
        }

        public static void RemoveAllContainingUnits()
        {
            foreach (var containingUnit in ContainingUnitsDict.Values)
            {
                containingUnit.DataContext = null;
            }
        }
    }

    
}
