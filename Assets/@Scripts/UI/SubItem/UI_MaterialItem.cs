using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Define;

public class UI_MaterialItem : UI_SubItem
{
    #region Enum
    enum GameObjects
    {
        GetEffectObject,
    }
    enum Buttons
    {
        MaterialInfoButton,
    }

    enum Texts
    {
        ItemCountValueText
    }

    enum Images
    {
        MaterialItemImage,
        MaterialItemBackgroundImage,
    }
    #endregion

    Data.MaterialData _materialData;
    Transform _makeSubItemParents;

    protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));
        BindImages(typeof(Images));

        GetObject((int)GameObjects.GetEffectObject).SetActive(false);

        GetButton((int)Buttons.MaterialInfoButton).gameObject.BindEvent(OnDrag, Define.ETouchEvent.Drag);
        GetButton((int)Buttons.MaterialInfoButton).gameObject.BindEvent(OnBeginDrag, Define.ETouchEvent.BeginDrag);
        GetButton((int)Buttons.MaterialInfoButton).gameObject.BindEvent(OnEndDrag, Define.ETouchEvent.EndDrag);
        gameObject.BindEvent(OnClickMaterialInfoButton);
        GetButton((int)Buttons.MaterialInfoButton).gameObject.BindEvent(OnClickMaterialInfoButton);
    }

    public void SetInfo(string spriteName, int count)
    {
        transform.localScale = Vector3.one;
        GetImage((int)Images.MaterialItemImage).sprite = Managers.Resource.Load<Sprite>(spriteName);
        GetImage((int)Images.MaterialItemBackgroundImage).color = DEquipmentUIColors.Epic;
        GetText((int)Texts.ItemCountValueText).text = $"{count}";
        GetObject((int)GameObjects.GetEffectObject).SetActive(true);
    }

    public void SetInfo(MaterialData data, Transform makeSubItemParents, int count, ScrollRect scrollRect = null)
    {
        transform.localScale = Vector3.one;
        _parentScrollRect = scrollRect;
        _makeSubItemParents = makeSubItemParents;
        _materialData = data;

        GetImage((int)Images.MaterialItemImage).sprite = Managers.Resource.Load<Sprite>(_materialData.SpriteName);
        GetText((int)Texts.ItemCountValueText).text = $"{count}";

        switch (data.MaterialGrade)
        {
            case Define.EMaterialGrade.Common:
                GetImage((int)Images.MaterialItemBackgroundImage).color = DEquipmentUIColors.Common;
                break;
            case Define.EMaterialGrade.Uncommon:
                GetImage((int)Images.MaterialItemBackgroundImage).color = DEquipmentUIColors.Uncommon;
                break;
            case Define.EMaterialGrade.Rare:
                GetImage((int)Images.MaterialItemBackgroundImage).color = DEquipmentUIColors.Rare;
                break;
            case Define.EMaterialGrade.Epic:
            case Define.EMaterialGrade.Epic1:
            case Define.EMaterialGrade.Epic2:
                GetImage((int)Images.MaterialItemBackgroundImage).color = DEquipmentUIColors.Epic;
                break;
            case Define.EMaterialGrade.Legendary:
            case Define.EMaterialGrade.Legendary1:
            case Define.EMaterialGrade.Legendary2:
            case Define.EMaterialGrade.Legendary3:
                GetImage((int)Images.MaterialItemBackgroundImage).color = DEquipmentUIColors.Legendary;
                break;
            default:
                break;
        }
    }

    #region EventHandler
    // 툴팁 호출
    private void OnClickMaterialInfoButton(PointerEventData evt)
    {
        Managers.Sound.PlayButtonClick();
        UI_ToolTipItem item = Managers.UI.MakeSubItem<UI_ToolTipItem>(_makeSubItemParents);
        item.transform.localScale = Vector3.one;
        RectTransform targetPos = this.gameObject.GetComponent<RectTransform>();
        RectTransform parentsCanvas = _makeSubItemParents.gameObject.GetComponent<RectTransform>();
        item.SetInfo(_materialData, targetPos, parentsCanvas);
        item.transform.SetAsLastSibling();
    }
    #endregion
}
