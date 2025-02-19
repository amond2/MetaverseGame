using UnityEngine;

public class BaseController : MonoBehaviour
{
    protected Rigidbody2D rigidbody;
    
    [SerializeField] private SpriteRenderer characterRenderer;
    [SerializeField] private Transform weaponPivot;

    protected Vector2 movementDirection = Vector2.zero;
    public Vector2 MovementDirection{get{return movementDirection;}}
    
    protected Vector2 lookDirection = Vector2.zero;
    public Vector2 LookDirection{get{return lookDirection;}}

    private Vector2 knockback = Vector2.zero;
    private float knockbackDuration = 0.0f;
    
    protected AnimationHandler animationHandler;
    protected StatHandler statHandler;

    // 무기 장착
    [SerializeField] public WeaponHandler WeaponPrefab;
    protected WeaponHandler weaponHandler;
    
    protected bool isAttacking;
    private float timeSinceLastAttack = float.MaxValue;
    
    protected virtual void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animationHandler = GetComponent<AnimationHandler>();
        statHandler = GetComponent<StatHandler>();
        
        // 무기 장착
        if (WeaponPrefab != null)
            weaponHandler = Instantiate(WeaponPrefab, weaponPivot); // 무기 장창
        else
            weaponHandler = GetComponentInChildren<WeaponHandler>(); // 무기 찾기

    }

    protected virtual void Start()
    {
        
    }
    
    protected virtual void Update()
    {
        HandleAction();
        Rotate(lookDirection);
        
        // 무기 장착
        HandleAttackDelay();
    }
    
    protected virtual void FixedUpdate()
    {
        Movement(movementDirection);
        if(knockbackDuration > 0.0f)
        {
            knockbackDuration -= Time.fixedDeltaTime;
        }
    }
    
    protected virtual void HandleAction()
    {
        
    }

    private void Movement(Vector2 direction)
    {
        direction = direction * statHandler.Speed; // 속도
        if(knockbackDuration > 0.0f)
        {
            direction *= 0.2f;
            direction += knockback;
        }
        
        rigidbody.velocity = direction;
        animationHandler.Move(direction);
    }

    private void Rotate(Vector2 direction)
    {
        float rotZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bool isLeft = Mathf.Abs(rotZ) > 90f;
        
        characterRenderer.flipX = isLeft;
        
        if (weaponPivot != null)
        {
            weaponPivot.rotation = Quaternion.Euler(0, 0, rotZ);
        }
        
        weaponHandler?.Rotate(isLeft); // 무기 장착
        
    }
    
    public void ApplyKnockback(Transform other, float power, float duration)
    {
        knockbackDuration = duration;
        knockback = -(other.position - transform.position).normalized * power;
    }

    private void HandleAttackDelay()
    {
        if(weaponHandler == null) // 무기 장착 - 일정 시간 마다 어택 호출.
            return;
        
        if (timeSinceLastAttack <= weaponHandler.Delay)
        {
            timeSinceLastAttack += Time.deltaTime;
        }

        if (isAttacking && timeSinceLastAttack > weaponHandler.Delay) // 무기 장착 - 일정 시간마다 발사 처리
        {
            timeSinceLastAttack = 0;
            Attack();
        }
    }

    protected virtual void Attack()
    {
        if(lookDirection != Vector2.zero) // 무기 장착 - 룩디랙션 0이 아니면 어택 실행
            weaponHandler?.Attack();
    }
    
    //사망 추가
    public virtual void Death()
    {
        rigidbody.velocity = Vector3.zero;

        foreach (SpriteRenderer renderer in transform.GetComponentsInChildren<SpriteRenderer>()) // 캐릭터가 모든 스프라이트 찾기, 컬러 변경
        {
            Color color = renderer.color;
            color.a = 0.3f;
            renderer.color = color;
        }

        foreach (Behaviour component in transform.GetComponentsInChildren<Behaviour>()) // 컴포넌트들이 동작하지 않도록 비활성화
        {
            component.enabled = false; 
        }

        Destroy(gameObject, 2f); // 게임 오브젝트를 2초뒤 삭제
    }
}