using System.Numerics;
using EcsTestProject.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace EcsTestProject.Systems
{
    internal class CheckTriggersStateSys : IEcsRunSystem
    {
        private EcsFilterInject<Inc<TriggerableComp, PositionInfoComp>, Exc<CanTriggerTag>> _triggerableCompFilter = default;
        private EcsFilterInject<Inc<CanTriggerTag, PositionInfoComp>, Exc<TriggerableComp>> _canTriggerCompFilter = default;
        
        private EcsFilterInject<Inc<GameSettingsComp>> _gameSettingsCompFilter = default;

        public void Run(EcsSystems systems)
        {
            CheckForTriggering();
        }

        private void CheckForTriggering()
        {
            foreach (var triggerableEnt in _triggerableCompFilter.Value)
            {
                bool triggered = false;
                
                foreach (var canTriggerEnt in _canTriggerCompFilter.Value)
                {
                    ref PositionInfoComp triggerablePosComp = ref _triggerableCompFilter.Pools.Inc2.Get(triggerableEnt);
                    ref PositionInfoComp canTriggerPosComp = ref _canTriggerCompFilter.Pools.Inc2.Get(canTriggerEnt);

                    var distance = Vector3.DistanceSquared(triggerablePosComp.Position, canTriggerPosComp.Position);
                    var triggerDistance = GetTriggerDistance();
                    
                    if (distance <= triggerDistance)
                    {
                        triggered = true;
                        break;
                    }
                }

                ref TriggerableComp triggerableComp = ref _triggerableCompFilter.Pools.Inc1.Get(triggerableEnt);
                triggerableComp.Triggered = triggered;
            }
        }
        
        private float GetTriggerDistance()
        {
            foreach (var settingsEnt in _gameSettingsCompFilter.Value)
            {
                return _gameSettingsCompFilter.Pools.Inc1.Get(settingsEnt).CommonSettings.TriggerDistance;
            }

            return default;
        }
    }
}