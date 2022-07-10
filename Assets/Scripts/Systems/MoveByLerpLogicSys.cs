using System.Numerics;
using EcsTestProject.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace EcsTestProject.Systems
{
    internal class MoveByLerpLogicSys : IEcsRunSystem
    {
        private EcsFilterInject<
            Inc<PositionInfoComp, MoveToTargetComp, MovementParamsComp, MovementMethodTag.Lerp>> _lerpMovingFilter = default;
        private EcsCustomInject<GameData> _gameData = default;
        
        public void Run(EcsSystems systems)
        {
            UpdatePositionByLerpLogic();
        }

        private void UpdatePositionByLerpLogic()
        {
            foreach (var movingEnt in _lerpMovingFilter.Value)
            {
                ref PositionInfoComp positionInfoComp = ref _lerpMovingFilter.Pools.Inc1.Get(movingEnt);
                MoveToTargetComp targetComp = _lerpMovingFilter.Pools.Inc2.Get(movingEnt);
                MovementParamsComp paramsComp = _lerpMovingFilter.Pools.Inc3.Get(movingEnt);

                float speed = paramsComp.Speed;
                float deltaTime = _gameData.Value.GameSettings.DeltaTime;
                
                Vector3 currentPos = positionInfoComp.Position;
                Vector3 targetPos = targetComp.TargetPosition;
                
                float t = Vector3.Distance(targetPos ,currentPos) / speed;
                Vector3 newPos = Vector3.Lerp(currentPos, targetPos, 1/t * deltaTime);

                positionInfoComp.Position = newPos;
            }
        }
    }
}