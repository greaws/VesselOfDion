using UnityEngine;
using UnityEngine.InputSystem;

public class Rumble : MonoBehaviour
{
    [Range(0, 1)]
    public float lowFrequemcy = 0; // Low frequency rumble value
    [Range(0, 1)]
    public float highFrequency = 0; // High frequency rumble value

    void Update()
    {
        Gamepad.current.SetMotorSpeeds(lowFrequemcy, highFrequency);
    }
}
