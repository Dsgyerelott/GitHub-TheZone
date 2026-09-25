using UnityEngine;

public class PlayerRockInput : MonoBehaviour
{
    [Header("Referencias")]
    public PlayerCombat playerCombat;

    [Header("Input")]
    [SerializeField] private KeyCode throwRockKey =
        KeyCode.G;

    private void Update()
    {
        if (Input.GetKeyDown(throwRockKey))
        {
            ThrowRock();
        }
    }

    private void ThrowRock()
    {
        if (playerCombat != null)
        {
            playerCombat.ThrowRock();
        }
    }
}