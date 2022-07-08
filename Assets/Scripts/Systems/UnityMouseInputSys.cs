using EcsTestProject.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace EcsTestProject.Systems
{
    internal class UnityMouseInputSys : IEcsRunSystem
    {
        private EcsWorldInject _world = default;
        private EcsFilterInject<Inc<MouseInputComp>> _mouseInputCompFilter = default;
        
        public void Run(EcsSystems systems)
        {
            CheckMouseCompExist();
            GetMouseState();
        }

        private void CheckMouseCompExist()
        {
            if (_mouseInputCompFilter.Value.GetEntitiesCount() == 0)
            {
                var inputEnt = _world.Value.NewEntity();
                _mouseInputCompFilter.Pools.Inc1.Add(inputEnt);
            }
        }

        private void GetMouseState()
        {
            foreach (var mouseInputEnt in _mouseInputCompFilter.Value)
            {
                ref MouseInputComp inputComp = ref _mouseInputCompFilter.Pools.Inc1.Get(mouseInputEnt);

                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                
                SetRaycastingState(ray, ref inputComp);
                
                if (Input.GetKey(KeyCode.Mouse0))
                {
                    if (Physics.Raycast(ray, out hit))
                    {
                        Vector3 hitPos = hit.point;
                        var hitPosNumVec = new System.Numerics.Vector3(hitPos.x, hitPos.y, hitPos.z);

                        inputComp.LastPressedPos = hitPosNumVec;
                    }

                    inputComp.IsPressed = true;
                }
                else
                {
                    inputComp.IsPressed = false;
                }
            }
        }

        private static void SetRaycastingState(Ray ray, ref MouseInputComp inputComp)
        {
            RaycastHit hit;
            inputComp.IsRaycasting = Physics.Raycast(ray, out hit);
        }
    }
}