using EcsTestProject;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ProjectEnvironmentInstaller", menuName = "Installers/ProjectEnvironmentInstaller")]
public class ProjectEnvironmentInstaller : ScriptableObjectInstaller<LevelDataInstaller>
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<Startup>().AsSingle().NonLazy();
    }
}