using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int Health;

    public int MaxHealth;

    private int Stamina = 100;

    public bool IsStaminaRestroing;

    private void Start()
    {
        Health = MaxHealth;

    }

    private IEnumerator StaminaRestore()
    {
        IsStaminaRestroing = true;
        yield return new WaitForSeconds(3);
        Stamina = 100;
        IsStaminaRestroing = false;
    }

    public void SpendStamina()
    {
        Stamina -= 1;
    }

    private void StaminaCheck()
    {
        if (Stamina <= 0)
        {
            StartCoroutine(StaminaRestore());
        }
    }

    private void FixedUpdate()
    {
        StaminaCheck();
    }

    public void Healing(int HealthPointCount)
    {
        if (Health + HealthPointCount >= MaxHealth) Health = MaxHealth;
        else Health += HealthPointCount;

        Debug.Log("HP: " + Health);
    }

    public void Damaging(int DamagePointCount)
    {
        if (Health - DamagePointCount <= 0) Health = 0;
        else Health -= DamagePointCount;

        Debug.Log("HP: " + Health);
    }

}
