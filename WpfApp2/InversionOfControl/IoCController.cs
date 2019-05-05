using Ninject;
using Ninject.Modules;
using UI.UIPresenter.ViewModels;

namespace UI.InversionOfControl
{
    public class IoCController
    {
        public static IKernel Kernel { get; private set; } = new StandardKernel();

        /// <summary>
        /// Binds all dependencies
        /// </summary>
        public static void SetUp()
        {
            Bind();
        }

        public static void Bind()
        {
            Kernel.Bind<ApplicationViewModel>().ToConstant(new ApplicationViewModel());
        }
    }
}
