using UnityEngine;
using Data;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using UnityEditor;

public class UI_MissionItem : UI_SubItem
{
    #region Enum
    enum GameObjects
    {
        ProgressSliderObject,
    }

    enum Buttons
    {
        GetButton,
    }

    enum Texts
    {
        RewardItemValueText,
        ProgressText,
        CompleteText,
        MissionNameValueText,
        MissionProgressValueText,
    }

    enum Images
    {
        RewardItmeIconImage,
    }
    #endregion

    enum EMissionState
    {
        Progress,
        Complete,
        Rewarded,
    }

    MissionData _missionData;

    protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));
        BindImages(typeof(Images));

        GetButton((int)Buttons.GetButton).gameObject.BindEvent(OnClickGetButton);
        GetButton((int)Buttons.GetButton).GetOrAddComponent<UI_ButtonAnimation>();

        AchievementContentInit();

        RefreshUI();
    }

    public void SetInfo(MissionData missionData)
    {
        transform.localScale = Vector3.one;
        _missionData = missionData;

        RefreshUI();
    }

    private void RefreshUI()
    {
        if (GetText((int)Texts.RewardItemValueText) == null)
            return;

        if (_missionData == null)
            return;

        GetText((int)Texts.RewardItemValueText).text = $"{_missionData.RewardValue}";
        GetText((int)Texts.MissionNameValueText).text = $"{_missionData.DescriptionTextID}";
        GetObject((int)GameObjects.ProgressSliderObject).GetComponent<Slider>().value = 0;

        if (Managers.Game.DicMission.TryGetValue(_missionData.MissionTarget, out MissionInfo missionInfo))
        {
            if (missionInfo.Progress > 0)
                GetObject((int)GameObjects.ProgressSliderObject).GetComponent<Slider>().value = (float)missionInfo.Progress / _missionData.MissionTargetValue;

            if (missionInfo.Progress >= _missionData.MissionTargetValue)
            {
                SetButtonUI(EMissionState.Complete);
                if (missionInfo.IsRewarded == true)
                    SetButtonUI(EMissionState.Rewarded);
            }
            else
            {
                SetButtonUI(EMissionState.Progress);
            }
            GetText((int)Texts.MissionProgressValueText).text = $"{missionInfo.Progress}/{_missionData.MissionTargetValue}";
        }
        string sprName = Managers.Data.MaterialDic[_missionData.ClearRewardItmeId].SpriteName;
        GetImage((int)Images.RewardItmeIconImage).sprite = Managers.Resource.Load<Sprite>(sprName);
    }

    private void SetButtonUI(EMissionState state)
    {
        GameObject objComplte = GetButton((int)Buttons.GetButton).gameObject;
        GameObject objProgress = GetText((int)Texts.ProgressText).gameObject;
        GameObject objRewarded = GetText((int)Texts.CompleteText).gameObject;

        switch (state)
        {
            case EMissionState.Rewarded:
                objRewarded.SetActive(true);
                objComplte.SetActive(false);
                objProgress.SetActive(false);
                break;
            case EMissionState.Complete:
                objRewarded.SetActive(false);
                objComplte.SetActive(true);
                objProgress.SetActive(false);
                break;
            case EMissionState.Progress:
                objRewarded.SetActive(false);
                objComplte.SetActive(false);
                objProgress.SetActive(true);
                GetText((int)Texts.ProgressText).text = $"진행중";

                break;
        }
    }

    private void AchievementContentInit()
    {
        GetButton((int)Buttons.GetButton).gameObject.SetActive(true); // 임시로 활성화
        GetText((int)Texts.ProgressText).gameObject.SetActive(false);
        GetText((int)Texts.CompleteText).gameObject.SetActive(false);
    }

    // 보상 받기 버튼
    private void OnClickGetButton(PointerEventData evt)
    {
        Managers.Sound.PlayButtonClick();
        string[] spriteName = new string[1];
        int[] count = new int[1];

        spriteName[0] = Managers.Data.MaterialDic[Define.ID_DIA].SpriteName;
        count[0] = _missionData.RewardValue;

        UI_RewardPopup rewardPopup = (Managers.UI.SceneUI as UI_LobbyScene).RewardPopupUI;
        rewardPopup.gameObject.SetActive(true);
        Managers.Game.Dia += _missionData.RewardValue;
        if (Managers.Game.DicMission.TryGetValue(_missionData.MissionTarget, out MissionInfo info))
        {
            info.IsRewarded = true;
        }
        RefreshUI();

        rewardPopup.SetInfo(spriteName, count);
    }
}
