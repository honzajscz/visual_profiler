using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using VisualProfilerUI.Model.ContainingUnits;

namespace VisualProfilerUI.ViewModel
{
    public class ContainingUnitViewModel : ViewModelBase
    {
        private string _name;

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        private IEnumerable<MethodViewModel> _methodViewModels;

        public IEnumerable<MethodViewModel> MethodViewModels
        {
            get { return _methodViewModels; }
            set
            {
                _methodViewModels = value;
                OnPropertyChanged("MethodViewModels");
            }
        }

        public ContainingUnitViewModel(string name)
        {
            _name = name;
        }

        public int Height { get; set; }

    }
}
