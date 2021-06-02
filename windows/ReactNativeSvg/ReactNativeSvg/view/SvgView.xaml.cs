using Microsoft.ReactNative.Managed;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace ReactNativeSvg.svg.view
{
    public sealed partial class SvgView : UserControl
    {
        public double? VbWidth;
        public double? VbHeight;
        public double? MinX = 0;
        public double? MinY = 0;
        public uint? RootCoor;


        public List<ShadowElement> ChildrenElement { get; set; } = new List<ShadowElement>();

        public SvgView()
        {
            this.InitializeComponent();
        }

        public Canvas GetCanvas() { return canvas; }

        public void UpdateViewBox()
        {
            if (MinX != null && VbWidth != null)
            {
                canvas.Width = (double)(VbWidth - MinX);
                foreach (UIElement ele in canvas.Children)
                {
                    Canvas.SetLeft(ele, -(double)MinX);
                }
            }

            if (MinY != null && VbHeight != null)
            {
                canvas.Height = (double)(VbHeight - MinY);
                foreach (UIElement ele in canvas.Children)
                {
                    Canvas.SetTop(ele, -(double)MinY);
                }
            }
            Debug.WriteLine("UpdateViewBox width:" + viewbox.ActualWidth, canvas.ActualWidth);
        }

        private void SvgView_LayoutUpdated(object sender, object e)
        {
            //Debug.WriteLine("SvgView_LayoutUpdated:" + this.ActualWidth + " " + this.ActualHeight);
            //Debug.WriteLine("SvgView_LayoutUpdated viewbox:" + viewbox.ActualWidth + " " + viewbox.ActualHeight);
            //Debug.WriteLine("SvgView_LayoutUpdated canvas:" + canvas.ActualWidth + " " + canvas.ActualHeight);
            viewbox.Width = this.ActualWidth;
            viewbox.Height = this.ActualHeight;
            if (MinX == null || VbWidth == null)
            {
                canvas.Width = this.ActualWidth;
            }
            if (MinY == null || VbHeight == null)
            {
                canvas.Height = this.ActualHeight;
            }
        }
    }
}
