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
                        Vector3 currPos = _doorsFilter.Pools.Inc1.Get(doorEnt).Position;
                        Vector3 targetPos =
                            currPos - Vector3.UnitY * currPos.Y - Vector3.UnitY * 0.5f; //TODO Get from data

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
    }
}