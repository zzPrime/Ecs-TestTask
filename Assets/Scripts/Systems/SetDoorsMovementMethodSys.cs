using EcsTestProject.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace EcsTestProject.Systems
{
    internal class SetDoorsMovementMethodSys : IEcsRunSystem
    {
        private EcsFilterInject<Inc<LinkedObserverComp>, Exc<MovementMethodComp>> _doorsWithoutMovementMethodFilter = default;
        private EcsPoolInject<MovementMethodComp> _lerpTagPool = default;

        public void Run(EcsSystems systems)
        {
            AddLerpMovementMethodTag();
        }

        private void AddLerpMovementMethodTag()
        {
            foreach (var doorEnt in _doorsWithoutMovementMethodFilter.Value)
            {
                ref MovementMethodComp methodComp = ref _lerpTagPool.Value.Add(doorEnt);
                methodComp.MovementMethod = TranslationLogic.Lerp;
            }
        }
    }
}