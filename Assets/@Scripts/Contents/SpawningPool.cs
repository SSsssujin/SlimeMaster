using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawningPool : MonoBehaviour
{
    private float _spawnInterval = 0.01f;
    private int _maxMonsterCount = 100;
    private Coroutine _coUpdateSpawningPool;

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
        int monsterCount = Managers.Object.Monsters.Count;
        if (monsterCount >= _maxMonsterCount)
            return;

        Vector3 randPos = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
        MonsterController mc = Managers.Object.Spawn<MonsterController>(randPos, Random.Range(0, 2));
    }
}
