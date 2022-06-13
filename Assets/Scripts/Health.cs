using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{

    public int CurrentHealth;
    public int MaxHealth;

    public void HealthCHange(int healthValue)
    {
        CurrentHealth -= healthValue;

        if (CurrentHealth < 1)
        {
            KillCharacter();
        }

    }

    void KillCharacter()
    {

    }

    public void RestartCharacter()
    {
        CurrentHealth = MaxHealth;
    }

}
