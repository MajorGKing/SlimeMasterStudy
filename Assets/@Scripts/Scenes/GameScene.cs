using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameScene : BaseScene
{
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
            //GenerateRandomExperience(30);
        }
    }
}