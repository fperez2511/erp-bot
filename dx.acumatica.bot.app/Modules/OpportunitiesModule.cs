using Autofac;
using dx.acumatica.bot.app.Dialogs;
using dx.acumatica.bot.app.Services;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Internals.Fibers;
using Microsoft.Bot.Builder.Luis;

namespace dx.acumatica.bot.app.Modules
{
    /// <summary>
    ///     These are the services (and their dependency structure) for the alarm sample.
    /// </summary>
    public sealed class OpprtunitiesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.Register(
                c => new LuisModelAttribute("d31591c9-ee2d-4856-a859-eca541cff15e", "7687843975f7495d9fb5471a2ac983bc"))
                .AsSelf()
                .AsImplementedInterfaces()
                .SingleInstance();

            // register the top level dialog
            builder.RegisterType<OpportunitiesDialog>().As<IDialog<object>>().InstancePerDependency();

            // register some singleton services
            builder.RegisterType<LuisService>()
                .Keyed<ILuisService>(FiberModule.Key_DoNotSerialize)
                .AsImplementedInterfaces()
                .SingleInstance();
            builder.RegisterType<OpportunitiesService>()
                .Keyed<IOpportunitiesService>(FiberModule.Key_DoNotSerialize)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();
        }
    }
}