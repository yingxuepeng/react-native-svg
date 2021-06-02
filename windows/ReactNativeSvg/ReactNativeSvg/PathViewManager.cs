using Microsoft.ReactNative.Managed;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace ReactNativeSvg.svg
{
    class PathDataContext : BaseShapeContext
    {
        public string pathStr;
    }
    class PathViewManager : BaseShapeManager
    {
        public override string Name => nameof(SVGName.RNSVGPath);

        public override FrameworkElement CreateView()
        {
            var viewContext = new PathDataContext();
            var path = new Path()
            {
                DataContext = viewContext
            };

            var shadowElement = new ShadowElement();
            shadowElement.RealElement = path;
            return shadowElement;
        }

        [ViewManagerProperty("d")]
        public void SetD(ShadowElement view, string d)
        {
            if (string.IsNullOrEmpty(d))
            {
                return;
            }
            var ele = view.RealElement as Path;
            var viewContext = ele.DataContext as PathDataContext;
            if (d.Equals(viewContext.pathStr))
            {
                return;
            }
            viewContext.pathStr = d;
            var geo = new PathParser(d).getPath();
            if (viewContext.FillRule != null)
            {
                geo.FillRule = (FillRule)viewContext.FillRule;
            }
            ele.Data = geo;
        }

        protected override void UpdateFill(FrameworkElement view, BaseShapeContext viewContext)
        {
            if (viewContext.FillRule != null)
            {
                var pathView = view as Path;
                if (pathView.Data != null)
                {
                    var geo = pathView.Data as PathGeometry;
                    geo.FillRule = (FillRule)viewContext.FillRule;
                }
            }
            base.UpdateFill(view, viewContext);
        }
    }
}
