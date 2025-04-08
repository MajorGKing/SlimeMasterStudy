using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : BaseScene
{
    protected override void Awake()
    {
        base.Awake();

        SceneType = Define.EScene.LobbyScene;

        //TitleUI
        Managers.UI.ShowSceneUI<UI_LobbyScene>();
        Screen.sleepTimeout = SleepTimeout.SystemSetting;

        Managers.Sound.Play(Define.ESound.Bgm, "Bgm_Lobby");
    }

    public override void Clear()
    {
        
    }
}
