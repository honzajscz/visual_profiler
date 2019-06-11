using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using VisualProfilerUI.Model.Criteria;

namespace VisualProfilerUI.ViewModel
{
    public class CriterionSwitchViewModel : ViewModelBase
    {
        private readonly Criterion _criterion;
        private readonly RelayCommand _command;

        public CriterionSwitchViewModel(Criterion criterion)
        {
            _criterion = criterion;
            _command = new RelayCommand(o =>{
                                                Action<Criterion> handler = CriterionChanged;
                                                if (handler != null)handler(_criterion);});
        }

        public event Action<Criterion> CriterionChanged;

        public string Name { get { return _criterion.Name; }}

        public ICommand ActivateCriterion
        {
            get { return _command; }
        }

        private bool _isActive;
        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                
                _isActive = value;
                OnPropertyChanged("IsActive");
            }
        }
    }
    
}
