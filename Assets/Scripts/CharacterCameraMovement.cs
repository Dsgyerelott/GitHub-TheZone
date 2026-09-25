using UnityEngine;

public class CharacterCameraMovement : MonoBehaviour
{
    [Header("References")]
    public Transform m_cameraTr;

    [Header("Offset")]
    public Vector3 normalOffset = new Vector3(0f, 1.8f, -4f);
    public Vector3 aimOffset = new Vector3(0f, 1.5f, -1.5f);

    private Vector3 currentOffset;

    [Header("Settings")]
    public float smoothSpeed = 10f;
    public float sensitivity = 2f;

    public float m_maxPitch = 60f;
    public float m_minPitch = -30f;

    public float aimLerpSpeed = 10f;

    private float yaw;
    private float pitch;

    private bool isAiming;

    void Start()
    {
        if (m_cameraTr == null)
            return;

        Vector3 angles = m_cameraTr.eulerAngles;

        yaw = angles.y;
        pitch = angles.x;

        currentOffset = normalOffset;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Click derecho = apuntar
        isAiming = Input.GetMouseButton(1);

        // Cambio suave entre cámara normal y cámara de apuntado
        Vector3 targetOffset = isAiming ? aimOffset : normalOffset;

        currentOffset = Vector3.Lerp(
            currentOffset,
            targetOffset,
            aimLerpSpeed * Time.deltaTime
        );

        // ESC libera el mouse
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Click izquierdo vuelve a capturar el mouse
        if (Input.GetMouseButtonDown(0) &&
            Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void LateUpdate()
    {
        if (m_cameraTr == null)
            return;

        if (Cursor.lockState == CursorLockMode.Locked)
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            yaw += mouseX * sensitivity;
            pitch -= mouseY * sensitivity;

            pitch = Mathf.Clamp(
                pitch,
                m_minPitch,
                m_maxPitch
            );
        }

        Quaternion rotation =
            Quaternion.Euler(pitch, yaw, 0f);

        Vector3 targetPosition =
            transform.position +
            rotation * currentOffset;

        // Punto desde donde comprobamos colisiones
        Vector3 origin =
            transform.position +
            new Vector3(0f, 1.6f, 0f);

        Vector3 direction =
            (targetPosition - origin).normalized;

        float distance =
            Vector3.Distance(origin, targetPosition);

        float sphereRadius = 0.25f;

        // Evitar que la cámara atraviese paredes
        if (Physics.SphereCast(
            origin,
            sphereRadius,
            direction,
            out RaycastHit hit,
            distance))
        {
            targetPosition =
                hit.point -
                direction * sphereRadius;
        }

        m_cameraTr.position = Vector3.Lerp(
            m_cameraTr.position,
            targetPosition,
            smoothSpeed * Time.deltaTime
        );

        m_cameraTr.rotation = Quaternion.Slerp(
            m_cameraTr.rotation,
            rotation,
            smoothSpeed * Time.deltaTime
        );
    }

    public void SetAiming(bool aiming)
    {
        isAiming = aiming;
    }
}