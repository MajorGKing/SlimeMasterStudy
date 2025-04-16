using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_AchievementPopup : UI_Popup
{
    #region Enum
    enum GameObjects
    {
        ContentObject,
        AchievementScrollObject,
    }
    enum Buttons
    {
        BackgroundButton,
    }
    enum Texts
    {
        BackgroundText,
        AchievementTitleText,
    }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);

        RefreshUI();
    }

    public void SetInfo()
    {

    }

    private void RefreshUI()
    {
        GetObject((int)GameObjects.AchievementScrollObject).DestroyChildren();

        // TODO ILHAK after Achievement Manager

        //foreach (AchievementData data in Managers.Achievement.GetProceedingAchievment())
        //{
        //    UI_AchievementItem item = Managers.UI.MakeSubItem<UI_AchievementItem>(GetObject((int)GameObjects.AchievementScrollObject).transform);
        //    item.SetInfo(data);
        //}
    }

    private void OnClickBackgroundButton(PointerEventData evt)
    {
        Managers.UI.ClosePopupUI(this);
    }
}
