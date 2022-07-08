using System.Collections;
using System.Collections.Generic;
using EcsTestProject.LevelData;
using UnityEngine;

[CreateAssetMenu(fileName = "GameDataSo", menuName = "CustomSo/GameDataSo")]
public class GameData : ScriptableObject
{
    public LevelView LevelView;
    public GameObject PlayerView;
}
