using Microsoft.ReactNative;
using Microsoft.ReactNative.Managed;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace ReactNativeSvg.svg
{
    class RectViewManager : BaseShapeManager
    {
        public override string Name => nameof(SVGName.RNSVGRect);
        public override FrameworkElement CreateView()
        {
            var viewContext = new BaseShapeContext();
            var rect = new Rectangle()
            {
                DataContext = viewContext
            };

            var shadowElement = new ShadowElement();
            shadowElement.RealElement = rect;

            return shadowElement;
        }

        [ViewManagerProperty("x")]
        public void SetX(ShadowElement view, double? x)
        {
            if (x == null)
            {
                return;
            }
            var ele = view.RealElement as Rectangle;
            Canvas.SetLeft(ele, (double)x);
        }

        [ViewManagerProperty("y")]
        public void SetY(ShadowElement view, double? y)
        {
            if (y == null)
            {
                return;
            }
            var ele = view.RealElement as Rectangle;
            Canvas.SetLeft(ele, (double)y);
        }

        [ViewManagerProperty("width")]
        public void SetWidth(ShadowElement view, double? width)
        {
            if (width == null)
            {
                return;
            }
            var ele = view.RealElement as Rectangle;
            ele.Width = (double)width;
        }

        [ViewManagerProperty("height")]
        public void SetHeight(ShadowElement view, double? height)
        {
            if (height == null)
            {
                return;
            }
            var ele = view.RealElement as Rectangle;
            ele.Height = (double)height;
        }
    }
}
