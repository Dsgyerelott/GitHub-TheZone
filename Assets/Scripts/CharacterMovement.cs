using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Referencias")]
    public CharacterController m_chController;
    public Transform m_camera;

    [Header("Movimiento")]
    public float m_speed = 5f;
    public float runMultiplier = 1.6f;
    public float aimSpeedMultiplier = 0.5f;
    public float rotationSpeed = 10f;

    [Header("Salto y gravedad")]
    public float jumpHeight = 1.5f;
    public float gravity = -9.81f;

    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;

    [Header("Apuntado")]
    public float aimRange = 100f;

    private float verticalSpeed;

    private Camera playerCamera;

    private void Start()
    {
        if (m_chController == null)
        {
            m_chController =
                GetComponent<CharacterController>();
        }

        if (m_camera != null)
        {
            playerCamera =
                m_camera.GetComponent<Camera>();
        }

        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
    }

    private void Update()
    {
        Movement();

        bool isAiming =
            Input.GetMouseButton(1);

        if (isAiming)
        {
            RotateTowardsAim();
        }
        else
        {
            RotateWithMovement();
        }
    }

    // ==================================================
    // MOVIMIENTO
    // ==================================================

    private void Movement()
    {
        if (m_chController == null ||
            m_camera == null)
        {
            return;
        }

        float horizontal =
            Input.GetAxisRaw("Horizontal");

        float vertical =
            Input.GetAxisRaw("Vertical");

        Vector3 inputDirection =
            new Vector3(
                horizontal,
                0f,
                vertical
            ).normalized;

        Vector3 moveDirection =
            Vector3.zero;

        if (inputDirection.magnitude >=
            0.1f)
        {
            moveDirection =
                m_camera.TransformDirection(
                    inputDirection
                );

            moveDirection.y = 0f;

            moveDirection.Normalize();
        }

        bool isAiming =
            Input.GetMouseButton(1);

        bool isRunning =
            Input.GetKey(
                KeyCode.LeftShift
            );

        float currentSpeed =
            m_speed;

        if (isAiming)
        {
            currentSpeed *=
                aimSpeedMultiplier;
        }
        else if (isRunning)
        {
            currentSpeed *=
                runMultiplier;
        }

        bool isGrounded =
            IsGrounded();

        if (isGrounded &&
            verticalSpeed < 0f)
        {
            verticalSpeed = -2f;
        }

        if (Input.GetKeyDown(
                KeyCode.Space) &&
            isGrounded)
        {
            verticalSpeed =
                Mathf.Sqrt(
                    jumpHeight *
                    -2f *
                    gravity
                );
        }

        verticalSpeed +=
            gravity *
            Time.deltaTime;

        Vector3 finalMovement =
            moveDirection *
            currentSpeed +
            Vector3.up *
            verticalSpeed;

        m_chController.Move(
            finalMovement *
            Time.deltaTime
        );
    }

    // ==================================================
    // GROUND CHECK
    // ==================================================

    private bool IsGrounded()
    {
        if (groundCheck == null)
            return m_chController.isGrounded;

        return Physics.CheckSphere(
            groundCheck.position,
            groundDistance,
            groundMask
        );
    }

    // ==================================================
    // ROTACIÓN NORMAL
    // ==================================================

    private void RotateWithMovement()
    {
        if (m_camera == null)
            return;

        float horizontal =
            Input.GetAxisRaw("Horizontal");

        float vertical =
            Input.GetAxisRaw("Vertical");

        Vector3 inputDirection =
            new Vector3(
                horizontal,
                0f,
                vertical
            );

        if (inputDirection.magnitude <
            0.1f)
        {
            return;
        }

        Vector3 direction =
            m_camera.TransformDirection(
                inputDirection.normalized
            );

        direction.y = 0f;

        if (direction.sqrMagnitude <
            0.01f)
        {
            return;
        }

        Quaternion targetRotation =
            Quaternion.LookRotation(
                direction
            );

        transform.rotation =
            Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed *
                Time.deltaTime
            );
    }

    // ==================================================
    // ROTACIÓN AL APUNTAR
    // ==================================================

    private void RotateTowardsAim()
    {
        if (playerCamera == null)
            return;

        Vector3 screenCenter =
            new Vector3(
                Screen.width / 2f,
                Screen.height / 2f,
                0f
            );

        Ray ray =
            playerCamera.ScreenPointToRay(
                screenCenter
            );

        Vector3 targetPoint;

        if (Physics.Raycast(
                ray,
                out RaycastHit hit,
                aimRange))
        {
            targetPoint =
                hit.point;
        }
        else
        {
            targetPoint =
                ray.GetPoint(
                    aimRange
                );
        }

        Vector3 directionToTarget =
            targetPoint -
            transform.position;

        directionToTarget.y = 0f;

        if (directionToTarget.magnitude <
            0.1f)
        {
            return;
        }

        Quaternion targetRotation =
            Quaternion.LookRotation(
                directionToTarget
            );

        transform.rotation =
            Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed *
                Time.deltaTime
            );
    }

    // ==================================================
    // GIZMOS
    // ==================================================

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
            return;

        Gizmos.DrawWireSphere(
            groundCheck.position,
            groundDistance
        );
    }
}