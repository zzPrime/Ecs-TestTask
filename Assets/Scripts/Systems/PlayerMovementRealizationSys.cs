using System;
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
        private EcsFilterInject<Inc<PlayerTag, PositionInfoComp, MoveToTargetComp, RotateToTargetComp>> _playerFilter = default;

        public void Run(EcsSystems systems)
        {
            UpdatePositionByLerpLogic();
            UpdateRotationByConstSpeedLogic();
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
    }
}