using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;

namespace ReactNativeSvg.svg
{
    public enum DefElementType
    {
        Group, Brush, ClipPath
    }
    class DefElement : ShadowElement
    {
        public DefElementType DefType { get; set; }
        public string DefId { get; set; }
        public Brush DefBrush { get; set; }
    }
}
