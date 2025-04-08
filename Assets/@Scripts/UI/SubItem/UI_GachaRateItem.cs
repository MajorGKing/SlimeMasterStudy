using UnityEngine;
using Data;

public class UI_GachaRateItem : UI_SubItem
{
    #region enum
    enum Texts
    {
        EquipmentNameValueText,
        EquipmentReteValueText,
    }

    enum Images
    {
        BackgroundImage,
    }
    #endregion

    GachaRateData _gachaRateData;

    private void OnEnable()
    {
    }

    protected override void Awake()
    {
        base.Awake();

        BindTexts(typeof(Texts));
        BindImages(typeof(Images));
    }

    public void SetInfo(GachaRateData gachaRateData)
    {
        _gachaRateData = gachaRateData;

        RefreshUI();
        transform.localScale = Vector3.one;
    }

    void RefreshUI()
    {
        string weaponName = Managers.Data.EquipDataDic[_gachaRateData.EquipmentID].NameTextID;
        GetText((int)Texts.EquipmentNameValueText).text = weaponName;
        GetText((int)Texts.EquipmentReteValueText).text = _gachaRateData.GachaRate.ToString("P2");
        switch (_gachaRateData.EquipGrade)
        {
            case Define.EEquipmentGrade.Common:
                GetImage((int)Images.BackgroundImage).color = DEquipmentUIColors.Common;
                break;
            case Define.EEquipmentGrade.Uncommon:
                GetImage((int)Images.BackgroundImage).color = DEquipmentUIColors.Uncommon;
                break;
            case Define.EEquipmentGrade.Rare:
                GetImage((int)Images.BackgroundImage).color = DEquipmentUIColors.Rare;
                break;
            case Define.EEquipmentGrade.Epic:
                GetImage((int)Images.BackgroundImage).color = DEquipmentUIColors.Epic;
                break;
            default:
                break;
        }
    }
}
