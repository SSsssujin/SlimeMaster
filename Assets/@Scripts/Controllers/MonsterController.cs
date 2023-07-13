using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterContorller : CreatureController
{
    public override bool Init()
    {
        if (base.Init())
            return false;
        
        ObjectType = Define.ObjectType.Monster;

        return true;
    }
}
