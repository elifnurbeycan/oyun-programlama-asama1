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
        yield return new WaitForSeconds(1.0f);

        bool actionDone = false;
        int safety = 0;

        while (!actionDone && safety < 10)
        {
            safety++;

            // 🔥 TEST KODU: RASTGELELİK KAPALI 🔥
            // Normalde: int choice = Random.Range(0, 5);
            
            // Sürekli OK ATMAYA (1) çalışsın
            int choice = 1; 

            // AMA DİKKAT: Kural gereği "Close" mesafede ok atamaz.
            // Eğer yakındaysak mecburen hareket etsin (0) ki oyun donmasın.
            if (GameManager.Instance.currentDistance == DistanceLevel.Close)
            {
                choice = 0; // Move
            }

            switch (choice)
            {
                case 0: actionDone = EnemyMove(); break;
                case 1: actionDone = EnemyRanged(); break;
                case 2: actionDone = EnemyMelee(); break;
                case 3: actionDone = EnemySleep(); break;
                case 4: actionDone = EnemyArmorUp(); break;
            }
            yield return null; 
        }

        yield return new WaitForSeconds(1.5f);
        GameManager.Instance.EndEnemyTurn();
    }

    // --- AKSİYONLAR ---

    private bool EnemyMove()
    {
        if (!enemy.SpendMana(4)) return false;
        // Test için hep geri kaçsın ki ok atabilsin
        GameManager.Instance.MoveAway(false); 
        return true;
    }

    private bool EnemyRanged()
    {
        if (enemy.currentAmmo <= 0 || !enemy.SpendMana(20)) return false;
        if (GameManager.Instance.currentDistance == DistanceLevel.Close) return false;

        enemy.currentAmmo--;
        
        int damage = Random.Range(15, 21);
        
        // 🔥 OKU FIRLAT (Yönü FirePoint belirleyecek)
        enemy.ShootProjectile("Player", damage);

        return true;
    }

    private bool EnemyMelee()
    {
        if (GameManager.Instance.currentDistance != DistanceLevel.Close) return false;
        // ... (Diğer kodlar aynı kalabilir, buraya girmeyecek zaten)
        return true; 
    }

    private bool EnemySleep() { return true; } // Basitleştirildi
    private bool EnemyArmorUp() { return true; } // Basitleştirildi
}
