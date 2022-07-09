using System.Numerics;
using EcsTestProject.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace EcsTestProject.Systems
{
    internal class PlayerMovementRealizationSys : IEcsRunSystem
    {
        private EcsWorldInject _world = default;
        private EcsCustomInject<GameData> _gameData = default;
        private EcsFilterInject<Inc<PlayerTag, PositionInfoComp, MoveToTargetComp>> _playerFilter = default;

        public void Run(EcsSystems systems)
        {
            UpdatePositionByLerpLogic();
        }

        private void UpdatePositionByLerpLogic()
        {
            foreach (var playerEnt in _playerFilter.Value)
            {
                ref PositionInfoComp positionInfoComp = ref _playerFilter.Pools.Inc2.Get(playerEnt);
                MoveToTargetComp targetComp = _playerFilter.Pools.Inc3.Get(playerEnt);

                float playerSpeed = _gameData.Value.GameSettings.PlayerMovementSpeed;
                float deltaTime = _gameData.Value.GameSettings.DeltaTime;
                
                Vector3 currentPos = positionInfoComp.Position;
                Vector3 targetPos = targetComp.TargetPosition;
                
                float t = Vector3.Distance(targetPos ,currentPos) / playerSpeed;
                Vector3 newPos = Vector3.Lerp(currentPos, targetPos, 1/t * deltaTime);

                positionInfoComp.Position = newPos;
            }
        }
    }
}