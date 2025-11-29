using UnityEngine;

public class Gladiator : MonoBehaviour
{
    [Header("Base Stats")]
    public int maxHP = 100;
    public int currentHP;

    public int maxMana = 120;
    [Tooltip("Oyuna başlarken sahip olacağı mana")]
    public int startMana = 80;
    public int currentMana;

    [Header("Ranged")]
    public int maxAmmo = 10;
    public int currentAmmo;

    [Header("Armor Up")]
    public bool armorUpActive = false;
    public int armorUpTurnsRemaining = 0;   // 2 tur

    private void Awake()
    {
        currentHP = maxHP;
        currentMana = Mathf.Clamp(startMana, 0, maxMana);
        currentAmmo = maxAmmo;
    }

    public void TakeDamage(int amount)
    {
        float finalDamage = amount;

        if (armorUpActive)
        {
            finalDamage *= 0.8f; // %20 azalt
        }

        currentHP -= Mathf.RoundToInt(finalDamage);
        if (currentHP < 0) currentHP = 0;
    }

    public bool SpendMana(int amount)
    {
        if (currentMana < amount) return false;
        currentMana -= amount;
        return true;
    }

    public void RestoreMana(int amount)
    {
        currentMana += amount;
        if (currentMana > maxMana) currentMana = maxMana;
    }

    public void RestoreHP(int amount)
    {
        currentHP += amount;
        if (currentHP > maxHP) currentHP = maxHP;
    }

    public void ActivateArmorUp(int turns)
    {
        armorUpActive = true;
        armorUpTurnsRemaining = turns;
    }

    // Her turun sonunda çağrılacak
    public void OnTurnEnd()
    {
        if (armorUpActive)
        {
            armorUpTurnsRemaining--;
            if (armorUpTurnsRemaining <= 0)
            {
                armorUpActive = false;
            }
        }
    }
}
