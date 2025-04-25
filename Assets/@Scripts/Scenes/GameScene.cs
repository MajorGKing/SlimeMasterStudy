using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameScene : BaseScene
{
    enum eDropType
    {
        Potion,
        Magnet,
        Bomb
    }

    public GameObject Weapon;

    private int _lastSecond = 30;
    bool isGameEnd = false;
    SpawningPool _spawningPool;
    PlayerController _player;
    GameManager _game;

    #region Action
    public Action<int> OnWaveStart;
    public Action<int> OnSecondChange;
    public Action OnWaveEnd;
    #endregion
    UI_GameScene _ui;
    //BossController _boss;

    protected override void Awake()
    {
        base.Awake();

#if UNITY_EDITOR
        gameObject.AddComponent<CaptureScreenShot>();
#endif

        Debug.Log("@>> GameScene Init()");
        SceneType = Define.EScene.GameScene;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        _game = Managers.Game;
        Managers.UI.ShowSceneUI<UI_Joystick>();

        if (_game.ContinueInfo.isContinue == true)
        {
            _player = Managers.Object.Spawn<PlayerController>(Vector3.zero, _game.ContinueInfo.PlayerDataId);
        }
        else
        {
            _game.ClearContinueData();
            _player = Managers.Object.Spawn<PlayerController>(Vector3.zero, 201000);
        }

        LoadStage();

        _player.OnPlayerDead = OnPlayerDead;
    }

    public override void Clear()
    {
    }

    public void LoadStage()
    {
        if (_spawningPool == null)
            _spawningPool = gameObject.AddComponent<SpawningPool>();

        Managers.Object.LoadMap(_game.CurrentStageData.MapName);

        // 웨이브 정보 적용
        StopAllCoroutines();
        StartCoroutine(StartWave(_game.CurrentStageData.WaveArray[_game.CurrentWaveIndex]));

    }

    IEnumerator StartWave(WaveData wave)
    {
        yield return new WaitForEndOfFrame();
        OnWaveStart?.Invoke(wave.WaveIndex);

        if (wave.WaveIndex == 1)
        {
            GenerateRandomExperience(30);
        }

        SpawnWaveReward();
    }

    public void GenerateRandomExperience(int n)
    {
        int[] coins = new int[] { 1, 2, 5, 10 };
        List<Define.EGemType> combination = new List<Define.EGemType>();

        int remainingValue = n;

        while (remainingValue > 0)
        {
            int coinIndex = UnityEngine.Random.Range(0, coins.Length);
            int coinValue = coins[coinIndex];

            if (remainingValue >= coinValue)
            {
                Define.EGemType gemType = (Define.EGemType)coinIndex;
                combination.Add(gemType);
                remainingValue -= coinValue;
            }
        }

        foreach (Define.EGemType type in combination)
        {
            GemController gem = Managers.Object.Spawn<GemController>(Utils.RandomPointInAnnulus(Managers.Game.Player.CenterPosition, 6, 10));
            gem.SetInfo(Managers.Game.GetGemInfo(type));
        }
    }


    void SpawnWaveReward()
    {
        eDropType spawnType = (eDropType)UnityEngine.Random.Range(0, 3);

        Vector3 spawnPos = Utils.RandomPointInAnnulus(Managers.Game.Player.CenterPosition, 3, 6);
        Data.DropItemData dropItem;
        switch (spawnType)
        {
            case eDropType.Potion:
                if (Managers.Data.DropItemDataDic.TryGetValue(Define.ID_POTION, out dropItem) == true)
                    Managers.Object.Spawn<PotionController>(spawnPos).SetInfo(dropItem);
                break;
            case eDropType.Magnet:
                if (Managers.Data.DropItemDataDic.TryGetValue(Define.ID_MAGNET, out dropItem) == true)
                    Managers.Object.Spawn<MagnetController>(spawnPos).SetInfo(dropItem);
                break;
            case eDropType.Bomb:
                if (Managers.Data.DropItemDataDic.TryGetValue(Define.ID_BOMB, out dropItem) == true)
                    Managers.Object.Spawn<BombController>(spawnPos).SetInfo(dropItem);
                break;
        }
    }
}