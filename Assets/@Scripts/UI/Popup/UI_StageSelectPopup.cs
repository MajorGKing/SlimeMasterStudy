using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI.Extensions;

public class UI_StageSelectPopup : UI_Popup
{
    #region Enum
    enum GameObjects
    {
        ContentObject,
        StageScrollContentObject,
        AppearingMonsterContentObject,
        StageSelectScrollView,
    }

    enum Buttons
    {
        StageSelectButton,
        BackButton,
    }

    enum Texts
    {
        StageSelectTitleText,
        AppearingMonsterText,
        StageSelectButtonText,
    }

    enum Images
    {
        LArrowImage,
        RArrowImage
    }
    #endregion

    StageData _stageData;
    HorizontalScrollSnap _scrollsnap;

    public Action OnPopupClosed;

    // TODO ILHAK
    protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));
        BindImages(typeof(Images));

        GetButton((int)Buttons.StageSelectButton).gameObject.SetActive(false);
        GetButton((int)Buttons.StageSelectButton).gameObject.BindEvent(OnClickStageSelectButton);
        GetButton((int)Buttons.StageSelectButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.BackButton).gameObject.BindEvent(OnClickBackButton);
        GetButton((int)Buttons.BackButton).GetOrAddComponent<UI_ButtonAnimation>();

        _scrollsnap = Utils.FindChild<HorizontalScrollSnap>(gameObject, recursive: true);
        _scrollsnap.OnSelectionPageChangedEvent.AddListener(OnChangeStage);
        _scrollsnap.StartingScreen = Managers.Game.CurrentStageData.StageIndex - 1;
    }

    public void SetInfo(StageData stageData)
    {
        _stageData = stageData;
        RefreshUI();
    }

    private void RefreshUI()
    {
        if (GetObject((int)GameObjects.StageScrollContentObject) == null)
            return;

        if (_stageData == null)
            return;

        #region 스테이지 리스트
        GameObject StageContainer = GetObject((int)GameObjects.StageScrollContentObject);
        StageContainer.DestroyChildren();

        _scrollsnap.ChildObjects = new GameObject[Managers.Data.StageDic.Count];

        //foreach (StageData stageData in Managers.Data.StageDic.Values)
        //{
        //    UI_StageInfoItem item = Managers.UI.MakeSubItem<UI_StageInfoItem>(StageContainer.transform);
        //    item.SetInfo(stageData);
        //    _scrollsnap.ChildObjects[stageData.StageIndex - 1] = item.gameObject;
        //}

        #endregion
    }

    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));
    }

    void OnChangeStage(int index)
    {

    }

    private void OnClickStageSelectButton(PointerEventData evt)
    {

    }

    private void OnClickBackButton(PointerEventData evt)
    {

    }

    private void OnChangeStage(PointerEventData evt)
    {

    }
}
