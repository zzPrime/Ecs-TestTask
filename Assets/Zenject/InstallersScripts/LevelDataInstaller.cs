using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "LevelDataInstaller", menuName = "Installers/LevelDataInstaller")]
public class LevelDataInstaller : ScriptableObjectInstaller<LevelDataInstaller>
{
    public GameData GameData;

    public override void InstallBindings()
    {
        Container.Bind<GameData>().FromInstance(GameData).AsSingle().NonLazy();
    }
}