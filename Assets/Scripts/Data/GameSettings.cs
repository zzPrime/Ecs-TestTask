using UnityEngine;

[CreateAssetMenu(fileName = "GameSettingsSo", menuName = "CustomSo/GameSettingsSo")]
public class GameSettings : ScriptableObject
{
    public float PlayerMovementSpeed = 1f;
    public float PlayerRotationSpeed = 1f;
    public float TriggerDistance = 1f;
    public float DoorSpeed = 0.1f;
    public float DoorHeight = 1f;
    public float DestinationReachDistance = 0.01f;
    public float DeltaTime => Time.deltaTime;
}