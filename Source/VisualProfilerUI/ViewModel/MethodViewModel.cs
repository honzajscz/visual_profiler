using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Windows.Input;
using System.Windows.Media;
using VisualProfilerUI.Model;
using VisualProfilerUI.Model.Methods;
using VisualProfilerUI.Model.Values;
using VisualProfilerUI.View;

namespace VisualProfilerUI.ViewModel
{
    public class MethodViewModel : ViewModelBase
    {
        private readonly string _name;
        public const int PixelPerLine = 1;
        public MethodViewModel(Method method)
            : this(
            method.Id,
            MethodView.MethodColor.ToBrush(),
            MethodView.MethodBorderColor.ToBrush(),
            3,
            method.FirstLineNumber * PixelPerLine,
            method.LineExtend, method.Name )
        { }

        public MethodViewModel(uint id, Brush fill, Brush borderBrush, int borderThickness, int top, int height, string name)
        {
            _name = name;
            Id = id;
            Fill = fill;
            BorderBrush = borderBrush;
            BorderThinkness = borderThickness;
            Top = top;
            Height = height;

            ActivateCommand = new RelayCommand(o => InvokeEvent(Activate));
            DeactivateCommand = new RelayCommand(o => InvokeEvent(Deactivate));
            HighlightCommand = new RelayCommand(o => InvokeEvent(Highlight));
        }

        private void InvokeEvent(Action<MethodViewModel> handler)
        {
            if (handler != null)
                handler(this);
        }

        public event Action<MethodViewModel> Activate;
        public event Action<MethodViewModel> Deactivate;
        public event Action<MethodViewModel> Highlight;

        private Brush _fill;
        public Brush Fill
        {
            get { return _fill; }
            set
            {
                _fill = value;
                OnPropertyChanged("Fill");
            }
        }

        private Brush _borderBrush;
        public Brush BorderBrush
        {
            get { return _borderBrush; }
            set
            {
                _borderBrush = value;
                OnPropertyChanged("BorderBrush");
            }
        }

        private int _borderThinkness;
        public int BorderThinkness
        {
            get { return _borderThinkness; }
            set
            {
                _borderThinkness = value;
                OnPropertyChanged("BorderThinkness");
            }
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

        public uint Id { get; set; }

        private bool _isHighlighted;
        public bool IsHighlighted
        {
            get { return _isHighlighted; }
            set
            {
                _isHighlighted = value;
                OnPropertyChanged("IsHighlighted");
            }
        }

        private double _opacity;
        public double Opacity
        {
            get { return _opacity; }
            set
            {
                _opacity = value;
                OnPropertyChanged("Opacity");
            }
        }

        public int Top { get; set; }
        public int Height { get; set; }
        public ICommand ActivateCommand { get; set; }
        public ICommand DeactivateCommand { get; set; }
        public ICommand HighlightCommand { get; set; }
        public IValue ActiveValue { get; set; }
        public double OpacityTemp { get; set; }
    }
}
