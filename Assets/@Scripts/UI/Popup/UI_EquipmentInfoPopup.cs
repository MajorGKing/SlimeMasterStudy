using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class UI_EquipmentInfoPopup : UI_Popup
{
    #region BindingEnum
    enum GameObjects
    {
        ContentObject,
        UncommonSkillOptionObject,
        RareSkillOptionObject,
        EpicSkillOptionObject,
        LegendarySkillOptionObject,
        EquipmentGradeSkillScrollContentObject,
        ButtonGroupObject,
        CostGoldObject,
        CostMaterialObject,
        LevelupCostGroupObject,
    }

    enum Buttons
    {
        BackgroundButton,
        EquipmentResetButton,
        EquipButton,
        UnquipButton,
        LevelupButton,
        MergeButton,
    }

    enum Texts
    {
        EquipmentGradeValueText,
        EquipmentNameValueText,
        EquipmentLevelValueText,
        EquipmentOptionValueText,
        UncommonSkillOptionDescriptionValueText,
        RareSkillOptionDescriptionValueText,
        EpicSkillOptionDescriptionValueText,
        LegendarySkillOptionDescriptionValueText,
        CostGoldValueText,
        CostMaterialValueText,
        EquipButtonText,
        UnequipButtonText,
        LevelupButtonText,
        MergeButtonText,
        EquipmentGradeSkillText,
        BackgroundText,
        EnforceValueText,
    }

    enum Images
    {
        EquipmentGradeBackgroundImage,
        EquipmentOptionImage,
        CostMaterialImage,
        EquipmentImage,
        GradeBackgroundImage,
        EquipmentEnforceBackgroundImage,
        EquipmentTypeBackgroundImage,
        EquipmentTypeImage,

        UncommonSkillLockImage,
        RareSkillLockImage,
        EpicSkillLockImage,
        LegendarySkillLockImage,
    }
    #endregion

    public Equipment _equipment;
    Action _closeAction;

    protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));
        BindImages(typeof(Images));

        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);
        GetButton((int)Buttons.EquipmentResetButton).gameObject.BindEvent(OnClickEquipmentResetButton);
        GetButton((int)Buttons.EquipmentResetButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.EquipButton).gameObject.BindEvent(OnClickEquipButton);
        GetButton((int)Buttons.EquipButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.UnquipButton).gameObject.BindEvent(OnClickUnequipButton);
        GetButton((int)Buttons.UnquipButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.LevelupButton).gameObject.BindEvent(OnClickLevelupButton);
        GetButton((int)Buttons.LevelupButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.MergeButton).gameObject.BindEvent(OnClickMergeButton);
        GetButton((int)Buttons.MergeButton).GetOrAddComponent<UI_ButtonAnimation>();
    }

    public void SetInfo(Equipment equipment)
    {
        _equipment = equipment;
        RefreshUI();
    }

    private void RefreshUI()
    {
        GetButton((int)Buttons.EquipButton).gameObject.SetActive(true);
        GetButton((int)Buttons.UnquipButton).gameObject.SetActive(true);
        if (_equipment.IsEquipped == true)
            GetButton((int)Buttons.EquipButton).gameObject.SetActive(false);
        else
            GetButton((int)Buttons.UnquipButton).gameObject.SetActive(false);

        // 장비 레벨이 1이라면 리셋 버튼 비활성화
        if (_equipment.Level == 1)
            GetButton((int)Buttons.EquipmentResetButton).gameObject.SetActive(false);
        else
            GetButton((int)Buttons.EquipmentResetButton).gameObject.SetActive(true);

        GetImage((int)Images.EquipmentTypeImage).sprite = Managers.Resource.Load<Sprite>($"{_equipment.EquipmentData.EquipmentType}_Icon");
        GetImage((int)Images.EquipmentImage).sprite = Managers.Resource.Load<Sprite>(_equipment.EquipmentData.SpriteName);

        switch (_equipment.EquipmentData.EquipmentGrade)
        {
            case Define.EEquipmentGrade.Common:
                //GetText((int)Texts.EquipmentGradeValueText).text = _equipment.EquipmentData.EquipmentGrade.ToString();
                GetText((int)Texts.EquipmentGradeValueText).text = "일반";
                GetText((int)Texts.EquipmentGradeValueText).color = DEquipmentUIColors.CommonNameColor;
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = DEquipmentUIColors.Common;
                GetImage((int)Images.GradeBackgroundImage).color = DEquipmentUIColors.Common;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = DEquipmentUIColors.Common;
                break;

            case Define.EEquipmentGrade.Uncommon:
                //GetText((int)Texts.EquipmentGradeValueText).text = _equipment.EquipmentData.EquipmentGrade.ToString();
                GetText((int)Texts.EquipmentGradeValueText).text = "고급";
                GetText((int)Texts.EquipmentGradeValueText).color = DEquipmentUIColors.UncommonNameColor;
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = DEquipmentUIColors.Uncommon;
                GetImage((int)Images.GradeBackgroundImage).color = DEquipmentUIColors.Uncommon;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = DEquipmentUIColors.Uncommon;
                break;

            case Define.EEquipmentGrade.Rare:
                //GetText((int)Texts.EquipmentGradeValueText).text = _equipment.EquipmentData.EquipmentGrade.ToString();
                GetText((int)Texts.EquipmentGradeValueText).text = "희귀";
                GetText((int)Texts.EquipmentGradeValueText).color = DEquipmentUIColors.RareNameColor;
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = DEquipmentUIColors.Rare;
                GetImage((int)Images.GradeBackgroundImage).color = DEquipmentUIColors.Rare;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = DEquipmentUIColors.Rare;
                break;

            case Define.EEquipmentGrade.Epic:
                //GetText((int)Texts.EquipmentGradeValueText).text = EquipmentGrade.Epic.ToString();
                GetText((int)Texts.EquipmentGradeValueText).text = "에픽";
                GetText((int)Texts.EquipmentGradeValueText).color = DEquipmentUIColors.EpicNameColor;
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = DEquipmentUIColors.Epic;
                GetImage((int)Images.EquipmentEnforceBackgroundImage).color = DEquipmentUIColors.EpicBg;
                GetImage((int)Images.GradeBackgroundImage).color = DEquipmentUIColors.Epic;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = DEquipmentUIColors.EpicBg;
                break;

            case Define.EEquipmentGrade.Epic1:
                //GetText((int)Texts.EquipmentGradeValueText).text = EquipmentGrade.Epic.ToString();
                GetText((int)Texts.EquipmentGradeValueText).text = "에픽 1";
                GetText((int)Texts.EquipmentGradeValueText).color = DEquipmentUIColors.EpicNameColor;
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = DEquipmentUIColors.Epic;
                GetImage((int)Images.EquipmentEnforceBackgroundImage).color = DEquipmentUIColors.EpicBg;
                GetImage((int)Images.GradeBackgroundImage).color = DEquipmentUIColors.Epic;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = DEquipmentUIColors.EpicBg;
                break;

            case Define.EEquipmentGrade.Epic2:
                //GetText((int)Texts.EquipmentGradeValueText).text = EquipmentGrade.Epic.ToString();
                GetText((int)Texts.EquipmentGradeValueText).text = "에픽 2";
                GetText((int)Texts.EquipmentGradeValueText).color = DEquipmentUIColors.EpicNameColor;
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = DEquipmentUIColors.Epic;
                GetImage((int)Images.EquipmentEnforceBackgroundImage).color = DEquipmentUIColors.EpicBg;
                GetImage((int)Images.GradeBackgroundImage).color = DEquipmentUIColors.Epic;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = DEquipmentUIColors.EpicBg;
                break;

            case Define.EEquipmentGrade.Legendary:
                //GetText((int)Texts.EquipmentGradeValueText).text = EquipmentGrade.Legendary.ToString();
                GetText((int)Texts.EquipmentGradeValueText).text = "전설";
                GetText((int)Texts.EquipmentGradeValueText).color = DEquipmentUIColors.LegendaryNameColor;
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = DEquipmentUIColors.Legendary;
                GetImage((int)Images.EquipmentEnforceBackgroundImage).color = DEquipmentUIColors.LegendaryBg;
                GetImage((int)Images.GradeBackgroundImage).color = DEquipmentUIColors.Legendary;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = DEquipmentUIColors.LegendaryBg;
                break;

            case Define.EEquipmentGrade.Legendary1:
                //GetText((int)Texts.EquipmentGradeValueText).text = EquipmentGrade.Legendary.ToString();
                GetText((int)Texts.EquipmentGradeValueText).text = "전설 1";
                GetText((int)Texts.EquipmentGradeValueText).color = DEquipmentUIColors.LegendaryNameColor;
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = DEquipmentUIColors.Legendary;
                GetImage((int)Images.EquipmentEnforceBackgroundImage).color = DEquipmentUIColors.LegendaryBg;
                GetImage((int)Images.GradeBackgroundImage).color = DEquipmentUIColors.Legendary;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = DEquipmentUIColors.LegendaryBg;
                break;

            case Define.EEquipmentGrade.Legendary2:
                //GetText((int)Texts.EquipmentGradeValueText).text = EquipmentGrade.Legendary.ToString();
                GetText((int)Texts.EquipmentGradeValueText).text = "전설 2";
                GetText((int)Texts.EquipmentGradeValueText).color = DEquipmentUIColors.LegendaryNameColor;
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = DEquipmentUIColors.Legendary;
                GetImage((int)Images.EquipmentEnforceBackgroundImage).color = DEquipmentUIColors.LegendaryBg;
                GetImage((int)Images.GradeBackgroundImage).color = DEquipmentUIColors.Legendary;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = DEquipmentUIColors.LegendaryBg;
                break;

            case Define.EEquipmentGrade.Legendary3:
                //GetText((int)Texts.EquipmentGradeValueText).text = EquipmentGrade.Legendary.ToString();
                GetText((int)Texts.EquipmentGradeValueText).text = "전설 3";
                GetText((int)Texts.EquipmentGradeValueText).color = DEquipmentUIColors.LegendaryNameColor;
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = DEquipmentUIColors.Legendary;
                GetImage((int)Images.EquipmentEnforceBackgroundImage).color = DEquipmentUIColors.LegendaryBg;
                GetImage((int)Images.GradeBackgroundImage).color = DEquipmentUIColors.Legendary;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = DEquipmentUIColors.LegendaryBg;
                break;

            default:
                break;
        }

        // EquipmentNameValueText : 대상 장비의 이름
        GetText((int)Texts.EquipmentNameValueText).text = _equipment.EquipmentData.NameTextID;
        // EquipmentLevelValueText : 장비의 레벨 (현재 레벨/최대 레벨)
        GetText((int)Texts.EquipmentLevelValueText).text = $"{_equipment.Level}/{_equipment.EquipmentData.MaxLevel}";
        // EquipmentOptionImage : 장비 옵션의 아이콘
        string sprName = _equipment.MaxHpBonus == 0 ? "AttackPoint_Icon" : "HealthPoint_Icon";
        GetImage((int)Images.EquipmentOptionImage).sprite = Managers.Resource.Load<Sprite>(sprName);
        // EquipmentOptionValueText : 장비 옵션 수치
        string bonusVale = _equipment.MaxHpBonus == 0 ? _equipment.AttackBonus.ToString() : _equipment.MaxHpBonus.ToString();
        GetText((int)Texts.EquipmentOptionValueText).text = $"+{bonusVale}";

        // CostGoldValueText : 레벨업 비용 (보유 / 필요) 만약 코스트가 부족하다면 보유량을 빨간색(#F3614D)으로 보여준다. 부족하지 않다면 흰색(#FFFFFF)
        if (Managers.Data.EquipLevelDataDic.ContainsKey(_equipment.Level))
        {
            GetText((int)Texts.CostGoldValueText).text = $"{Managers.Data.EquipLevelDataDic[_equipment.Level].UpgradeCost}";
            if (Managers.Game.Gold < Managers.Data.EquipLevelDataDic[_equipment.Level].UpgradeCost)
                GetText((int)Texts.CostGoldValueText).color = Utils.HexToColor("F3614D");

            GetText((int)Texts.CostMaterialValueText).text = $"{Managers.Data.EquipLevelDataDic[_equipment.Level].UpgradeRequiredItems}";
        }

        // 레벨업 재료의 아이콘
        GetImage((int)Images.CostMaterialImage).sprite = Managers.Resource.Load<Sprite>(Managers.Data.MaterialDic[_equipment.EquipmentData.LevelupMaterialID].SpriteName);

        string gradeName = _equipment.EquipmentData.EquipmentGrade.ToString();
        int num = 0;

        // Epic1 -> 1 리턴 Epic2 ->2 리턴 Common처럼 숫자가 없으면 0 리턴
        Match match = Regex.Match(gradeName, @"\d+$");
        if (match.Success)
            num = int.Parse(match.Value);

        if (num == 0)
        {
            GetText((int)Texts.EnforceValueText).text = "";
            GetImage((int)Images.EquipmentEnforceBackgroundImage).gameObject.SetActive(false);
        }
        else
        {
            GetText((int)Texts.EnforceValueText).text = num.ToString();
            GetImage((int)Images.EquipmentEnforceBackgroundImage).gameObject.SetActive(true);
        }

        // 만약 장비 데이터 테이블의 각 등급셜 옵션(스킬ID)에 스킬이 없다면 등급에 맞는 옵션 오브젝트 비활성화
        GetObject((int)GameObjects.UncommonSkillOptionObject).SetActive(false);
        GetObject((int)GameObjects.RareSkillOptionObject).SetActive(false);
        GetObject((int)GameObjects.EpicSkillOptionObject).SetActive(false);
        GetObject((int)GameObjects.LegendarySkillOptionObject).SetActive(false);

        // 스킬타입에서 서포트스킬 타입 데이터로 교체
        if (Managers.Data.SupportSkillDic.ContainsKey(_equipment.EquipmentData.UncommonGradeSkill))
        {
            SupportSkillData skillData = Managers.Data.SupportSkillDic[_equipment.EquipmentData.UncommonGradeSkill];
            GetText((int)Texts.UncommonSkillOptionDescriptionValueText).text = $"+{skillData.Description}";
            GetObject((int)GameObjects.UncommonSkillOptionObject).SetActive(true);
        }

        if (Managers.Data.SupportSkillDic.ContainsKey(_equipment.EquipmentData.RareGradeSkill))
        {
            SupportSkillData skillData = Managers.Data.SupportSkillDic[_equipment.EquipmentData.RareGradeSkill];
            GetText((int)Texts.RareSkillOptionDescriptionValueText).text = $"+{skillData.Description}";
            GetObject((int)GameObjects.RareSkillOptionObject).SetActive(true);
        }

        if (Managers.Data.SupportSkillDic.ContainsKey(_equipment.EquipmentData.EpicGradeSkill))
        {
            SupportSkillData skillData = Managers.Data.SupportSkillDic[_equipment.EquipmentData.EpicGradeSkill];
            GetText((int)Texts.EpicSkillOptionDescriptionValueText).text = $"+{skillData.Description}";
            GetObject((int)GameObjects.EpicSkillOptionObject).SetActive(true);
        }

        if (Managers.Data.SupportSkillDic.ContainsKey(_equipment.EquipmentData.LegendaryGradeSkill))
        {
            SupportSkillData skillData = Managers.Data.SupportSkillDic[_equipment.EquipmentData.LegendaryGradeSkill];
            GetText((int)Texts.LegendarySkillOptionDescriptionValueText).text = $"+{skillData.Description}";
            GetObject((int)GameObjects.LegendarySkillOptionObject).SetActive(true);
        }

        Define.EEquipmentGrade equipmentGrade = _equipment.EquipmentData.EquipmentGrade;

        // 공통 색상 변경
        GetText((int)Texts.UncommonSkillOptionDescriptionValueText).color = Utils.HexToColor("9A9A9A");
        GetText((int)Texts.RareSkillOptionDescriptionValueText).color = Utils.HexToColor("9A9A9A");
        GetText((int)Texts.EpicSkillOptionDescriptionValueText).color = Utils.HexToColor("9A9A9A");
        GetText((int)Texts.LegendarySkillOptionDescriptionValueText).color = Utils.HexToColor("9A9A9A");

        GetImage((int)Images.UncommonSkillLockImage).gameObject.SetActive(true);
        GetImage((int)Images.RareSkillLockImage).gameObject.SetActive(true);
        GetImage((int)Images.EpicSkillLockImage).gameObject.SetActive(true);
        GetImage((int)Images.LegendarySkillLockImage).gameObject.SetActive(true);

        // 등급별 색상 추가 및 변경
        if (equipmentGrade >= Define.EEquipmentGrade.Uncommon)
        {
            GetText((int)Texts.UncommonSkillOptionDescriptionValueText).color = DEquipmentUIColors.Uncommon;
            GetImage((int)Images.UncommonSkillLockImage).gameObject.SetActive(false);
        }

        if (equipmentGrade >= Define.EEquipmentGrade.Rare)
        {
            GetText((int)Texts.RareSkillOptionDescriptionValueText).color = DEquipmentUIColors.Rare;
            GetImage((int)Images.RareSkillLockImage).gameObject.SetActive(false);
        }

        if (equipmentGrade >= Define.EEquipmentGrade.Epic)
        {
            GetText((int)Texts.EpicSkillOptionDescriptionValueText).color = DEquipmentUIColors.Epic;
            GetImage((int)Images.EpicSkillLockImage).gameObject.SetActive(false);
        }

        if (equipmentGrade >= Define.EEquipmentGrade.Legendary)
        {
            GetText((int)Texts.LegendarySkillOptionDescriptionValueText).color = DEquipmentUIColors.Legendary;
            GetImage((int)Images.LegendarySkillLockImage).gameObject.SetActive(false);
        }

        #region 리프레시 버그 대응
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.EquipmentGradeSkillScrollContentObject).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.ButtonGroupObject).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.CostGoldObject).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.CostMaterialObject).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetText((int)Texts.CostGoldValueText).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetText((int)Texts.CostMaterialValueText).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.LevelupCostGroupObject).GetComponent<RectTransform>());
        #endregion
    }

    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.EquipmentGradeSkillScrollContentObject).GetComponent<RectTransform>());

    }

    private void OnClickBackgroundButton(PointerEventData evt)
    {
        // TODO ILHAK GameManager Equipment after
        //Managers.Sound.PlayPopupClose();
        gameObject.SetActive(false);
        Managers.UI.ClosePopupUI(this);

        //(Managers.UI.SceneUI as UI_LobbyScene).EquipmentPopupUI.SetInfo();
    }

    private void OnClickEquipmentResetButton(PointerEventData evt)
    {
        //Managers.Sound.PlayButtonClick();
        ////UI_EquipmentResetPopup resetPopup = (Managers.UI.SceneUI as UI_LobbyScene).EquipmentResetPopupUI;
        //resetPopup.SetInfo(_equipment);
        //resetPopup.gameObject.SetActive(true);
    }

    private void OnClickEquipButton(PointerEventData evt)
    {

    }

    private void OnClickUnequipButton(PointerEventData evt)
    {

    }

    private void OnClickLevelupButton(PointerEventData evt)
    {

    }

    private void OnClickMergeButton(PointerEventData evt)
    {

    }
}
