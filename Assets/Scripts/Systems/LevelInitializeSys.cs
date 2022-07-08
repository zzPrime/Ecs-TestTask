using EcsTestProject.Components;
using EcsTestProject.LevelData;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace EcsTestProject.Systems
{
    internal class LevelInitializeSys : IEcsRunSystem
    {
        private EcsWorldInject _world = default;
        private EcsFilterInject<Inc<LevelInstantiatedTag>> _instantiateRequreTagFilter = default;
        
        private EcsPoolInject<SpawnMbViewComp> _spawnMbViewCompPool = default;
        private EcsPoolInject<PositionInfoComp> _positionInfoCompPool = default;
        private EcsPoolInject<PlayerTag> _playerTagPool = default;
        
        private EcsCustomInject<GameData> _levelData = default;

        private LevelView LevelView => _levelData.Value.LevelView;
        
        public void Run(EcsSystems systems)
        {
            if (!IsInstantiateRequired())
            {
                return;
            }

            InitializePlayerData();
            InitializeLevel();
            InitializeDoorsData();
            InitializeButtonsData();
            SetLevelInstantiate();
        }

        private void InitializeLevel()
        {
            var levelEnt = _world.Value.NewEntity();
            ref SpawnMbViewComp spawnComp = ref _spawnMbViewCompPool.Value.Add(levelEnt);
            spawnComp.Prefab = LevelView.gameObject;
            spawnComp.SpawnPosition = LevelView.transform.position;
            spawnComp.SpawnRotation = LevelView.transform.rotation;
        }

        private bool IsInstantiateRequired()
        {
            return _instantiateRequreTagFilter.Value.GetEntitiesCount() == 0;
        }

        private void InitializePlayerData()
        {
            foreach (var element in LevelView.ElementData)
            {
                if (element.LevelElementType == LevelElementType.PlayerSpawn)
                {
                    var playerEnt = _world.Value.NewEntity();
                    
                    //TODO Add Data components
                    ref SpawnMbViewComp spawnComp = ref _spawnMbViewCompPool.Value.Add(playerEnt);
                    spawnComp.Prefab = _levelData.Value.PlayerView;
                    spawnComp.SpawnPosition = element.ElementTf.position;
                    spawnComp.SpawnRotation = element.ElementTf.rotation;

                    _playerTagPool.Value.Add(playerEnt);
                    _positionInfoCompPool.Value.Add(playerEnt);
                }
            }
        }
        
        private void InitializeDoorsData()
        {
            foreach (var element in LevelView.ElementData)
            {
                if (element.LevelElementType == LevelElementType.Door)
                {
                    var doorEnt = _world.Value.NewEntity();
                    _positionInfoCompPool.Value.Add(doorEnt);
                    
                    //TODO Add Data components

                    ref SpawnMbViewComp spawnComp = ref _spawnMbViewCompPool.Value.Add(doorEnt);
                    spawnComp.Prefab = element.ElementTf.gameObject;
                    spawnComp.SpawnPosition = element.ElementTf.position;
                    spawnComp.SpawnRotation = element.ElementTf.rotation;
                }
            }
        }

        private void InitializeButtonsData()
        {
            foreach (var element in LevelView.ElementData)
            {
                if (element.LevelElementType == LevelElementType.Button)
                {
                    var buttonEnt = _world.Value.NewEntity();
                    _positionInfoCompPool.Value.Add(buttonEnt);
                    
                    //TODO Add Data components

                    ref SpawnMbViewComp spawnComp = ref _spawnMbViewCompPool.Value.Add(buttonEnt);
                    spawnComp.Prefab = element.ElementTf.gameObject;
                    spawnComp.SpawnPosition = element.ElementTf.position;
                    spawnComp.SpawnRotation = element.ElementTf.rotation;
                }
            }
        }

        private void SetLevelInstantiate()
        {
            var ent = _world.Value.NewEntity();
            _instantiateRequreTagFilter.Pools.Inc1.Add(ent);
        }
    }
}