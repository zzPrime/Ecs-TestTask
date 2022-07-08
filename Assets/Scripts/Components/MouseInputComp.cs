using System.Numerics;

namespace EcsTestProject.Components
{
    internal struct MouseInputComp
    {
        public Vector3 LastPressedPos;
        public bool IsPressed;
    }
}