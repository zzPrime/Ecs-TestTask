using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EcsTestProject.LevelData
{
    public class LevelView : MonoBehaviour
    {
        [SerializeField] private ElementData [] _elementData;

        public ElementData [] ElementData => _elementData;
    }
}