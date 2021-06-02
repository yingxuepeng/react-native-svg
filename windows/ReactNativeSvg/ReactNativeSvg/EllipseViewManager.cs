using Microsoft.ReactNative.Managed;
using ReactNativeSvg.svg.view;
using Windows.UI.Xaml;

namespace ReactNativeSvg.svg
{
    class EllipseViewManager : BaseShapeManager
    {
        public override string Name => nameof(SVGName.RNSVGEllipse);

        public override FrameworkElement CreateView()
        {
            return new EllipseView();
        }

        [ViewManagerProperty("cx")]
        public void SetCX(ShadowElement view, double? cx)
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
        public void SetCY(ShadowElement view, double? cy)
        {
            if (cy == null)
            {
                return;
            }
            var ele = view as EllipseView;
            ele.cy = cy;
            ele.UpdateEllipse();
        }

        [ViewManagerProperty("rx")]
        public void SetRX(ShadowElement view, double? rx)
        {
            if (rx == null)
            {
                return;
            }
            var ele = view as EllipseView;
            ele.rx = rx;
            ele.UpdateEllipse();
        }

        [ViewManagerProperty("ry")]
        public void SetRY(ShadowElement view, double? ry)
        {
            if (ry == null)
            {
                return;
            }
            var ele = view as EllipseView;
            ele.ry = ry;
            ele.UpdateEllipse();
        }
    }
}
