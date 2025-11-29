using UnityEngine;

public enum DistanceLevel
{
    Close,
    Mid,
    Far
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("References")]
    public Gladiator player;
    public Gladiator enemy;
    public UIManager uiManager;
    public EnemyController enemyController;

    [Header("Transforms")]
    public Transform playerTransform;
    public Transform enemyTransform;

    [Header("Turn / State")]
    public bool isPlayerTurn = true;
    public DistanceLevel currentDistance = DistanceLevel.Mid;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // Her zaman oyuncu turu ile başlasın
        isPlayerTurn = true;
        currentDistance = DistanceLevel.Mid;

        float music = PlayerPrefs.GetFloat("MusicVolume", 0.8f);
        AudioListener.volume = music;

        uiManager.UpdateAllUI();
        uiManager.UpdateDistanceText(currentDistance);
        UpdateDistanceVisual();                 // 🔹 başlangıç pozisyonlarını ayarla
        uiManager.SetTurnText("Oyuncu Sırası");
        uiManager.UpdateActionButtonsInteractable(true);
    }

    public void EndPlayerTurn()
    {
        player.OnTurnEnd();
        uiManager.UpdateAllUI();

        CheckGameEnd();
        if (IsGameOver()) return;

        isPlayerTurn = false;
        uiManager.SetTurnText("Rakip Sırası");
        uiManager.UpdateActionButtonsInteractable(false);

        enemyController.StartEnemyTurn();
    }

    public void EndEnemyTurn()
    {
        enemy.OnTurnEnd();
        uiManager.UpdateAllUI();

        CheckGameEnd();
        if (IsGameOver()) return;

        isPlayerTurn = true;
        uiManager.SetTurnText("Oyuncu Sırası");
        uiManager.UpdateActionButtonsInteractable(true);
    }

    private void CheckGameEnd()
    {
        if (player.currentHP <= 0)
        {
            uiManager.SetTurnText("Kaybettin!");
            uiManager.UpdateActionButtonsInteractable(false);
        }
        else if (enemy.currentHP <= 0)
        {
            uiManager.SetTurnText("Kazandın!");
            uiManager.UpdateActionButtonsInteractable(false);
        }
    }

    private bool IsGameOver()
    {
        return player.currentHP <= 0 || enemy.currentHP <= 0;
    }

    // --- MESAFE DEĞİŞTİRME ---

    public void MoveCloser()
    {
        if (currentDistance == DistanceLevel.Far)
            currentDistance = DistanceLevel.Mid;
        else if (currentDistance == DistanceLevel.Mid)
            currentDistance = DistanceLevel.Close;

        uiManager.UpdateDistanceText(currentDistance);
        UpdateDistanceVisual();    // 🔹 görsel olarak da yaklaştır
    }

    public void MoveAway()
    {
        if (currentDistance == DistanceLevel.Close)
            currentDistance = DistanceLevel.Mid;
        else if (currentDistance == DistanceLevel.Mid)
            currentDistance = DistanceLevel.Far;

        uiManager.UpdateDistanceText(currentDistance);
        UpdateDistanceVisual();    // 🔹 görsel olarak uzaklaştır
    }

    // --- GLADYATÖRLERİN POZİSYONUNU GÜNCELLE ---

    private void UpdateDistanceVisual()
    {
        if (playerTransform == null || enemyTransform == null) return;

        float playerX = -4f;
        float enemyX  =  4f;

        switch (currentDistance)
        {
            case DistanceLevel.Close:
                playerX = -2f;
                enemyX  =  2f;
                break;

            case DistanceLevel.Mid:
                playerX = -4f;
                enemyX  =  4f;
                break;

            case DistanceLevel.Far:
                playerX = -6f;
                enemyX  =  6f;
                break;
        }

        Vector3 p = playerTransform.position;
        Vector3 e = enemyTransform.position;

        playerTransform.position = new Vector3(playerX, p.y, p.z);
        enemyTransform.position  = new Vector3(enemyX,  e.y, e.z);
    }
}
