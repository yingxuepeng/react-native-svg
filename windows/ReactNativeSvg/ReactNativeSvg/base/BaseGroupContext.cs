using Microsoft.ReactNative.Managed;
using ReactNativeSvg.svg.view;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace ReactNativeSvg.svg
{
    class BaseGroupContext : BaseShapeContext
    {
        /**
         * COMMON
         **/
        public List<ShadowElement> ChildrenElement { get; set; } = new List<ShadowElement>();
        public List<DefElement> DefsElement { get; set; } = new List<DefElement>();
        public override void UpdateSvgView(SvgView svgView, ShadowElement element)
        {
            SvgView = svgView;
            foreach (ShadowElement se in ChildrenElement)
            {

                if (se.Type == ShadowElementType.View)
                {
                    var context = se.RealElement.DataContext as BaseShapeContext;
                    context.UpdateSvgView(SvgView, se);
                }
                else if (se.Type == ShadowElementType.Def)
                {
                    var context = se.DataContext as BaseDefContext;
                    context.UpdateSvgView(SvgView, se as DefElement);
                }
            }
        }

        public static void UpdateStrokeCanvas(Canvas canvas, BaseShapeContext canvasContext)
        {
            foreach (var childView in canvas.Children)
            {
                var childContext = (BaseShapeContext)((FrameworkElement)childView).DataContext;

                // copy context
                CopyStrokeContext(canvasContext, childContext);

                // update
                if (childView is Shape)
                {
                    var child = childView as Shape;

                    UpdateStroke(child, childContext);
                }
                else if (childView is Canvas)
                {
                    var child = childView as Canvas;
                    UpdateStrokeCanvas(child, childContext);
                }
            }
        }

        public static void CopyStrokeContext(BaseShapeContext canvasContext, BaseShapeContext childContext)
        {
            // stroke
            if (childContext.Stroke == null || JSValue.Null == childContext.Stroke)
            {
                childContext.Stroke = canvasContext.Stroke;
            }
            if (childContext.StrokeThickness == null)
            {
                childContext.StrokeThickness = canvasContext.StrokeThickness;
            }
            if (childContext.StrokeOpacity == null)
            {
                childContext.StrokeOpacity = canvasContext.StrokeOpacity;
            }
            if (JSValue.Null == childContext.StrokeDashArray)
            {
                childContext.StrokeDashArray = canvasContext.StrokeDashArray;
            }
            if (childContext.StrokeDashOffset == null)
            {
                childContext.StrokeDashOffset = canvasContext.StrokeDashOffset;
            }
            if (childContext.StrokeLinecap == null)
            {
                childContext.StrokeLinecap = canvasContext.StrokeLinecap;
            }
            if (childContext.StrokeLinejoin == null)
            {
                childContext.StrokeLinejoin = canvasContext.StrokeLinejoin;
            }
            if (childContext.StrokeMiterlimit == null)
            {
                childContext.StrokeMiterlimit = canvasContext.StrokeMiterlimit;
            }
        }

        public static void UpdateFillCanvas(Canvas canvas, BaseShapeContext canvasContext)
        {
            foreach (var childView in canvas.Children)
            {
                var childContext = (BaseShapeContext)((FrameworkElement)childView).DataContext;

                // copy context
                CopyFillContext(canvasContext, childContext);

                if (childView is Shape)
                {
                    var child = childView as Shape;
                    UpdateFill(child, childContext);
                }
                else if (childView is Canvas)
                {
                    var child = childView as Canvas;
                    UpdateFillCanvas(child, childContext);
                }
            }
        }


        public static void CopyFillContext(BaseShapeContext canvasContext, BaseShapeContext childContext)
        {
            // fill
            if (childContext.Fill == null || JSValue.Null == childContext.Fill)
            {
                Debug.WriteLine("CopyFillContext Fill:" + childContext.Fill + ", " + canvasContext.Fill);
                childContext.Fill = canvasContext.Fill;
            }
            if (childContext.FillOpacity == null)
            {
                childContext.FillOpacity = canvasContext.FillOpacity;
            }
            if (childContext.FillRule == null)
            {
                childContext.FillRule = canvasContext.FillRule;
            }
        }
    }
}
