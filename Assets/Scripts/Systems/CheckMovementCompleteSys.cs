using System.Numerics;
using EcsTestProject.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace EcsTestProject.Systems
{
    internal class CheckMovementCompleteSys : IEcsRunSystem
    {
        private EcsFilterInject<Inc<PositionInfoComp, MoveToTargetComp>> _movingObjFilter = default;
        private EcsFilterInject<Inc<GameSettingsComp>> _gameSettingsCompFilter = default;
        
        public void Run(EcsSystems systems)
        {
            RemoveTargetWhenReached();
        }

        private void RemoveTargetWhenReached()
        {
            foreach (var movingEnt in _movingObjFilter.Value)
            {
                ref PositionInfoComp positionComp = ref _movingObjFilter.Pools.Inc1.Get(movingEnt);
                ref MoveToTargetComp moveToTargetComp = ref _movingObjFilter.Pools.Inc2.Get(movingEnt);

                var distance = Vector3.DistanceSquared(positionComp.Position, moveToTargetComp.TargetPosition);
                float destinationReachDistance = GetDestinationReachDistance();

                if (distance <= destinationReachDistance)
                {
                    _movingObjFilter.Pools.Inc2.Del(movingEnt);
                }
            }
        }

        private float GetDestinationReachDistance()
        {
            foreach (var settingsEnt in _gameSettingsCompFilter.Value)
            {
                return _gameSettingsCompFilter.Pools.Inc1.Get(settingsEnt).CommonSettings.DestinationReachDistance;
            }

            return default;
        }
    }
}