using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;

public class UI_EquipItem : UI_SubItem
{
    #region Enum
    enum GameObjects
    {
        EquipmentRedDotObject,
        NewTextObject,
        EquippedObject,
        SelectObject,
        LockObject,
        SpecialImage,
        GetEffectObject,
    }

    enum Texts
    {
        EquipmentLevelValueText,
        EnforceValueText,
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

    public Equipment Equipment;
    public Action OnClickEquipItem;
    //ScrollRect _scrollRect;
    Define.EUI_ItemParentType _parentType;

    bool _isDrag = false;

    protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindTexts(typeof(Texts));
        BindImages(typeof(Images));

        GetObject((int)GameObjects.GetEffectObject).SetActive(false);
        gameObject.BindEvent(OnDrag, Define.ETouchEvent.Drag);
        gameObject.BindEvent(OnBeginDrag, Define.ETouchEvent.BeginDrag);
        gameObject.BindEvent(OnEndDrag, Define.ETouchEvent.EndDrag);

        gameObject.BindEvent(OnClickEquipItemButton);
        gameObject.BindEvent(OnClickEquipItemButton);
        gameObject.BindEvent(OnClickEquipItemButton);

    }

    public void SetInfo(Equipment item, Define.EUI_ItemParentType parentType, ScrollRect scrollRect = null)
    {
        Equipment = item;
        transform.localScale = Vector3.one;
        _parentScrollRect = scrollRect;
        _parentType = parentType;

        #region 색상 변경
        // EquipmentGradeBackgroundImage : 합성 할 장비 등급의 테두리 (색상 변경)
        // EquipmentEnforceBackgroundImage : 유일 +1 등급부터 활성화되고 등급에 따라 이미지 색깔 변경
        switch (Equipment.EquipmentData.EquipmentGrade)
        {
            case Define.EEquipmentGrade.Common:
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = DEquipmentUIColors.Common;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = DEquipmentUIColors.Common;
                break;

            case Define.EEquipmentGrade.Uncommon:
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = DEquipmentUIColors.Uncommon;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = DEquipmentUIColors.Uncommon;
                break;

            case Define.EEquipmentGrade.Rare:
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = DEquipmentUIColors.Rare;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = DEquipmentUIColors.Rare;
                break;

            case Define.EEquipmentGrade.Epic:
            case Define.EEquipmentGrade.Epic1:
            case Define.EEquipmentGrade.Epic2:
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = DEquipmentUIColors.Epic;
                GetImage((int)Images.EquipmentEnforceBackgroundImage).color = DEquipmentUIColors.EpicBg;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = DEquipmentUIColors.EpicBg;
                break;

            case Define.EEquipmentGrade.Legendary:
            case Define.EEquipmentGrade.Legendary1:
            case Define.EEquipmentGrade.Legendary2:
            case Define.EEquipmentGrade.Legendary3:
                GetImage((int)Images.EquipmentGradeBackgroundImage).color = DEquipmentUIColors.Legendary;
                GetImage((int)Images.EquipmentEnforceBackgroundImage).color = DEquipmentUIColors.LegendaryBg;
                GetImage((int)Images.EquipmentTypeBackgroundImage).color = DEquipmentUIColors.LegendaryBg;
                break;

            default:
                break;
        }
        #endregion

        #region 유일 +1 등의 등급 벨류
        string gradeName = Equipment.EquipmentData.EquipmentGrade.ToString();
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
        #endregion

        // EquipmentImage : 장비의 아이콘
        GetImage((int)Images.EquipmentImage).sprite = Managers.Resource.Load<Sprite>(Equipment.EquipmentData.SpriteName);
        // 장비 타입 아이콘
        GetImage((int)Images.EquipmentTypeImage).sprite = Managers.Resource.Load<Sprite>($"{Equipment.EquipmentData.EquipmentType}_Icon");
        // EquipmentLevelValueText : 장비의 현재 레벨
        GetText((int)Texts.EquipmentLevelValueText).text = $"Lv.{Equipment.Level}";
        // EquipmentRedDotObject : 장비가 강화가 가능할때 출력 
        GetObject((int)GameObjects.EquipmentRedDotObject).SetActive(Equipment.IsUpgradable);
        // NewTextObject : 장비를 처음 습득했을때 출력
        GetObject((int)GameObjects.NewTextObject).SetActive(!Equipment.IsConfirmed);
        // EquippedObject : 합성 팝업에서 착용장비 표시용
        GameObject obj = GetObject((int)GameObjects.EquippedObject);

        GetObject((int)GameObjects.EquippedObject).SetActive(Equipment.IsEquipped);

        // SelectObject : 합성 팝업에서 장비 선택 표시용
        GetObject((int)GameObjects.SelectObject).SetActive(Equipment.IsSelected);
        // LockObject : 합성 팝업에서 선택 불가 표시용
        GetObject((int)GameObjects.LockObject).SetActive(Equipment.IsUnavailable);
        // Special아이템일때 
        bool isSpecial = Equipment.EquipmentData.GachaRarity == Define.EGachaRarity.Special ? true : false;
        //케릭터 장착중인 슬롯에 있으면 "착용중" 오브젝트 끔
        GetObject((int)GameObjects.SpecialImage).SetActive(isSpecial);
        if (parentType == Define.EUI_ItemParentType.CharacterEquipmentGroup)
        {
            GetObject((int)GameObjects.EquippedObject).SetActive(false);
        }
        // 가챠 결과 팝업 획득 효과 
        if (_parentType == Define.EUI_ItemParentType.GachaResultPopup)
        {
            GetObject((int)GameObjects.GetEffectObject).SetActive(true);
        }
    }

    #region EventHandler
    private void OnClickEquipItemButton(PointerEventData evt)
    {
        Managers.Sound.PlayButtonClick();
        if (_isDrag)
            return;

        // TODO ILHAK
    }
    #endregion
}
