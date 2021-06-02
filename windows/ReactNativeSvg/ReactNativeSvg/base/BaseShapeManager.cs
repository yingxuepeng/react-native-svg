using Microsoft.ReactNative;
using Microsoft.ReactNative.Managed;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace ReactNativeSvg.svg
{

    public enum SVGName
    {
        // root?
        RNSVGSvgView,
        // shape
        RNSVGGroup, RNSVGPath, RNSVGCircle, RNSVGEllipse, RNSVGLine, RNSVGRect,
        // not supported
        RNSVGText, RNSVGTSpan, RNSVGTextPath, RNSVGImage, RNSVGClipPath, RNSVGDefs, RNSVGUse, RNSVGSymbol, RNSVGLinearGradient, RNSVGRadialGradient,
        RNSVGPattern, RNSVGMask, RNSVGMarker, RNSVGForeignObject,
    }
    public enum StrokeProps
    {
        stroke, strokeWidth, strokeOpacity, strokeDashoffset, strokeDasharray, strokeLinecap, strokeLinejoin, strokeMiterlimit
    }

    public enum FillProps
    {
        fill, fillOpacity, fillRule
    }

    public enum TransformProps
    {
        x, y, rotation, scale, origin, originX, originY
    }
    class BaseShapeManager : MyAttributedViewManager<ShadowElement>
    {
        [ViewManagerProperty(nameof(StrokeProps.stroke))]
        public void SetStroke(ShadowElement view, JSValue stroke)
        {
            var ele = view.RealElement;
            BaseShapeContext viewContext = (BaseShapeContext)ele.DataContext;
            Debug.WriteLine("SetStroke" + stroke.ToString());
            viewContext.Stroke = stroke;
            UpdateStroke(ele, viewContext);
        }

        [ViewManagerProperty(nameof(StrokeProps.strokeWidth))]
        public void SetStrokeWidth(ShadowElement view, double? strokeWidth)
        {
            var ele = view.RealElement;
            BaseShapeContext viewContext = (BaseShapeContext)ele.DataContext;
            if (viewContext.StrokeThickness == strokeWidth)
            {
                return;
            }
            viewContext.StrokeThickness = (double)strokeWidth;
            UpdateStroke(ele, viewContext);
        }

        [ViewManagerProperty(nameof(StrokeProps.strokeOpacity))]
        public void SetStrokeOpacity(ShadowElement view, double? strokeOpacity)
        {
            var ele = view.RealElement;
            BaseShapeContext viewContext = (BaseShapeContext)ele.DataContext;
            if (viewContext.StrokeOpacity == strokeOpacity)
            {
                return;
            }
            viewContext.StrokeOpacity = (double)strokeOpacity;
            UpdateStroke(ele, viewContext);
        }


        [ViewManagerProperty(nameof(StrokeProps.strokeDashoffset))]
        public void SetStrokeDashoffset(ShadowElement view, JSValue? strokeDashOffset)
        {
            var ele = view.RealElement;
            BaseShapeContext viewContext = (BaseShapeContext)ele.DataContext;
            if (viewContext.StrokeDashOffset == strokeDashOffset)
            {
                return;
            }
            if (strokeDashOffset == null || JSValue.Null == strokeDashOffset)
            {
                viewContext.StrokeDashOffset = null;
            }
            else
            {
                viewContext.StrokeDashOffset = ((JSValue)strokeDashOffset).AsDouble();
            }

            UpdateStroke(ele, viewContext);
        }


        [ViewManagerProperty(nameof(StrokeProps.strokeDasharray), ViewManagerPropertyType.Map)]
        public void SetStrokeDasharray(ShadowElement view, JSValueArray strokeDasharray)
        {
            var ele = view.RealElement;
            BaseShapeContext viewContext = (BaseShapeContext)ele.DataContext;
            viewContext.StrokeDashArray = strokeDasharray;
            UpdateStroke(ele, viewContext);
        }

        [ViewManagerProperty(nameof(StrokeProps.strokeLinecap))]
        public void SetStrokeLinecap(ShadowElement view, uint? strokeLinecap)
        {
            Debug.WriteLine("SetStrokeLinecap" + strokeLinecap.ToString());
            var ele = view.RealElement;

            PenLineCap? penLineCap = null;
            if (strokeLinecap != null)
            {
                switch (strokeLinecap)
                {
                    case 0:
                        penLineCap = PenLineCap.Flat;
                        break;
                    case 1:
                        penLineCap = PenLineCap.Round;
                        break;
                    case 2:
                        penLineCap = PenLineCap.Flat;
                        break;
                    default:
                        break;
                }
            }

            BaseShapeContext viewContext = (BaseShapeContext)ele.DataContext;
            if (viewContext.StrokeLinecap == penLineCap)
            {
                return;
            }
            viewContext.StrokeLinecap = penLineCap;
            UpdateStroke(ele, viewContext);

        }

        [ViewManagerProperty(nameof(StrokeProps.strokeLinejoin))]
        public void SetStrokeLinejoin(ShadowElement view, uint? strokeLinejoin)
        {
            var ele = view.RealElement;
            PenLineJoin? penLineJoin = null;
            if (strokeLinejoin != null)
            {
                switch (strokeLinejoin)
                {
                    case 0:
                        penLineJoin = PenLineJoin.Miter;
                        break;
                    case 1:
                        penLineJoin = PenLineJoin.Round;
                        break;
                    case 2:
                        penLineJoin = PenLineJoin.Bevel;
                        break;
                    default:
                        break;
                }
            }

            BaseShapeContext viewContext = (BaseShapeContext)ele.DataContext;
            viewContext.StrokeLinejoin = penLineJoin;
            if (viewContext.StrokeLinejoin == penLineJoin)
            {
                return;
            }
            UpdateStroke(ele, viewContext);
        }

        [ViewManagerProperty(nameof(StrokeProps.strokeMiterlimit))]
        public void SetStrokeMiterlimit(ShadowElement view, double? strokeMiterlimit)
        {
            var ele = view.RealElement;
            BaseShapeContext viewContext = (BaseShapeContext)ele.DataContext;
            if (strokeMiterlimit == viewContext.StrokeMiterlimit)
            {
                return;
            }
            viewContext.StrokeMiterlimit = strokeMiterlimit;
            UpdateStroke(ele, viewContext);
        }

        protected virtual void UpdateStroke(FrameworkElement view, BaseShapeContext viewContext)
        {
            Debug.WriteLine("UpdateStroke");
            BaseShapeContext.UpdateStroke((Shape)view, viewContext);
        }


        [ViewManagerProperty(nameof(FillProps.fill))]
        public void SetFill(ShadowElement view, JSValue fill)
        {
            var ele = view.RealElement;
            BaseShapeContext viewContext = (BaseShapeContext)ele.DataContext;
            viewContext.Fill = fill;
            UpdateFill(ele, viewContext);
        }

        [ViewManagerProperty(nameof(FillProps.fillOpacity))]
        public void SetFillOpacity(ShadowElement view, double? fillOpacity)
        {
            if (fillOpacity == null)
            {
                return;
            }
            var ele = view.RealElement;
            BaseShapeContext viewContext = (BaseShapeContext)ele.DataContext;
            if (viewContext.FillOpacity == fillOpacity)
            {
                return;
            }
            viewContext.FillOpacity = fillOpacity;
            UpdateFill(ele, viewContext);
        }

        [ViewManagerProperty(nameof(FillProps.fillRule))]
        public void SetFillRule(ShadowElement view, uint? fillRule)
        {
            var ele = view.RealElement;

            FillRule rule;
            switch (fillRule)
            {
                case 0:
                    rule = FillRule.EvenOdd;
                    break;
                case 1:
                    rule = FillRule.Nonzero;
                    break;
                default:
                    rule = FillRule.Nonzero;
                    break;
            }
            BaseShapeContext viewContext = (BaseShapeContext)ele.DataContext;
            if (rule == viewContext.FillRule)
            {
                return;
            }

            viewContext.FillRule = rule;
            UpdateFill(ele, viewContext);
        }

        protected virtual void UpdateFill(FrameworkElement view, BaseShapeContext viewContext)
        {
            Debug.WriteLine("UpdateFill");
            BaseShapeContext.UpdateFill((Shape)view, viewContext);
        }


        // TransformProps
        [ViewManagerProperty(nameof(TransformProps.rotation), ViewManagerPropertyType.Number)]
        public void SetTransformRotation(ShadowElement view, double? rotation)
        {
            if (rotation == null)
            {
                return;
            }

        }
    }
}
