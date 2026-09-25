using UnityEngine;

public class ArrowProjectile : MonoBehaviour
{
    [Header("Configuración")]
    public float lifeTime = 10f;

    [Header("Impacto")]
    public float impactForce = 3f;

    [Header("Comportamiento")]
    public bool stickOnImpact = true;

    private Rigidbody rb;
    private bool hasHit;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        IgnorePlayerCollision();

        Destroy(gameObject, lifeTime);
    }

    private void IgnorePlayerCollision()
    {
        GameObject player =
            GameObject.FindGameObjectWithTag("Player");

        if (player == null)
            return;

        Collider arrowCollider =
            GetComponent<Collider>();

        Collider[] playerColliders =
            player.GetComponentsInChildren<Collider>();

        if (arrowCollider == null)
            return;

        foreach (Collider playerCollider in playerColliders)
        {
            Physics.IgnoreCollision(
                arrowCollider,
                playerCollider
            );
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasHit)
            return;

        hasHit = true;

        Rigidbody targetRb =
            collision.rigidbody;

        if (targetRb != null &&
            rb != null)
        {
            targetRb.AddForce(
                rb.linearVelocity.normalized *
                impactForce,
                ForceMode.Impulse
            );
        }

        if (stickOnImpact)
        {
            StickToSurface(collision);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void StickToSurface(Collision collision)
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.isKinematic = true;
        }

        Collider arrowCollider =
            GetComponent<Collider>();

        if (arrowCollider != null)
        {
            arrowCollider.enabled = false;
        }

        transform.SetParent(
            collision.transform,
            true
        );
    }
}