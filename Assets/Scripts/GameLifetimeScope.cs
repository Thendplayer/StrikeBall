using MessagePipe;
using StrikeBall.Core.Repositories;
using StrikeBall.Core.Events;
using StrikeBall.Core.UseCases;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace StrikeBall.Core
{
    public class GameLifetimeScope : LifetimeScope
    {
        [Header("References")]
        [SerializeField] private Canvas _canvas;
        [SerializeField] private RepositoryScopeContainer _repositoryScopeContainer;
        [SerializeField] private Repository[] _repositories;
        
        protected override void Configure(IContainerBuilder builder)
        {
            RegisterEvents(builder);
            RegisterUseCases(builder);
            RegisterInstances(builder);
            RegisterRepositories(builder);
            builder.RegisterBuildCallback(RegisterScopedRepositories);
        }

        protected override void OnDestroy()
        {
            _repositoryScopeContainer.Dispose();
            base.OnDestroy();
        }

        private static void RegisterEvents(IContainerBuilder builder)
        {
            var options = builder.RegisterMessagePipe();
            builder.RegisterMessageBroker<JoystickDragEvent>(options);
            builder.RegisterMessageBroker<JoystickPressedEvent>(options);
            builder.RegisterMessageBroker<JoystickReleasedEvent>(options);
        }
        
        private void RegisterUseCases(IContainerBuilder builder)
        {
            builder.Register<PredictBallPositionUseCase>(Lifetime.Transient);
            builder.Register<TriggerBallHitUseCase>(Lifetime.Transient);
            
#if UNITY_EDITOR
            builder.Register<EditorTouchInputUseCase>(Lifetime.Transient).As<TouchInputHandlerUseCase>();
#else
            builder.Register<MobileTouchInputUseCase>(Lifetime.Transient).As<TouchInputHandlerUseCase>();
#endif
        }
        
        private void RegisterInstances(IContainerBuilder builder)
        {
            builder.RegisterInstance(_canvas).As<Canvas>();
            builder.RegisterInstance(Camera.main).As<Camera>();
        }
        
        private void RegisterRepositories(IContainerBuilder builder)
        {
            foreach (var repository in _repositories)
            {
                repository.Register(builder);
            }
        }
        
        private void RegisterScopedRepositories(IObjectResolver _)
        {
            _repositoryScopeContainer.Register(this);
        }
    }
}