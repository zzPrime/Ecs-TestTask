using EcsTestProject.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Vector3 = System.Numerics.Vector3;

namespace EcsTestProject.Systems
{
    internal class MonobehViewUpdateSys : IEcsRunSystem
    {
        private EcsFilterInject<Inc<MonobehViewComp, PositionInfoComp>> _monobehFilter = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var monobehEnt in _monobehFilter.Value)
            {
                ref MonobehViewComp monobehComp = ref _monobehFilter.Pools.Inc1.Get(monobehEnt);
                PositionInfoComp positionInfoComp = _monobehFilter.Pools.Inc2.Get(monobehEnt);
                monobehComp.ViewTf.transform.position = Utils.GetUnityVec3FromNumericsVec3(positionInfoComp.Position);
                
                Vector3 rotationVector = Vector3.Normalize(positionInfoComp.RotationVector);
                monobehComp.ViewTf.transform.forward = Utils.GetUnityVec3FromNumericsVec3(rotationVector);
            }
        }
    }
}