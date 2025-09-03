using StrikeBall.Core.Repositories;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace StrikeBall.Core.Joystick
{
    public class JoystickRepository : Repository
    {
        [SerializeField] private Transform _parent;
        [SerializeField] private JoystickView _view;
        [SerializeField] private JoystickData _data;
        
        public override void Register(IContainerBuilder builder)
        {
            builder.RegisterInstance(_data);
            builder.RegisterComponentInNewPrefab(_view, Lifetime.Singleton).UnderTransform(_parent);
            builder.Register<JoystickModel>(Lifetime.Singleton);
            builder.Register<JoystickMediator>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}