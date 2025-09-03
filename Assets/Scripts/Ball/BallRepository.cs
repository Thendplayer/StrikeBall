using StrikeBall.Core.Repositories;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace StrikeBall.Core.Ball
{
    public class BallRepository : Repository
    {
        [SerializeField] private BallView _view;
        [SerializeField] private BallData _data;
        
        public override void Register(IContainerBuilder builder)
        {
            builder.RegisterInstance(_data);
            builder.RegisterComponentInNewPrefab(_view, Lifetime.Singleton);
            builder.Register<BallModel>(Lifetime.Singleton);
            builder.Register<BallMediator>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}