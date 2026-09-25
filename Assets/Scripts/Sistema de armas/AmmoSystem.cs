using System;
using UnityEngine;

public class AmmoSystem : MonoBehaviour
{
    [Header("Munición actual")]
    public int arrowAmmo = 10;
    public int rockAmmo = 3;

    [Header("Capacidad máxima")]
    public int arrowMaxAmmo = 30;
    public int rockMaxAmmo = 10;

    public Action OnAmmoChanged;

    // ==================================================
    // FLECHAS
    // ==================================================

    public bool CanUseArrow()
    {
        return arrowAmmo > 0;
    }

    public void UseArrow()
    {
        if (arrowAmmo <= 0)
            return;

        arrowAmmo--;
        OnAmmoChanged?.Invoke();
    }

    public void AddArrows(int amount)
    {
        arrowAmmo = Mathf.Min(
            arrowAmmo + amount,
            arrowMaxAmmo
        );

        OnAmmoChanged?.Invoke();
    }

    public int GetArrowAmmo()
    {
        return arrowAmmo;
    }

    // ==================================================
    // PIEDRAS
    // ==================================================

    public bool CanUseRock()
    {
        return rockAmmo > 0;
    }

    public void UseRock()
    {
        if (rockAmmo <= 0)
            return;

        rockAmmo--;
        OnAmmoChanged?.Invoke();
    }

    public void AddRocks(int amount)
    {
        rockAmmo = Mathf.Min(
            rockAmmo + amount,
            rockMaxAmmo
        );

        OnAmmoChanged?.Invoke();
    }

    public int GetRockAmmo()
    {
        return rockAmmo;
    }
}