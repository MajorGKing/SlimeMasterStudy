using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningPool : MonoBehaviour
{
    public int _maxMonsterCount = 1000;
    Coroutine _coUpdateSpawningPool;

    public void StartSpawn()
    {
        if (_coUpdateSpawningPool == null)
            _coUpdateSpawningPool = StartCoroutine(CoUpdateSpawningPool());
    }

    IEnumerator CoUpdateSpawningPool()
    {
        //while (true)
        //{
        //    if (Managers.Game.CurrentWaveData.MonsterId.Count == 1)
        //    {
        //        for (int i = 0; i < Managers.Game.CurrentWaveData.OnceSpawnCount; i++)
        //        {
        //            Vector2 spawnPos = Utils.GenerateMonsterSpawnPosition(Managers.Game.Player.PlayerCenterPos);
        //            Managers.Object.Spawn<MonsterController>(spawnPos, _game.CurrentWaveData.MonsterId[0]);
        //        }
        //        yield return new WaitForSeconds(Managers.Game.CurrentWaveData.SpawnInterval);
        //    }
        //    else
        //    {
        //        for (int i = 0; i < Managers.Game.CurrentWaveData.OnceSpawnCount; i++)
        //        {
        //            Vector2 spawnPos = Utils.GenerateMonsterSpawnPosition(Managers.Game.Player.PlayerCenterPos);

        //            if (Random.value <= Managers.Game.CurrentWaveData.FirstMonsterSpawnRate) // 90%의 확률로 첫번째 MonsterId 사용
        //            {
        //                Managers.Object.Spawn<MonsterController>(spawnPos, Managers.Game.CurrentWaveData.MonsterId[0]);
        //            }
        //            else // 10%의 확률로 다른 MonsterId 사용
        //            {
        //                int randomIndex = Random.Range(1, Managers.Game.CurrentWaveData.MonsterId.Count);
        //                Managers.Object.Spawn<MonsterController>(spawnPos, Managers.Game.CurrentWaveData.MonsterId[randomIndex]);
        //            }
        //        }
        //        yield return new WaitForSeconds(Managers.Game.CurrentWaveData.SpawnInterval);
        //    }
        //}

        yield return null;
    }
}
