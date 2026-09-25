using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerCombat : MonoBehaviour
{
    [Header("Puntos de ataque")]
    public Transform arrowSpawnPoint;
    public Transform rockSpawnPoint;
    public Transform playerBody;

    [Header("Prefabs")]
    public GameObject arrowPrefab;
    public GameObject rockPrefab;

    [Header("Referencias")]
    public Camera playerCamera;
    public EquipmentState equipmentState;
    public Animator animator;
    public AmmoSystem ammoSystem;

    [Header("IK lanzamiento de piedra")]
    public TwoBoneIKConstraint leftHandIK;

    [Header("Arco")]
    public float arrowForce = 35f;
    public float aimRange = 100f;

    [Header("Piedra")]
    public float rockThrowForce = 18f;
    public float rockUpwardForce = 0.4f;
    public float rotationSpeed = 10f;

    [Header("Animación de piedra")]
    public float timeWithoutIK = 1.2f;
    public int rockAnimatorLayer = 2;

    [Header("Apuntado")]
    public bool isAiming;

    private bool isRotatingForRock;
    private Quaternion targetBodyRotation;

    private void Start()
    {
        if (ammoSystem == null)
        {
            ammoSystem = GetComponentInParent<AmmoSystem>();
        }

        if (equipmentState == null)
        {
            equipmentState = GetComponentInParent<EquipmentState>();
        }

        if (animator == null)
        {
            animator = GetComponentInParent<Animator>();
        }

        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
    }

    private void Update()
    {
        if (isRotatingForRock && playerBody != null)
        {
            RotateTowardsRockDirection();
        }
    }

    // ==================================================
    // ATAQUE DEL EQUIPAMIENTO ACTUAL
    // ==================================================

    public void AttackCurrentEquipment()
    {
        if (equipmentState == null)
            return;

        switch (equipmentState.GetCurrentEquipment())
        {
            case EquipmentState.Equipment.Spear:
                AttackWithSpear();
                break;

            case EquipmentState.Equipment.Bow:
                ShootBow();
                break;
        }
    }

    // ==================================================
    // LANZA
    // ==================================================

    private void AttackWithSpear()
    {
        if (animator != null)
        {
            animator.SetTrigger("SpearAttack");
        }

        /*
         * El daño real de la lanza lo añadiremos después.
         *
         * Aquí posteriormente pondremos:
         * - Hitbox de la lanza.
         * - Raycast / OverlapSphere.
         * - Daño.
         * - Sonido de impacto.
         */
    }

    // ==================================================
    // ARCO
    // ==================================================

    private void ShootBow()
    {
        if (ammoSystem != null && !ammoSystem.CanUseArrow())
        {
            return;
        }

        if (arrowSpawnPoint == null || arrowPrefab == null)
        {
            return;
        }

        if (animator != null)
        {
            animator.SetTrigger("BowShoot");
        }

        Vector3 direction = GetAimDirection(
            arrowSpawnPoint
        );

        GameObject arrow = Instantiate(
            arrowPrefab,
            arrowSpawnPoint.position,
            Quaternion.LookRotation(direction)
        );

        Rigidbody rb = arrow.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(
                direction * arrowForce,
                ForceMode.Impulse
            );
        }

        if (ammoSystem != null)
        {
            ammoSystem.UseArrow();
        }
    }

    // ==================================================
    // PIEDRA
    // ==================================================

    public void ThrowRock()
    {
        if (rockSpawnPoint == null ||
            rockPrefab == null ||
            playerBody == null)
        {
            return;
        }

        if (ammoSystem != null &&
            !ammoSystem.CanUseRock())
        {
            return;
        }

        if (leftHandIK != null)
        {
            leftHandIK.weight = 0f;
        }

        if (animator != null)
        {
            animator.Play(
                "ThrowRock",
                rockAnimatorLayer,
                0f
            );
        }

        StartCoroutine(RestoreLeftHandIK());

        Vector3 cameraDirection =
            GetCameraHorizontalDirection();

        if (cameraDirection.sqrMagnitude < 0.01f)
        {
            cameraDirection = playerBody.forward;
        }

        targetBodyRotation =
            Quaternion.LookRotation(
                cameraDirection.normalized
            );

        isRotatingForRock = true;
    }

    private void RotateTowardsRockDirection()
    {
        playerBody.rotation = Quaternion.Slerp(
            playerBody.rotation,
            targetBodyRotation,
            rotationSpeed * Time.deltaTime
        );

        if (Quaternion.Angle(
                playerBody.rotation,
                targetBodyRotation
            ) < 1f)
        {
            playerBody.rotation =
                targetBodyRotation;

            isRotatingForRock = false;

            ExecuteRockThrow();
        }
    }

    private void ExecuteRockThrow()
    {
        Vector3 throwDirection =
            playerBody.forward +
            Vector3.up * rockUpwardForce;

        throwDirection.Normalize();

        GameObject rock = Instantiate(
            rockPrefab,
            rockSpawnPoint.position,
            Quaternion.identity
        );

        Rigidbody rb =
            rock.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(
                throwDirection * rockThrowForce,
                ForceMode.Impulse
            );

            rb.AddTorque(
                new Vector3(
                    Random.Range(-3f, 3f),
                    Random.Range(-3f, 3f),
                    Random.Range(-3f, 3f)
                ),
                ForceMode.Impulse
            );
        }

        if (ammoSystem != null)
        {
            ammoSystem.UseRock();
        }
    }

    private IEnumerator RestoreLeftHandIK()
    {
        yield return new WaitForSeconds(
            timeWithoutIK
        );

        if (leftHandIK != null)
        {
            leftHandIK.weight = 1f;
        }
    }

    // ==================================================
    // APUNTADO
    // ==================================================

    public void SetAiming(bool aiming)
    {
        isAiming = aiming;
    }

    // ==================================================
    // DIRECCIÓN DE ARCO
    // ==================================================

    private Vector3 GetAimDirection(
        Transform spawnPoint
    )
    {
        if (playerCamera == null)
        {
            return spawnPoint.forward;
        }

        Vector3 screenCenter = new Vector3(
            Screen.width / 2f,
            Screen.height / 2f,
            0f
        );

        Ray ray =
            playerCamera.ScreenPointToRay(
                screenCenter
            );

        if (Physics.Raycast(
                ray,
                out RaycastHit hit,
                aimRange))
        {
            return (
                hit.point -
                spawnPoint.position
            ).normalized;
        }

        Vector3 targetPoint =
            ray.GetPoint(aimRange);

        return (
            targetPoint -
            spawnPoint.position
        ).normalized;
    }

    // ==================================================
    // DIRECCIÓN DE PIEDRA
    // ==================================================

    private Vector3 GetCameraHorizontalDirection()
    {
        if (playerCamera == null)
        {
            return playerBody.forward;
        }

        Vector3 screenCenter = new Vector3(
            Screen.width / 2f,
            Screen.height / 2f,
            0f
        );

        Ray ray =
            playerCamera.ScreenPointToRay(
                screenCenter
            );

        if (Physics.Raycast(
                ray,
                out RaycastHit hit,
                aimRange))
        {
            Vector3 direction =
                hit.point -
                playerBody.position;

            direction.y = 0f;

            return direction.normalized;
        }

        Vector3 cameraDirection =
            playerCamera.transform.forward;

        cameraDirection.y = 0f;

        return cameraDirection.normalized;
    }
}