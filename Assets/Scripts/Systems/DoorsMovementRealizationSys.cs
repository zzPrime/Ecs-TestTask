using System.Numerics;
using EcsTestProject.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace EcsTestProject.Systems
{
    internal class DoorsMovementRealizationSys : IEcsRunSystem
    {
        private EcsFilterInject<Inc<PositionInfoComp, MoveToTargetComp, LinkedObserverComp>, Exc<PlayerTag>> _doorsFilter = default;

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
                Vector3 targetPos = targetComp.TargetPosition; //TODO Get height
                
                float t = Vector3.Distance(targetPos ,currentPos) / 0.1f; //TODO Set speed from data
                Vector3 newPos = Vector3.Lerp(currentPos, targetPos, 1/t * 0.01f);

                positionInfoComp.Position = newPos;
            }
        }
    }
}