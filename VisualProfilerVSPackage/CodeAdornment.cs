using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Formatting;
using VisualProfilerVSPackage.View;
using ContainingUnitView = VisualProfilerVSPackage.View.ContainingUnitView;
using System.Linq;

namespace CodeAdornment
{
    /// <summary>
    /// Adornment class that draws a square box in the top right hand corner of the viewport
    /// </summary>
    class CodeAdornment
    {

        private IWpfTextView _view;
        private IAdornmentLayer _adornmentLayer;
        private string _sourceFilePath;
        private ContainingUnitView containingUnitView;

        public CodeAdornment(IWpfTextView view)
        {
            _view = view;
            _sourceFilePath = GetSourceFilePath();
            _adornmentLayer = view.GetAdornmentLayer("CodeAdornment");

            _adornmentLayer.RemoveAllAdornments();
            containingUnitView = ContainingUnitView.GetContainingUnitViewByName(_sourceFilePath);

            if (containingUnitView.Parent != null)
            {
                var adornmentLayer = (IAdornmentLayer)containingUnitView.Parent;
                adornmentLayer.RemoveAdornment(containingUnitView);
            }

            //TODO Some how anchor the adornment layer to prevent random moving.
            Canvas.SetTop(containingUnitView, 0);
            _adornmentLayer.AddAdornment(AdornmentPositioningBehavior.OwnerControlled, null, null, containingUnitView, null);

            // _view.ViewportWidthChanged += delegate { Initialize(); };
            //_view.ViewportHeightChanged += delegate { Initialize(); };
            //    _view.LayoutChanged += delegate { Initialize(); };
            //   _view.ViewportLeftChanged += delegate { Initialize(); }; 
            //   _view.ZoomLevelChanged += delegate { Initialize(); }; 
            // _view.VisualElement.SizeChanged +=delegate { Initialize(); }; 
        }

        private bool _init = false;
        private void Initialize()
        {
           
            //     SourceLineHeightConverter.LineHeight = _view.LineHeight;
            //    Debug.WriteLine("Line height = {0}", _view.LineHeight);
            //if (_init) return;
            // _init = true;
            //double top = Canvas.GetTop(containingUnitView);
            //   Canvas.SetTop(containingUnitView, 0);
            //var top =_view.ViewportTop;



            //    Canvas.SetLeft(containingUnitView, 0);

            //      System.Console.Beep();
            //  System.Windows.MessageBox.Show(_view.ViewportTop.ToString());

        }

        private string GetSourceFilePath()
        {
            Microsoft.VisualStudio.Text.ITextDocument document;
            if ((_view == null) ||
                    (!_view.TextDataModel.DocumentBuffer.Properties.TryGetProperty(typeof(Microsoft.VisualStudio.Text.ITextDocument), out document)))
                return String.Empty;

            // If we have no document, just ignore it.
            if ((document == null) || (document.TextBuffer == null))
                return String.Empty;

            return document.FilePath;
        }
    }
}
