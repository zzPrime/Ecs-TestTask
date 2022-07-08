using System;
using UnityEngine;

namespace EcsTestProject.LevelData
{
    [Serializable]
    public struct ElementData
    {
        public ColorID ColorID;
        public LevelElementType LevelElementType;
        public Transform ElementTf;
    }
}