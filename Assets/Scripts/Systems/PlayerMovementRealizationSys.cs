using System.Numerics;
using EcsTestProject.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace EcsTestProject.Systems
{
    internal class PlayerMovementRealizationSys : IEcsRunSystem
    {
        private EcsWorldInject _world = default;
        private EcsFilterInject<Inc<PlayerTag, MovableComp>> _playerFilter = default;
        private EcsPoolInject<MoveToTargetComp> _moveToTargetPool = default;

        public void Run(EcsSystems systems)
        {
            UpdatePositionByLerpLogic();
        }

        private void UpdatePositionByLerpLogic()
        {
            foreach (var playerEnt in _playerFilter.Value)
            {
                ref MovableComp movableComp = ref _playerFilter.Pools.Inc2.Get(playerEnt);
                MoveToTargetComp targetComp = _moveToTargetPool.Value.Get(playerEnt);

                Vector3 currentPos = movableComp.Position;
                Vector3 targetPos = targetComp.TargetPosition;
                
                float t = Vector3.Distance(targetPos ,currentPos) / 0.5f; //TODO Set speed from data
                Vector3 newPos = Vector3.Lerp(currentPos, targetPos, 1/t * 0.01f);

                movableComp.Position = newPos;
            }
        }
    }
}