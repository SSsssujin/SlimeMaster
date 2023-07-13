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
        GameObject go = new GameObject() { name = "@Monsters" };

        //_slime.AddComponent<PlayerController>();

        Camera.main.GetComponent<CameraController>().Target = _slime;
    }

    void Update()
    {
        
    }
}
