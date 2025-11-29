using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Gladiator player;
    public Gladiator enemy;

    public void OnMoveForward()
    {
        if (!GameManager.Instance.isPlayerTurn) return;
        if (!player.SpendMana(4)) return;

        GameManager.Instance.MoveCloser();
        GameManager.Instance.EndPlayerTurn();
    }

    public void OnMoveBackward()
    {
        if (!GameManager.Instance.isPlayerTurn) return;
        if (!player.SpendMana(4)) return;

        GameManager.Instance.MoveAway();
        GameManager.Instance.EndPlayerTurn();
    }

    public void OnRangedAttack()
    {
        if (!GameManager.Instance.isPlayerTurn) return;

        if (player.currentAmmo <= 0) return;
        if (!player.SpendMana(20)) return;

        // Mid veya Far'da olmalı
        if (GameManager.Instance.currentDistance == DistanceLevel.Close)
            return;

        player.currentAmmo--;

        float roll = Random.value; // 0.0 - 1.0
        if (roll <= 0.90f)
        {
            int damage = Random.Range(15, 21); // 15-20
            enemy.TakeDamage(damage);
        }

        GameManager.Instance.EndPlayerTurn();
    }

    public void OnMeleeButton()
    {
        if (!GameManager.Instance.isPlayerTurn) return;
        if (GameManager.Instance.currentDistance != DistanceLevel.Close) return;

        GameManager.Instance.uiManager.ShowMeleeChoicePanel(true);
    }

    public void OnQuickAttack()
    {
        if (!GameManager.Instance.isPlayerTurn) return;
        if (GameManager.Instance.currentDistance != DistanceLevel.Close) return;
        if (!player.SpendMana(10)) return;

        if (Random.value <= 0.85f)
        {
            int dmg = Random.Range(10, 13);
            enemy.TakeDamage(dmg);
        }

        GameManager.Instance.uiManager.ShowMeleeChoicePanel(false);
        GameManager.Instance.EndPlayerTurn();
    }

    public void OnPowerAttack()
    {
        if (!GameManager.Instance.isPlayerTurn) return;
        if (GameManager.Instance.currentDistance != DistanceLevel.Close) return;
        if (!player.SpendMana(30)) return;

        if (Random.value <= 0.50f)
        {
            int dmg = Random.Range(25, 36);
            enemy.TakeDamage(dmg);
        }

        GameManager.Instance.uiManager.ShowMeleeChoicePanel(false);
        GameManager.Instance.EndPlayerTurn();
    }

    public void OnSleep()
    {
        if (!GameManager.Instance.isPlayerTurn) return;
        if (player.currentMana >= 50) return;

        player.RestoreMana(40);
        player.RestoreHP(15);

        GameManager.Instance.EndPlayerTurn();
    }

    public void OnArmorUp()
    {
        if (!GameManager.Instance.isPlayerTurn) return;
        if (!player.SpendMana(25)) return;

        player.ActivateArmorUp(2);
        GameManager.Instance.EndPlayerTurn();
    }
}
