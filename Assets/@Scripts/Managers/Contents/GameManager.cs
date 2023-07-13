using System;
using UnityEngine;

public class GameManager
{
    private Vector2 _moveDir;

    public event Action<Vector2> OnMoveDirChanged;
    
    public Vector2 MoveDir
    {
        get => _moveDir;
        set
        {
            _moveDir = value;
            OnMoveDirChanged?.Invoke(_moveDir);
        }
    }
}
