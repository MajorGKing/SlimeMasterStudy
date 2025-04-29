using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_PausePopup : UI_Popup
{
    enum GameObjects
    {
        ContentObject,
    }
    enum Buttons
    {
        ResumeButton,
        HomeButton,
        StatisticsButton,
        SoundButton,
        SettingButton,
    }


    enum Texts
    {
        PauseTitleText,
        ResumeButtonText
    }

    SkillBase skill;

    protected override void Awake()
    {
        base.Awake();
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));
        BindObjects(typeof(GameObjects));

        GetButton((int)Buttons.HomeButton).gameObject.BindEvent(OnClickHomeButton);
        GetButton((int)Buttons.HomeButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.ResumeButton).gameObject.BindEvent(OnClickResumeButton);
        GetButton((int)Buttons.ResumeButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.SettingButton).gameObject.BindEvent(OnClickSettingButton);
        GetButton((int)Buttons.SettingButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.SoundButton).gameObject.BindEvent(OnClickSoundButton);
        GetButton((int)Buttons.SoundButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.StatisticsButton).gameObject.BindEvent(OnClickStatisticsButton);
        GetButton((int)Buttons.StatisticsButton).GetOrAddComponent<UI_ButtonAnimation>();
    }

    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));
    }

    public void SetInfo()
    {

    }

    private void RefreshUI()
    {

    }

    void OnClickResumeButton(PointerEventData evt) // 되돌아가기 버튼
    {
        Managers.UI.ClosePopupUI(this);
    }

    void OnClickHomeButton(PointerEventData evt) // 로비 버튼
    {
        Managers.Sound.PlayButtonClick();
        Managers.UI.ShowPopupUI<UI_BackToHomePopup>();
    }

    void OnClickSettingButton(PointerEventData evt) // 설정 버튼
    {
        Managers.Sound.PlayButtonClick();
        Managers.UI.ShowPopupUI<UI_SettingPopup>();
    }

    void OnClickSoundButton(PointerEventData evt)
    {
        Managers.Sound.PlayButtonClick();
    }

    void OnClickStatisticsButton(PointerEventData evt)
    {
        Managers.Sound.PlayButtonClick();
        Managers.UI.ShowPopupUI<UI_TotalDamagePopup>().SetInfo();
    }
}