using System;
using UnityEngine;

public class GameManager
{
    public PlayerController Player => Managers.Object?.Player;
    
    private Vector2 _moveDir;
    
    private int _killCount;
    private int _gem = 0;

    public event Action<int> OnGemCountChanged;
    public event Action<int> OnKillCountChanged;
    public event Action<Vector2> OnMoveDirChanged;
    
    // 재화
    public int Gold { get; set; }

    public int Gem
    {
        get => _gem;
        set
        {
            _gem = value;
            OnGemCountChanged?.Invoke(value);
        }
    }

    public Vector2 MoveDir
    {
        get => _moveDir;
        set
        {
            _moveDir = value;
            OnMoveDirChanged?.Invoke(_moveDir);
        }
    }

    public int KillCount
    {
        get => _killCount;
        set
        {
            _killCount = value;
            OnKillCountChanged?.Invoke(value);
        }
    }
}
