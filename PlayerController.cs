using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public Gladiator player;
    public Gladiator enemy;

    // Tur sonunu gecikmeli çalıştırmak için coroutine
    private IEnumerator EndPlayerTurnWithDelay()
    {
        yield return new WaitForSeconds(2f);   // 2 saniye bekle
        GameManager.Instance.EndPlayerTurn();
    }

    // Oyuncu hamleyi seçtiği anda inputu kilitle
    private void LockPlayerTurn()
    {
        GameManager.Instance.isPlayerTurn = false;
        GameManager.Instance.uiManager.UpdateActionButtonsInteractable(false);
    }

    public void OnMoveForward()
    {
        if (!GameManager.Instance.isPlayerTurn) return;
        if (!player.SpendMana(4)) return;

        LockPlayerTurn();

        // Oyuncu ileri gidiyor → distance bir kademe azalıyor
        GameManager.Instance.MoveCloser(true);     // true = oyuncu hareket ediyor
        StartCoroutine(EndPlayerTurnWithDelay());
    }

    public void OnMoveBackward()
    {
        if (!GameManager.Instance.isPlayerTurn) return;
        if (!player.SpendMana(4)) return;

        LockPlayerTurn();

        // Oyuncu geri gidiyor → distance bir kademe artıyor
        GameManager.Instance.MoveAway(true);       // true = oyuncu hareket ediyor
        StartCoroutine(EndPlayerTurnWithDelay());
    }

    public void OnRangedAttack()
    {
        Debug.Log("1. Butona Basıldı. Kontroller başlıyor...");

        if (!GameManager.Instance.isPlayerTurn) 
        {
            Debug.Log("HATA: Sıra oyuncuda değil!");
            return;
        }

        if (player.currentAmmo <= 0) 
        {
            Debug.Log("HATA: Mermi bitti!");
            return;
        }

        // Mesafe kontrolü
        if (GameManager.Instance.currentDistance == DistanceLevel.Close)
        {
            Debug.Log("HATA: Mesafe çok yakın (Close), ateş edilemez!");
            return;
        }

        // Mana kontrolü
        if (!player.SpendMana(20)) 
        {
            Debug.Log("HATA: Mana yetersiz!");
            return;
        }

        Debug.Log("2. Tüm şartlar sağlandı! Ateş emri veriliyor...");

        LockPlayerTurn();
        player.currentAmmo--;

        int damage = Random.Range(15, 21);
        
        // Asıl fırlatma işlemi
        player.ShootProjectile("Enemy", damage);

        Debug.Log("3. ShootProjectile fonksiyonu çağrıldı.");

        StartCoroutine(EndPlayerTurnWithDelay());
    }

    public void OnMeleeButton()
    {
        if (!GameManager.Instance.isPlayerTurn) return;

        if (GameManager.Instance.currentDistance != DistanceLevel.Close)
            return;

        GameManager.Instance.uiManager.ShowMeleeChoicePanel(true);
    }

    public void OnQuickAttack()
    {
        if (!GameManager.Instance.isPlayerTurn) return;
        if (GameManager.Instance.currentDistance != DistanceLevel.Close) return;
        if (!player.SpendMana(10)) return;

        LockPlayerTurn();

        player.TriggerAttack();

        if (Random.value <= 0.85f)
        {
            int dmg = Random.Range(10, 13);
            enemy.TakeDamage(dmg);
        }

        GameManager.Instance.uiManager.ShowMeleeChoicePanel(false);
        StartCoroutine(EndPlayerTurnWithDelay());
    }

    public void OnPowerAttack()
    {
        if (!GameManager.Instance.isPlayerTurn) return;
        if (GameManager.Instance.currentDistance != DistanceLevel.Close) return;
        if (!player.SpendMana(30)) return;

        LockPlayerTurn();

        player.TriggerAttack();

        if (Random.value <= 0.50f)
        {
            int dmg = Random.Range(25, 36);
            enemy.TakeDamage(dmg);
        }

        GameManager.Instance.uiManager.ShowMeleeChoicePanel(false);
        StartCoroutine(EndPlayerTurnWithDelay());
    }

    public void OnSleep()
    {
        if (!GameManager.Instance.isPlayerTurn) return;
        if (player.currentMana >= 50) return;

        LockPlayerTurn();

        player.RestoreMana(40);
        player.RestoreHP(15);

        StartCoroutine(EndPlayerTurnWithDelay());
    }

    public void OnArmorUp()
    {
        if (!GameManager.Instance.isPlayerTurn) return;
        if (!player.SpendMana(25)) return;

        LockPlayerTurn();

        player.ActivateArmorUp(2);
        StartCoroutine(EndPlayerTurnWithDelay());
    }
}
