using UnityEngine;

public class PlayerAimInput : MonoBehaviour
{
    public static bool isAiming;

    [Header("Referencias")]
    public PlayerCombat playerCombat;
    public CharacterCameraMovement cameraMovement;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            StartAiming();
        }

        if (Input.GetMouseButtonUp(1))
        {
            StopAiming();
        }
    }

    private void StartAiming()
    {
        isAiming = true;

        if (cameraMovement != null)
        {
            cameraMovement.SetAiming(true);
        }

        if (playerCombat != null)
        {
            playerCombat.SetAiming(true);
        }
    }

    private void StopAiming()
    {
        isAiming = false;

        if (cameraMovement != null)
        {
            cameraMovement.SetAiming(false);
        }

        if (playerCombat != null)
        {
            playerCombat.SetAiming(false);
        }
    }
}