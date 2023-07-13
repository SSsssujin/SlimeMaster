using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class BaseController : MonoBehaviour
{
    public ObjectType ObjectType { get; protected set; }
    
    private void Awake()
    {
        Init();
    }

    private bool _init = false;
    
    // 자식 오브젝트에서 얘를 상속받아서 초기화를 진행
    // 부모클래스 Awake에서 실행, 자식이 상속받음 -> Start 메소드를 매번 호출하지 않아도 됨
    public virtual bool Init()
    {
        if (_init)
            return false;
        
        return true;
    }

    void Update()
    {
        
    }
}
