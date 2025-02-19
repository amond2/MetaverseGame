using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private LayerMask levelCollisionLayer;

    private RangeWeaponHandler rangeWeaponHandler;
    
    private float currentDuration;
    private Vector2 direction;
    private bool isReady;
    private Transform pivot;
    
    private Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;

    public bool fxOnDestory = true;
    
    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        pivot = transform.GetChild(0);
    }

    private void Update()
    {
        if (!isReady) 
        { 
            return; 
        }

        currentDuration += Time.deltaTime;

        if(currentDuration > rangeWeaponHandler.Duration)
        {
            DestroyProjectile(transform.position, false);
        }

        rigidbody.velocity = direction * rangeWeaponHandler.Speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(levelCollisionLayer.value == (levelCollisionLayer.value | (1 << collision.gameObject.layer))) // 레벨과의 충돌시 투사체 삭제
        {
            DestroyProjectile(collision.ClosestPoint(transform.position) - direction * .2f, fxOnDestory);
        }
        else if(rangeWeaponHandler.target.value == (rangeWeaponHandler.target.value | (1 << collision.gameObject.layer))) // 투사체와 타겟과의 충돌
        {
            ResourceController resourceController = collision.GetComponent<ResourceController>();
            if(resourceController != null)
            {
                resourceController.ChangeHealth(-rangeWeaponHandler.Power); // 데미지 , 넉백 처리
                if(rangeWeaponHandler.IsOnKnockback) 
                {
                    BaseController controller = collision.GetComponent<BaseController>();
                    if(controller != null)
                    {
                        controller.ApplyKnockback(transform, rangeWeaponHandler.KnockbackPower, rangeWeaponHandler.KnockbackTime);
                    }
                }
            }
            
            DestroyProjectile(collision.ClosestPoint(transform.position), fxOnDestory);
        }
    }


    public void Init(Vector2 direction, RangeWeaponHandler weaponHandler)
    {
        rangeWeaponHandler = weaponHandler;
        
        this.direction = direction;
        currentDuration = 0;
        transform.localScale = Vector3.one * weaponHandler.BulletSize;
        spriteRenderer.color = weaponHandler.ProjectileColor;

        transform.right = this.direction;
        
        if (this.direction.x < 0)
            pivot.localRotation = Quaternion.Euler(180, 0, 0);
        else
            pivot.localRotation = Quaternion.Euler(0, 0, 0);

        isReady = true;
    }
    
    private void DestroyProjectile(Vector3 position, bool createFx)
    {
        Destroy(this.gameObject);
    }
}
