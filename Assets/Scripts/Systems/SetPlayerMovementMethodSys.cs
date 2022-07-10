using System;
using System.Numerics;
using EcsTestProject.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace EcsTestProject.Systems
{
    internal class SetPlayerMovementMethodSys : IEcsRunSystem
    {
        private EcsFilterInject<Inc<PlayerTag>, Exc<TranslationMethodComp>> _playerFilter = default;
        private EcsPoolInject<TranslationMethodComp> _lerpTagPool = default;

        public void Run(EcsSystems systems)
        {
            AddLerpMovementMethodTag();
            //UpdateRotationByConstSpeedLogic();
        }

        private void AddLerpMovementMethodTag()
        {
            foreach (var playerEnt in _playerFilter.Value)
            {
                ref TranslationMethodComp methodComp = ref _lerpTagPool.Value.Add(playerEnt);
                methodComp.TranslationLogic = TranslationLogic.Lerp;
            }
        }

        /*
        private void UpdateRotationByConstSpeedLogic()
        {
            foreach (var playerEnt in _playerFilter.Value)
            {
                ref PositionInfoComp positionInfoComp = ref _playerFilter.Pools.Inc2.Get(playerEnt);
                ref RotateToTargetComp rotateToTargetComp = ref _playerFilter.Pools.Inc4.Get(playerEnt);

                var newRotationVector = CalculateNewRotation(positionInfoComp, rotateToTargetComp);
                positionInfoComp.RotationVector = newRotationVector;
            }
        }

        private Vector3 CalculateNewRotation(PositionInfoComp positionInfoComp, RotateToTargetComp rotateToTargetComp)
        {
            Vector3 currentRotVec = positionInfoComp.RotationVector;
            Vector3 targetRotVec = rotateToTargetComp.TargetRotationVector;

            var cos = Vector3.Dot(currentRotVec, targetRotVec) / (currentRotVec.Length() * targetRotVec.Length());
            var angleInRad = Math.Acos(cos);

            if (angleInRad < 0.05f) // TODO Delete rotation comp when target achieved
            {
                return currentRotVec;
            }

            Vector3 crossProduct = Vector3.Cross(currentRotVec, targetRotVec);
            float rotationSpeed = 10f * _gameData.Value.GameSettings.DeltaTime; //TODO Get from config;
            if (crossProduct.Y < 0f)
            {
                rotationSpeed *= -1;
            }
             
            Quaternion rotQuat = Quaternion.CreateFromYawPitchRoll(rotationSpeed, 0f, 0f);
            var newRotationVector = Vector3.Transform(currentRotVec, rotQuat);

            return newRotationVector;
        }
        */
    }
}