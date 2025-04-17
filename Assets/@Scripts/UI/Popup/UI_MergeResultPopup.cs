using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.EventSystems;
using static Define;

public class UI_MergeResultPopup : UI_Popup
{
    #region NnumObjects
    enum GameObjects
    {
        ContentObject,
        ImprovATKObject,
        ImprovHPObject,
    }
    enum Buttons
    {
        BackgroundButton,
        //ImprovHPObject,
    }

    enum Texts
    {
        BackgroundText,
        MergeResultCommentText,
        EquipmentNameValueText,
        EquipmentLevelValueText,
        EnforceValueText,
        ImprovLevelText,
        BeforeLevelValueText,
        AfterLevelValueText,
        ImprovATKText,
        BeforeATKValueText,
        AfterATKValueText,
        ImprovHPText,
        BeforeHPValueText,
        AfterHPValueText,
        ImprovOptionValueText,
        EquipmentGradeValueText,
    }
    enum Images
    {
        EquipmentGradeBackgroundImage,
        EquipmentImage,
        EquipmentEnforceBackgroundImage,
        EquipmentTypeBackgroundImage,
        EquipmentTypeImage,
    }
    #endregion

    Equipment _beforeEquipment;
    Equipment _afterEquipment;

    Action _closeAction;

    protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindImages(typeof(Images));
        BindTexts(typeof(Texts));

        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);
    }

    public void SetInfo(Equipment beforeEquipment, Equipment afterEquipment, Action callback = null)
    {
        _beforeEquipment = beforeEquipment;
        _afterEquipment = afterEquipment;
        _closeAction = callback;
        RefreshUI();
    }

    private void RefreshUI()
    {
        if (_beforeEquipment == null)
            return;
        if (_afterEquipment == null)
            return;

        // EquipmentImage : 장비의 아이콘
        Sprite spr = Managers.Resource.Load<Sprite>(_afterEquipment.EquipmentData.SpriteName);
        GetImage((int)Images.EquipmentTypeImage).sprite = Managers.Resource.Load<Sprite>($"{_afterEquipment.EquipmentData.EquipmentType}_Icon.sprite");

        GetImage((int)Images.EquipmentImage).sprite = spr;
        // 장비 이름
        GetText((int)Texts.EquipmentNameValueText).text = $"{_afterEquipment.EquipmentData.NameTextID}";
        // 장비 레벨
        GetText((int)Texts.EquipmentLevelValueText).text = $"Lv.{_beforeEquipment.Level}";

        switch (_afterEquipment.EquipmentData.EquipmentGrade)
        {
            case Define.EEquipmentGrade.Uncommon:
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = DEquipmentUIColors.Uncommon;
                GetText((int)Texts.EquipmentGradeValueText).text = "고급";
                GetText((int)Texts.EquipmentGradeValueText).color = DEquipmentUIColors.UncommonNameColor;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = DEquipmentUIColors.Uncommon;
                int uncommonGradeSkillId = _afterEquipment.EquipmentData.UncommonGradeSkill;
                GetText((int)Texts.ImprovOptionValueText).text = $"+ {Managers.Data.SupportSkillDic[uncommonGradeSkillId].Description}"; // 추가 옵션
                break;

            case Define.EEquipmentGrade.Rare:
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = DEquipmentUIColors.Rare;
                GetText((int)Texts.EquipmentGradeValueText).text = "희귀";
                GetText((int)Texts.EquipmentGradeValueText).color = DEquipmentUIColors.RareNameColor;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = DEquipmentUIColors.Rare;

                int rareGradeSkillId = _afterEquipment.EquipmentData.RareGradeSkill;
                GetText((int)Texts.ImprovOptionValueText).text = $"+ {Managers.Data.SupportSkillDic[rareGradeSkillId].Description}"; // 추가 옵션
                break;

            case Define.EEquipmentGrade.Epic:
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = DEquipmentUIColors.Epic;
                GetText((int)Texts.EquipmentGradeValueText).text = "에픽";
                GetText((int)Texts.EquipmentGradeValueText).color = DEquipmentUIColors.EpicNameColor;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = DEquipmentUIColors.EpicBg;

                int epicGradeSkillId = _afterEquipment.EquipmentData.EpicGradeSkill;
                GetText((int)Texts.ImprovOptionValueText).text = $"+ {Managers.Data.SupportSkillDic[epicGradeSkillId].Description}"; // 추가 옵션
                break;

            case Define.EEquipmentGrade.Epic1:
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = DEquipmentUIColors.Epic;
                GetImage((int)Images.EquipmentEnforceBackgroundImage).color = DEquipmentUIColors.EpicBg;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = DEquipmentUIColors.EpicBg;

                GetText((int)Texts.EquipmentGradeValueText).text = "에픽 1";
                GetText((int)Texts.EquipmentGradeValueText).color = DEquipmentUIColors.EpicNameColor;
                break;

            case Define.EEquipmentGrade.Epic2:
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = DEquipmentUIColors.Epic;
                GetImage((int)Images.EquipmentEnforceBackgroundImage).color = DEquipmentUIColors.EpicBg;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = DEquipmentUIColors.EpicBg;
                GetText((int)Texts.EquipmentGradeValueText).text = "에픽 2";
                GetText((int)Texts.EquipmentGradeValueText).color = DEquipmentUIColors.EpicNameColor;
                break;

            case Define.EEquipmentGrade.Legendary:
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = DEquipmentUIColors.Legendary;
                GetText((int)Texts.EquipmentGradeValueText).text = "전설";
                GetText((int)Texts.EquipmentGradeValueText).color = DEquipmentUIColors.LegendaryNameColor;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = DEquipmentUIColors.LegendaryBg;
                int legendaryGradeSkillId = _afterEquipment.EquipmentData.LegendaryGradeSkill;
                GetText((int)Texts.ImprovOptionValueText).text = $"+ {Managers.Data.SupportSkillDic[legendaryGradeSkillId].Description}"; // 추가 옵션
                break;

            case Define.EEquipmentGrade.Legendary1:
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = DEquipmentUIColors.Legendary;
                GetImage((int)Images.EquipmentEnforceBackgroundImage).color = DEquipmentUIColors.LegendaryBg;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = DEquipmentUIColors.LegendaryBg;
                GetText((int)Texts.EquipmentGradeValueText).text = "전설 1";
                GetText((int)Texts.EquipmentGradeValueText).color = DEquipmentUIColors.LegendaryNameColor;
                break;

            case Define.EEquipmentGrade.Legendary2:
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = DEquipmentUIColors.Legendary;
                GetImage((int)Images.EquipmentEnforceBackgroundImage).color = DEquipmentUIColors.LegendaryBg;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = DEquipmentUIColors.LegendaryBg;
                GetText((int)Texts.EquipmentGradeValueText).text = "전설 2";
                GetText((int)Texts.EquipmentGradeValueText).color = DEquipmentUIColors.LegendaryNameColor;
                break;

            case Define.EEquipmentGrade.Legendary3:
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = DEquipmentUIColors.Legendary;
                GetImage((int)Images.EquipmentEnforceBackgroundImage).color = DEquipmentUIColors.LegendaryBg;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = DEquipmentUIColors.LegendaryBg;
                GetText((int)Texts.EquipmentGradeValueText).text = "전설 3";
                GetText((int)Texts.EquipmentGradeValueText).color = DEquipmentUIColors.LegendaryNameColor;
                break;

            default:
                break;
        }

        string gradeName = _afterEquipment.EquipmentData.EquipmentGrade.ToString();
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

        // 최대 레벨
        GetText((int)Texts.BeforeLevelValueText).text = $"{Managers.Data.EquipDataDic[_beforeEquipment.EquipmentData.DataId].MaxLevel}"; // 합성 전 공격력
        GetText((int)Texts.AfterLevelValueText).text = $"{Managers.Data.EquipDataDic[_beforeEquipment.EquipmentData.DataId].MaxLevel}"; // 합성 전 공격력

        // 능력치 상승
        if (_beforeEquipment.EquipmentData.AtkDmgBonus != 0) // 공격력이 0이 아니면 무기
        {
            // 공격력 옵션
            GetObject((int)GameObjects.ImprovATKObject).gameObject.SetActive(true);
            GetObject((int)GameObjects.ImprovHPObject).gameObject.SetActive(false);

            GetText((int)Texts.BeforeATKValueText).text = $"{Managers.Data.EquipDataDic[_beforeEquipment.EquipmentData.DataId].MaxLevel}"; // 합성 전 공격력
            GetText((int)Texts.AfterATKValueText).text = $"{Managers.Data.EquipDataDic[_afterEquipment.EquipmentData.DataId].MaxLevel}"; // 합성 후 공격력
        }
        else
        {
            // 체력 옵션
            GetObject((int)GameObjects.ImprovATKObject).gameObject.SetActive(false);
            GetObject((int)GameObjects.ImprovHPObject).gameObject.SetActive(true);

            GetText((int)Texts.BeforeHPValueText).text = Managers.Data.EquipDataDic[_beforeEquipment.EquipmentData.DataId].MaxLevel.ToString(); // 합성 전 체력
            GetText((int)Texts.AfterHPValueText).text = Managers.Data.EquipDataDic[_afterEquipment.EquipmentData.DataId].MaxLevel.ToString(); // 합성 후 체력 
        }
    }

    private void OnClickBackgroundButton(PointerEventData evt)
    {
        Managers.Sound.PlayPopupClose();
        _closeAction?.Invoke();
        gameObject.SetActive(false);
    }
}
