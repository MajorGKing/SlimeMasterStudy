using Data;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_BattlePopup : UI_Popup
{
    #region Enum
    enum GameObjects
    {
        ContentObject,
        SettingButtonRedDotObject,
        //BattlepassButtonRedDotObject,
        //AccountPassButtonRedDotObject,
        MissionButtonRedDotObject,
        AchievementButtonRedDotObject,
        AttendanceCheckButtonRedDotObject,
        //BagIconImageRedDotObject,
        //EventCenterButtonRedDotObject,
        OfflineRewardButtonRedDotObject,
        GameStartCostGroupObject, // 리프레시
        SurvivalTimeObject, // 리프레시
        StageRewardProgressFillArea,
        StageRewardProgressSliderObject,
        FirstClearRewardUnlockObject,
        SecondClearRewardUnlockObject,
        ThirdClearRewardUnlockObject,
        FirstClearRedDotObject,
        SecondClearRedDotObject,
        ThirdClearRedDotObject,
        FirstClearRewardCompleteObject,
        SecondClearRewardCompleteObject,
        ThirdClearRewardCompleteObject,
    }

    enum Buttons
    {
        SettingButton,
        //PaymentRewardButton,
        //AccountPassButton,
        MissionButton,
        AchievementButton,
        AttendanceCheckButton,
        StageSelectButton,
        OfflineRewardButton,
        GameStartButton,

        FirstClearRewardButton,
        SecondClearRewardButton,
        ThirdClearRewardButton,
    }

    enum Texts
    {
        StageNameText,
        SurvivalWaveText,
        SurvivalWaveValueText,
        GameStartButtonText,
        GameStartCostValueText,
        OfflineRewardText,

        //PaymentRewardTextText,
        //AccountPassText,
        SettingButtonText,
        MissionButtonText,
        AchievementButtonText,
        AttendanceCheckButtonText,

        FirstClearRewardText,
        SecondClearRewardText,
        ThirdClearRewardText,
    }

    enum Images
    {
        StageImage,
        //StageRewardIconImage, // 챕터 보상 상자

        FirstClearRewardItemImage,
        SecondClearRewardItemImage,
        ThirdClearRewardItemImage,
    }
    #endregion

    enum ERewardBoxState
    {
        Lock,
        Unlock,
        Complete,
        RedDot
    }

    Data.StageData _currentStageData;

    class RewardBox
    {
        public GameObject ItemImage;
        public GameObject UnLockObject;
        public GameObject CompleteObject;
        public GameObject RedDotObject;
        public ERewardBoxState state;
    }

    List<RewardBox> _boxes = new List<RewardBox>();

    protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));
        BindImages(typeof(Images));

        // 버튼 레드닷 (초기상태 비활성화)
        GetObject((int)GameObjects.SettingButtonRedDotObject).SetActive(false);
        //GetObject((int)GameObjects.AccountPassButtonRedDotObject).SetActive(false);
        GetObject((int)GameObjects.MissionButtonRedDotObject).SetActive(false);
        GetObject((int)GameObjects.AchievementButtonRedDotObject).SetActive(false);
        GetObject((int)GameObjects.AttendanceCheckButtonRedDotObject).SetActive(false);
        GetObject((int)GameObjects.OfflineRewardButtonRedDotObject).SetActive(false);

        // 버튼 기능 
        GetButton((int)Buttons.GameStartButton).gameObject.BindEvent(OnClickGameStartButton);
        GetButton((int)Buttons.GameStartButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.StageSelectButton).gameObject.BindEvent(OnClickStageSelectButton);
        GetButton((int)Buttons.StageSelectButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.OfflineRewardButton).gameObject.BindEvent(OnClickOfflineRewardButton);
        GetButton((int)Buttons.OfflineRewardButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.SettingButton).gameObject.BindEvent(OnClickSettingButton);
        GetButton((int)Buttons.SettingButton).GetOrAddComponent<UI_ButtonAnimation>();
        //GetButton((int)Buttons.PaymentRewardButton).gameObject.BindEvent(OnClickPaymentRewardButton);
        //GetButton((int)Buttons.PaymentRewardButton).GetOrAddComponent<UI_ButtonAnimation>();
        //GetButton((int)Buttons.AccountPassButton).gameObject.BindEvent(OnClickAccountPassButton);
        //GetButton((int)Buttons.AccountPassButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.MissionButton).gameObject.BindEvent(OnClickMissionButton);
        GetButton((int)Buttons.MissionButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.AchievementButton).gameObject.BindEvent(OnClickAchievementButton);
        GetButton((int)Buttons.AchievementButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.AttendanceCheckButton).gameObject.BindEvent(OnClickAttendanceCheckButton);
        GetButton((int)Buttons.AttendanceCheckButton).GetOrAddComponent<UI_ButtonAnimation>();

        // 생존 웨이브
        GetText((int)Texts.SurvivalWaveText).gameObject.SetActive(false);
        GetText((int)Texts.SurvivalWaveValueText).gameObject.SetActive(false);

        // 스테이지 보상
        GetButton((int)Buttons.FirstClearRewardButton).gameObject.BindEvent(OnClickFirstClearRewardButton);
        GetButton((int)Buttons.FirstClearRewardButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.SecondClearRewardButton).gameObject.BindEvent(OnClickSecondClearRewardButton);
        GetButton((int)Buttons.SecondClearRewardButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.ThirdClearRewardButton).gameObject.BindEvent(OnClickThirdClearRewardButton);
        GetButton((int)Buttons.ThirdClearRewardButton).GetOrAddComponent<UI_ButtonAnimation>();

        InitBoxes();

        RefreshUI();
    }

    public void SetInfo()
    {

    }

    private void RefreshUI()
    {
        if (Managers.Game.CurrentStageData == null)
        {
            Managers.Game.CurrentStageData = Managers.Data.StageDic[1];
        }

        // StageNameText : 마지막 도전한 스테이지 표시
        GetText((int)Texts.StageNameText).text = Managers.Game.CurrentStageData.StageName;
        
        // SurvivalWaveValueText : 해당 스테이지에서 도달했던 맥스 웨이브 수 (스테이지 클리어 시 처리 고민 필요)
        if (Managers.Game.DicStageClearInfo.TryGetValue(Managers.Game.CurrentStageData.StageIndex, out StageClearInfo info))
        {
            if (info.MaxWaveIndex == 0)
            {
                //GetObject((int)GameObjects.StageRewardProgressFillArea).GetComponent<Slider>().value = 0;
                GetText((int)Texts.SurvivalWaveValueText).text = "기록 없음";
            }
            else
                GetText((int)Texts.SurvivalWaveValueText).text = (info.MaxWaveIndex + 1).ToString();
        }
        else
            GetText((int)Texts.SurvivalWaveValueText).text = "기록 없음";

        // StageImage : 마지막 도전한 스테이지의 이미지
        GetImage((int)Images.StageImage).sprite = Managers.Resource.Load<Sprite>(Managers.Game.CurrentStageData.StageImage);

        // 스테이지 보상 ( 클리어 조건에 따라 상태 변화 필요)
        if (info != null)
        {
            _currentStageData = Managers.Game.CurrentStageData;
            int itemCode = _currentStageData.FirstWaveClearRewardItemId;

            //박스
            InitBoxes();
            SetRewardBoxes(info);
            GetText((int)Texts.FirstClearRewardText).text = $"{_currentStageData.FirstWaveCountValue}";
            GetText((int)Texts.SecondClearRewardText).text = $"{_currentStageData.SecondWaveCountValue}";
            GetText((int)Texts.ThirdClearRewardText).text = $"{_currentStageData.ThirdWaveCountValue}";

            #region 생존 웨이브 
            //슬라이더
            int wave = info.MaxWaveIndex;

            if (info.isClear == true)
            {
                GetText((int)Texts.SurvivalWaveText).gameObject.SetActive(false);
                GetText((int)Texts.SurvivalWaveValueText).gameObject.SetActive(true);
                GetText((int)Texts.SurvivalWaveValueText).color = Utils.HexToColor("60FF08");
                GetText((int)Texts.SurvivalWaveValueText).text = "스테이지 클리어";
                GetObject((int)GameObjects.StageRewardProgressFillArea).GetComponent<Slider>().value = wave + 1;
            }
            else
            {
                // 처음 접속
                if (info.MaxWaveIndex == 0)
                {
                    GetText((int)Texts.SurvivalWaveText).gameObject.SetActive(false);
                    GetText((int)Texts.SurvivalWaveValueText).gameObject.SetActive(true);
                    GetText((int)Texts.SurvivalWaveValueText).color = Utils.HexToColor("FFDB08");
                    GetText((int)Texts.SurvivalWaveValueText).text = "기록 없음";
                    GetObject((int)GameObjects.StageRewardProgressFillArea).GetComponent<Slider>().value = wave;
                }
                // 진행중
                else
                {
                    GetText((int)Texts.SurvivalWaveText).gameObject.SetActive(true);
                    GetText((int)Texts.SurvivalWaveValueText).gameObject.SetActive(true);
                    GetText((int)Texts.SurvivalWaveValueText).color = Utils.HexToColor("FFDB08");
                    GetText((int)Texts.SurvivalWaveValueText).text = (info.MaxWaveIndex + 1).ToString();
                    GetObject((int)GameObjects.StageRewardProgressFillArea).GetComponent<Slider>().value = wave + 1;
                }

                //// 새로운 스테이지
                //if (Managers.Game.DicStageClearInfo.TryGetValue(_currentStageData.StageIndex - 1, out StageClearInfo PrevInfo) == false)
                //    return;
                //if (PrevInfo.isClear == true)
                //{
                //    GetText((int)Texts.SurvivalWaveText).gameObject.SetActive(false);
                //    GetText((int)Texts.SurvivalWaveValueText).gameObject.SetActive(true);
                //    GetText((int)Texts.SurvivalWaveValueText).text = "기록 없음";
                //    GetObject((int)GameObjects.StageRewardProgressFillArea).GetComponent<Slider>().value = wave;
                //}
            }
            #endregion
        }

        // 리프레시 버그 대응
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.GameStartCostGroupObject).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.SurvivalTimeObject).GetComponent<RectTransform>());

    }

    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));
        StartCoroutine(CoCheckContinue());
    }

    void InitBoxes()
    {
        #region init
        RewardBox box = new RewardBox
        {
            ItemImage = GetImage((int)Images.FirstClearRewardItemImage).gameObject,
            UnLockObject = GetObject((int)GameObjects.FirstClearRewardUnlockObject).gameObject,
            CompleteObject = GetObject((int)GameObjects.FirstClearRewardCompleteObject).gameObject,
            RedDotObject = GetObject((int)GameObjects.FirstClearRedDotObject).gameObject,
        };
        _boxes.Add(box);

        RewardBox box2 = new RewardBox
        {
            ItemImage = GetImage((int)Images.SecondClearRewardItemImage).gameObject,
            UnLockObject = GetObject((int)GameObjects.SecondClearRewardUnlockObject).gameObject,
            CompleteObject = GetObject((int)GameObjects.SecondClearRewardCompleteObject).gameObject,
            RedDotObject = GetObject((int)GameObjects.SecondClearRedDotObject).gameObject,
        };
        _boxes.Add(box2);

        RewardBox box3 = new RewardBox
        {
            ItemImage = GetImage((int)Images.ThirdClearRewardItemImage).gameObject,
            UnLockObject = GetObject((int)GameObjects.ThirdClearRewardUnlockObject).gameObject,
            CompleteObject = GetObject((int)GameObjects.ThirdClearRewardCompleteObject).gameObject,
            RedDotObject = GetObject((int)GameObjects.ThirdClearRedDotObject).gameObject,
        };
        _boxes.Add(box3);

        #endregion
        for (int i = 0; i < _boxes.Count; i++)
        {
            _boxes[i].UnLockObject.SetActive(true);
            _boxes[i].CompleteObject.SetActive(false);
            _boxes[i].RedDotObject.SetActive(false);
        }
    }

    IEnumerator CoCheckContinue()
    {
        yield return new WaitForEndOfFrame();

        // TODO ILHAK

        if (PlayerPrefs.GetInt("ISFIRST") == 1)
        {
            Managers.UI.ShowPopupUI<UI_BeginnerSupportRewardPopup>();
            PlayerPrefs.SetInt("ISFIRST", 0);
        }

        if (Managers.Game.ContinueInfo.isContinue == true)
        {
            Managers.UI.ShowPopupUI<UI_BackToBattlePopup>();
        }
        else
        {
            Managers.Game.ClearContinueData();
        }
    }

    private void SetRewardBoxes(StageClearInfo info)
    {
        int wave = info.MaxWaveIndex + 1;

        if (wave < 3)
        {
            InitBoxes();
        }
        else if (wave < 6)
        {
            //첫번째 상자 세팅
            if (info.isOpenFirstBox == true)
                SetBoxState(0, ERewardBoxState.Complete);
            else
                SetBoxState(0, ERewardBoxState.RedDot);
        }
        else if (wave < 10)
        {
            // 1 2 상자 세팅
            if (info.isOpenFirstBox == true)
                SetBoxState(0, ERewardBoxState.Complete);
            else
                SetBoxState(0, ERewardBoxState.RedDot);

            if (info.isOpenSecondBox == true)
                SetBoxState(1, ERewardBoxState.Complete);
            else
                SetBoxState(1, ERewardBoxState.RedDot);
        }
        else
        {
            //모든 상자 세팅
            if (info.isOpenFirstBox == true)
                SetBoxState(0, ERewardBoxState.Complete);
            else
                SetBoxState(0, ERewardBoxState.RedDot);

            if (info.isOpenSecondBox == true)
                SetBoxState(1, ERewardBoxState.Complete);
            else
                SetBoxState(1, ERewardBoxState.RedDot);

            if (info.isOpenThirdBox == true)
                SetBoxState(2, ERewardBoxState.Complete);
            else
                SetBoxState(2, ERewardBoxState.RedDot);
        }
    }

    private void SetBoxState(int index, ERewardBoxState state)
    {
        _boxes[index].UnLockObject.SetActive(false);
        _boxes[index].RedDotObject.SetActive(false);
        _boxes[index].CompleteObject.SetActive(false);
        _boxes[index].state = state;

        switch (state)
        {
            case ERewardBoxState.Unlock:
                _boxes[index].UnLockObject.SetActive(false);
                break;
            case ERewardBoxState.Complete:
                _boxes[index].CompleteObject.SetActive(true);
                break;
            case ERewardBoxState.RedDot:
                _boxes[index].RedDotObject.SetActive(true);
                break;
            case ERewardBoxState.Lock:
                _boxes[index].UnLockObject.SetActive(true);
                break;
        }
    }

    #region EventHandler
    private void OnClickGameStartButton(PointerEventData evt)
    {
        Managers.Sound.PlayButtonClick();

        Managers.Game.IsGameEnd = false;
        if (Managers.Game.Stamina < Define.GAME_PER_STAMINA)
        {
            Managers.UI.ShowPopupUI<UI_StaminaChargePopup>();
            return;
        }

        Managers.Game.Stamina -= Define.GAME_PER_STAMINA;
        if (Managers.Game.DicMission.TryGetValue(Define.EMissionTarget.StageEnter, out MissionInfo mission))
            mission.Progress++;
        Managers.Scene.LoadScene(Define.EScene.GameScene, transform);
    }

    private void OnClickStageSelectButton(PointerEventData evt) // 스테이지 선택 버튼
    {
        Managers.Sound.PlayButtonClick();

        //TODO Continue ILHAK
        UI_StageSelectPopup stageSelectPopupUI = Managers.UI.ShowPopupUI<UI_StageSelectPopup>();

        stageSelectPopupUI.OnPopupClosed = () =>
        {
            RefreshUI();
        };
        //스테이지 저장 관련해서 처리 한 후에 최신 스테이지 불러오게 처리 필요.
        //현재는 임시로 1스테이지 불러오게 해놨음
        stageSelectPopupUI.SetInfo(Managers.Game.CurrentStageData);
    }

    private void OnClickOfflineRewardButton(PointerEventData evt)
    {
        Managers.Sound.PlayButtonClick();
        // TODO ILHAK After Time Manager
        //Managers.UI.ShowPopupUI<UI_OfflineRewardPopup>();
    }

    private void OnClickMissionButton(PointerEventData evt)
    {
        Managers.Sound.PlayButtonClick();
        Managers.UI.ShowPopupUI<UI_MissionPopup>();
    }

    private void OnClickSettingButton(PointerEventData evt)
    {
        Managers.Sound.PlayButtonClick();
        Managers.UI.ShowPopupUI<UI_SettingPopup>();
    }

    private void OnClickAchievementButton(PointerEventData evt)
    {
        Managers.Sound.PlayButtonClick();
        Managers.UI.ShowPopupUI<UI_AchievementPopup>();
    }

    private void OnClickAttendanceCheckButton(PointerEventData evt)
    {
        // TODO ILHAK after Time Managers
        Managers.Sound.PlayButtonClick();
        //UI_CheckOutPopup popup = Managers.UI.ShowPopupUI<UI_CheckOutPopup>();
        //popup.SetInfo(Managers.Time.AttendanceDay);
    }

    private void OnClickFirstClearRewardButton(PointerEventData evt)
    {
        OnClickClearRewardButton(0);
    }

    private void OnClickSecondClearRewardButton(PointerEventData evt)
    {
        OnClickClearRewardButton(1);
    }

    private void OnClickThirdClearRewardButton(PointerEventData evt)
    {
        OnClickClearRewardButton(2);
    }

    private void OnClickClearRewardButton(int index)
    {
        Managers.Sound.PlayButtonClick();
        if (_boxes[index].state != ERewardBoxState.RedDot)
            return;

        if (Managers.Game.DicStageClearInfo.ContainsKey(_currentStageData.StageIndex))
        {
            int itemId = -1;
            string[] spriteName = new string[1];
            int[] count = new int[1];

            if (index == 0)
            {
                Managers.Game.DicStageClearInfo[_currentStageData.StageIndex].isOpenFirstBox = true;
                itemId = _currentStageData.FirstWaveClearRewardItemId;
                count[0] = _currentStageData.FirstWaveClearRewardItemValue;
            }
            if (index == 1)
            {
                Managers.Game.DicStageClearInfo[_currentStageData.StageIndex].isOpenSecondBox = true;
                itemId = _currentStageData.SecondWaveClearRewardItemId;
                count[0] = _currentStageData.SecondWaveClearRewardItemValue;
            }
            if (index == 2)
            {
                Managers.Game.DicStageClearInfo[_currentStageData.StageIndex].isOpenThirdBox = true;
                itemId = _currentStageData.ThirdWaveClearRewardItemId;
                count[0] = _currentStageData.ThirdWaveClearRewardItemValue;
            }

            SetBoxState(index, ERewardBoxState.Complete);

            if (Managers.Data.MaterialDic.TryGetValue(itemId, out MaterialData materialData))
            {
                spriteName[0] = materialData.SpriteName;
                
                UI_RewardPopup rewardPopup = (Managers.UI.SceneUI as UI_LobbyScene).RewardPopupUI;
                rewardPopup.gameObject.SetActive(true);
                Managers.Game.ExchangeMaterial(materialData, count[0]);
                rewardPopup.SetInfo(spriteName, count);
            }
        }
    }
    #endregion
}
