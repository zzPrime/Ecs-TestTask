using System.Numerics;
using EcsTestProject.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace EcsTestProject.Systems
{
    internal class SetDoorsMovementMethodSys : IEcsRunSystem
    {
        private EcsFilterInject<Inc<LinkedObserverComp>, Exc<MovementMethodTag.Lerp>> _doorsFilter = default;
        private EcsPoolInject<MovementMethodTag.Lerp> _lerpTagPool = default;

        public void Run(EcsSystems systems)
        {
            AddLerpMovementMethodTag();
        }

        private void AddLerpMovementMethodTag()
        {
            foreach (var doorEnt in _doorsFilter.Value)
            {
                _lerpTagPool.Value.Add(doorEnt);
            }
        }
    }
}