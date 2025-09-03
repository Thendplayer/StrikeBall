using System;
using VContainer;
using VContainer.Unity;

namespace StrikeBall.Core.Entity.Enemy
{
    public class EnemyEntityRepository : EntityRepository
    {
        public override void Register(IContainerBuilder builder)
        {
            base.Register(builder);
            builder.Register<EnemyEntityMediator>(Lifetime.Scoped)
                .AsSelf()
                .As<IInitializable>()
                .As<IDisposable>()
                .As<IFixedTickable>();
        }
    }
}