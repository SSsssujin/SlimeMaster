using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawningPool : MonoBehaviour
{
    private float _spawnInterval = 1f;
    private int _maxMonsterCount = 100;
    private Coroutine _coUpdateSpawningPool;

    public bool Stopped { get; set; } = false;

    private void Start()
    {
        _coUpdateSpawningPool = StartCoroutine(_cUpdateSpawningPool());
    }

    IEnumerator _cUpdateSpawningPool()
    {
        while (true)
        {
            TrySpawn();
            yield return new WaitForSeconds(_spawnInterval);
        }
    }

    void TrySpawn()
    {
        if (Stopped)
            return;
        
        int monsterCount = Managers.Object.Monsters.Count;
        if (monsterCount >= _maxMonsterCount)
            return;

        Vector3 randPos = Utils.GenerateMonsterSpawnPosition(Managers.Game.Player.transform.position, 10, 15);
        MonsterController mc = Managers.Object.Spawn<MonsterController>(randPos, 1 + Random.Range(0, 2));
    }
}
