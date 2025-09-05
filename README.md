# StrikeBall - Clean Architecture Showcase in Unity

> Enterprise-grade clean architecture pattern demonstration 3D game in Unity, showcasing modern software engineering principles with cutting-edge dependency injection and event-driven design.

- **Unity Version:** 2022.3.25f1
- **Architecture Style:** Clean Architecture with MVP and Repository patterns  
- **Key Technologies:** VContainer DI, MessagePipe Events
- **Developer:** Pol Vega Díaz

## 🔧 Core Architecture

- **Clean Architecture principles** with proper dependency inversion and separation of concerns
- **MVP (Model-View-Presenter) patterns** adapted for Unity's component system
- **Advanced Dependency Injection** using VContainer for lifecycle management and scoping
- **Event-Driven Architecture** with MessagePipe for decoupled, type-safe communication
- **Use Case pattern** for encapsulating business logic
- **Repository pattern** for dependency management and object creation

### 1. Clean Architecture Implementation - Five-Layer Design

Every game feature follows a consistent **clean architecture pattern**:

```
ScriptableObject Data Layer    ← Configuration & design-time parameters
             ↓
        Model Layer            ← Business logic & state management
             ↓
      Mediator Layer           ← MVP coordination & Unity lifecycle
             ↓
         View Layer            ← Unity engine interactions & rendering
             ↓
     Repository Layer          ← Dependency injection & factory services
```

#### Why This Pattern?

- **Testability**: Pure C# models and use cases can be unit tested in isolation
- **Unity Decoupling**: Business logic doesn't depend on MonoBehaviour or Unity APIs
- **Separation of Concerns**: Each layer has a single, well-defined responsibility
- **Dependency Inversion**: High-level modules don't depend on low-level implementations
- **Scalability**: New features follow established patterns for consistency

### 2. Dependency Injection with VContainer

I've implemented a DI system using VContainer that handles complex scenarios:

#### **Lifetime Management Strategy**:
```csharp
public class GameLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        RegisterEvents(builder);        // Singleton event system
        RegisterUseCases(builder);      // Transient business operations  
        RegisterInstances(builder);     // Unity singletons (Camera, Canvas)
        RegisterRepositories(builder);  // Scoped entity management
    }
}
```

#### **Scoped Architecture for Dynamic Entities**:
- **RepositoryScopeContainer**: Creates child scopes for player/enemy entities
- **Proper Disposal**: Automatic cleanup when entities are destroyed
- **Isolation**: Each entity instance has its own dependency graph
- **Performance**: Efficient lifetime management without memory leaks

### 3. Event-Driven Architecture with MessagePipe

I've implemented a type-safe event system:

```csharp
// Type-safe event definitions
public readonly struct JoystickDragEvent { public Vector2 Direction; }
public readonly struct JoystickPressedEvent { }
public readonly struct JoystickReleasedEvent { }

// Publisher (Joystick System)
[Inject] private IPublisher<JoystickDragEvent> _dragPublisher;
_dragPublisher.Publish(new JoystickDragEvent { Direction = normalizedDirection });

// Subscriber (Player System)  
[Inject] private ISubscriber<JoystickDragEvent> _dragSubscriber;
_dragSubscriber.Subscribe(OnJoystickDrag);
```

**Benefits of This Design:**
- **Type Safety**: Compile-time verification of event contracts
- **Performance**: Zero allocation events with value types
- **Decoupling**: Publishers and subscribers have no direct dependencies
- **Debugging**: Clear event flow with proper tooling support

### 4. Use Case Pattern for Business Logic

I've encapsulated business operations in dedicated use case classes:

```csharp
public class PredictBallPositionUseCase
{
    public Vector3 GetCurrentBallPosition() => _ballRepository.Ball.View.Position;
    public Vector3 PredictPosition(float timeAhead) => /* Physics prediction logic */
}

public class TriggerBallHitUseCase  
{
    public void ExecuteHit(Vector3 direction, float force) => /* Ball physics application */
}
```

**Why Use Cases?**
- **Single Responsibility**: Each use case handles one business operation
- **Reusability**: Same logic can be used by multiple presenters
- **Testing**: Easy to unit test business logic in isolation
- **Coordination**: Complex operations span multiple repositories/services

### 5. Repository Pattern for Dependency Management

Each domain has its own repository handling dependency configuration:

```csharp
public class PlayerEntityRepository : Repository
{
    public override void Register(IContainerBuilder builder)
    {
        builder.Register<PlayerEntityData>(Lifetime.Singleton);
        builder.Register<PlayerEntityModel>(Lifetime.Scoped);
        builder.Register<PlayerEntityView>(Lifetime.Scoped);
        builder.Register<PlayerEntityMediator>(Lifetime.Scoped)
               .As<IInitializable>()
               .As<IFixedTickable>()
               .As<IDisposable>();
    }
}
```

## 🎮 Game-Specific Implementations

### Joystick System - Complete Clean Architecture Example

The Joystick system demonstrates the full architectural pattern in action, showing how all five layers work together:

**JoystickData** (Configuration Layer):
```csharp
[CreateAssetMenu(menuName = "Create Data/Joystick", fileName = "JoystickData")]
public class JoystickData : ScriptableObject
{
    private float _maxDistance;
    private float _relocateThreshold;
    private float _inputDeadZone;
}
```

