using System;
using EcsTestProject.Systems;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.UnityEditor;
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
                .Add(new LevelInitializeSys())
                .Add(new MonobehViewSpawnSys())
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