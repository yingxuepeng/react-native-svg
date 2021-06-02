using Microsoft.ReactNative;
using Microsoft.ReactNative.Managed;

namespace ReactNativeSvg
{
    public partial class ReactPackageProvider : IReactPackageProvider
    {
        public void CreatePackage(IReactPackageBuilder packageBuilder)
        {
            CreatePackageImplementation(packageBuilder);
            //packageBuilder.AddAttributedModules();
            packageBuilder.AddViewManagers();
        }

        /// <summary>
        /// This method is implemented by the C# code generator
        /// </summary>
        partial void CreatePackageImplementation(IReactPackageBuilder packageBuilder);
    }

}
