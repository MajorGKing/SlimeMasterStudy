using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;
using UnityEngine.UI;

public class UI_MergePopup : UI_Popup
{
    [SerializeField]
    public ScrollRect _scrollRect;
    Equipment _equipment;

    Equipment _mergeEquipment1;
    Equipment _mergeEquipment2;

    Define.EEquipmentSortType _equipmentSortType;

    #region Enum
    enum GameObjects
    {
        ContentObject,
        SelectedEquipObject,
        OptionResultObject,
        ImprovATKObject,
        ImprovHPObject,
        FirstCostEquipNeedObject,
        FirstCostEquipSelectObject,
        SecondCostEquipNeedObject,
        SecondCostEquipSelectObject,
        MergeAllButtonRedDotObject,
        EquipInventoryScrollContentObject,

        MurgeStartEffect,
        MurgeFinishEffect,
    }

    enum Buttons
    {
        EquipResultButton,
        FirstCostButton,
        SecondCostButton,
        SortButton,
        MergeAllButton,
        MergeButton,
        BackButton,
    }

    enum Texts
    {
        SelectedEquipLevelValueText,
        SelectedEquipEnforceValueText,
        EquipmentNameText,
        BeforeGradeValueText,
        AfterGradeValueText,
        BeforeLevelValueText,
        AfterLevelValueText,
        ImprovLevelText,
        BeforeATKValueText,
        AfterATKValueText,
        ImprovHPText,
        BeforeHPValueText,
        AfterHPValueText,
        ImprovOptionValueText,
        FirstCostEquipEnforceValueText,
        FirstSelectEquipLevelValueText,
        FirstSelectEquipEnforceValueText,
        SecondSelectEquipLevelValueText,
        SecondSelectEquipEnforceValueText,
        EquipmentTlileText,
        SortButtonText,
        MergeAllButtonText,
        SelectEquipmentCommentText,
        SelectMergeCommentText,
    }

    enum Images
    {
        MergePossibleOutlineImage,
        SelectedEquipGradeBackgroundImage,
        SelectedEquipImage,
        SelectedEquipEnforceBackgroundImage,
        SelectedEquipTypeBackgroundImage,
        SelectedEquipTypeImage,
        LevelArrowImage,
        ATKArrowImage,
        HPArrowImage,
        FirstCostEquipGradeBackgroundImage,
        FirstCostEquipImage,
        FirstCostEquipBackgroundImage,
        FirstSelectEquipGradeBackgroundImage,
        FirstSelectEquipImage,
        FirstSelectEquipEnforceBackgroundImage,
        FirstSelectEquipTypeBackgroundImage,
        FirstSelectEquipTypeImage,
        SecondCostEquipGradeBackgroundImage,
        SecondCostEquipImage,
        SecondSelectEquipGradeBackgroundImage,
        SecondSelectEquipImage,
        SecondSelectEquipEnforceBackgroundImage,
        SecondSelectEquipTypeBackgroundImage,
        SecondSelectEquipTypeImage
    }
    #endregion

    // TODO ILHAK
    protected override void Awake()
    {
        base.Awake();
    }

    public void SetInfo(Equipment equipment)
    {

    }

    private void RefreshUI()
    {

    }

    #region EventHandler

    #endregion
}