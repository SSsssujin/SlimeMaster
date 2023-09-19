using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RepeatSkill : SkillBase
{
    public float CoolTime { get; set; } = 1.0f;
    
    public RepeatSkill() : base(Define.SkillType.Repeat)
    {
        
    }

    private Coroutine _coSkill;

    public override void ActivateSkill()
    {
        if (_coSkill != null)
            StopCoroutine(_coSkill);

        _coSkill = StartCoroutine(CoStartSkill());
    }

    // 스킬 발동 시 실행할 메소드를 반드시 상속하도록
    protected abstract void DoSkillJob();

    protected virtual IEnumerator CoStartSkill()
    {
        WaitForSeconds wait = new WaitForSeconds(CoolTime);

        while (true)
        {
            DoSkillJob();
            
            yield return wait;
        }
    }
}
