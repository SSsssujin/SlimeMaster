using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonsterController
{
    public override bool Init()
    {
        base.Init();

        _animator = GetComponent<Animator>();
        CreatureState = Define.CreatureState.Moving;
        Hp = 10000;

        CreatureState = Define.CreatureState.Skill;

       Skills.AddSkill<Move>(transform.position);
       Skills.AddSkill<Dash>(transform.position);
       Skills.AddSkill<Dash>(transform.position);
       Skills.AddSkill<Dash>(transform.position);
       Skills.StartNextSequenceSkill();
        
        return true;
    }

    public override void UpdateAnimation()
    {
        switch (CreatureState)
        {
            case Define.CreatureState.Idle:
                _animator.Play("Idle");
                break;
            case Define.CreatureState.Moving:
                _animator.Play("Moving");
                break;
            case Define.CreatureState.Skill:
                //_animator.Play("Attack");
                // 어차피 스킬 안에 애니메이션이 있을 테니 그 안에서 지정해줄 것임
                break;
            case Define.CreatureState.Dead:
                _animator.Play("Death");
                break;
        }
    }

    private float _range = 2.0f;

    protected override void UpdateDead()
    {
        if (_coWait == null)
            Managers.Object.Despawn(this);
    }

    private Coroutine _coWait;

    void Wait(float waitSeconds)
    {
        if (_coWait != null)
            StopCoroutine(_coWait);

        _coWait = StartCoroutine(CoStartWait(waitSeconds));
    }

    IEnumerator CoStartWait(float waitSeconds)
    {
        yield return new WaitForSeconds(waitSeconds);
        _coWait = null;
    }

    public override void OnDamaged(BaseController attacker, int damage)
    {
        //base.OnDamaged(attacker, damage);
    }

    protected override void OnDead()
    {
        CreatureState = Define.CreatureState.Dead;
        Wait(2.0f);
    }
}
