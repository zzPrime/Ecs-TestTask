using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "LevelDataInstaller", menuName = "Installers/LevelDataInstaller")]
public class LevelDataInstaller : ScriptableObjectInstaller<LevelDataInstaller>
{
    public override void InstallBindings()
    {
    }
}