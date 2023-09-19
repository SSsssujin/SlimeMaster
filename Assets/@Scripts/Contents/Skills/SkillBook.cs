using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBook : MonoBehaviour
{
    // 일종의 스킬 매니저
    public List<SkillBase> Skills { get; } = new List<SkillBase>();
    
    public List<SkillBase> RepeatedSkills { get; } = new List<SkillBase>();
    public List<SequenceSkill> SequenceSkills { get; } = new List<SequenceSkill>();
    
    public T AddSkill<T>(Vector3 position, Transform parent = null) where T : SkillBase
    {
        Type type = typeof(T);

        if (type == typeof(EgoSword))
        {
            var egoSword = Managers.Object.Spawn<EgoSword>(position, Define.EGO_SWORD_ID);
            egoSword.transform.SetParent(parent);
            egoSword.ActivateSkill();
            // 지금은 AddSkill 하자마자 바로 발동
            // 나중에 이부분 주석처리하고
            // SkillLevel 혹은 IsLearned ? 체크해서 발동하게 하거나 해야할듯

            Skills.Add(egoSword);
            RepeatedSkills.Add(egoSword);

            return egoSword as T;
        }
        else if (type == typeof(FireballSkill))
        {
            var fireball = Managers.Object.Spawn<FireballSkill>(position, Define.EGO_SWORD_ID);
            fireball.transform.SetParent(parent);
            fireball.ActivateSkill();

            Skills.Add(fireball);
            RepeatedSkills.Add(fireball);

            return fireball as T;
        }
        else if (type.IsSubclassOf(typeof(SequenceSkill)))
        {
            var skill = gameObject.GetOrAddComponent<T>();
            Skills.Add(skill); 
            SequenceSkills.Add(skill as SequenceSkill);

            return null;
        }

        return null;
    }

    private int _sequenceIndex = 0;
    private bool _stopped = false;

    // 등록된 시퀀스 스킬 순차적으로 실행
    public void StartNextSequenceSkill()
    {
        if (_stopped)
            return;

        if (SequenceSkills.Count == 0)
            return;
        
        SequenceSkills[_sequenceIndex].DoSkill(OnFinishedSequenceSkill);

    }

    private void OnFinishedSequenceSkill()
    {
        _sequenceIndex = (_sequenceIndex + 1) % SequenceSkills.Count;
        StartNextSequenceSkill();
    }

    public void StopSkills()
    {
        _stopped = true;

        foreach (var skill in RepeatedSkills)
        {
            skill.StopAllCoroutines();
        }
    }
}
