using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;


public class GameScene : MonoBehaviour
{
    public GameObject _joystickPrefab;

    private GameObject _snake;
    private GameObject _slime;
    private GameObject _goblin;
    
    // 다른 클래스에 있는 게 더 좋을지도?
    private int _collectedGemCount = 0;
    private int _remainingTotalGemCount = 10;
    
    private SpawningPool _spawningPool;
    private Define.StageType _stageType;

    void Start()
    {
        // 비동기 방식
        // 일처리를 맡겨놓고 다른 부분 진행
        // 끝난 후 Callback 함수 실행
        // "야 무슨 일 있으면 연락 줘!"
        // 코드 효율성 Up, 어려워짐
        Managers.Resource.LoadAllAsync<Object>("PreLoad", (key, count, totalCount) =>
        {
            //Debug.Log($"{key} {count}/{totalCount}");

            // 로드 끝나면
            if (count == totalCount)
            {
                StartLoaded();
            }
        });
        // 윗부분 리팩토링 필요.
    }

    public Define.StageType StageType
    {
        get => _stageType;
        set
        {
            _stageType = value;
            if (_spawningPool != null)
            {
                switch (value)
                {
                    case Define.StageType.Normal:
                        _spawningPool.Stopped = false;
                        break;
                    case Define.StageType.Boss:
                        _spawningPool.Stopped = true;
                        break;
                }
            }
        }
    }

    void StartLoaded()
    {
        Managers.Data.Init();
        
        Managers.UI.ShowSceneUI<UI_GameScene>();
        
        _spawningPool = gameObject.AddComponent<SpawningPool>();

        var player = Managers.Object.Spawn<PlayerController>(Vector3.zero);

        var joystick = Managers.Resource.Instantiate("UI_Joystick.prefab");
        joystick.name = "@UI_Joystick";

        var map = Managers.Resource.Instantiate("Map_01.prefab");
        map.name = "@Map";

        Camera.main.GetComponent<CameraController>().Target = player.gameObject;

        foreach (var playerData in Managers.Data.PlayerDic.Values)
        {
            //Debug.Log($"Lev : {playerData.level}, Hp : {playerData.maxHp}");
        }

        Managers.Game.OnGemCountChanged -= HandleOnGemCountChanged;
        Managers.Game.OnGemCountChanged += HandleOnGemCountChanged;

        Managers.Game.OnKillCountChanged -= HandleOnKillCountChanged;
        Managers.Game.OnKillCountChanged += HandleOnKillCountChanged;
    }
  
    public void HandleOnGemCountChanged(int gemCount)
    {
        _collectedGemCount++;

        if (_collectedGemCount == _remainingTotalGemCount)
        {
            Managers.UI.ShowPopup<UI_SkillSelectPopup>();
            _collectedGemCount = 0;
            _remainingTotalGemCount *= 2;
        }
        
        Managers.UI.GetSceneUI<UI_GameScene>()
            .SetGemCountRatio((float)_collectedGemCount / _remainingTotalGemCount);
    }

    public void HandleOnKillCountChanged(int killCount)
    {
        Managers.UI.GetSceneUI<UI_GameScene>().SetKillCount(killCount);

        if (killCount == 5)
        {
            // Boss
            StageType = Define.StageType.Boss;
            
            Managers.Object.DespawnAllMonsters();

            Vector2 spawnPos = Utils.GenerateMonsterSpawnPosition(Managers.Game.Player.transform.position, 5, 10);

            Managers.Object.Spawn<MonsterController>(spawnPos, Define.BOSS_ID);
        }
    }

    private void OnDestroy()
    {
        if (Managers.Game != null)
            Managers.Game.OnGemCountChanged -= HandleOnGemCountChanged;
    }
}