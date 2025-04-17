using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_LobbyScene : UI_Scene
{
    #region Enum
    enum GameObjects
    {
        ShopToggleRedDotObject, // 알림 상황 시 사용 할 레드닷
        EquipmentToggleRedDotObject,
        BattleToggleRedDotObject,
        //ChallengeToggleRedDotObject,
        //EvolveToggleRedDotObject,

        MenuToggleGroup,
        CheckShopImageObject,
        CheckEquipmentImageObject,
        CheckBattleImageObject,
        //CheckChallengeImageObject,
        //CheckEvolveImageObject,
    }

    enum Buttons
    {
        //UserIconButton, // 추후 유저 정보 팝업 만들어서 호출
    }

    enum Texts
    {
        ShopToggleText,
        EquipmentToggleText,
        BattleToggleText,
        //ChallengeToggleText,
        //EvolveToggleText,
        //UserNameText,
        //UserLevelText,
        StaminaValueText,
        DiaValueText,
        GoldValueText
    }

    enum Toggles
    {
        ShopToggle,
        EquipmentToggle,
        BattleToggle,
        //ChallengeToggle,
        //EvolveToggle,
    }

    enum Images
    {
        Backgroundimage,
    }
    #endregion

    UI_BattlePopup _battlePopupUI;
    bool _isSelectedBattle = false;
    //UI_EvolvePopup _evolvePopupUI;
    public UI_EquipmentPopup EquipmentPopupUI { get; private set; }
    bool _isSelectedEquipment = false;
    UI_ShopPopup _shopPopupUI;
    bool _isSelectedShop = false;
    //UI_ChallengePopup _challengePopupUI;
    public UI_MergePopup MergePopupUI { get; private set; }
    public UI_EquipmentInfoPopup EquipmentInfoPopupUI { get; private set; }
    public UI_RewardPopup RewardPopupUI { get; private set; }
}
