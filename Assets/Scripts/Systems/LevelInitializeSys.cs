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

        private LevelView LevelView => _levelData.Value.LevelView;
        
        public void Run(EcsSystems systems)
        {
            if (!IsInstantiateRequired())
            {
                return;
            }

            InitializePlayerData();
            InitializeLevel();
            SetLevelInstantiate();
        }

        private void InitializeLevel()
        {
            var levelEnt = _world.Value.NewEntity();
            ref SpawnMbViewComp spawnComp = ref _spawnMbViewCompFilter.Value.Add(levelEnt);
            spawnComp.Prefab = LevelView.gameObject;
            spawnComp.SpawnPosition = LevelView.transform.position;
            spawnComp.SpawnRotation = LevelView.transform.rotation;
            
            InitializeDoorsData();
            InitializeButtonsData();
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
                    ref SpawnMbViewComp spawnComp = ref _spawnMbViewCompFilter.Value.Add(playerEnt);
                    spawnComp.Prefab = _levelData.Value.PlayerView;
                    spawnComp.SpawnPosition = element.ElementTf.position;
                    spawnComp.SpawnRotation = element.ElementTf.rotation;
                }
            }
        }
        
        private void InitializeDoorsData()
        {
            foreach (var element in LevelView.ElementData)
            {
                if (element.LevelElementType == LevelElementType.Door)
                {
                    //var doorEnt = _world.Value.NewEntity();
                    //TODO Add Data components
                    //We don't need to spawn it but we have ent for it and we will bind it when level will be spawned
                }
            }
        }

        private void InitializeButtonsData()
        {
            foreach (var element in LevelView.ElementData)
            {
                if (element.LevelElementType == LevelElementType.Button)
                {
                    //var buttonEnt = _world.Value.NewEntity();
                    //TODO Add Data components
                    //We don't need to spawn it but we have ent for it and we will bind it when level will be spawned
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