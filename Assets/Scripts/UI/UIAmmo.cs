using UnityEngine;
using TMPro;

public class UIAmmo : MonoBehaviour
{
    [Header("Textos")]
    public TMP_Text arrowText;
    public TMP_Text rockText;

    [Header("Referencias")]
    public AmmoSystem ammoSystem;

    private void Start()
    {
        if (ammoSystem == null)
        {
            ammoSystem =
                FindFirstObjectByType<AmmoSystem>();
        }

        if (ammoSystem != null)
        {
            ammoSystem.OnAmmoChanged += UpdateUI;

            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        if (ammoSystem == null)
            return;

        if (arrowText != null)
        {
            arrowText.text =
                ammoSystem.GetArrowAmmo().ToString();
        }

        if (rockText != null)
        {
            rockText.text =
                ammoSystem.GetRockAmmo().ToString();
        }
    }

    private void OnDestroy()
    {
        if (ammoSystem != null)
        {
            ammoSystem.OnAmmoChanged -= UpdateUI;
        }
    }
}