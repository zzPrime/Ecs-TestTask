﻿using System;
using System.Numerics;
using EcsTestProject.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace EcsTestProject.Systems
{
    internal class CheckRotationCompleteSys : IEcsRunSystem
    {
        private EcsFilterInject<Inc<PositionInfoComp, RotateToTargetComp>> _rotatingObjFilter = default;
        private EcsCustomInject<GameData> _gameData = default;
        
        public void Run(EcsSystems systems)
        {
            RemoveTargetWhenReached();
        }

        private void RemoveTargetWhenReached()
        {
            foreach (var rotatingEnt in _rotatingObjFilter.Value)
            {
                PositionInfoComp positionInfoComp = _rotatingObjFilter.Pools.Inc1.Get(rotatingEnt);
                RotateToTargetComp rotateToTargetComp = _rotatingObjFilter.Pools.Inc2.Get(rotatingEnt);

                Vector3 currentRotVec = positionInfoComp.RotationVector;
                Vector3 targetRotVec = rotateToTargetComp.TargetRotationVector;

                var cos = Vector3.Dot(currentRotVec, targetRotVec) / (currentRotVec.Length() * targetRotVec.Length());
                var angleInRad = Math.Acos(cos);

                if (angleInRad < _gameData.Value.GameSettings.StopRotationAngleRad)
                {
                    _rotatingObjFilter.Pools.Inc2.Del(rotatingEnt);
                }
            }
        }
    }
}