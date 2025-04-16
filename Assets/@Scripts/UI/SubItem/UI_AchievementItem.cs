using Data;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_AchievementItem : UI_SubItem
{
    #region Enum
    enum GameObjects
    {
        ProgressSlider,
    }

    enum Buttons
    {
        GetButton,
        //GoNowButton,
    }

    enum Texts
    {
        RewardItmeValueText,
        CompleteText,
        AchievementNameValueText,
        AchievementValueText,
        ProgressText
    }

    enum Images
    {
        RewardItmeIcon,
    }

    enum MissionState
    {
        Progress,
        Complete,
        Rewarded,
    }
    #endregion

    AchievementData _achievementData;

    protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));
        BindImages(typeof(Images));

        // TODO ILHAK After Achievement Manager

        //GetButton((int)Buttons.GetButton).gameObject.BindEvent(OnClickGetButton);
        //GetButton((int)Buttons.GetButton).GetOrAddComponent<UI_ButtonAnimation>();
        ////GetButton((int)Buttons.GoNowButton).gameObject.BindEvent(OnClickGoNowButton);
        ////GetButton((int)Buttons.GoNowButton).GetOrAddComponent<UI_ButtonAnimation>();
        //AchievementContentInit();
    }

    public void SetInfo(AchievementData achievementData)
    {
        transform.localScale = Vector3.one;
        _achievementData = achievementData;
        RefreshUI();
    }

    private void RefreshUI()
    {
        if (GetText((int)Texts.RewardItmeValueText) == null)
            return;

        GetText((int)Texts.RewardItmeValueText).text = $"{_achievementData.RewardValue}";
        GetText((int)Texts.AchievementNameValueText).text = $"{_achievementData.DescriptionTextID}";
        GetObject((int)GameObjects.ProgressSlider).GetComponent<Slider>().value = 0;

        //int progress = Managers.Achievement.GetProgressValue(_achievementData.MissionTarget);
    }
}
