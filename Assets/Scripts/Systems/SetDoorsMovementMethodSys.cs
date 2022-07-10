using System.Numerics;
using EcsTestProject.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace EcsTestProject.Systems
{
    internal class SetDoorsMovementMethodSys : IEcsRunSystem
    {
        private EcsFilterInject<Inc<LinkedObserverComp>, Exc<TranslationMethodComp>> _doorsFilter = default;
        private EcsPoolInject<TranslationMethodComp> _lerpTagPool = default;

        public void Run(EcsSystems systems)
        {
            AddLerpMovementMethodTag();
        }

        private void AddLerpMovementMethodTag()
        {
            foreach (var doorEnt in _doorsFilter.Value)
            {
                ref TranslationMethodComp methodComp = ref _lerpTagPool.Value.Add(doorEnt);
                methodComp.TranslationLogic = TranslationLogic.Lerp;
            }
        }
    }
}