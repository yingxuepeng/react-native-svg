
using Microsoft.ReactNative.Managed;
using ReactNativeSvg.svg.view;
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
    enum SvgColorType
    {
        UrlColor = 1,
        CurrentColor = 2,
        ContextFill = 3,
        ContextStroke = 4,
    }

    class BaseShapeContext : DependencyObject
    {
        /**
         * COMMON
         **/

        public ShadowElement ShadowElement { get; set; }
        public SvgView SvgView { get; set; }
        public BaseShapeContext ParentContext { get; set; }


        public virtual void UpdateSvgView(SvgView svgView, ShadowElement element)
        {
            SvgView = svgView;
            UpdateStroke(element.RealElement as Shape, this);
            UpdateFill(element.RealElement as Shape, this);
        }
        /**
         * STROKE context
         **/
        public JSValue? Stroke { get; set; }
        public double? StrokeThickness { get; set; }
        public double? StrokeOpacity { get; set; }
        public double? StrokeDashOffset { get; set; }
        public JSValue? StrokeDashArray { get; set; }
        public PenLineCap? StrokeLinecap { get; set; }
        public PenLineJoin? StrokeLinejoin { get; set; }
        public double? StrokeMiterlimit { get; set; }

        public static void UpdateStroke(Shape view, BaseShapeContext viewContext)
        {
            view.Stroke = GetStrokeBrush(viewContext);
            if (view.Stroke != null)
            {
                if (viewContext.StrokeOpacity != null)
                {
                    view.Stroke.Opacity = (double)viewContext.StrokeOpacity;
                }
                else { view.Stroke.Opacity = 1; };
            }
            if (viewContext.StrokeThickness != null)
            {
                view.StrokeThickness = (double)viewContext.StrokeThickness;
            }
            else { view.StrokeThickness = 1; }

            DoubleCollection dCollection = new DoubleCollection();
            if (viewContext.StrokeDashArray != null && JSValue.Null != viewContext.StrokeDashArray)
            {
                for (int i = 0; i < ((JSValue)viewContext.StrokeDashArray).AsArray().Count; ++i)
                {
                    dCollection.Add(Convert.ToDouble(((JSValue)viewContext.StrokeDashArray)[i].ToString()));
                }
            }
            view.StrokeDashArray = dCollection;

            if (viewContext.StrokeDashOffset != null)
            {
                view.StrokeDashOffset = (double)viewContext.StrokeDashOffset;
            }
            else
            {
                view.StrokeDashOffset = 0;
            }

            if (viewContext.StrokeMiterlimit != null)
            {
                view.StrokeMiterLimit = (double)viewContext.StrokeMiterlimit;
            }
            else
            {
                view.StrokeMiterLimit = 4;
            }

            if (viewContext.StrokeLinejoin != null)
            {
                view.StrokeLineJoin = (PenLineJoin)viewContext.StrokeLinejoin;
            }
            else { view.StrokeLineJoin = PenLineJoin.Miter; }

            if (viewContext.StrokeLinecap != null)
            {
                //view.StrokeStartLineCap = (PenLineCap)viewContext.StrokeLinecap;
                //view.StrokeDashCap = (PenLineCap)viewContext.StrokeLinecap;
                view.StrokeEndLineCap = (PenLineCap)viewContext.StrokeLinecap;
            }
            else
            {
                //view.StrokeStartLineCap = PenLineCap.Square;
                //view.StrokeDashCap = PenLineCap.Square;
                view.StrokeEndLineCap = PenLineCap.Square;
            }
        }

        public static Brush GetStrokeBrush(BaseShapeContext viewContext)
        {
            if (viewContext.Stroke == null || JSValue.Null == viewContext.Stroke)
            {
                return null;
            }
            if (((JSValue)viewContext.Stroke).AsArray().Count > 0)
            {
                // context color
                var type = (SvgColorType)((JSValue)viewContext.Stroke).AsArray()[0].AsInt32();
                Brush colorBrush = null;
                switch (type)
                {
                    case SvgColorType.UrlColor:
                        Debug.WriteLine("GetStrokeBrush", viewContext.Stroke.ToString());
                        break;
                    case SvgColorType.CurrentColor:
                        if (viewContext.SvgView != null && viewContext.SvgView.RootCoor != null)
                        {
                            var colorBytes = BitConverter.GetBytes((uint)viewContext.SvgView.RootCoor);
                            var color = Color.FromArgb(colorBytes[3], colorBytes[2], colorBytes[1], colorBytes[0]);
                            return new SolidColorBrush(color);
                        }
                        break;

                    default: break;
                }
                return colorBrush;
            }
            else
            {
                // plain color
                uint stroke = ((JSValue)viewContext.Stroke).AsUInt32();
                var colorBytes = BitConverter.GetBytes(stroke);
                var color = Color.FromArgb(colorBytes[3], colorBytes[2], colorBytes[1], colorBytes[0]);
                return new SolidColorBrush(color);
            }
        }


        /**
         * FILL context
         **/
        public JSValue? Fill { get; set; }
        public double? FillOpacity { get; set; }
        public FillRule? FillRule { get; set; }



        public static void UpdateFill(Shape view, BaseShapeContext viewContext)
        {
            if (viewContext.Fill != JSValue.Null)
            {
                var brush = GetFillBrush(viewContext);

                view.Fill = brush;
                if (viewContext.FillOpacity != null && view.Fill != null)
                {
                    view.Fill.Opacity = (double)viewContext.FillOpacity;
                }
            }
        }
        public static Brush GetFillBrush(BaseShapeContext viewContext)
        {

            if (viewContext.Fill == null || JSValue.Null == viewContext.Fill)
            {
                return new SolidColorBrush(Colors.Black);
            }
            if (((JSValue)viewContext.Fill).AsArray().Count > 0)
            {
                // context color
                var type = (SvgColorType)((JSValue)viewContext.Fill).AsArray()[0].AsInt32();
                Brush colorBrush = null;
                switch (type)
                {
                    case SvgColorType.UrlColor:
                        var key = ((JSValue)viewContext.Fill).AsArray()[1].AsString();
                        Debug.WriteLine("GetFillBrush:" + key);
                        break;
                    case SvgColorType.CurrentColor:
                        if (viewContext.SvgView != null && viewContext.SvgView.RootCoor != null)
                        {
                            var colorBytes = BitConverter.GetBytes((uint)viewContext.SvgView.RootCoor);
                            var color = Color.FromArgb(colorBytes[3], colorBytes[2], colorBytes[1], colorBytes[0]);
                            return new SolidColorBrush(color);
                        }
                        break;

                    default: break;
                }
                return colorBrush;
            }
            else
            {
                // plain color
                uint fill = ((JSValue)viewContext.Fill).AsUInt32();
                var colorBytes = BitConverter.GetBytes(fill);
                var color = Color.FromArgb(colorBytes[3], colorBytes[2], colorBytes[1], colorBytes[0]);
                return new SolidColorBrush(color);
            }
        }


        /**
         * TRANSFORM context
         **/
        public double? Rotation { get; set; }
        public double? Scale { get; set; }
        public static void UpdateTransform(Shape view, BaseShapeContext viewContext)
        {
            if (viewContext.Rotation != null)
            {
                view.Rotation = (float)viewContext.Rotation;
            }

            if (viewContext.Scale != null)
            {

            }
        }

    }
}
