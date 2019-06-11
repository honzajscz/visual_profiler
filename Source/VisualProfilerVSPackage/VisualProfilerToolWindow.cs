using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

using Microsoft.VisualStudio.Shell;
using VisualProfilerUI;
using VisualProfilerUIView = VisualProfilerUI.View.VisualProfilerUIView;

namespace JanVratislav.VisualProfilerVSPackage
{
    
    /// <summary>
    /// This class implements the tool window exposed by this package and hosts a user control.
    ///
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane, 
    /// usually implemented by the package implementer.
    ///
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its 
    /// implementation of the IVsUIElementPane interface.
    /// </summary>
    [System.Runtime.InteropServices.Guid("fa4d1b50-5999-44b4-9c50-030b5687f2f2")]
    public class VisualProfilerToolWindow : ToolWindowPane
    {
        private readonly VisualProfilerUIView _visualProfilerUIView;

        /// <summary>
        /// Standard constructor for the tool window.
        /// </summary>
        public VisualProfilerToolWindow() :
            base(null)
        {
            // Set the window title reading it from the resources.
            //this.Caption = "Visual Profiler";
            // Set the image that will appear on the tab of the window frame
            // when docked with an other window
            // The resource ID correspond to the one defined in the resx file
            // while the Index is the offset in the bitmap strip. Each image in
            // the strip being 16x16.
            this.BitmapResourceID = 301;
            this.BitmapIndex = 1;
            this.Caption = "Visual Profiler - Not Started";

            _visualProfilerUIView = new VisualProfilerUIView();
            base.Content = _visualProfilerUIView;           
        }

        protected override void OnClose()
        {
            _visualProfilerUIView.CloseProfileeProcess();
        }

        public VisualProfilerUIView VisualProfilerUIView
        {
            get { return _visualProfilerUIView; }
        }
    }
}
