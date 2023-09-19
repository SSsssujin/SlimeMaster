using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CreatureController : BaseController
{
    // 나한테 걸어준 버프 목록
    // 보통 Creature가 들고 있는게 일반적이다.
    
    protected float _speed = 1.0f;
    
    public int Hp { get; set; } = 100;
    public int MaxHp { get; set; } = 100;
    
    public SkillBook Skills { get; protected set; }

    public override bool Init()
    {
        base.Init();

        Skills = gameObject.GetOrAddComponent<SkillBook>();

        return true;
    }

    public virtual void OnDamaged(BaseController attacker, int damage)
    {
        if (Hp <= 0)
            return;
        
        Hp -= damage;

        if (Hp <= 0)
        {
            Hp = 0;
            OnDead();
        }
    }

    protected virtual void OnDead()
    {
        
    }
}
