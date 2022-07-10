using System;
using EcsTestProject.Systems;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
#if UNITY_EDITOR
using Leopotam.EcsLite.UnityEditor;
#endif
using Zenject;

namespace EcsTestProject
{
    internal class Startup : IInitializable, ITickable, IFixedTickable, IDisposable
    {
        private EcsWorld _world;
        private EcsSystems _systems;
        private EcsSystems _fixedUpdSystems;

        private GameData _gameData;
        
        [Inject]
        private void Construct(GameData gameData)
        {
            _gameData = gameData;
        }

        public void Initialize()
        {
            _world = new EcsWorld();
            InitializeUpdateSystems();
            InitializeFixedUpdateSystems();
        }

        public void Tick()
        {
            _systems?.Run();
        }

        public void FixedTick()
        {
            _fixedUpdSystems?.Run();
        }

        public void Dispose()
        {
            if (_systems != null)
            {
                _systems.Destroy();
                _systems = null;
            }

            if (_fixedUpdSystems != null)
            {
                _fixedUpdSystems.Destroy();
                _fixedUpdSystems = null;
            }

            _world?.Destroy();
            _world = null;
        }

        private void InitializeUpdateSystems()
        {
            _systems = new EcsSystems(_world);
            _systems
#if UNITY_EDITOR
                .Add(new EcsWorldDebugSystem())
#endif
                // Initialization
                .Add(new UnitySettingsBindingSys())
                .Add(new UnityLevelInitializeSys())
                .Add(new UnityMonobehViewSpawnSys())
                .Add(new UnityBindSpawnedMonobehsSys())
                
                // Services
                .Add(new UnityMouseInputSys())
                
                // Player
                .Add(new SetPlayerTargetSys())
                .Add(new SetPlayerTranslationMethodSys())
                .Add(new SetPlayerAnimationStateSys())
                
                // Buttons
                .Add(new CheckTriggersStateSys())
                .Add(new SetLinkedObserverStateSys())
                
                // Doors
                .Add(new SetDoorsTargetSys())
                .Add(new SetDoorsMovementMethodSys())
                
                // Common
                .Add(new MoveByLerpLogicSys())
                .Add(new RotateByConstantLogicSys())
                .Add(new CheckMovementCompleteSys())
                .Add(new CheckRotationCompleteSys())
                
                // MonobehViews
                .Add(new UnityMonobehViewUpdateSys())
                .Add(new UnityAnimationMonobehViewUpdateSys())
                
                .Inject(_gameData)
                .Init();
        }

        private void InitializeFixedUpdateSystems()
        {
            _fixedUpdSystems = new EcsSystems(_world);
            _fixedUpdSystems
#if UNITY_EDITOR
                .Add(new EcsWorldDebugSystem())
#endif
                .Init();
        }
    }
}