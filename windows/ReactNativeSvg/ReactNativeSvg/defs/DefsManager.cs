using Microsoft.ReactNative;
using Microsoft.ReactNative.Managed;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace ReactNativeSvg.svg.defs
{
    class DefsContext : BaseDefContext
    {

    }
    class DefsManager : MyAttributedViewManager<ShadowElement>, IViewManagerWithChildren
    {
        public override string Name => nameof(SVGName.RNSVGDefs);
        public override FrameworkElement CreateView()
        {
            Debug.WriteLine("RNSVGDefs CreateView");

            var shadowElement = new DefElement()
            {
                DefType = DefElementType.Group,
                Type = ShadowElementType.Def,
                DataContext = new DefsContext()
            };

            return shadowElement;
        }
        public void AddView(FrameworkElement parent, UIElement child, long index)
        {
        }

        void IViewManagerWithChildren.RemoveAllChildren(FrameworkElement parent)
        {
        }

        void IViewManagerWithChildren.RemoveChildAt(FrameworkElement parent, long index)
        {
        }

        void IViewManagerWithChildren.ReplaceChild(FrameworkElement parent, UIElement oldChild, UIElement newChild)
        {
        }
    }
}
