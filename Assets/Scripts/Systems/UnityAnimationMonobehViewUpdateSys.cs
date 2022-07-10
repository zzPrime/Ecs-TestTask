using EcsTestProject.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace EcsTestProject.Systems
{
    internal class UnityAnimationMonobehViewUpdateSys : IEcsRunSystem
    {
        private EcsFilterInject<Inc<AnimationMonobehComp, AnimationStateComp>> _animationFilter = default;

        public void Run(EcsSystems systems)
        {
            foreach (var animationEnt in _animationFilter.Value)
            {
                AnimationMonobehComp animationComp = _animationFilter.Pools.Inc1.Get(animationEnt);
                AnimationStateComp animStateComp = _animationFilter.Pools.Inc2.Get(animationEnt);
                AnimatorStateInfo currentAnimationState = animationComp.Animator.GetCurrentAnimatorStateInfo(0);

                if (!currentAnimationState.IsName(animStateComp.AnimationState))
                {
                    animationComp.Animator.Play(animStateComp.AnimationState);
                }
            }
        }
    }
}