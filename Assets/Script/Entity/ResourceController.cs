using UnityEngine;

public class ResourceController : MonoBehaviour
{
    [SerializeField] private float healthChangeDelay = 0.5f; // 연속으로 데미지를 입는 경우, 일정 주기동안 무적 상태 존재

    private BaseController baseController;
    private StatHandler statHandler;
    private AnimationHandler animationHandler;
    
    private float timeSinceLastChange = float.MaxValue;

    public float CurrentHealth { get; private set; }
    public float MaxHealth => statHandler.Health; // hp의 정보를 가져옴

    private void Awake()
    {
        statHandler = GetComponent<StatHandler>();
        animationHandler = GetComponent<AnimationHandler>();
        baseController = GetComponent<BaseController>();
    }

    private void Start()
    {
        CurrentHealth = statHandler.Health;
    }

    private void Update()
    {
        if (timeSinceLastChange < healthChangeDelay) 
        {
            timeSinceLastChange += Time.deltaTime;
            if (timeSinceLastChange >= healthChangeDelay)
            {
                animationHandler.InvincibilityEnd(); // 무적해제
            }
        }
    }

    public bool ChangeHealth(float change) // 데미지 적용
    {
        if (change == 0 || timeSinceLastChange < healthChangeDelay)
        {
            return false;
        }

        timeSinceLastChange = 0f; // 데미지 받았다면 0으로 초기화
        CurrentHealth += change;
        CurrentHealth = CurrentHealth > MaxHealth ? MaxHealth : CurrentHealth;
        CurrentHealth = CurrentHealth < 0 ? 0 : CurrentHealth;

        if (change < 0)
        {
            animationHandler.Damage(); // 데미지 호출
            
        }

        if (CurrentHealth <= 0f)
        {
            Death(); // 데스 호출
        }

        return true;
    }

    private void Death() // 실제로 사망 판정하는 곳은 여기
    {
        baseController.Death();
    }

}