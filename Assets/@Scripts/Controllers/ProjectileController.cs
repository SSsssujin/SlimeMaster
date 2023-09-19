using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

public class ProjectileController : SkillBase
{

    // 나를 쏜 주체
    private CreatureController _owner;

    private Vector3 _moveDir;
    private float _speed = 10.0f;
    private float _lifeTime = 10.0f;

    public ProjectileController() :  base(Define.SkillType.None)
    {
        
    }

    public override bool Init()
    {
        // 나중에 false로 꺼주는 코드 추가 필수
        base.Init();
        
        StartDestroy(_lifeTime);

        return true;
    }

    public void SetInfo(int templateID, CreatureController owner, Vector3 moveDir)
    {
        if (Managers.Data.SkillDic.TryGetValue(templateID, out Data.SkillData data) == false)
        {
            Debug.LogError("ProjectileController SetInfo Failed");
            return;
        }

        _owner = owner;
        _moveDir = moveDir;
        SkillData = data;
        
        // TODO : data Parsing
    }

    public override void UpdateController()
    {
        base.UpdateController();

        transform.position += _moveDir * _speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MonsterController mc = collision.gameObject.GetComponent<MonsterController>();
        // 몬스터 & 나(탄환) 풀링 됐는지 더블 체크
        if (mc.IsValid() == false)
            return;
        if (this.IsValid() == false)
            return;

        mc.OnDamaged(_owner, SkillData.damage);
    }
}
