using Microsoft.ReactNative.Managed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactNativeSvg.svg
{
    public enum DefProps
    {
        id,
    }
    class BaseDefManager : MyAttributedViewManager<DefElement>
    {
        [ViewManagerProperty(nameof(DefProps.id))]
        public void SetStroke(DefElement view, string id)
        {
            var ele = view.RealElement;
            BaseDefContext viewContext = view.DataContext as BaseDefContext;
            viewContext.Id = id;
        }


        public void UpdateDefBrush(DefElement view) {
            var defContext = view.DataContext as BaseDefContext;
        }
    }
}
