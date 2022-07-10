using EcsTestProject.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace EcsTestProject.Systems
{
    internal class SetPlayerAnimationStateSys : IEcsRunSystem
    {
        private EcsFilterInject<Inc<PlayerTag, AnimationStateComp>> _playerFilter = default;
        private EcsPoolInject<MoveToTargetComp> _moveToTargetComp = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var playerEnt in _playerFilter.Value)
            {
                ref AnimationStateComp animationStateComp = ref _playerFilter.Pools.Inc2.Get(playerEnt);
                
                if (_moveToTargetComp.Value.Has(playerEnt))
                {
                    animationStateComp.AnimationState = AnimationState.Run;
                }
                else
                {
                    animationStateComp.AnimationState = AnimationState.Idle;
                }
            }
        }
    }
}