**JoystickModel** (Pure Domain Logic):
```csharp
public class JoystickModel
{
    private Vector2 _direction;
    private float _magnitude;
    private bool _isActive;
    
    // Pure business logic - no Unity dependencies
    public void UpdateInput(Vector2 handlePosition)
    {
        var inputVector = handlePosition / _maxDistance;
        _direction = inputVector.normalized;
        _magnitude = Mathf.Clamp01(inputVector.magnitude);
        _isActive = Magnitude > _inputDeadZone;
    }
}
```

**JoystickView** (Unity Presentation):
```csharp
public class JoystickView : MonoBehaviour
{
    [Inject]
    public void Construct(Canvas parentCanvas, Camera camera)
    {
        // Unity-specific setup using injected dependencies
        _parentCanvas = parentCanvas;
        _camera = camera;
    }
    
    public void SetPosition(Vector2 position) => _rectTransform.anchoredPosition = position;
    public void SetHandlePosition(Vector2 position) => _joystickHandle.anchoredPosition = position;
}
```

**JoystickMediator** (MVP Presenter):
```csharp
public class JoystickMediator : IInitializable, ITickable, IDisposable
{
    private readonly JoystickView _view;                             // Presentation layer
    private readonly JoystickModel _model;                           // Domain layer
    private readonly TouchInputHandlerUseCase _touchInputHandler;    // Use case layer
    private readonly IPublisher<JoystickDragEvent> _dragPublisher;   // Event system
    
    public void Tick()
    {
        // Coordinate between model state and view presentation
        var isDragging = _model.IsDragging;
        _touchInputHandler.HandleInput(ref isDragging);
        _model.SetDragging(isDragging);

        // Publish events for decoupled communication
        if (_model.IsActive)
        {
            var inputEvent = new JoystickDragEvent(_model.Direction, _model.Magnitude);
            _dragPublisher.Publish(inputEvent);
        }
    }
}
```

Notice how the architecture demonstrates:
- **Layer Separation**: Each layer has distinct responsibilities and dependencies
- **Dependency Injection**: All dependencies injected through constructor/method injection
- **Event-Driven Communication**: Input events published for decoupled consumption
- **Pure Business Logic**: Model contains no Unity dependencies for easy testing
- **MVP Coordination**: Mediator orchestrates between Model, View, and external systems

## 📊 Architecture Patterns Implemented

This project showcases implementation of multiple enterprise patterns:

1. **Clean Architecture** - Dependency rule enforcement with proper layer separation
2. **MVP (Model-View-Presenter)** - Unity-adapted presentation pattern with Mediators
3. **Repository Pattern** - Dependency injection configuration and lifecycle management
4. **Use Case Pattern** - Business logic encapsulation and coordination
5. **Template Method Pattern** - Platform-specific input handling with common logic
6. **Observer Pattern** - Event-driven communication with MessagePipe
7. **Dependency Injection** - Advanced DI with VContainer and scoped lifetimes
8. **Mediator Pattern** - Coordination between Model and View layers
9. **Strategy Pattern** - AI vs Player input handling strategies
10. **Factory Method** - Repository-based object creation and configuration

## 🚀 Performance & Quality Benefits

### Architecture Benefits
- **Testability**: Pure C# business logic with dependency injection enables comprehensive unit testing
- **Maintainability**: Clear separation of concerns and single responsibility principle
- **Scalability**: Repository pattern and DI enable easy addition of new entity types
- **Platform Flexibility**: Abstract input handling supports multiple platforms seamlessly

### Unity-Specific Optimizations  
- **Lifecycle Management**: Proper disposal through VContainer prevents memory leaks
- **Update Loop Optimization**: Centralized FixedTick through VContainer interfaces
- **Event Performance**: Zero-allocation events with value types and MessagePipe
- **Assembly Definitions**: Faster compilation with modular dependencies

## 📈 Scalability Features

### Team Scaling
- **Consistent Patterns**: Every feature follows the same architectural template
- **Clear Boundaries**: Assembly definitions and layer separation prevent integration conflicts
- **Testable Design**: Business logic can be developed and tested independently of Unity

### Feature Scaling  
- **Repository Templates**: New entity types follow established dependency patterns
- **Event-Driven**: New features integrate without modifying existing systems
- **Use Case Reusability**: Business operations can be shared across different presenters

### Performance Scaling
- **Efficient DI**: VContainer provides Unity-optimized dependency resolution
- **Zero-Allocation Events**: MessagePipe events don't generate garbage
- **Proper Disposal**: Scoped lifetimes prevent memory leaks in dynamic scenarios

## 🎪 What This Project Demonstrates

This architecture showcase demonstrates ability to:

- **Implement Clean Architecture**: Properly separate concerns with dependency rule enforcement
- **Master Advanced Unity**: Leverage cutting-edge Unity technologies and patterns effectively  
- **Design for Scale**: Create systems that work for both prototypes and production games
- **Balance Trade-offs**: Optimize for maintainability, testability, and performance simultaneously
- **Think Systematically**: Design holistic solutions considering all stakeholders (developers, designers, QA)

The result is a game architecture that serves as a comprehensive blueprint for building complex, maintainable Unity projects while showcasing modern software engineering practices adapted specifically for game development constraints and requirements.
