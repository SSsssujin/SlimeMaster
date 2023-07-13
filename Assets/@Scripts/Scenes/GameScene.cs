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
        Managers.Resource.LoadAllAsync<GameObject>("Prefabs", (key, count, totalCount) =>
        {
            Debug.Log($"{key} {count}/{totalCount}");
            
            // 로드 끝나면
            if (count == totalCount)
            {
                StartLoaded();
            }
        });

    }

    void StartLoaded()
    {
        var player = Managers.Resource.Instantiate("Slime_01.prefab");
        player.AddComponent<PlayerController>();

        var snake = Managers.Resource.Instantiate("Snake_01.prefab");
        var goblin = Managers.Resource.Instantiate("Goblin_01.prefab");
        var joystick = Managers.Resource.Instantiate("UI_Joystick.prefab");
        joystick.name = "@UI_Joystick";

        var map = Managers.Resource.Instantiate("Map.prefab");
        map.name = "@Map";
        Camera.main.GetComponent<CameraController>().Target = player;
    }

    void Update()
    {
        
    }
}
