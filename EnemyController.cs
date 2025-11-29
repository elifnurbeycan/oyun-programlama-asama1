using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public Gladiator enemy;
    public Gladiator player;

    public void StartEnemyTurn()
    {
        StartCoroutine(EnemyTurnRoutine());
    }

    private IEnumerator EnemyTurnRoutine()
    {
        yield return new WaitForSeconds(0.5f);

        bool actionDone = false;
        int safety = 0;

        while (!actionDone && safety < 10)
        {
            safety++;

            int choice = Random.Range(0, 5); 
            // 0: Move, 1: Ranged, 2: Melee, 3: Sleep, 4: ArmorUp

            switch (choice)
            {
                case 0:
                    actionDone = EnemyMove();
                    break;
                case 1:
                    actionDone = EnemyRanged();
                    break;
                case 2:
                    actionDone = EnemyMelee();
                    break;
                case 3:
                    actionDone = EnemySleep();
                    break;
                case 4:
                    actionDone = EnemyArmorUp();
                    break;
            }
        }

        GameManager.Instance.EndEnemyTurn();
    }

    private bool EnemyMove()
    {
        if (!enemy.SpendMana(4)) return false;

        bool forward = Random.value > 0.5f;

        if (forward)
            GameManager.Instance.MoveCloser();
        else
            GameManager.Instance.MoveAway();

        return true;
    }

    private bool EnemyRanged()
    {
        if (enemy.currentAmmo <= 0) return false;
        if (!enemy.SpendMana(20)) return false;

        if (GameManager.Instance.currentDistance == DistanceLevel.Close)
            return false;

        enemy.currentAmmo--;

        if (Random.value <= 0.90f)
        {
            int dmg = Random.Range(15, 21);
            player.TakeDamage(dmg);
        }

        return true;
    }

    private bool EnemyMelee()
    {
        if (GameManager.Instance.currentDistance != DistanceLevel.Close)
            return false;

        bool power = Random.value > 0.5f;

        if (power)
        {
            if (!enemy.SpendMana(30)) return false;

            if (Random.value <= 0.50f)
            {
                int dmg = Random.Range(25, 36);
                player.TakeDamage(dmg);
            }
        }
        else
        {
            if (!enemy.SpendMana(10)) return false;

            if (Random.value <= 0.85f)
            {
                int dmg = Random.Range(10, 13);
                player.TakeDamage(dmg);
            }
        }

        return true;
    }

    private bool EnemySleep()
    {
        if (enemy.currentMana >= 50) return false;

        enemy.RestoreMana(40);
        enemy.RestoreHP(15);
        return true;
    }

    private bool EnemyArmorUp()
    {
        if (!enemy.SpendMana(25)) return false;

        enemy.ActivateArmorUp(2);
        return true;
    }
}
