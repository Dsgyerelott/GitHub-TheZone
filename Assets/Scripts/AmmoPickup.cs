using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    public enum AmmoType
    {
        Arrow,
        Rock
    }

    public enum RotationAxis
    {
        Horizontal,
        Vertical,
        Both,
        None
    }

    [Header("Tipo de recurso")]
    public AmmoType ammoType;

    [Min(1)]
    public int amount = 1;

    [Header("Animación flotante")]
    public float floatHeight = 0.3f;
    public float floatSpeed = 2f;

    [Header("Rotación")]
    public RotationAxis rotationAxis = RotationAxis.Horizontal;
    public float rotationSpeed = 90f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;

        Collider col = GetComponent<Collider>();

        if (col != null)
        {
            col.isTrigger = true;
        }
    }

    private void Update()
    {
        FloatMovement();
        RotatePickup();
    }

    private void FloatMovement()
    {
        float newY =
            startPosition.y +
            Mathf.Sin(Time.time * floatSpeed) * floatHeight;

        transform.position = new Vector3(
            startPosition.x,
            newY,
            startPosition.z
        );
    }

    private void RotatePickup()
    {
        switch (rotationAxis)
        {
            case RotationAxis.Horizontal:

                transform.Rotate(
                    Vector3.up *
                    rotationSpeed *
                    Time.deltaTime
                );

                break;

            case RotationAxis.Vertical:

                transform.Rotate(
                    Vector3.right *
                    rotationSpeed *
                    Time.deltaTime
                );

                break;

            case RotationAxis.Both:

                transform.Rotate(
                    Vector3.up *
                    rotationSpeed *
                    Time.deltaTime
                );

                transform.Rotate(
                    Vector3.right *
                    rotationSpeed *
                    Time.deltaTime
                );

                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        AmmoSystem ammoSystem =
            other.GetComponent<AmmoSystem>();

        if (ammoSystem == null)
        {
            ammoSystem =
                other.GetComponentInParent<AmmoSystem>();
        }

        if (ammoSystem == null)
            return;

        switch (ammoType)
        {
            case AmmoType.Arrow:

                ammoSystem.AddArrows(amount);

                break;

            case AmmoType.Rock:

                ammoSystem.AddRocks(amount);

                break;
        }

        Destroy(gameObject);
    }
}