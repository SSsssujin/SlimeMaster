using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class MonsterController : CreatureController
{
    private class MonsterData
    {
        
        public int Hp;
        
    }
    
    private MonsterData _monsterData;
    private Define.CreatureState _creatureState = Define.CreatureState.Moving;

    public virtual Define.CreatureState CreatureState
    {
        get => _creatureState;
        set
        {
            _creatureState = value;
            UpdateAnimation();
        }
    }

    protected Animator _animator;

    public virtual void UpdateAnimation()
    {
        
    }

    public override void UpdateController()
    {
        base.UpdateController();

        switch (CreatureState)
        {
            case Define.CreatureState.Idle:
                UpdateIdle();
                break;
            case Define.CreatureState.Moving:
                UpdateMoving();
                break;
            case Define.CreatureState.Skill:
                UpdateSkill();
                break;
            case Define.CreatureState.Dead:
                UpdateDead();
                break;
        }
    }

    protected virtual void UpdateIdle() { }
    protected virtual void UpdateSkill() { }
    protected virtual void UpdateMoving() { }
    protected virtual void UpdateDead() { }

    public override bool Init()
    {
        if (base.Init())
            return false;

        _animator = GetComponent<Animator>();
        ObjectType = Define.ObjectType.Monster;
        CreatureState = Define.CreatureState.Moving;

        return true;
    }

    void FixedUpdate()
    {
        if (CreatureState != Define.CreatureState.Moving) 
            return;
        
        PlayerController pc = Managers.Object.Player;

        if (pc == null)
            return;

        // 상대방의 위치를 바라보는 방향벡터 구하기
        Vector3 dir = pc.transform.position - transform.position;
        Vector3 newPos = transform.position + dir.normalized * Time.deltaTime * _speed;
        GetComponent<Rigidbody2D>().MovePosition(newPos);
        
        // 방향에 따라 sprite 좌우 뒤집기
        GetComponent<SpriteRenderer>().flipX = dir.x > 0;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        PlayerController target = other.gameObject.GetComponent<PlayerController>();
        
        if (!target.IsValid() || !this.IsValid())
            return;
        
        if (_coDotDamage != null)
            StopCoroutine(_coDotDamage);

        _coDotDamage = StartCoroutine(CoStartDotDamage(target));
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        PlayerController target = other.gameObject.GetComponent<PlayerController>();

        if (!target.IsValid() || !this.IsValid())
            return;
        
        // 오브젝트풀로 오브젝트 관리하다보면
        // 해당 물체를 SetActive(false) 처리했지만 실제로는 삭제된 게 아니라서
        // null 처리가 되지 않을 수도 있다.
        // 따라서 다음과 같은 코드로 예외처리 해줘야 할 수도 있음.
        // if (!target.isActiveAndEnabled)
        //     return;
        
        if (_coDotDamage != null)
            StopCoroutine(_coDotDamage);

        _coDotDamage = null;
    }

    private Coroutine _coDotDamage;
    
    public IEnumerator CoStartDotDamage(PlayerController target)
    {
        while (true)
        {
            target.OnDamaged(this, 2);
            
            yield return new WaitForSeconds(0.1f);
        }
    }

    protected override void OnDead()
    {
        base.OnDead();

        Managers.Game.KillCount++;
        
        if (_coDotDamage != null)
            StopCoroutine(_coDotDamage);

        _coDotDamage = null;
        
        // 죽을 때 보석 스폰
        GemController gc =  Managers.Object.Spawn<GemController>(transform.position);
        
        Managers.Object.Despawn(this);
    }
}
