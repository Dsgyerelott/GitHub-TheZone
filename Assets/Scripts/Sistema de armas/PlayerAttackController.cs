using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    [Header("Referencias")]
    public PlayerCombat playerCombat;

    [Header("Ataque")]
    public float attackCooldown = 0.5f;

    private float nextAttackTime;

    private void Start()
    {
        if (playerCombat == null)
        {
            playerCombat =
                GetComponentInChildren<PlayerCombat>(
                    true
                );
        }

        if (playerCombat == null)
        {
            Debug.LogError(
                "PlayerAttackController: PlayerCombat no encontrado."
            );
        }
    }

    public void Attack()
    {
        if (playerCombat == null)
            return;

        if (Time.time < nextAttackTime)
            return;

        nextAttackTime =
            Time.time + attackCooldown;

        playerCombat.AttackCurrentEquipment();
    }
}