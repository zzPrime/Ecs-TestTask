using System.Numerics;
using EcsTestProject.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace EcsTestProject.Systems
{
    internal class DoorsMovementRealizationSys : IEcsRunSystem
    {
        private EcsFilterInject<Inc<PositionInfoComp, MoveToTargetComp, LinkedObserverComp>, Exc<PlayerTag>> _doorsFilter = default;
        private EcsCustomInject<GameData> _gameData = default;

        public void Run(EcsSystems systems)
        {
            UpdatePositionByLerpLogic();
        }

        private void UpdatePositionByLerpLogic()
        {
            foreach (var doorEnt in _doorsFilter.Value)
            {
                ref PositionInfoComp positionInfoComp = ref _doorsFilter.Pools.Inc1.Get(doorEnt);
                MoveToTargetComp targetComp = _doorsFilter.Pools.Inc2.Get(doorEnt);

                Vector3 currentPos = positionInfoComp.Position;
                Vector3 targetPos = targetComp.TargetPosition;

                float doorSpeed = _gameData.Value.GameSettings.DoorSpeed;
                float deltaTime = _gameData.Value.GameSettings.DeltaTime;
                
                float t = Vector3.Distance(targetPos ,currentPos) / doorSpeed;
                Vector3 newPos = Vector3.Lerp(currentPos, targetPos, 1/t * deltaTime);

                positionInfoComp.Position = newPos;
            }
        }
    }
}