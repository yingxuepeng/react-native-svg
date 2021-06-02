using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace ReactNativeSvg.svg.view
{
    class EllipseView : ShadowElement
    {
        public double? cx;
        public double? cy;
        public double? rx;
        public double? ry;

        private Ellipse ellipse;
        public EllipseView()
        {
            RealElement = ellipse = new Ellipse()
            {
                DataContext = new BaseShapeContext()
            };
        }

        public void UpdateEllipse()
        {
            if (this.cx == null || this.cy == null || this.rx == null || this.ry == null)
            {
                return;
            }

            Canvas.SetLeft(ellipse, (double)(this.cx - this.rx));
            Canvas.SetTop(ellipse, (double)(this.cy - this.ry));

            ellipse.Width = (double)(this.rx * 2);
            ellipse.Height = (double)(this.ry * 2);
        }
    }
}
