using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : SequenceSkill
{
    private Rigidbody2D _rb;
    private Coroutine _coroutine;

    public override void DoSkill(Action callback = null)
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        _coroutine = StartCoroutine(CoDash(callback));
    }

    private float WaitTime { get; } = 1.0f;
    private float Speed { get; } = 10.0f;
    private string AnimationName { get; } = "Charge";

    IEnumerator CoDash(Action callback = null)
    {
        
        Debug.Log("DoDash");
        
        _rb = GetComponent<Rigidbody2D>();

        yield return new WaitForSeconds(WaitTime);
        
        GetComponent<Animator>().Play(AnimationName);

        Vector3 dir = ((Vector2)Managers.Game.Player.transform.position - _rb.position).normalized;
        Vector2 targetPosition = Managers.Game.Player.transform.position + dir * UnityEngine.Random.Range(1, 5);
        // Player 위치보다 조금 더 가게
        
        while (Vector3.Distance(_rb.position, targetPosition) > 0.2f)
        {
            Vector2 dirVec = targetPosition - _rb.position;

            Vector2 nextVec = dirVec.normalized * Speed * Time.fixedDeltaTime;
            _rb.MovePosition(_rb.position + nextVec);
            
            yield return null;
        }
        
        // 끝났음을 알림
        callback?.Invoke();
    }
}
