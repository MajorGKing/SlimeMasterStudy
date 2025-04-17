using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using static Define;

public class UI_EquipmentResetPopup : UI_Popup
{
    #region EnumObjects
    enum GameObjects
    {
        ContentObject,
        ToggleGroupObject,
        TargetEquipment,

        ResetInfoGroupObject,
        DowngradeGroupObject,
    }

    enum Buttons
    {
        BackgroundButton,
        EquipmentResetButton,
        EquipmentDowngradeButton,
    }

    enum Texts
    {
        BackgroundText,
        EquipmentResetPopupTitleText,
        ResetInfoCommentText,
        DowngradeCommentText,
        EquipmentResetButtonText,
        ResultGoldCountValueText,
        ResultMaterialCountValueText,
        TargetEquipmentLevelValueText,
        TargetEnforceValueText,
        ResultEnforceValueText,

        ResetTapToggleText,
        DowngradeTapToggleText,

        DowngradeTargetEquipmentLevelValueText,
        DowngradeTargetEnforceValueText,
        DowngradEnchantStoneCountValueText,
        DowngradeResultGoldCountValueText,
        DowngradeResultMaterialCountValueText,
        EquipmentDowngradeButtonText,
    }
    enum Images
    {
        TargetEquipmentGradeBackgroundImage,
        TargetEquipmentImage,
        TargetEquipmentEnforceBackgroundImage,
        ResultEquipmentGradeBackgroundImage,
        ResultEquipmentImage,
        ResultEquipmentEnforceBackgroundImage,
        ResultMaterialImage,

        DowngradeTargetEquipmentGradeBackgroundImage,
        DowngradeTargetEquipmentImage,
        DowngradeTargetEquipmentEnforceBackgroundImage,
        DowngradeEquipmentGradeBackgroundImage,
        DowngradeEquipmentImage,
        DowngradEnchantStoneBackgroundImage,
        DowngradEnchantStoneImage,
        DowngradeResultMaterialImage,
    }

    enum Toggles
    {
        ResetTapToggle,
        DowngradeTapToggle,
    }
    #endregion

    bool _isSelectedResetTapTap = false;
    bool _isSelectedDowngradeTapTap = false;
    public Equipment _equipment;

    protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));
        BindImages(typeof(Images));
        BindToggles(typeof(Toggles));

        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);
        GetButton((int)Buttons.EquipmentResetButton).gameObject.BindEvent(OnClickEquipmentResetButton);
        GetButton((int)Buttons.EquipmentResetButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.EquipmentDowngradeButton).gameObject.BindEvent(OnClickEquipmentDowngradeButton);
        GetButton((int)Buttons.EquipmentDowngradeButton).GetOrAddComponent<UI_ButtonAnimation>();

        GetToggle((int)Toggles.ResetTapToggle).gameObject.BindEvent(OnClickResetTapToggle);
        GetToggle((int)Toggles.DowngradeTapToggle).gameObject.BindEvent(OnClickDowngradeTapToggle);

        EquipmentResetPopupContentInit();
        OnClickResetTapToggle();
    }

    public void SetInfo(Equipment equipment)
    {
        _equipment = equipment;
        RefreshUI();
        OnClickResetTapToggle();
    }

    private void RefreshUI()
    {
        if (_equipment == null)
        {

            GetObject((int)GameObjects.TargetEquipment).SetActive(false);
        }
        else
        {
            EquipmentResetRefresh();
        }

        // 일반 장비 토글 처리 (다운그레이드 불가상태)
        if (_equipment.EquipmentData.EquipmentGrade == Define.EEquipmentGrade.Common)
            GetObject((int)GameObjects.ToggleGroupObject).SetActive(false);
        else if (_equipment.EquipmentData.EquipmentGrade == Define.EEquipmentGrade.Uncommon)
            GetObject((int)GameObjects.ToggleGroupObject).SetActive(false);
        else if (_equipment.EquipmentData.EquipmentGrade == Define.EEquipmentGrade.Rare)
            GetObject((int)GameObjects.ToggleGroupObject).SetActive(false);
        else if (_equipment.EquipmentData.EquipmentGrade == Define.EEquipmentGrade.Epic)
            GetObject((int)GameObjects.ToggleGroupObject).SetActive(false);
        else if (_equipment.EquipmentData.EquipmentGrade == Define.EEquipmentGrade.Legendary)
            GetObject((int)GameObjects.ToggleGroupObject).SetActive(false);
        else
        {
            EquipmentDowngradeRefresh();
            GetObject((int)GameObjects.ToggleGroupObject).SetActive(true);
        }

        OnClickResetTapToggle();
    }

    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));
    }

    private void EquipmentResetPopupContentInit()
    {
        _isSelectedResetTapTap = false;
        _isSelectedDowngradeTapTap = false;

        GetObject((int)GameObjects.ResetInfoGroupObject).gameObject.SetActive(false);
        GetObject((int)GameObjects.DowngradeGroupObject).gameObject.SetActive(false);
    }

    private void EquipmentResetRefresh()
    {
        GetImage((int)Images.TargetEquipmentImage).sprite = Managers.Resource.Load<Sprite>(_equipment.EquipmentData.SpriteName);
        GetText((int)Texts.TargetEquipmentLevelValueText).text = $"Lv. {_equipment.Level}";

        GetImage((int)Images.ResultEquipmentImage).sprite = Managers.Resource.Load<Sprite>(_equipment.EquipmentData.SpriteName);
        GetText((int)Texts.ResultGoldCountValueText).text = $"{CalculateResetGold()}";

        GetImage((int)Images.ResultMaterialImage).sprite = Managers.Resource.Load<Sprite>(Managers.Data.MaterialDic[_equipment.EquipmentData.LevelupMaterialID].SpriteName);
        GetText((int)Texts.ResultMaterialCountValueText).text = $"{CalculateResetMaterialCount()}";

        switch (_equipment.EquipmentData.EquipmentGrade)
        {
            case Define.EEquipmentGrade.Common:
                GetImage((int)Images.TargetEquipmentGradeBackgroundImage).color = DEquipmentUIColors.Common;
                GetImage((int)Images.ResultEquipmentGradeBackgroundImage).color = DEquipmentUIColors.Common;
                break;
            case Define.EEquipmentGrade.Uncommon:
                GetImage((int)Images.TargetEquipmentGradeBackgroundImage).color = DEquipmentUIColors.Uncommon;
                GetImage((int)Images.ResultEquipmentGradeBackgroundImage).color = DEquipmentUIColors.Uncommon;
                break;
            case Define.EEquipmentGrade.Rare:
                GetImage((int)Images.TargetEquipmentGradeBackgroundImage).color = DEquipmentUIColors.Rare;
                GetImage((int)Images.ResultEquipmentGradeBackgroundImage).color = DEquipmentUIColors.Rare;
                break;
            case Define.EEquipmentGrade.Epic:
            case Define.EEquipmentGrade.Epic1:
            case Define.EEquipmentGrade.Epic2:
                GetImage((int)Images.TargetEquipmentGradeBackgroundImage).color = DEquipmentUIColors.Epic;
                GetImage((int)Images.TargetEquipmentEnforceBackgroundImage).color = DEquipmentUIColors.EpicBg;
                GetImage((int)Images.ResultEquipmentGradeBackgroundImage).color = DEquipmentUIColors.Epic;
                GetImage((int)Images.ResultEquipmentEnforceBackgroundImage).color = DEquipmentUIColors.EpicBg;
                break;
            case Define.EEquipmentGrade.Legendary:
            case Define.EEquipmentGrade.Legendary1:
            case Define.EEquipmentGrade.Legendary2:
            case Define.EEquipmentGrade.Legendary3:
                GetImage((int)Images.TargetEquipmentGradeBackgroundImage).color = DEquipmentUIColors.Legendary;
                GetImage((int)Images.TargetEquipmentEnforceBackgroundImage).color = DEquipmentUIColors.LegendaryBg;
                GetImage((int)Images.ResultEquipmentGradeBackgroundImage).color = DEquipmentUIColors.Legendary;
                GetImage((int)Images.ResultEquipmentEnforceBackgroundImage).color = DEquipmentUIColors.LegendaryBg;
                break;
            case Define.EEquipmentGrade.Myth:
            case Define.EEquipmentGrade.Myth1:
            case Define.EEquipmentGrade.Myth2:
            case Define.EEquipmentGrade.Myth3:
                GetImage((int)Images.TargetEquipmentGradeBackgroundImage).color = DEquipmentUIColors.Myth;
                GetImage((int)Images.TargetEquipmentEnforceBackgroundImage).color = DEquipmentUIColors.MythBg;
                GetImage((int)Images.ResultEquipmentGradeBackgroundImage).color = DEquipmentUIColors.Myth;
                GetImage((int)Images.ResultEquipmentEnforceBackgroundImage).color = DEquipmentUIColors.MythBg;
                break;
            default:
                break;
        }

        string gradeName = _equipment.EquipmentData.EquipmentGrade.ToString();
        int num = 0;

        // Epic1 -> 1 리턴 Epic2 ->2 리턴 Common처럼 숫자가 없으면 0 리턴
        Match match = Regex.Match(gradeName, @"\d+$");
        if (match.Success)
            num = int.Parse(match.Value);

        if (num == 0)
        {
            GetImage((int)Images.TargetEquipmentEnforceBackgroundImage).gameObject.SetActive(false);
            GetImage((int)Images.ResultEquipmentEnforceBackgroundImage).gameObject.SetActive(false);
        }
        else
        {
            GetText((int)Texts.TargetEnforceValueText).text = num.ToString();
            GetImage((int)Images.TargetEquipmentEnforceBackgroundImage).gameObject.SetActive(true);
            GetText((int)Texts.ResultEnforceValueText).text = num.ToString();
            GetImage((int)Images.ResultEquipmentEnforceBackgroundImage).gameObject.SetActive(true);
        }
    }

    private void EquipmentDowngradeRefresh()
    {
        GetImage((int)Images.DowngradeTargetEquipmentImage).sprite = Managers.Resource.Load<Sprite>(_equipment.EquipmentData.SpriteName);
        GetText((int)Texts.DowngradeTargetEquipmentLevelValueText).text = $"Lv. {_equipment.Level}";

        GetImage((int)Images.DowngradeEquipmentImage).sprite = Managers.Resource.Load<Sprite>(_equipment.EquipmentData.SpriteName);
        GetText((int)Texts.DowngradeResultGoldCountValueText).text = $"{CalculateResetGold()}";

        GetImage((int)Images.DowngradEnchantStoneImage).sprite = Managers.Resource.Load<Sprite>(Managers.Data.EquipDataDic[_equipment.EquipmentData.DowngradeMaterialCode].SpriteName);
        GetText((int)Texts.DowngradEnchantStoneCountValueText).text = $"{_equipment.EquipmentData.DowngradeMaterialCount}";

        GetImage((int)Images.DowngradeResultMaterialImage).sprite = Managers.Resource.Load<Sprite>(Managers.Data.MaterialDic[_equipment.EquipmentData.LevelupMaterialID].SpriteName);
        GetText((int)Texts.DowngradeResultMaterialCountValueText).text = $"{CalculateResetMaterialCount()}";

        switch (_equipment.EquipmentData.EquipmentGrade)
        {
            case Define.EEquipmentGrade.Epic1:
            case Define.EEquipmentGrade.Epic2:
                GetImage((int)Images.DowngradeTargetEquipmentGradeBackgroundImage).color = DEquipmentUIColors.Epic;
                GetImage((int)Images.DowngradeTargetEquipmentEnforceBackgroundImage).color = DEquipmentUIColors.EpicBg;
                GetImage((int)Images.DowngradeEquipmentGradeBackgroundImage).color = DEquipmentUIColors.Epic;
                GetImage((int)Images.DowngradEnchantStoneBackgroundImage).color = DEquipmentUIColors.Epic;
                break;
            case Define.EEquipmentGrade.Legendary1:
            case Define.EEquipmentGrade.Legendary2:
            case Define.EEquipmentGrade.Legendary3:
                GetImage((int)Images.DowngradeTargetEquipmentGradeBackgroundImage).color = DEquipmentUIColors.Legendary;
                GetImage((int)Images.DowngradeTargetEquipmentEnforceBackgroundImage).color = DEquipmentUIColors.LegendaryBg;
                GetImage((int)Images.DowngradeEquipmentGradeBackgroundImage).color = DEquipmentUIColors.Legendary;
                GetImage((int)Images.DowngradEnchantStoneBackgroundImage).color = DEquipmentUIColors.Legendary;
                break;
            default:
                break;
        }

        string gradeName = _equipment.EquipmentData.EquipmentGrade.ToString();
        int num = 0;

        // Epic1 -> 1 리턴 Epic2 ->2 리턴 Common처럼 숫자가 없으면 0 리턴
        Match match = Regex.Match(gradeName, @"\d+$");
        if (match.Success)
            num = int.Parse(match.Value);

        if (num == 0)
        {
            GetImage((int)Images.DowngradeTargetEquipmentEnforceBackgroundImage).gameObject.SetActive(false);
        }
        else
        {
            GetText((int)Texts.DowngradeTargetEnforceValueText).text = num.ToString();
            GetImage((int)Images.DowngradeTargetEquipmentEnforceBackgroundImage).gameObject.SetActive(true);
        }
    }

    private int CalculateResetGold()
    {
        int gold = 0;
        for (int i = 1; i < _equipment.Level; i++)
        {
            gold += Managers.Data.EquipLevelDataDic[i].UpgradeCost;
        }
        return gold;
    }

    private int CalculateResetMaterialCount()
    {
        int materialCount = 0;
        for (int i = 1; i < _equipment.Level; i++)
        {
            materialCount += Managers.Data.EquipLevelDataDic[i].UpgradeRequiredItems;
        }

        return materialCount;
    }

    private void OnClickResetTapToggle()
    {
        // 활성화 후 토글 클릭 방지
        if (_isSelectedResetTapTap == true) 
            return;
        EquipmentResetPopupContentInit();
        _isSelectedResetTapTap = true;

        GetObject((int)GameObjects.ResetInfoGroupObject).gameObject.SetActive(true);
        GetToggle((int)Toggles.ResetTapToggle).isOn = true;
    }

    private void OnClickBackgroundButton(PointerEventData evt)
    {
        Managers.Sound.PlayPopupClose();
        Managers.UI.ClosePopupUI(this);
    }

    private void OnClickEquipmentResetButton(PointerEventData evt)
    {
        int gold = CalculateResetGold();
        int materialCount = CalculateResetMaterialCount();
        _equipment.Level = 1;

        string[] spriteName = new string[2];
        int[] count = new int[2];

        spriteName[0] = Define.GOLD_SPRITE_NAME;
        count[0] = gold;

        int materialCode = _equipment.EquipmentData.LevelupMaterialID;
        spriteName[1] = Managers.Data.MaterialDic[materialCode].SpriteName;
        count[1] = materialCount;

        UI_RewardPopup rewardPopup = (Managers.UI.SceneUI as UI_LobbyScene).RewardPopupUI;
        rewardPopup.gameObject.SetActive(true);

        Managers.Game.ExchangeMaterial(Managers.Data.MaterialDic[Define.ID_GOLD], gold);
        Managers.Game.ExchangeMaterial(Managers.Data.MaterialDic[materialCode], materialCount);
        (Managers.UI.SceneUI as UI_LobbyScene).EquipmentInfoPopupUI.gameObject.SetActive(false);

        // 버튼 누를 때  EuipmentPopup refresh
        (Managers.UI.SceneUI as UI_LobbyScene).EquipmentPopupUI.SetInfo();
        gameObject.SetActive(false);

        rewardPopup.SetInfo(spriteName, count);
    }

    private void OnClickEquipmentDowngradeButton(PointerEventData evt)
    {
        //TODO ILHAK After Game Manager Equipment
    }

    private void OnClickResetTapToggle(PointerEventData evt)
    {
        OnClickResetTapToggle();
    }

    private void OnClickDowngradeTapToggle(PointerEventData evt)
    {
        // 활성화 후 토글 클릭 방지
        if (_isSelectedDowngradeTapTap == true) 
            return;
        EquipmentResetPopupContentInit();
        _isSelectedDowngradeTapTap = true;

        GetObject((int)GameObjects.DowngradeGroupObject).gameObject.SetActive(true);
        GetToggle((int)Toggles.DowngradeTapToggle).isOn = true;
    }
}
