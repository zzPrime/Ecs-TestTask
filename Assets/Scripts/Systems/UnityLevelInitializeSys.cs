using EcsTestProject.Components;
using EcsTestProject.LevelData;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace EcsTestProject.Systems
{
    internal class UnityLevelInitializeSys : IEcsRunSystem
    {
        private EcsWorldInject _world = default;
        
        private EcsFilterInject<Inc<LevelInstantiatedTag>> _instantiateRequireTagFilter = default;
        
        private EcsPoolInject<SpawnMbViewComp> _spawnMbViewCompPool = default;
        private EcsPoolInject<PlayerTag> _playerTagPool = default;
        private EcsPoolInject<TriggerableComp> _triggerableCompPool = default;
        private EcsPoolInject<CanTriggerTag> _canTriggerTagPool = default;
        private EcsPoolInject<LinkByColorComp> _linkByColorCompPool = default;
        private EcsPoolInject<LinkedObserverComp> _linkedObserverCompPool = default;
        private EcsPoolInject<MovementParamsComp> _movementParamsCompPool = default;
        private EcsPoolInject<RotationParamsComp> _rotationParamsCompPool = default;
        
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
            return _instantiateRequireTagFilter.Value.GetEntitiesCount() == 0;
        }

        private void InitializePlayerData()
        {
            foreach (var element in LevelView.ElementData)
            {
                if (element.LevelElementType == LevelElementType.PlayerSpawn)
                {
                    var playerEnt = _world.Value.NewEntity();
                    _playerTagPool.Value.Add(playerEnt);
                    _canTriggerTagPool.Value.Add(playerEnt);
                    
                    ref MovementParamsComp movementparamsComp = ref _movementParamsCompPool.Value.Add(playerEnt);
                    movementparamsComp.MovementSpeed = _levelData.Value.GameSettings.PlayerMovementSpeed;
                    
                    ref RotationParamsComp rotationParamsComp = ref _rotationParamsCompPool.Value.Add(playerEnt);
                    rotationParamsComp.RotationSpeed = _levelData.Value.GameSettings.PlayerRotationSpeed;
                    
                    ref SpawnMbViewComp spawnComp = ref _spawnMbViewCompPool.Value.Add(playerEnt);
                    spawnComp.Prefab = _levelData.Value.PlayerView.Transform.gameObject;
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
                    var doorEnt = _world.Value.NewEntity();
                    _linkedObserverCompPool.Value.Add(doorEnt);

                    ref LinkByColorComp linkByColorComp = ref _linkByColorCompPool.Value.Add(doorEnt);
                    linkByColorComp.ColorID = element.ColorID;
                    
                    ref MovementParamsComp paramsComp = ref _movementParamsCompPool.Value.Add(doorEnt);
                    paramsComp.MovementSpeed = _levelData.Value.GameSettings.DoorSpeed;
                    
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
                    _triggerableCompPool.Value.Add(buttonEnt);
                    
                    ref LinkByColorComp linkByColorComp = ref _linkByColorCompPool.Value.Add(buttonEnt);
                    linkByColorComp.ColorID = element.ColorID;
                    
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
            _instantiateRequireTagFilter.Pools.Inc1.Add(ent);
        }
    }
}