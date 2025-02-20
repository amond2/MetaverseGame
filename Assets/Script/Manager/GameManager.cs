using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public PlayerController player { get; private set; }
    private ResourceController _playerResourceController;
    
    [SerializeField] private int currentWaveIndex = 0;

    private EnemyManager enemyManager;

    private void Awake()
    {
        instance = this;
        player = FindObjectOfType<PlayerController>();
        player.Init(this);

        enemyManager = GetComponentInChildren<EnemyManager>();

        if (enemyManager != null) // 임시 구분
        {
            enemyManager.Init(this);
        }
    }

    public void StartGame()
    {
        StartNextWave();
    }
    
    void StartNextWave()
    {
        currentWaveIndex += 1;
        enemyManager.StartWave(1 + currentWaveIndex / 5);
    }

    public void EndOfWave()
    {
        StartNextWave();
    }

    public void GameOver()
    {
        enemyManager.StopWave();
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 임시 구분, 마우스 죄클릭시 스테이지 업뎃 됨.
        {
            StartGame();
        }
        
    }
}