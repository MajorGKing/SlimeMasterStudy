using Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using static Define;

public class UI_GachaListPopup : UI_Popup
{
    #region UI 기능 리스트
    // 정보 갱신
    // GachaInfoContentObject : 장비 확률을 표시할 GachaGradeListItem이 들어갈 부모 개체

    // 로컬라이징
    // GachaListPopupTitleText : 상품 확률

    #endregion

    #region enum
    enum GameObjects
    {
        ContentObject,
        GachaInfoContentObject,
        CommonGachaGradeRateItem,
        CommonGachaRateListObject,
        UncommonGachaGradeRateItem,
        UncommonGachaRateListObject,
        RareGachaGradeRateItem,
        RareGachaRateListObject,
        EpicGachaGradeRateItem,
        EpicGachaRateListObject,
    }
    enum Buttons
    {
        BackgroundButton,
    }

    enum Texts
    {
        GachaListPopupTitleText,
        CommonGradeTitleText,
        CommonGradeRateValueText,
        UncommonGradeTitleText,
        UncommonGradeRateValueText,
        RareGradeTitleText,
        RareGradeRateValueText,
        EpicGradeTitleText,
        EpicGradeRateValueText,
    }
    #endregion

    Define.EGachaType _gachaType;

    protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);
    }

    public void SetInfo(Define.EGachaType gachaType)
    {
        _gachaType = gachaType;
        RefreshUI();
    }

    private void RefreshUI()
    {
        if (_gachaType == Define.EGachaType.None)
            return;

        float commonRate = 0f;
        float uncommonRate = 0f;
        float rareRate = 0f;
        float epicRate = 0f;

        GetObject((int)GameObjects.CommonGachaRateListObject).transform.DestroyChildren();
        GetObject((int)GameObjects.UncommonGachaRateListObject).transform.DestroyChildren();
        GetObject((int)GameObjects.RareGachaRateListObject).transform.DestroyChildren();
        GetObject((int)GameObjects.EpicGachaRateListObject).transform.DestroyChildren();

        List<GachaRateData> list = Managers.Data.GachaTableDataDic[_gachaType].GachaRateTable.ToList();
        list.Reverse();

        foreach (GachaRateData item in Managers.Data.GachaTableDataDic[_gachaType].GachaRateTable)
        {
            switch (Managers.Data.EquipDataDic[item.EquipmentID].EquipmentGrade)
            {
                case Define.EEquipmentGrade.Common:
                    commonRate += item.GachaRate;
                    UI_GachaRateItem commonItem = Managers.Resource.Instantiate("UI_GachaRateItem", pooling: true).GetOrAddComponent<UI_GachaRateItem>();
                    commonItem.transform.SetParent(GetObject((int)GameObjects.CommonGachaRateListObject).transform);
                    commonItem.SetInfo(item);
                    break;

                case Define.EEquipmentGrade.Uncommon:
                    uncommonRate += item.GachaRate;
                    UI_GachaRateItem uncommonItem = Managers.Resource.Instantiate("UI_GachaRateItem", pooling: true).GetOrAddComponent<UI_GachaRateItem>();
                    uncommonItem.transform.SetParent(GetObject((int)GameObjects.UncommonGachaRateListObject).transform);
                    uncommonItem.SetInfo(item);
                    break;

                case Define.EEquipmentGrade.Rare:
                    rareRate += item.GachaRate;
                    UI_GachaRateItem rareItem = Managers.Resource.Instantiate("UI_GachaRateItem", pooling: true).GetOrAddComponent<UI_GachaRateItem>();
                    rareItem.transform.SetParent(GetObject((int)GameObjects.RareGachaRateListObject).transform);
                    rareItem.SetInfo(item);
                    break;

                case Define.EEquipmentGrade.Epic:
                    epicRate += item.GachaRate;
                    UI_GachaRateItem epicItem = Managers.Resource.Instantiate("UI_GachaRateItem", pooling: true).GetOrAddComponent<UI_GachaRateItem>();
                    epicItem.transform.SetParent(GetObject((int)GameObjects.EpicGachaRateListObject).transform);
                    epicItem.SetInfo(item);
                    break;
            }
        }

        GetText((int)Texts.CommonGradeRateValueText).text = commonRate.ToString("P2");
        GetText((int)Texts.UncommonGradeRateValueText).text = uncommonRate.ToString("P2");
        GetText((int)Texts.RareGradeRateValueText).text = rareRate.ToString("P2");
        GetText((int)Texts.EpicGradeRateValueText).text = epicRate.ToString("P2");
        gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));
    }

    #region EventHandler
    void OnClickBackgroundButton(PointerEventData evt)
    {
        Managers.Sound.PlayPopupClose();
        Managers.UI.ClosePopupUI(this);
    }
    #endregion
}
