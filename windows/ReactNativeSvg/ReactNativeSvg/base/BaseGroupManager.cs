using Microsoft.ReactNative;
using Microsoft.ReactNative.Managed;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace ReactNativeSvg.svg
{
    abstract class BaseGroupManager : BaseShapeManager, IViewManagerWithChildren
    {
        public override FrameworkElement CreateView()
        {
            Debug.WriteLine("RNSVGGroup CreateView");
            var viewContext = new BaseGroupContext();
            var canvas = new Canvas()
            {
                DataContext = viewContext
            };
            var shadowElement = new ShadowElement();
            shadowElement.RealElement = canvas;
            return shadowElement;
        }

        protected override void UpdateStroke(FrameworkElement view, BaseShapeContext viewContext)
        {
            Debug.WriteLine("UpdateStrokeCanvas");
            var canvas = view as Canvas;
            BaseGroupContext.UpdateStrokeCanvas(canvas, viewContext);
        }


        protected override void UpdateFill(FrameworkElement view, BaseShapeContext viewContext)
        {
            Debug.WriteLine("UpdateFillCanvas");
            var canvas = view as Canvas;
            BaseGroupContext.UpdateFillCanvas(canvas, viewContext);
        }


        public void AddView(FrameworkElement parent, UIElement child, long index)
        {
            var canvas = (parent as ShadowElement).RealElement as Canvas;
            var parentViewContext = canvas.DataContext as BaseGroupContext;
            var newShadow = (child as ShadowElement);

            // group shadow structure
            parentViewContext.ChildrenElement.Add(newShadow);

            if (newShadow.Type == ShadowElementType.View)
            {
                // child shadow structure
                var ele = newShadow.RealElement;
                var childContext = (BaseShapeContext)ele.DataContext;
                childContext.ParentContext = parentViewContext;
                childContext.UpdateSvgView(parentViewContext.SvgView, newShadow);

                // view
                canvas.Children.Add(ele);

                // update common props
                UpdateChildCommonProps(parentViewContext, newShadow);
            }
        }

        void IViewManagerWithChildren.RemoveAllChildren(FrameworkElement parent)
        {
            var canvas = (parent as ShadowElement).RealElement as Canvas;
            canvas.Children.Clear();
            var viewContext = canvas.DataContext as BaseGroupContext;
            viewContext.ChildrenElement.Clear();
        }

        void IViewManagerWithChildren.RemoveChildAt(FrameworkElement parent, long index)
        {
            var canvas = (parent as ShadowElement).RealElement as Canvas;
            var viewContext = canvas.DataContext as BaseGroupContext;
            var shadow = viewContext.ChildrenElement[(int)index];
            viewContext.ChildrenElement.RemoveAt((int)index);

            // view
            if (shadow.Type == ShadowElementType.View)
            {
                canvas.Children.Remove(shadow.RealElement);
            }

        }

        void IViewManagerWithChildren.ReplaceChild(FrameworkElement parent, UIElement oldChild, UIElement newChild)
        {
            Debug.WriteLine("RNSVGGroup ReplaceChild");
            var canvas = (parent as ShadowElement).RealElement as Canvas;
            var oldShadow = (oldChild as ShadowElement);
            var newShadow = (newChild as ShadowElement);

            var parentViewContext = canvas.DataContext as BaseGroupContext;

            // group shadow structure
            var shadowIndex = parentViewContext.ChildrenElement.IndexOf(oldShadow);
            parentViewContext.ChildrenElement.RemoveAt(shadowIndex);
            parentViewContext.ChildrenElement.Insert(shadowIndex, newShadow);

            // child shadow structure
            if (newShadow.Type == ShadowElementType.View)
            {
                var childContext = newShadow.DataContext as BaseShapeContext;
                childContext.ParentContext = parentViewContext;
                childContext.UpdateSvgView(parentViewContext.SvgView, newShadow);
            }

            // view
            var viewIndex = -1;
            if (oldShadow.Type == ShadowElementType.View)
            {
                viewIndex = canvas.Children.IndexOf(oldShadow.RealElement);
                canvas.Children.RemoveAt(viewIndex);
            }
            if (newShadow.Type == ShadowElementType.View)
            {
                if (viewIndex < 0)
                {
                    for (int i = shadowIndex - 1; i >= 0; i++)
                    {
                        ShadowElement se = parentViewContext.ChildrenElement[i];
                        if (se.Type == ShadowElementType.View)
                        {
                            viewIndex = i;
                            break;
                        }
                    }
                }

                if (viewIndex < 0)
                {
                    viewIndex = 0;
                }
                canvas.Children.Insert(viewIndex, newShadow.RealElement);

                // update common props
                UpdateChildCommonProps(parentViewContext, newShadow);
            }
        }

        private void UpdateChildCommonProps(BaseShapeContext parentViewContext, ShadowElement child)
        {
            var ele = child.RealElement;
            var childContext = ele.DataContext as BaseShapeContext;
            BaseGroupContext.CopyStrokeContext(parentViewContext, childContext);
            if (ele is Canvas)
            {
                BaseGroupContext.UpdateStrokeCanvas(ele as Canvas, childContext);
            }
            else if (ele is Shape)
            {
                BaseShapeContext.UpdateStroke(ele as Shape, childContext);
            }
            BaseGroupContext.CopyFillContext(parentViewContext, childContext);
            if (ele is Canvas)
            {
                BaseGroupContext.UpdateFillCanvas(ele as Canvas, childContext);
            }
            else if (ele is Shape)
            {
                BaseShapeContext.UpdateFill(ele as Shape, childContext);
            }
        }
    }
}
