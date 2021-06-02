using Microsoft.ReactNative;
using Microsoft.ReactNative.Managed;
using ReactNativeSvg.svg.view;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace ReactNativeSvg.svg
{
    class CircleViewManager : BaseShapeManager
    {
        public override string Name => nameof(SVGName.RNSVGCircle);

        public override FrameworkElement CreateView()
        {

            return new EllipseView();
        }

        [ViewManagerProperty("cx")]
        public void SetX(ShadowElement view, double? cx)
        {
            if (cx == null)
            {
                return;
            }
            var ele = view as EllipseView;
            ele.cx = cx;
            ele.UpdateEllipse();
        }

        [ViewManagerProperty("cy")]
        public void SetY(ShadowElement view, double? cy)
        {
            if (cy == null)
            {
                return;
            }
            var ele = view as EllipseView;
            ele.cy = cy;
            ele.UpdateEllipse();
        }

        [ViewManagerProperty("r")]
        public void SetWidth(ShadowElement view, double? radius)
        {
            if (radius == null)
            {
                return;
            }
            var ele = view as EllipseView;
            ele.rx = ele.ry = radius;
            ele.UpdateEllipse();
        }
    }
}
