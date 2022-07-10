using System.Numerics;
using EcsTestProject.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace EcsTestProject.Systems
{
    internal class SetDoorsTargetSys : IEcsRunSystem
    {
        private EcsFilterInject<Inc<PositionInfoComp, LinkedObserverComp>, Exc<TriggerableComp>> _doorsFilter = default;
        private EcsPoolInject<MoveToTargetComp> _moveToTargetCompPool = default;
        private EcsFilterInject<Inc<GameSettingsComp>> _gameSettingsCompFilter = default;

        public void Run(EcsSystems systems)
        {
            MoveDoorWhenActive();
        }

        private void MoveDoorWhenActive()
        {
            foreach (var doorEnt in _doorsFilter.Value)
            {
                var isActive = _doorsFilter.Pools.Inc2.Get(doorEnt).IsActive;
                
                if (isActive)
                {
                    if (!_moveToTargetCompPool.Value.Has(doorEnt))
                    {
                        ref MoveToTargetComp moveToTargetComp = ref _moveToTargetCompPool.Value.Add(doorEnt);
                        ref PositionInfoComp positionInfoComp = ref _doorsFilter.Pools.Inc1.Get(doorEnt);
                        
                        Vector3 spawnPosition = positionInfoComp.SpawnPosition;
                        float doorHeight = GetDoorHeight();
                        Vector3 targetPos =
                            spawnPosition - Vector3.UnitY * doorHeight;

                        moveToTargetComp.TargetPosition = targetPos;
                    }
                }
                else
                {
                    if (_moveToTargetCompPool.Value.Has(doorEnt))
                    {
                        _moveToTargetCompPool.Value.Del(doorEnt);
                    }
                }
            }
        }
        
        private float GetDoorHeight()
        {
            foreach (var settingsEnt in _gameSettingsCompFilter.Value)
            {
                return _gameSettingsCompFilter.Pools.Inc1.Get(settingsEnt).DoorSettings.DoorHeight;
            }

            return default;
        }
    }
}