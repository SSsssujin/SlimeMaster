using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameScene : MonoBehaviour
{
    public GameObject _joystickPrefab;

    private GameObject _snake;
    private GameObject _slime;
    private GameObject _goblin;

    void Start()
    {
        // 비동기 방식
        // 일처리를 맡겨놓고 다른 부분 진행
        // 끝난 후 Callback 함수 실행
        // "야 무슨 일 있으면 연락 줘!"
        // 코드 효율성 Up, 어려워짐
        Managers.Resource.LoadAllAsync<Object>("PreLoad", (key, count, totalCount) =>
        {
            Debug.Log($"{key} {count}/{totalCount}");

            // 로드 끝나면
            if (count == totalCount)
            {
                StartLoaded();
            }
        });
        // 윗부분 리팩토링 필요.
    }

    private SpawningPool _spawningPool;

    void StartLoaded()
    {
        _spawningPool = gameObject.AddComponent<SpawningPool>();

        var player = Managers.Object.Spawn<PlayerController>(Vector3.zero);

        //for (int i = 0; i < 10; i++)
        //{
        //    MonsterController mc = Managers.Object.Spawn<MonsterController>(Random.Range(0, 2));
        //    mc.transform.position = new Vector2(Random.Range(-50, 50), Random.Range(-50, 50));
        //}

        var joystick = Managers.Resource.Instantiate("UI_Joystick.prefab");
        joystick.name = "@UI_Joystick";

        var map = Managers.Resource.Instantiate("Map.prefab");
        map.name = "@Map";

        Camera.main.GetComponent<CameraController>().Target = player.gameObject;
        
        // Data Test
        Managers.Data.Init();

        foreach (var playerData in Managers.Data.PlayerDic.Values)
        {
            Debug.Log($"Lev : {playerData.level}, Hp : {playerData.maxHp}");
        }
    }

    void Update()
    {
    }
}