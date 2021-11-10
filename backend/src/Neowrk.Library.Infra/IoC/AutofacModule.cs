using Autofac;

namespace Neowrk.Library.Infra.IoC
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            AutofacConfiguration.Load(builder);
        }
    }
}
