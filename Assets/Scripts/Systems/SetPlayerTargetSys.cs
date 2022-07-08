using EcsTestProject.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace EcsTestProject.Systems
{
    internal class SetPlayerTargetSys : IEcsRunSystem
    {
        private EcsWorldInject _world = default;
        private EcsFilterInject<Inc<MouseInputComp>> _mouseInputCompFilter = default;
        private EcsFilterInject<Inc<PlayerTag, PositionInfoComp>> _playerFilter = default;
        
        private EcsPoolInject<MoveToTargetComp> _moveToTargetPool = default;
        
        public void Run(EcsSystems systems)
        {
            UpdateTargetPosWhenMouseRaycasting();
        }
        
        private void UpdateTargetPosWhenMouseRaycasting()
        {
            foreach (var playerEnt in _playerFilter.Value)
            {
                foreach (var mouseInpEnt in _mouseInputCompFilter.Value)
                {
                    MouseInputComp mouseComp = _mouseInputCompFilter.Pools.Inc1.Get(mouseInpEnt);

                    if (mouseComp.IsPressed && mouseComp.IsRaycasting)
                    {
                        TryAddMoveToTargetComp(playerEnt);
                        UpdateTargetPosition(playerEnt, mouseComp);
                    }
                }
            }
        }
        
        private void TryAddMoveToTargetComp(int playerEnt)
        {
            if (!_moveToTargetPool.Value.Has(playerEnt))
            {
                _moveToTargetPool.Value.Add(playerEnt);
            }
        }
        
        private void UpdateTargetPosition(int playerEnt, MouseInputComp mouseComp)
        {
            ref MoveToTargetComp moveToTargetComp = ref _moveToTargetPool.Value.Get(playerEnt);
            moveToTargetComp.TargetPosition = mouseComp.LastPressedPos;
        }
    }
}