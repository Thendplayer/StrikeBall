using System;
using VContainer;
using VContainer.Unity;

namespace StrikeBall.Core.Entity.Player
{
    public class PlayerEntityRepository : EntityRepository
    {
        public override void Register(IContainerBuilder builder)
        {
            base.Register(builder);
            builder.Register<PlayerEntityMediator>(Lifetime.Scoped)
                .AsSelf()
                .As<IInitializable>()
                .As<IDisposable>()
                .As<IFixedTickable>();
        }
    }
}