using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace ReactNativeSvg.svg
{
    public enum ShadowElementType
    {
        View, Def

    }
    public class ShadowElement : FrameworkElement
    {
        // type
        public ShadowElementType Type { get; set; } = ShadowElementType.View;
        // self view
        public FrameworkElement RealElement { get; set; }
        // group only
        public readonly List<ShadowElement> ShadowChildren = new List<ShadowElement>();
    }
}
