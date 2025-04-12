using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class UI_MissionPopup : UI_Popup
{
    #region Enum
    enum GameObjects
    {
        ContentObject,
        DailyMissionContentObject,
        DailyMissionScrollObject,
    }
    enum Buttons
    {
        BackgroundButton,
    }

    enum Texts
    {
        BackgroundText,
        DailyMissionTitleText,
        DailyMissionCommentText,
    }

    enum Images
    {
        GradientImage,
    }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));
        BindImages(typeof(Images));

        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);

        RefreshUI();
    }

    public void SetInfo()
    {
        
    }

    private void RefreshUI()
    {
        GetObject((int)GameObjects.DailyMissionScrollObject).DestroyChildren();
        foreach (KeyValuePair<int, MissionData> data in Managers.Data.MissionDataDic)
        {
            if (data.Value.MissionType == Define.EMissionType.Daily)
            {
                UI_MissionItem dailyMission = Managers.UI.MakeSubItem<UI_MissionItem>(GetObject((int)GameObjects.DailyMissionScrollObject).transform);
                dailyMission.SetInfo(data.Value);
            }
        }
    }

    private void OnClickBackgroundButton(PointerEventData evt)
    {
        Managers.UI.ClosePopupUI(this);
    }

}
