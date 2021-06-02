using ReactNativeSvg.svg.view;
using Windows.UI.Xaml;

namespace ReactNativeSvg.svg
{
    class BaseDefContext : DependencyObject
    {
        /**
         * COMMON
         **/
        public DefElement DefElement { get; set; }
        public SvgView SvgView { get; set; }
        public BaseDefContext ParentContext { get; set; }


        public virtual void UpdateSvgView(SvgView svgView, DefElement element)
        {
            SvgView = svgView;
        }

        /**
         * Def common
         **/
        public string Id { get; set; }
        
    }
}
