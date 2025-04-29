using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UI_SupportSkillItem : UI_SubItem
{
    enum Buttons
    {
        SupportSkillButton,
    }
    enum Images
    {
        SupportSkillImage,
        BackgroundImage,
    }

    SupportSkillData supportSkillData;
    Transform _makeSubItemParents;
    ScrollRect _scrollRect;

    protected override void Awake()
    {
        base.Awake();
        BindImages(typeof(Images));
        BindButtons(typeof(Buttons));

        //GetButton((int)Buttons.SupportSkillButton).gameObject.BindEvent(OnClickMaterialInfoButton, type: Define.UIEvent.Preseed);
        GetButton((int)Buttons.SupportSkillButton).gameObject.BindEvent(OnClickSupportSkillItem);
        GetButton((int)Buttons.SupportSkillButton).gameObject.BindEvent(OnDrag, Define.ETouchEvent.Drag);
        GetButton((int)Buttons.SupportSkillButton).gameObject.BindEvent(OnBeginDrag, Define.ETouchEvent.BeginDrag);
        GetButton((int)Buttons.SupportSkillButton).gameObject.BindEvent(OnEndDrag, Define.ETouchEvent.EndDrag);
    }

    public void SetInfo(Data.SupportSkillData skill, Transform makeSubItemParents, ScrollRect scrollRect)
    {
        transform.localScale = Vector3.one;
        Image img = GetImage((int)Images.SupportSkillImage);
        img.sprite = Managers.Resource.Load<Sprite>(skill.IconLabel);
        supportSkillData = skill;
        _makeSubItemParents = makeSubItemParents;
        _scrollRect = scrollRect;
        // 등급에 따른 배경 색상 변경
        switch (skill.SupportSkillGrade)
        {
            case Define.ESupportSkillGrade.Common:
                GetImage((int)Images.BackgroundImage).color = DEquipmentUIColors.Common;
                break;
            case Define.ESupportSkillGrade.Uncommon:
                GetImage((int)Images.BackgroundImage).color = DEquipmentUIColors.Uncommon;
                break;
            case Define.ESupportSkillGrade.Rare:
                GetImage((int)Images.BackgroundImage).color = DEquipmentUIColors.Rare;
                break;
            case Define.ESupportSkillGrade.Epic:
                GetImage((int)Images.BackgroundImage).color = DEquipmentUIColors.Epic;
                break;
            case Define.ESupportSkillGrade.Legend:
                GetImage((int)Images.BackgroundImage).color = DEquipmentUIColors.Legendary;
                break;
            default:
                break;
        }
    }

    private void OnClickSupportSkillItem(PointerEventData evt)
    {
        Managers.Sound.PlayButtonClick();
        // UI_ToolTipItem 프리팹 생성
        UI_ToolTipItem item = Managers.UI.MakeSubItem<UI_ToolTipItem>(_makeSubItemParents);
        item.transform.localScale = Vector3.one;
        RectTransform TargetPos = this.gameObject.GetComponent<RectTransform>();
        RectTransform parentsCanvas = _makeSubItemParents.gameObject.GetComponent<RectTransform>();
        item.SetInfo(supportSkillData, TargetPos, parentsCanvas);
        item.transform.SetAsLastSibling();
    }
}
