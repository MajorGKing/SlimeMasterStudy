using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_GameResultPopup : UI_Popup
{
    enum GameObjects
    {
        ContentObject,
        ResultRewardScrollContentObject,
        ResultGoldObject,
        ResultKillObject,
    }

    enum Texts
    {
        GameResultPopupTitleText,
        ResultStageValueText,
        ResultSurvivalTimeText,
        ResultSurvivalTimeValueText,
        ResultKillValueText,
        ConfirmButtonText,
        ResultGoldValueText
    }

    enum Buttons
    {
        StatisticsButton,
        ConfirmButton,
    }

    protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindTexts(typeof(Texts));
        BindButtons(typeof(Buttons));

        GetButton((int)Buttons.StatisticsButton).gameObject.BindEvent(OnClickStatisticsButton);
        GetButton((int)Buttons.StatisticsButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.ConfirmButton).gameObject.BindEvent(OnClickConfirmButton);
        GetButton((int)Buttons.ConfirmButton).GetOrAddComponent<UI_ButtonAnimation>();

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
        // ResultStageValueText : 해당 스테이지 수
        GetText((int)Texts.ResultStageValueText).text = $"{Managers.Game.CurrentStageData.StageIndex} STAGE";
        // ResultKillValueText : 죽기전 까지 킬 수
        GetText((int)Texts.ResultKillValueText).text = $"{Managers.Game.Player.KillCount}";
        GetText((int)Texts.ResultGoldValueText).text = $"{Managers.Game.CurrentStageData.ClearReward_Gold}";


        Managers.Game.Gold += Managers.Game.CurrentStageData.ClearReward_Gold;
        Managers.Game.ExchangeMaterial(Managers.Data.MaterialDic[Define.ID_RANDOM_SCROLL], Managers.Game.CurrentStageData.ClearReward_Gold);

        Transform container = GetObject((int)GameObjects.ResultRewardScrollContentObject).transform;
        container.gameObject.DestroyChildren();

        UI_MaterialItem item = Managers.UI.MakeSubItem<UI_MaterialItem>(container);
        item.SetInfo(Managers.Data.MaterialDic[Define.ID_RANDOM_SCROLL].SpriteName, Managers.Game.CurrentStageData.ClearReward_Gold);

        // 리프레시 버그 대응
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.ResultGoldObject).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.ResultKillObject).GetComponent<RectTransform>());
    }

    private void OnClickStatisticsButton(PointerEventData evt)
    {
        Managers.Sound.PlayButtonClick();
    }

    private void OnClickConfirmButton(PointerEventData evt)
    {
        Managers.Sound.PlayButtonClick();
        StageClearInfo info;
        if (Managers.Game.DicStageClearInfo.TryGetValue(Managers.Game.CurrentStageData.StageIndex, out info))
        {
            info.MaxWaveIndex = Managers.Game.CurrentWaveIndex;
            info.isClear = true;
            Managers.Game.DicStageClearInfo[Managers.Game.CurrentStageData.StageIndex] = info;
        }
        Managers.Game.ClearContinueData();
        Managers.Game.SetNextStage();
        Managers.Scene.LoadScene(Define.EScene.LobbyScene, transform);
    }
}
