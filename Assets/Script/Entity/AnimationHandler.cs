using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    // 애니메이터를 숫자열로 비교하여 쓰기위해 변환 작업. 고유한 숫자인 해시로 변환.
    private static readonly int IsMoving = Animator.StringToHash("isMove");
    private static readonly int IsDamage = Animator.StringToHash("isDamage");

    protected Animator animator;

    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void Move(Vector2 obj)
    {
        animator.SetBool(IsMoving, obj.magnitude > 0.5f);
    }

    public void Damage()
    {
        animator.SetBool(IsDamage, true);
    }
    
    //무적이 끝나는 시간을 체크
    public void InvincibilityEnd()
    {
        animator.SetBool(IsDamage, false);
    }
}
