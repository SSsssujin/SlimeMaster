using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers s_instance;
    private static bool s_init = false;

    // Contents
    private GameManager _game = new GameManager();
    private ObjectManager _object = new ObjectManager();
    private PoolManager _pool = new PoolManager();
    
    // Core
    private DataManager _data = new DataManager();
    private ResourceManager _resource = new ResourceManager();
    private SceneManagerEx _scene = new SceneManagerEx();
    private SoundManager _sound = new SoundManager();
    private UIManager _ui = new UIManager();

    public static Managers Instance
    {
        get
        {
            if (s_init == false)
            {
                GameObject go = GameObject.Find("@Managers");
                if (go == null)
                {
                    go = new GameObject() { name = "@Managers" };
                    go.AddComponent<Managers>();
                }
                
                DontDestroyOnLoad(go);
                s_instance = go.GetComponent<Managers>();
            }

            return s_instance;
        }
    }
    
    public static GameManager Game => Instance?._game;
    public static ObjectManager Object => Instance?._object;
    public static PoolManager Pool => Instance?._pool;
    
    public static DataManager Data => Instance?._data;
    public static ResourceManager Resource => Instance?._resource;
    public static SceneManagerEx Scene => Instance?._scene;
    public static SoundManager Sound => Instance?._sound;
    public static UIManager UI => Instance?._ui;
}
