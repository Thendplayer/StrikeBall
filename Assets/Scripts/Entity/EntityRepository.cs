using StrikeBall.Core.Repositories;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace StrikeBall.Core.Entity
{
    public abstract class EntityRepository : Repository
    {
        [SerializeField] private EntityView _view;
        [SerializeField] private EntityData _data;
        
        public override void Register(IContainerBuilder builder)
        {
            builder.RegisterInstance(_data);
            builder.RegisterComponentInNewPrefab(_view, Lifetime.Scoped);
            builder.Register<EntityModel>(Lifetime.Scoped);
        }
    }
}