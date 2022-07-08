using EcsTestProject.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace EcsTestProject.Systems
{
    internal class SetLinkedObserverStateSys : IEcsRunSystem
    {
        private EcsFilterInject<Inc<LinkByColorComp, LinkedObserverComp>, Exc<TriggerableComp>> _linkedObserverFilter = default;
        private EcsFilterInject<Inc<TriggerableComp, LinkByColorComp>> _triggerableFilter = default;

        public void Run(EcsSystems systems)
        {
            CheckObserverState();
        }

        private void CheckObserverState()
        {
            foreach (var observerEnt in _linkedObserverFilter.Value)
            {
                ref LinkedObserverComp linkedObserverComp = ref _linkedObserverFilter.Pools.Inc2.Get(observerEnt);
                linkedObserverComp.IsActive = IsLinkedObjectTriggered(observerEnt);
            }
        }

        private bool IsLinkedObjectTriggered(int observerEnt)
        {
            foreach (var triggerableEnt in _triggerableFilter.Value)
            {
                LinkByColorComp observerLinkComp = _linkedObserverFilter.Pools.Inc1.Get(observerEnt);
                LinkByColorComp triggerableLinkComp = _linkedObserverFilter.Pools.Inc1.Get(triggerableEnt);
                TriggerableComp triggerableComp = _triggerableFilter.Pools.Inc1.Get(triggerableEnt);

                if (observerLinkComp.ColorID == triggerableLinkComp.ColorID
                    && triggerableComp.Triggered)
                {
                    return true;
                }
            }

            return false;
        }

    }
}