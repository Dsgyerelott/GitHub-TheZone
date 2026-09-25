using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum EnemyState
    {
        Patrolling,
        Investigating,
        Chasing,
        Attacking
    }

    [Header("References")]
    public CharacterController controller;
    public Transform player;
    public Animator animator;

    [Header("Patrol Settings")]
    public Vector3 patrolCenter;
    public float patrolRadius = 8f;
    public float patrolSpeed = 2f;
    public float waitTimeAtPoint = 2f;

    [Header("Investigation Settings")]
    public float investigationSpeed = 2.5f;
    public float investigationWaitTime = 3f;
    public float investigationArrivalDistance = 0.7f;

    [Header("Chase Settings")]
    public float detectionRange = 10f;
    public float chaseSpeed = 4f;
    public float loseRange = 15f;

    [Header("Attack Settings")]
    public float attackRange = 1.5f;
    public float attackCooldown = 1f;

    [Header("Rotation")]
    public float rotationSpeed = 10f;

    private EnemyState currentState = EnemyState.Patrolling;

    private Vector3 currentPatrolPoint;
    private Vector3 investigationPosition;

    private float waitTimer;
    private float investigationTimer;
    private float attackTimer;

    private bool isReturningToPatrol;

    private void Start()
    {
        if (controller == null)
        {
            controller = GetComponent<CharacterController>();
        }

        if (player == null)
        {
            GameObject playerObject =
                GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }

        patrolCenter = transform.position;

        ChooseNewPatrolPoint();
    }

    private void Update()
    {
        if (player == null)
            return;

        if (attackTimer > 0f)
        {
            attackTimer -= Time.deltaTime;
        }

        switch (currentState)
        {
            case EnemyState.Patrolling:
                Patrolling();
                CheckDetection();
                break;

            case EnemyState.Investigating:
                Investigating();
                CheckDetection();
                break;

            case EnemyState.Chasing:
                Chasing();
                CheckLosePlayer();
                break;

            case EnemyState.Attacking:
                Attacking();
                break;
        }

        UpdateAnimations();
    }

    // ==================================================
    // PATRULLA
    // ==================================================

    private void Patrolling()
    {
        Vector3 direction =
            currentPatrolPoint - transform.position;

        direction.y = 0f;

        float distance =
            Vector3.Distance(
                transform.position,
                currentPatrolPoint
            );

        if (distance > 0.5f)
        {
            MoveInDirection(
                direction.normalized,
                patrolSpeed
            );
        }
        else
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= waitTimeAtPoint)
            {
                waitTimer = 0f;
                ChooseNewPatrolPoint();
            }
        }

        if (isReturningToPatrol &&
            Vector3.Distance(
                transform.position,
                patrolCenter
            ) < patrolRadius)
        {
            isReturningToPatrol = false;
        }
    }

    private void ChooseNewPatrolPoint()
    {
        Vector2 randomCircle =
            Random.insideUnitCircle *
            patrolRadius;

        currentPatrolPoint =
            new Vector3(
                patrolCenter.x + randomCircle.x,
                transform.position.y,
                patrolCenter.z + randomCircle.y
            );
    }

    // ==================================================
    // INVESTIGACIÓN DE RUIDO
    // ==================================================

    public void HearNoise(Vector3 noisePosition)
    {
        if (currentState == EnemyState.Chasing ||
            currentState == EnemyState.Attacking)
        {
            return;
        }

        investigationPosition = noisePosition;

        investigationTimer = 0f;

        currentState =
            EnemyState.Investigating;
    }

    private void Investigating()
    {
        Vector3 direction =
            investigationPosition -
            transform.position;

        direction.y = 0f;

        float distance =
            Vector3.Distance(
                transform.position,
                investigationPosition
            );

        if (distance > investigationArrivalDistance)
        {
            MoveInDirection(
                direction.normalized,
                investigationSpeed
            );

            return;
        }

        investigationTimer += Time.deltaTime;

        if (investigationTimer >=
            investigationWaitTime)
        {
            currentState =
                EnemyState.Patrolling;

            isReturningToPatrol = true;

            ChooseNewPatrolPoint();
        }
    }

    // ==================================================
    // DETECCIÓN
    // ==================================================

    private void CheckDetection()
    {
        float distanceToPlayer =
            Vector3.Distance(
                transform.position,
                player.position
            );

        if (distanceToPlayer >
            detectionRange)
        {
            return;
        }

        Vector3 rayOrigin =
            transform.position +
            Vector3.up * 0.5f;

        Vector3 directionToPlayer =
            (
                player.position -
                rayOrigin
            ).normalized;

        if (Physics.Raycast(
                rayOrigin,
                directionToPlayer,
                out RaycastHit hit,
                detectionRange))
        {
            if (hit.transform.CompareTag("Player"))
            {
                currentState =
                    EnemyState.Chasing;
            }
        }
    }

    // ==================================================
    // PERSECUCIÓN
    // ==================================================

    private void Chasing()
    {
        float distanceToPlayer =
            Vector3.Distance(
                transform.position,
                player.position
            );

        Vector3 direction =
            player.position -
            transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude > 0.01f)
        {
            MoveInDirection(
                direction.normalized,
                chaseSpeed
            );
        }

        if (distanceToPlayer <
            attackRange)
        {
            currentState =
                EnemyState.Attacking;
        }
    }

    private void CheckLosePlayer()
    {
        float distanceToPlayer =
            Vector3.Distance(
                transform.position,
                player.position
            );

        if (distanceToPlayer >
            loseRange)
        {
            currentState =
                EnemyState.Patrolling;

            isReturningToPatrol = true;

            ChooseNewPatrolPoint();
        }
        else if (distanceToPlayer <
                 attackRange)
        {
            currentState =
                EnemyState.Attacking;
        }
    }

    // ==================================================
    // ATAQUE
    // ==================================================

    private void Attacking()
    {
        Vector3 direction =
            player.position -
            transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude > 0.01f)
        {
            RotateTowards(
                direction.normalized
            );
        }

        if (attackTimer <= 0f)
        {
            attackTimer =
                attackCooldown;

            PlayerHealth playerHealth =
                player.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.RecibirDaño();
            }
        }

        float distanceToPlayer =
            Vector3.Distance(
                transform.position,
                player.position
            );

        if (distanceToPlayer >
            attackRange)
        {
            currentState =
                EnemyState.Chasing;
        }
    }

    // ==================================================
    // MOVIMIENTO
    // ==================================================

    private void MoveInDirection(
        Vector3 direction,
        float speed)
    {
        if (controller == null)
            return;

        controller.Move(
            direction *
            speed *
            Time.deltaTime
        );

        RotateTowards(direction);
    }

    private void RotateTowards(
        Vector3 direction)
    {
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
    // ANIMACIONES
    // ==================================================

    private void UpdateAnimations()
    {
        if (animator == null ||
            controller == null)
        {
            return;
        }

        float speed =
            controller.velocity.magnitude;

        animator.SetFloat(
            "Speed",
            speed
        );

        animator.SetBool(
            "IsChasing",
            currentState ==
            EnemyState.Chasing
        );

        animator.SetBool(
            "IsAttacking",
            currentState ==
            EnemyState.Attacking
        );
    }

    // ==================================================
    // GIZMOS
    // ==================================================

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(
            patrolCenter,
            patrolRadius
        );

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(
            transform.position,
            detectionRange
        );

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(
            transform.position,
            attackRange
        );

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(
            transform.position,
            loseRange
        );

        if (Application.isPlaying)
        {
            Gizmos.color = Color.cyan;

            Gizmos.DrawWireSphere(
                currentPatrolPoint,
                0.5f
            );

            if (currentState ==
                EnemyState.Investigating)
            {
                Gizmos.color =
                    Color.magenta;

                Gizmos.DrawWireSphere(
                    investigationPosition,
                    investigationArrivalDistance
                );
            }
        }
    }
}