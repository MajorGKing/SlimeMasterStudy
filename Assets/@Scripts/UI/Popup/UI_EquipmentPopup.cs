using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class UI_EquipmentPopup : UI_Popup
{
    #region Enum
    enum GameObjects
    {
        ContentObject,
        WeaponEquipObject, //무기 장착 시 들어갈 부모개체
        GlovesEquipObject, // 장갑 장착 시 들어갈 부모개체
        RingEquipObject, // 반지 장착 시 들어갈 부모개체
        BeltEquipObject, // 헬멧 장착 시 들어갈 부모개체
        ArmorEquipObject, // 갑옷 장착 시 들어갈 부모개체
        BootsEquipObject, // 부츠 장착 시 들어갈 부모개체
        CharacterRedDotObject,
        MergeButtonRedDotObject,
        EquipInventoryObject,
        ItemInventoryObject,
        EquipInventoryGroupObject,
        ItemInventoryGroupObject,
    }

    enum Buttons
    {
        CharacterButton,
        SortButton,
        MergeButton,
    }

    enum Images
    {
        CharacterImage,
    }

    enum Texts
    {
        AttackValueText,
        HealthValueText,
        SortButtonText,
        MergeButtonText,
        EquipInventoryTlileText,
        ItemInventoryTlileText,
    }
    #endregion

    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));
    }

    [SerializeField]
    public ScrollRect ScrollRect;
    Define.EEquipmentSortType _equipmentSortType;

    // 정렬 버튼 텍스트
    string sortText_Level = "정렬 : 레벨";
    string sortText_Grade = "정렬 : 등급";

    protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));
        BindImages(typeof(Images));

        GetObject((int)GameObjects.CharacterRedDotObject).gameObject.SetActive(false);
        GetObject((int)GameObjects.MergeButtonRedDotObject).gameObject.SetActive(false);
        GetButton((int)Buttons.CharacterButton).gameObject.BindEvent(OnClickCharacterButton);
        GetButton((int)Buttons.CharacterButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.CharacterButton).gameObject.SetActive(false); // 출시때 제외

        GetButton((int)Buttons.SortButton).gameObject.BindEvent(OnClickSortButton);
        GetButton((int)Buttons.SortButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.MergeButton).gameObject.BindEvent(OnClickMergeButton);
        GetButton((int)Buttons.MergeButton).GetOrAddComponent<UI_ButtonAnimation>();

        // 정렬 기준 디폴트
        _equipmentSortType = Define.EEquipmentSortType.Level;
        GetText((int)Texts.SortButtonText).text = sortText_Level;
    }

    public void SetInfo()
    {
        RefreshUI();
    }

    private void RefreshUI()
    {
        #region 초기화
        GameObject WeaponContainer = GetObject((int)GameObjects.WeaponEquipObject);
        GameObject GlovesContainer = GetObject((int)GameObjects.GlovesEquipObject);
        GameObject RingContainer = GetObject((int)GameObjects.RingEquipObject);
        GameObject BeltContainer = GetObject((int)GameObjects.BeltEquipObject);
        GameObject ArmorContainer = GetObject((int)GameObjects.ArmorEquipObject);
        GameObject BootsContainer = GetObject((int)GameObjects.BootsEquipObject);

        WeaponContainer.DestroyChildren();
        GlovesContainer.DestroyChildren();
        RingContainer.DestroyChildren();
        BeltContainer.DestroyChildren();
        ArmorContainer.DestroyChildren();
        BootsContainer.DestroyChildren();
        #endregion

        #region 장비
        //1. 장비 리스트를 불러와서 장비인벤토리에 추가
        foreach (Equipment item in Managers.Game.OwnedEquipments)
        {
            //착용중인장비 
            if (item.IsEquipped)
            {
                switch (item.EquipmentData.EquipmentType)
                {
                    case Define.EEquipmentType.Weapon:
                        UI_EquipItem weapon = Managers.UI.MakeSubItem<UI_EquipItem>(WeaponContainer.transform);
                        weapon.SetInfo(item, Define.EUI_ItemParentType.CharacterEquipmentGroup, ScrollRect);
                        break;
                    case Define.EEquipmentType.Boots:
                        UI_EquipItem boots = Managers.UI.MakeSubItem<UI_EquipItem>(BootsContainer.transform);
                        boots.SetInfo(item, Define.EUI_ItemParentType.CharacterEquipmentGroup, ScrollRect);
                        break;
                    case Define.EEquipmentType.Ring:
                        UI_EquipItem ring = Managers.UI.MakeSubItem<UI_EquipItem>(RingContainer.transform);
                        ring.SetInfo(item, Define.EUI_ItemParentType.CharacterEquipmentGroup, ScrollRect);
                        break;
                    case Define.EEquipmentType.Belt:
                        UI_EquipItem belt = Managers.UI.MakeSubItem<UI_EquipItem>(BeltContainer.transform);
                        belt.SetInfo(item, Define.EUI_ItemParentType.CharacterEquipmentGroup, ScrollRect);
                        break;
                    case Define.EEquipmentType.Armor:
                        UI_EquipItem armor = Managers.UI.MakeSubItem<UI_EquipItem>(ArmorContainer.transform);
                        armor.SetInfo(item, Define.EUI_ItemParentType.CharacterEquipmentGroup, ScrollRect);
                        break;
                    case Define.EEquipmentType.Gloves:
                        UI_EquipItem gloves = Managers.UI.MakeSubItem<UI_EquipItem>(GlovesContainer.transform);
                        gloves.SetInfo(item, Define.EUI_ItemParentType.CharacterEquipmentGroup, ScrollRect);
                        break;
                }
            }
        }
        SortEquipments();
        #endregion

        #region 캐릭터
        // 공격력,HP 설정
        var (hp, attack) = Managers.Game.GetCurrentChracterStat();
        // TODO ILHAK
        //GetText((int)Texts.AttackValueText).text = (Managers.Game.CurrentCharacter.Atk + attack).ToString();
        //GetText((int)Texts.HealthValueText).text = (Managers.Game.CurrentCharacter.MaxHp + hp).ToString();
        #endregion

        SetItem();

        // 리프레시 버그 대응ItemInventoryObject 
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.EquipInventoryObject).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.ItemInventoryObject).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.EquipInventoryGroupObject).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.ItemInventoryGroupObject).GetComponent<RectTransform>());
    }

    private void SortEquipments()
    {
        // TODO ILHAK
    }

    private void SetItem()
    {
        // TODO ILHAK
    }

    #region EventHandler
    private void OnClickCharacterButton(PointerEventData evt)
    {
        Managers.Sound.PlayButtonClick();
        // TODO ILHAK
        //Managers.UI.ShowPopupUI<UI_CharacterSelectPopup>();
    }

    private void OnClickSortButton(PointerEventData evt)
    {
        Managers.Sound.PlayButtonClick();

        // 레벨로 정렬, 등급으로 정렬 누를때마다 정렬방식 변경
        if (_equipmentSortType == Define.EEquipmentSortType.Level)
        {
            _equipmentSortType = Define.EEquipmentSortType.Grade;
            GetText((int)Texts.SortButtonText).text = sortText_Grade;
        }

        else if (_equipmentSortType == Define.EEquipmentSortType.Grade)
        {
            _equipmentSortType = Define.EEquipmentSortType.Level;
            GetText((int)Texts.SortButtonText).text = sortText_Level;
        }

        SortEquipments();
    }

    private void OnClickMergeButton(PointerEventData evt)
    {
        Managers.Sound.PlayButtonClick();
        UI_MergePopup mergePopupUI = (Managers.UI.SceneUI as UI_LobbyScene).MergePopupUI;
        mergePopupUI.SetInfo(null);
        mergePopupUI.gameObject.SetActive(true);
    }
    #endregion
}
