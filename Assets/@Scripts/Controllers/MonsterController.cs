using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : CreatureController
{
    public override bool Init()
    {
        if (base.Init())
            return false;
        
        ObjectType = Define.ObjectType.Monster;

        return true;
    }

    void FixedUpdate()
    {
        PlayerController pc = Managers.Object.Player;

        if (pc == null)
            return;

        // 상대방의 위치를 바라보는 방향벡터 구하기
        Vector3 dir = pc.transform.position - transform.position;
        Vector3 newPos = transform.position + dir.normalized * Time.deltaTime * _speed;
        GetComponent<Rigidbody2D>().MovePosition(newPos);
        
        // 방향에 따라 sprite 좌우 뒤ㅣㅂ기
        GetComponent<SpriteRenderer>().flipX = dir.x > 0;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        PlayerController target = other.gameObject.GetComponent<PlayerController>();

        if (target == null)
            return;
        
        if (_coDotDamage != null)
            StopCoroutine(_coDotDamage);

        _coDotDamage = StartCoroutine(CoStartDotDamage(target));
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        PlayerController target = other.gameObject.GetComponent<PlayerController>();

        if (target == null)
            return;
        
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
        
        if (_coDotDamage != null)
            StopCoroutine(_coDotDamage);

        _coDotDamage = null;
        
        Managers.Object.Despawn(this);
    }
}
