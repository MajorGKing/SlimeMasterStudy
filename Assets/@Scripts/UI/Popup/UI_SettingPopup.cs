using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_SettingPopup : UI_Popup
{
    #region Enum
    enum GameObjects
    {
        ContentObject,
        //LanguageObject
    }
    enum Buttons
    {
        BackgroundButton,
        SoundEffectOffButton,
        SoundEffectOnButton,
        BackgroundSoundOffButton,
        BackgroundSoundOnButton,
        JoystickFixedOffButton,
        JoystickFixedOnButton,
        //LanguageButton,
        //TermsOfServiceButton,
        //PrivacyPolicyButton,
    }

    enum Texts
    {
        SettingTlileText,
        //UserInfoText,
        //UseIDValueText,
        SoundEffectText,
        BackgroundSoundText,
        JoystickText,
        //LanguageText,
        //LanguageValueText,
        //TermsOfServiceButtonText,
        //PrivacyPolicyButtonText,
        VersionValueText
    }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);

        GetButton((int)Buttons.SoundEffectOffButton).gameObject.BindEvent(EffectSoundOn);
        GetButton((int)Buttons.SoundEffectOnButton).gameObject.BindEvent(EffectSoundOff);

        GetButton((int)Buttons.BackgroundSoundOffButton).gameObject.BindEvent(BackgroundSoundOn);
        GetButton((int)Buttons.BackgroundSoundOnButton).gameObject.BindEvent(BackgroundSoundOff);

        GetButton((int)Buttons.JoystickFixedOffButton).gameObject.BindEvent(OnCllickJoystickFixed);
        GetButton((int)Buttons.JoystickFixedOnButton).gameObject.BindEvent(OnCllickJoystickNonFixed);

        GetText((int)Texts.VersionValueText).text = $"버전 : {Application.version}";

        if (Managers.Game.BGMOn == false)
        {
            BackgroundSoundOff();
        }
        else
        {
            BackgroundSoundOn();
        }
        if (Managers.Game.EffectSoundOn == false)
        {
            EffectSoundOff();
        }
        else
        {
            EffectSoundOn();
        }

        if (Managers.Game.JoystickType == Define.EJoystickType.Fixed)
        {
            OnCllickJoystickFixed();
        }
        else
        {
            OnCllickJoystickNonFixed();
        }

        RefreshUI();
    }

    public void SetInfo()
    {

    }

    private void RefreshUI()
    {

    }

    private void EffectSoundOff()
    {
        Managers.Sound.PlayButtonClick();
        Managers.Game.EffectSoundOn = false;
        GetButton((int)Buttons.SoundEffectOnButton).gameObject.SetActive(false);
        GetButton((int)Buttons.SoundEffectOffButton).gameObject.SetActive(true);
    }

    private void EffectSoundOn()
    {
        Managers.Sound.PlayButtonClick();
        Managers.Game.EffectSoundOn = true;
        GetButton((int)Buttons.SoundEffectOnButton).gameObject.SetActive(true);
        GetButton((int)Buttons.SoundEffectOffButton).gameObject.SetActive(false);
    }

    private void OnCllickJoystickFixed()
    {
        Managers.Sound.PlayButtonClick();
        Managers.Game.JoystickType = Define.EJoystickType.Fixed;
        GetButton((int)Buttons.JoystickFixedOnButton).gameObject.SetActive(true);
        GetButton((int)Buttons.JoystickFixedOffButton).gameObject.SetActive(false);
    }

    private void OnCllickJoystickNonFixed()
    {
        Managers.Sound.PlayButtonClick();
        Managers.Game.JoystickType = Define.EJoystickType.Flexible;
        GetButton((int)Buttons.JoystickFixedOnButton).gameObject.SetActive(false);
        GetButton((int)Buttons.JoystickFixedOffButton).gameObject.SetActive(true);
    }

    private void BackgroundSoundOff()
    {
        Managers.Sound.PlayButtonClick();
        Managers.Game.BGMOn = false;
        GetButton((int)Buttons.BackgroundSoundOnButton).gameObject.SetActive(false);
        GetButton((int)Buttons.BackgroundSoundOffButton).gameObject.SetActive(true);
    }

    private void BackgroundSoundOn()
    {
        Managers.Sound.PlayButtonClick();
        Managers.Game.BGMOn = true;
        GetButton((int)Buttons.BackgroundSoundOnButton).gameObject.SetActive(true);
        GetButton((int)Buttons.BackgroundSoundOffButton).gameObject.SetActive(false);
    }

    private void OnClickBackgroundButton(PointerEventData evt)
    {
        Managers.UI.ClosePopupUI(this);
    }

    private void EffectSoundOn(PointerEventData evt)
    {
        EffectSoundOn();
    }

    private void EffectSoundOff(PointerEventData evt)
    {
        EffectSoundOff();
    }

    private void BackgroundSoundOn(PointerEventData evt)
    {
        BackgroundSoundOn();
    }

    private void BackgroundSoundOff(PointerEventData evt)
    {
        BackgroundSoundOff();
    }

    private void OnCllickJoystickFixed(PointerEventData evt)
    {
        OnCllickJoystickFixed();
    }

    private void OnCllickJoystickNonFixed(PointerEventData evt)
    {
        OnCllickJoystickNonFixed();
    }
}
