using UnityEngine;

public class PlayerAttackInput : MonoBehaviour
{
    [Header("Referencias")]
    public PlayerAttackController attackController;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (attackController != null)
            {
                attackController.Attack();
            }
        }
    }
}