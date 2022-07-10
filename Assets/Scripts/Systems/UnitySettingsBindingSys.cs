using EcsTestProject.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;

namespace EcsTestProject.Systems
{
    internal class UnitySettingsBindingSys : IEcsRunSystem
    {
        private EcsWorldInject _world = default;
        private EcsCustomInject<GameData> _gameData = default;
        private EcsFilterInject<Inc<GameSettingsComp>> _gameSettingsCompFilter = default;

        public void Run(EcsSystems systems)
        {
            CreateSettingsIfNotExist();
            UpdateSettingsComp();
        }

        private void CreateSettingsIfNotExist()
        {
            if (_gameSettingsCompFilter.Value.GetEntitiesCount() == 0)
            {
                var settingsEnt = _world.Value.NewEntity();
                _gameSettingsCompFilter.Pools.Inc1.Add(settingsEnt);
            }
        }

        private void UpdateSettingsComp()
        {
            foreach (var settingsEnt in _gameSettingsCompFilter.Value)
            {
                ref GameSettingsComp settingsComp = ref _gameSettingsCompFilter.Pools.Inc1.Get(settingsEnt);
                GameSettings gameSettings = _gameData.Value.GameSettings;

                CommonSettings commonSettings = GetCommonSettings(gameSettings);
                PlayerSettings playerSettings = GetPlayerSettings(gameSettings);
                DoorSettings doorSettings = GetDoorSettings(gameSettings);

                settingsComp.CommonSettings = commonSettings;
                settingsComp.PlayerSettings = playerSettings;
                settingsComp.DoorSettings = doorSettings;
            }
        }

        private static CommonSettings GetCommonSettings(GameSettings gameSettings)
        {
            CommonSettings commonSettings = new CommonSettings();
            commonSettings.TriggerDistance = gameSettings.TriggerDistance;
            commonSettings.DestinationReachDistance = gameSettings.DestinationReachDistance;
            commonSettings.StopRotationAngleRad = gameSettings.StopRotationAngleRad;
            commonSettings.DeltaTime = gameSettings.DeltaTime;
            return commonSettings;
        }
        
        private static PlayerSettings GetPlayerSettings(GameSettings gameSettings)
        {
            PlayerSettings playerSettings = new PlayerSettings();
            playerSettings.PlayerMovementSpeed = gameSettings.PlayerMovementSpeed;
            playerSettings.PlayerRotationSpeed = gameSettings.PlayerRotationSpeed;
            return playerSettings;
        }

        private static DoorSettings GetDoorSettings(GameSettings gameSettings)
        {
            DoorSettings doorSettings = new DoorSettings();
            doorSettings.DoorSpeed = gameSettings.DoorSpeed;
            doorSettings.DoorHeight = gameSettings.DoorHeight;
            return doorSettings;
        }
    }
}