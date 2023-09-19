using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : CreatureController
{
    private class PlayerData
    {
        public int Level;
        public float Attack;
        public int MaxHp;
        public float Exp;
    }

    private PlayerData _playerData;
    
    private Vector2 _moveDir = Vector2.zero;
    private float _speed = 5.0f;

    [SerializeField]
    private Transform _indicator;
    [SerializeField]
    private Transform _fireSocket;

    private float EnvCollectDist { get; set; } = 1.0f;

    public override bool Init()
    {
        if (base.Init() == false)
            return false;

        _playerData = new PlayerData()
        {
            Level = 1,
            Attack = 10,
            Exp = 0,
            MaxHp = 100
        };

        _speed = 5.0f;
        Managers.Game.OnMoveDirChanged += HandleOnMoveDirChanged;

        Skills.AddSkill<FireballSkill>(transform.position);
        Skills.AddSkill<EgoSword>(_indicator.position);
        
        //StartProjectie();
        //StartEgoSword();

        return true;
    }

    void OnDestroy()
    {
        if (Managers.Game != null)
            Managers.Game.OnMoveDirChanged -= HandleOnMoveDirChanged;
    }

    void HandleOnMoveDirChanged(Vector2 dir)
    {
        _moveDir = dir;
    }

    void Update()
    {
        MovePlayer();
        CollectEnv();
    }

    void MovePlayer()
    {
        Vector3 dir = _moveDir * _speed * Time.deltaTime;
        transform.position += dir;

        // 인디케이터 회전
        if (_moveDir != Vector2.zero)
        {
            _indicator.eulerAngles = new Vector3(0, 0, Mathf.Atan2(-dir.x, dir.y) * 180 / Mathf.PI);
        }

        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }

    void CollectEnv()
    {
        // dir.magnitude는 루트를 씌운 값
        // 루트를 씌우는 데 비용이 많이 들어서 루트 씌우기 전의 값인
        // dir.sqrMagnitude와 EnvCollectDist 제곱의 값을 비교하면 계산 비용 ↓
        float sqrCollectDist = EnvCollectDist * EnvCollectDist;
        
        // Spawn 되어있는 모든 Gems들 가지고 오기
        List<GemController> gems = Managers.Object.Gems.ToList();

        // Managers.Object.Gems로 바로 접근하지 않고 리스트를 만들어서 순회하는 이유?
        // 바로 접근해서 Despawn 하면 문제가 생길 수 있음
        foreach (GemController gem in gems)
        {
            Vector3 dir = gem.transform.position - transform.position;
            if (dir.sqrMagnitude <= sqrCollectDist)
            {
                Managers.Game.Gem += 1;
                Managers.Object.Despawn(gem);
            }
        }

        var findGems = GameObject.Find("@Grid")
            .GetComponent<GridController>()
            .GatherObjects(transform.position, EnvCollectDist + 0.5f);
        
        //Debug.Log($"SearchGems({findGems.Count}), TotalGems({gems.Count})");
    }

    public override void OnDamaged(BaseController attacker, int damage)
    {
        base.OnDamaged(attacker, damage);
        
        //Debug.Log($"OnDamaged : {Hp}");
        
        // Temp
        CreatureController cc = attacker as CreatureController;
        cc?.OnDamaged(this, 10000);
    }

    public void KillMonster()
    {
        
    }

    void LevelUp()
    {
        _playerData.Level += 1;
        _playerData.Attack *= (_playerData.Level * 1.5f);
        _playerData.MaxHp *= (int)(_playerData.Level * 1.5f);
        _playerData.Exp *= (_playerData.Level * 1.5f);
    }

    
    public Transform Indicator => _indicator;
    public Vector3 FireSocket => _fireSocket.position;
    public Vector3 ShootDir => (_fireSocket.position - _indicator.position).normalized;

}