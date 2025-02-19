using UnityEngine;

public class EnemyController : BaseController
{
    private EnemyManager enemyManager;
    private Transform target;
    
    [SerializeField] private float followRange = 15f;
    
    public void Init(EnemyManager enemyManager, Transform target)
    {
        this.enemyManager = enemyManager;
        this.target = target; // 적의 상대는 항상 플레이어임.
    }

    protected float DistanceToTarget()
    {
        return Vector3.Distance(transform.position, target.position); // 방향 구하기
    }

    protected override void HandleAction() // 적의 공격 판단 넣기
    {
        base.HandleAction();

        if (weaponHandler == null || target == null)
        {
            if(!movementDirection.Equals(Vector2.zero)) movementDirection = Vector2.zero;            
            return;
        }

        float distance = DistanceToTarget();
        Vector2 direction = DirectionToTarget();

        isAttacking = false;
        if (distance <= followRange) // 적이 플레이어를 따라갈 거리에 들어 왔는지 체크
        {
            lookDirection = direction;
            
            if (distance <= weaponHandler.AttackRange) // 더이상 이동하지 않고 공격 하겠다.
            {
                int layerMaskTarget = weaponHandler.target; // 충돌이 있는지 체크
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, weaponHandler.AttackRange * 1.5f, (1 << LayerMask.NameToLayer("Level")) | layerMaskTarget); // 이진수로 더하기?, 레벨과 충돌하는 경우는 공격 안함.

                if (hit.collider != null && layerMaskTarget == (layerMaskTarget | (1 << hit.collider.gameObject.layer))) // 실제 공격을 할 수 있는지 체크
                {
                    isAttacking = true;
                }
                
                movementDirection = Vector2.zero;
                return;
            }
            
            movementDirection = direction;
        }

    }

    protected Vector2 DirectionToTarget()
    {
        return (target.position - transform.position).normalized;
    }
    
    public override void Death() // 적 사망 추가 
    {
        base.Death();
    }
    
}