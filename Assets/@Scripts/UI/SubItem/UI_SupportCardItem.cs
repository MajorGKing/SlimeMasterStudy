using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_SupportCardItem : UI_SubItem
{
    enum GameObjects
    {
        SoldOutObject,
    }

    enum Texts
    {
        CardNameText,
        SoulValueText,
        LockToggleText,
        SkillDescriptionText,
    }

    enum Images
    {
        SupportSkillImage,
        SupportSkillCardBackgroundImage,
        SupportCardTitleImage,
    }
    enum Toggles
    {
        LockToggle,
    }

    Data.SupportSkillData _supportSkilllData;

    protected override void Awake()
    {
        base.Awake();
        BindObjects(typeof(GameObjects));
        BindTexts(typeof(Texts));
        BindImages(typeof(Images));
        BindToggles(typeof(Toggles));

        GetToggle((int)Toggles.LockToggle).gameObject.BindEvent(OnClickLockToggle);
        gameObject.BindEvent(OnClickBuy);
        GetToggle((int)Toggles.LockToggle).GetOrAddComponent<UI_ButtonAnimation>();
    }

    public void SetInfo(Data.SupportSkillData supportSkilll)
    {
        transform.localScale = Vector3.one;
        _supportSkilllData = supportSkilll;
        GetObject((int)GameObjects.SoldOutObject).SetActive(false);

        RefreshUI();
    }

    private void RefreshUI()
    {
        // CardNameText : 서포트 스킬 이름
        GetText((int)Texts.CardNameText).text = _supportSkilllData.Name;
        // TargetDescriptionText : 서포트 스킬 설명
        GetText((int)Texts.SkillDescriptionText).text = _supportSkilllData.Description;
        // SoulValueText : 서포트 스킬 코스트
        GetText((int)Texts.SoulValueText).text = _supportSkilllData.Price.ToString();
        // SupportSkillImage : 서포트 스킬 아이콘
        GetImage((int)Images.SupportSkillImage).sprite = Managers.Resource.Load<Sprite>(_supportSkilllData.IconLabel);
        // SoldOutObject : 서포트 카드 구매 완료시 활성화 
        GetObject((int)GameObjects.SoldOutObject).SetActive(_supportSkilllData.IsPurchased);
        // LockToggle : 토글이 활성화 되었다면 서포트 스킬이 변경되지 않음(잠금을 해제 할때까지 유지)
        GetToggle((int)Toggles.LockToggle).isOn = _supportSkilllData.IsLocked;

        // 등급에 따라 배경 색상 변경
        switch (_supportSkilllData.SupportSkillGrade)
        {
            case Define.ESupportSkillGrade.Common:
                GetImage((int)Images.SupportSkillCardBackgroundImage).color = DEquipmentUIColors.Common;
                GetImage((int)Images.SupportCardTitleImage).color = DEquipmentUIColors.CommonNameColor;
                break;
            case Define.ESupportSkillGrade.Uncommon:
                GetImage((int)Images.SupportSkillCardBackgroundImage).color = DEquipmentUIColors.Uncommon;
                GetImage((int)Images.SupportCardTitleImage).color = DEquipmentUIColors.UncommonNameColor;
                break;
            case Define.ESupportSkillGrade.Rare:
                GetImage((int)Images.SupportSkillCardBackgroundImage).color = DEquipmentUIColors.Rare;
                GetImage((int)Images.SupportCardTitleImage).color = DEquipmentUIColors.RareNameColor;
                break;
            case Define.ESupportSkillGrade.Epic:
                GetImage((int)Images.SupportSkillCardBackgroundImage).color = DEquipmentUIColors.Epic;
                GetImage((int)Images.SupportCardTitleImage).color = DEquipmentUIColors.EpicNameColor;
                break;
            case Define.ESupportSkillGrade.Legend:
                GetImage((int)Images.SupportSkillCardBackgroundImage).color = DEquipmentUIColors.Legendary;
                GetImage((int)Images.SupportCardTitleImage).color = DEquipmentUIColors.LegendaryNameColor;
                break;
            default:
                break;
        }
    }

    private void OnClickLockToggle(PointerEventData evt)
    {
        Managers.Sound.PlayButtonClick();
        if (_supportSkilllData.IsPurchased)
            return;
        if (GetToggle((int)Toggles.LockToggle).isOn == true)
        {
            _supportSkilllData.IsLocked = true;
            Managers.Game.Player.Skills.LockedSupportSkills.Add(_supportSkilllData);
        }
        else
        {
            _supportSkilllData.IsLocked = false;
            Managers.Game.Player.Skills.LockedSupportSkills.Remove(_supportSkilllData);
        }
    }

    private void OnClickBuy(PointerEventData evt)
    {
        if (GetObject((int)GameObjects.SoldOutObject).activeInHierarchy == true)
            return;
        if (Managers.Game.Player.SoulCount >= _supportSkilllData.Price)
        {
            Managers.Game.Player.SoulCount -= _supportSkilllData.Price;

            if (Managers.Game.Player.Skills.LockedSupportSkills.Contains(_supportSkilllData))
                Managers.Game.Player.Skills.LockedSupportSkills.Remove(_supportSkilllData);

            Managers.Game.Player.Skills.AddSupportSkill(_supportSkilllData);
            GetObject((int)GameObjects.SoldOutObject).SetActive(true);
            //구매완료
            GetObject((int)GameObjects.SoldOutObject).SetActive(true);
        }
    }
}
