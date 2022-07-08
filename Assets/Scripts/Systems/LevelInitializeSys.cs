using EcsTestProject.Components;
using EcsTestProject.LevelData;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace EcsTestProject.Systems
{
    internal class LevelInitializeSys : IEcsRunSystem
    {
        private EcsWorldInject _world = default;
        private EcsFilterInject<Inc<InstantiateLevelRequireTag>> _instantiateRequreTagFilter = default;
        private EcsPoolInject<SpawnMbViewComp> _spawnMbViewCompFilter = default;
        private EcsCustomInject<GameData> _levelData = default;
        
        public void Run(EcsSystems systems)
        {
            if (!IsInstantiateRequired())
            {
                return;
            }

            InitializePlayerData();
            InitializeDoorsData();
            InitializeButtonsData();
            SetLevelInstantiate();
        }

        private bool IsInstantiateRequired()
        {
            return _instantiateRequreTagFilter.Value.GetEntitiesCount() == 0;
        }

        private void InitializePlayerData()
        {
            LevelView levelView = _levelData.Value.LevelView;

            foreach (var element in levelView.ElementData)
            {
                if (element.LevelElementType == LevelElementType.PlayerSpawn)
                {
                    var playerEnt = _world.Value.NewEntity();
                    //TODO Add Data components
                    ref SpawnMbViewComp spawnComp = ref _spawnMbViewCompFilter.Value.Add(playerEnt);
                    spawnComp.Prefab = _levelData.Value.PlayerView;
                    spawnComp.SpawnPosition = element.ElementTf.position;
                    spawnComp.SpawnRotation = element.ElementTf.rotation;
                }
            }
        }
        
        private void InitializeDoorsData()
        {
            LevelView levelView = _levelData.Value.LevelView;

            foreach (var element in levelView.ElementData)
            {
                if (element.LevelElementType == LevelElementType.Door)
                {
                    var doorEnt = _world.Value.NewEntity();
                    //TODO Add Data components
                    ref SpawnMbViewComp spawnComp = ref _spawnMbViewCompFilter.Value.Add(doorEnt);
                    spawnComp.Prefab = _levelData.Value.PlayerView;
                    spawnComp.SpawnPosition = element.ElementTf.position;
                    spawnComp.SpawnRotation = element.ElementTf.rotation;
                }
            }
        }

        private void InitializeButtonsData()
        {
            LevelView levelView = _levelData.Value.LevelView;

            foreach (var element in levelView.ElementData)
            {
                if (element.LevelElementType == LevelElementType.Button)
                {
                    var buttonEnt = _world.Value.NewEntity();
                    //TODO Add Data components
                    ref SpawnMbViewComp spawnComp = ref _spawnMbViewCompFilter.Value.Add(buttonEnt);
                    spawnComp.Prefab = _levelData.Value.PlayerView;
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