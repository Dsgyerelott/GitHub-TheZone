using UnityEngine;

public class RockProjectile : MonoBehaviour
{
    [Header("Física")]
    public float mass = 1f;
    public float bounciness = 0.3f;

    [Header("Ruido")]
    public float noiseRadius = 8f;

    [Tooltip("Capas que podrán detectar el ruido.")]
    public LayerMask detectableLayers;

    [Header("Vida")]
    public float lifeTime = 15f;

    private bool hasGeneratedNoise;

    private void Start()
    {
        ConfigurePhysics();

        IgnorePlayerCollision();

        Destroy(gameObject, lifeTime);
    }

    private void ConfigurePhysics()
    {
        Rigidbody rb =
            GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.mass = mass;
        }

        Collider col =
            GetComponent<Collider>();

        if (col != null)
        {
            PhysicsMaterial material =
                new PhysicsMaterial();

            material.bounciness = bounciness;

            material.bounceCombine =
                PhysicsMaterialCombine.Average;

            col.material = material;
        }
    }

    private void IgnorePlayerCollision()
    {
        GameObject player =
            GameObject.FindGameObjectWithTag("Player");

        if (player == null)
            return;

        Collider rockCollider =
            GetComponent<Collider>();

        Collider[] playerColliders =
            player.GetComponentsInChildren<Collider>();

        if (rockCollider == null)
            return;

        foreach (Collider playerCollider in playerColliders)
        {
            Physics.IgnoreCollision(
                rockCollider,
                playerCollider
            );
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasGeneratedNoise)
            return;

        hasGeneratedNoise = true;

        GenerateNoise();
    }

    private void GenerateNoise()
    {
        Collider[] detectedObjects =
            Physics.OverlapSphere(
                transform.position,
                noiseRadius,
                detectableLayers
            );

        foreach (Collider detected in detectedObjects)
        {
            Debug.Log(
                "Ruido de piedra detectado por: " +
                detected.name
            );

            /*
             * Más adelante:
             *
             * EnemyAI enemy =
             *     detected.GetComponent<EnemyAI>();
             *
             * if (enemy != null)
             * {
             *     enemy.HearNoise(transform.position);
             * }
             */
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(
            transform.position,
            noiseRadius
        );
    }
}