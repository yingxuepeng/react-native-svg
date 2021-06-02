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
    class RadialGradientContext : BaseDefContext
    {
        public double? Fx { get; set; }
        public double? Fy { get; set; }
        public double? Rx { get; set; }
        public double? Ry { get; set; }
        public double? Cx { get; set; }
        public double? Cy { get; set; }
    }
    class RadialGradientManager : BaseDefManager
    {
        public override string Name => nameof(SVGName.RNSVGRadialGradient);
        public override FrameworkElement CreateView()
        {
            Debug.WriteLine("RNSVGDefs CreateView");

            var shadowElement = new DefElement()
            {
                DefType = DefElementType.Brush,
                Type = ShadowElementType.Def,
                DataContext = new RadialGradientContext()
            };

            return shadowElement;
        }


    }
}
