using Microsoft.ReactNative;
using Microsoft.ReactNative.Managed;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using System.Reflection;

namespace ReactNativeSvg.svg
{
    class LineViewManager : BaseShapeManager
    {
        public override string Name => nameof(SVGName.RNSVGLine);

        public override FrameworkElement CreateView()
        {
            var viewContext = new BaseShapeContext();
            var line = new Line()
            {
                DataContext = viewContext
            };
            var shadowElement = new ShadowElement();
            shadowElement.RealElement = line;
            return shadowElement;
        }

        [ViewManagerProperty("x1")]
        public void SetX1(ShadowElement view, double? x1)
        {
            if (x1 == null)
            {
                return;
            }
            var ele = view.RealElement as Line;
            ele.X1 = (double)x1;
            Debug.WriteLine("RNSVGLine x1：" + ele.X1);
        }

        [ViewManagerProperty("y1")]
        public void SetY1(ShadowElement view, double? y1)
        {

            if (y1 == null)
            {
                return;
            }
            var ele = view.RealElement as Line;
            ele.Y1 = (double)y1;
            Debug.WriteLine("RNSVGLine y1：" + ele.Y1);
        }

        [ViewManagerProperty("x2")]
        public void SetX2(ShadowElement view, double? x2)
        {

            if (x2 == null)
            {
                return;
            }
            var ele = view.RealElement as Line;
            ele.X2 = (double)x2;
            Debug.WriteLine("RNSVGLine x2：" + ele.X2);
        }

        [ViewManagerProperty("y2")]
        public void SetY2(ShadowElement view, double? y2)
        {
            if (y2 == null)
            {
                return;
            }
            var ele = view.RealElement as Line;
            ele.Y2 = (double)y2;
            Debug.WriteLine("RNSVGLine y2：" + ele.Y2);
        }
    }

}
