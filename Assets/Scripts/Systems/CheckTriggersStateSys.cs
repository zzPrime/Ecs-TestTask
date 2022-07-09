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
        private EcsCustomInject<GameData> _gameData = default;

        public void Run(EcsSystems systems)
        {
            CheckForTriggering();
        }

        private void CheckForTriggering()
        {
            foreach (var triggerableEnt in _triggerableCompFilter.Value)
            {
                ref TriggerableComp triggerableComp = ref _triggerableCompFilter.Pools.Inc1.Get(triggerableEnt);
                bool triggered = false;
                
                foreach (var canTriggerEnt in _canTriggerCompFilter.Value)
                {
                    ref PositionInfoComp triggerablePosComp = ref _triggerableCompFilter.Pools.Inc2.Get(triggerableEnt);
                    ref PositionInfoComp canTriggerPosComp = ref _canTriggerCompFilter.Pools.Inc2.Get(canTriggerEnt);

                    var distance = Vector3.DistanceSquared(triggerablePosComp.Position, canTriggerPosComp.Position);
                    var triggerDistance = _gameData.Value.GameSettings.TriggerDistance;
                    
                    if (distance <= triggerDistance)
                    {
                        triggered = true;
                        break;
                    }
                }

                triggerableComp.Triggered = triggered;
            }
        }
    }
}