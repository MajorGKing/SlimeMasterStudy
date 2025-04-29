using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_BackToHomePopup : UI_Popup
{
    enum GameObjects
    {
        ContentObject,
    }
    enum Buttons
    {
        ResumeButton,
        QuitButton,
    }

    enum Texts
    {
        BackToHomeTitleText,
        BackToHameContentText,
        ResumeText,
        QuitText,
    }

    protected override void Awake()
    {
        base.Awake();
        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        GetButton((int)Buttons.ResumeButton).gameObject.BindEvent(OnClickResumeButton);
        GetButton((int)Buttons.ResumeButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.QuitButton).gameObject.BindEvent(OnClickQuitButton);
        GetButton((int)Buttons.QuitButton).GetOrAddComponent<UI_ButtonAnimation>();

        RefreshUI();
    }

    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));
    }

    public void SetInfo()
    {
        RefreshUI();
    }

    private void RefreshUI()
    {

    }

    private void OnClickResumeButton(PointerEventData evt)
    {
        Managers.UI.ClosePopupUI(this);
    }

    void OnClickQuitButton(PointerEventData evt)
    {
        Managers.Sound.PlayButtonClick();

        Managers.Game.IsGameEnd = true;
        Managers.Game.Player.StopAllCoroutines();

        StageClearInfo info;
        if (Managers.Game.DicStageClearInfo.TryGetValue(Managers.Game.CurrentStageData.StageIndex, out info))
        {
            // 기록 갱신
            if (Managers.Game.CurrentWaveIndex > info.MaxWaveIndex)
            {
                info.MaxWaveIndex = Managers.Game.CurrentWaveIndex;
                Managers.Game.DicStageClearInfo[Managers.Game.CurrentStageData.StageIndex] = info;
            }
        }

        Managers.Game.ClearContinueData();
        Managers.Scene.LoadScene(Define.EScene.LobbyScene, transform);
    }
}
