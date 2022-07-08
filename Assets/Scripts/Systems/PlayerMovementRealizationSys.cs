using System.Numerics;
using EcsTestProject.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace EcsTestProject.Systems
{
    internal class PlayerMovementRealizationSys : IEcsRunSystem
    {
        private EcsWorldInject _world = default;
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

                Vector3 currentPos = positionInfoComp.Position;
                Vector3 targetPos = targetComp.TargetPosition + Vector3.UnitY * 1f; //TODO Get height
                
                float t = Vector3.Distance(targetPos ,currentPos) / 0.5f; //TODO Set speed from data
                Vector3 newPos = Vector3.Lerp(currentPos, targetPos, 1/t * 0.01f);

                positionInfoComp.Position = newPos;
            }
        }
    }
}