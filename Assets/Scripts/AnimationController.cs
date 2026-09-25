using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [Header("References")]
    public Animator animator;

    [Header("Settings")]
    public float smoothSpeed = 8f;

    private float movementValue;

    void Update()
    {
        if (animator == null)
            return;

        bool isMoving =
            Input.GetKey(KeyCode.W) ||
            Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.D);

        bool isRunning =
            isMoving && Input.GetKey(KeyCode.LeftShift);

        float targetMovement = 0f;

        if (isMoving)
        {
            if (isRunning)
                targetMovement = 1f;   // Correr
            else
                targetMovement = 0.5f; // Caminar
        }

        movementValue = Mathf.Lerp(
            movementValue,
            targetMovement,
            smoothSpeed * Time.deltaTime
        );

        animator.SetFloat("Movement", movementValue);
    }
}