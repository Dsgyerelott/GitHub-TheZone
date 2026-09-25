using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

public class EquipmentState : MonoBehaviour
{
    public enum Equipment
    {
        Spear,
        Bow
    }

    [Header("Equipamiento actual")]
    public Equipment currentEquipment = Equipment.Spear;

    [Header("UI opcional")]
    public Image equipmentImage;

    [Header("Animation Rigging")]
    public RigBuilder rigBuilder;
    public TwoBoneIKConstraint leftHandIK;
    public TwoBoneIKConstraint rightHandIK;

    [Header("Lanza")]
    public Sprite spearSprite;
    public Transform leftHandSpearTarget;
    public Transform rightHandSpearTarget;
    public GameObject spear;

    [Header("Arco")]
    public Sprite bowSprite;
    public Transform leftHandBowTarget;
    public Transform rightHandBowTarget;
    public GameObject bow;

    private void Start()
    {
        EquipSpear();
    }

    private void Update()
    {
        // 1 = Lanza
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipSpear();
        }

        // 2 = Arco
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EquipBow();
        }

        // Q = alternar entre Lanza y Arco
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ChangeEquipment();
        }
    }

    public void ChangeEquipment()
    {
        if (currentEquipment == Equipment.Spear)
        {
            EquipBow();
        }
        else
        {
            EquipSpear();
        }
    }

    public void SwitchEquipment(string equipmentName)
    {
        switch (equipmentName)
        {
            case "Spear":
                EquipSpear();
                break;

            case "Bow":
                EquipBow();
                break;
        }
    }

    public void EquipSpear()
    {
        currentEquipment = Equipment.Spear;

        if (equipmentImage != null && spearSprite != null)
        {
            equipmentImage.sprite = spearSprite;
        }

        if (leftHandIK != null && leftHandSpearTarget != null)
        {
            leftHandIK.data.target = leftHandSpearTarget;
        }

        if (rightHandIK != null && rightHandSpearTarget != null)
        {
            rightHandIK.data.target = rightHandSpearTarget;
        }

        if (bow != null)
        {
            bow.SetActive(false);
        }

        if (spear != null)
        {
            spear.SetActive(true);
        }

        RebuildRig();
    }

    public void EquipBow()
    {
        currentEquipment = Equipment.Bow;

        if (equipmentImage != null && bowSprite != null)
        {
            equipmentImage.sprite = bowSprite;
        }

        if (leftHandIK != null && leftHandBowTarget != null)
        {
            leftHandIK.data.target = leftHandBowTarget;
        }

        if (rightHandIK != null && rightHandBowTarget != null)
        {
            rightHandIK.data.target = rightHandBowTarget;
        }

        if (spear != null)
        {
            spear.SetActive(false);
        }

        if (bow != null)
        {
            bow.SetActive(true);
        }

        RebuildRig();
    }

    private void RebuildRig()
    {
        if (rigBuilder != null)
        {
            rigBuilder.Build();
        }
    }

    public Equipment GetCurrentEquipment()
    {
        return currentEquipment;
    }
}