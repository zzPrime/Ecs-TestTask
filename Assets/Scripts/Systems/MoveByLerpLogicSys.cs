using System.Numerics;
using EcsTestProject.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace EcsTestProject.Systems
{
    internal class MoveByLerpLogicSys : IEcsRunSystem
    {
        private EcsFilterInject<
            Inc<MovementMethodComp, PositionInfoComp, MoveToTargetComp, MovementParamsComp>> _movingObjFilter = default;
        private EcsCustomInject<GameData> _gameData = default;
        
        public void Run(EcsSystems systems)
        {
            UpdatePositionByLerpLogic();
        }

        private void UpdatePositionByLerpLogic()
        {
            foreach (var movingEnt in _movingObjFilter.Value)
            {
                MovementMethodComp methodComp = _movingObjFilter.Pools.Inc1.Get(movingEnt);

                if (methodComp.MovementMethod != TranslationLogic.Lerp)
                {
                    continue;
                }

                ref PositionInfoComp positionInfoComp = ref _movingObjFilter.Pools.Inc2.Get(movingEnt);
                MoveToTargetComp targetComp = _movingObjFilter.Pools.Inc3.Get(movingEnt);
                MovementParamsComp paramsComp = _movingObjFilter.Pools.Inc4.Get(movingEnt);

                float speed = paramsComp.MovementSpeed;
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