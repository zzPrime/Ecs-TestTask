using System.Numerics;
using EcsTestProject.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace EcsTestProject.Systems
{
    internal class RotateByConstantLogicSys : IEcsRunSystem
    {
        private EcsFilterInject<
            Inc<RotationMethodComp, PositionInfoComp, RotationParamsComp, RotateToTargetComp>> _rotatingObjFilter = default;

        private EcsFilterInject<Inc<GameSettingsComp>> _gameSettingsCompFilter = default;

        public void Run(EcsSystems systems)
        {
            UpdateRotationByConstSpeedLogic();
        }


        private void UpdateRotationByConstSpeedLogic()
        {
            foreach (var rotatingEnt in _rotatingObjFilter.Value)
            {
                ref RotationMethodComp rotationMethodComp = ref _rotatingObjFilter.Pools.Inc1.Get(rotatingEnt);
                
                if (rotationMethodComp.RotationMethod != TranslationLogic.Constant)
                {
                    continue;
                }

                ref PositionInfoComp positionInfoComp = ref _rotatingObjFilter.Pools.Inc2.Get(rotatingEnt);

                var newRotationVector = CalculateNewRotationVec(positionInfoComp, rotatingEnt);
                positionInfoComp.RotationVector = newRotationVector;
            }
        }

        private Vector3 CalculateNewRotationVec(PositionInfoComp positionInfoComp, int entity)
        {
            RotationParamsComp rotationParamsComp = _rotatingObjFilter.Pools.Inc3.Get(entity);
            RotateToTargetComp rotateToTargetComp = _rotatingObjFilter.Pools.Inc4.Get(entity);

            Vector3 currentRotVec = positionInfoComp.RotationVector;
            Vector3 targetRotVec = rotateToTargetComp.TargetRotationVector;

            Vector3 crossProduct = Vector3.Cross(currentRotVec, targetRotVec);
            float rotationSpeed = rotationParamsComp.RotationSpeed * GetDeltaTime();
            
            if (crossProduct.Y < 0f)
            {
                rotationSpeed *= -1;
            }

            Quaternion rotQuat = Quaternion.CreateFromYawPitchRoll(rotationSpeed, 0f, 0f);
            var newRotationVector = Vector3.Transform(currentRotVec, rotQuat);

            return newRotationVector;
        }
        
        private float GetDeltaTime()
        {
            foreach (var settingsEnt in _gameSettingsCompFilter.Value)
            {
                return _gameSettingsCompFilter.Pools.Inc1.Get(settingsEnt).CommonSettings.DeltaTime;
            }

            return default;
        }
    }
}