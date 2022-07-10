using EcsTestProject.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace EcsTestProject.Systems
{
    internal class SetPlayerTranslationMethodSys : IEcsRunSystem
    {
        private EcsFilterInject<Inc<PlayerTag>, Exc<MovementMethodComp>> _playerWithoutMovementMethodFilter = default;
        private EcsFilterInject<Inc<PlayerTag>, Exc<RotationMethodComp>> _playerWithoutRotationMethodFilter = default;
        
        private EcsPoolInject<MovementMethodComp> _movementMethodCompPool = default;
        private EcsPoolInject<RotationMethodComp> _rotationMethodCompPool = default;

        public void Run(EcsSystems systems)
        {
            AddLerpMovementMethodComp();
            AddLerpRotationMethodComp();
        }

        private void AddLerpMovementMethodComp()
        {
            foreach (var playerEnt in _playerWithoutMovementMethodFilter.Value)
            {
                ref MovementMethodComp methodComp = ref _movementMethodCompPool.Value.Add(playerEnt);
                methodComp.MovementMethod = TranslationLogic.Lerp;
            }
        }

        private void AddLerpRotationMethodComp()
        {
            foreach (var playerEnt in _playerWithoutRotationMethodFilter.Value)
            {
                ref RotationMethodComp methodComp = ref _rotationMethodCompPool.Value.Add(playerEnt);
                methodComp.RotationMethod = TranslationLogic.Constant;
            }
        }
    }
}