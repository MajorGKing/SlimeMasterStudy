using Data;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ShopPopup : UI_Popup
{
    #region enum
    enum GameObjects
    {
        ContentObject,
        ShopScrollContent,

        //StagePackageContentObject,

        //PickupGuaranteedCountSliderObject,
        //AdvancedGuaranteedCountSliderObject,

        //FreeDiaSoldOutObject,
        //FreeDiaRedDotObject,

        FreeGoldSoldOutObject,
        FreeGoldRedDotObject,

        //FirstDiaProductBonusObject,
        //SecondDiaProductBonusObject,
        //ThirdDiaProductBonusObject,
        //FourthDiaProductBonusObject,
        //FifthDiaProductBonusObject,

        //OpenTenCostObject,
        //OpenOneCostObject,
        CommonGachaCostObject,
        AdvancedGachaCostObject,
        //StagePackageCostObject,

        AdKeySoldOutObject,
        AdKeyRedDotObject,

        // 출시모델 제외
        //DailyShopTitle,
        //DailyShopGroup,
        //PickupGachaGroup,

    }

    enum Buttons
    {
        //StagePackageButton,
        //StagePackagePrevButton,
        //StagePackageNextButton,
        //BeginnerPackageButton,

        //PickupGachaInfoButton,
        //PickupGachaListButton,
        //OpenOneButton,
        //OpenTenButton,
        CommonGachaOpenButton,
        ADCommonGachaOpenButton,
        ADAdvancedGachaOpenButton,
        CommonGachaListButton,
        AdvancedGachaOpenButton,
        AdvancedGachaOpenTenButton,
        AdvancedGachaListButton,
        //FreeDiaButton,
        //FirstDiaProductButton,
        //SecondDiaProductButton,
        //ThirdDiaProductButton,
        //FourthDiaProductButton,
        //FifthDiaProductButton,

        AdKeyButton,
        SilverKeyProductButton,
        GoldKeyProductButton,

        FreeGoldButton,
        FirstGoldProductButton,
        SecondGoldProductButton,
    }

    enum Texts
    {
        //PackageTitleText,
        //DailyShopTitleText,
        //RefreshTimerText,
        //RefreshTimerValueText,
        GachaTitleText,
        //DiaShopTitleText,
        //DiaShopCommentText,
        KeyShopTitleText,
        GoldShopTitleText,

        //StagePackageTitleText,
        //StagePackageItemTitleText,
        //PackageFirstProductItemCountValueText,
        //PackageSecondProductItemCountValueText,
        //PackageThirdProductItemCountValueText,
        //PackageFourthProductItemCountValueText,
        //StagePackageCostValueText,

        //BeginnerPackageTitleText,
        //BeginnerPackageBuyLimitText,
        //BeginnerPackageDiscountText,

        //PickupGachaTitleText,
        //PickupGachaCommentText,
        //PickupGachaGuaranteedCountText,
        //PickupGachaGuaranteedCountValueText,

        CommonGachaTitleText,
        CommonGachaOpenButtonText,
        CommonGachaCostValueText,

        AdvancedGachaTitleText,
        //AdvancedGachaGuaranteedCountText,
        //AdvancedGachaGuaranteedCountValueText,
        AdvancedGachaOpenButtonText,
        AdvancedGachaCostValueText,
        AdvancedGachaTenCostValueText,
        //BuyLimitText,
        //BuyLimitValueText,
        //OpenOneButtonText,
        //OpenTenButtonText,

        //FreeDiaRequestTimeValueText,
        //FirstDiaCostValueText,
        //SecondDiaCostValueText, 
        //ThirdDiaCostValueText, 
        //FourthDiaCostValueText, 
        //FifthDiaCostValueText,

        FreeGoldRequestTimeValueText,
        AdKeyRequestTimeValueText,

        // 골드 상점
        FirstGoldProductTitleText,
        FreeGoldTitleText,
        SecondGoldProductTitleText
    }

    enum Images
    {
        //BeginnerPackageFirstlItemImage,
    }
    #endregion
    ScrollRect _scrollRect;

    UI_GachaListPopup _gachaListPopupUI;

    public UI_GachaListPopup GachaListPopupUI
    {
        get
        {
            if (_gachaListPopupUI == null)
            {
                _gachaListPopupUI = Managers.UI.ShowPopupUI<UI_GachaListPopup>();
            }
            return _gachaListPopupUI;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));
        BindImages(typeof(Images));

        // 패키지 상점
        //GetButton((int)Buttons.StagePackageButton).gameObject.BindEvent(OnClickStagePackageButton);
        //GetButton((int)Buttons.StagePackageButton).GetOrAddComponent<UI_ButtonAnimation>();
        //GetButton((int)Buttons.StagePackagePrevButton).gameObject.BindEvent(OnClickStagePackagePrevButton);
        //GetButton((int)Buttons.StagePackagePrevButton).GetOrAddComponent<UI_ButtonAnimation>();
        //GetButton((int)Buttons.StagePackageNextButton).gameObject.BindEvent(OnClickStagePackageNextButton);
        //GetButton((int)Buttons.StagePackageNextButton).GetOrAddComponent<UI_ButtonAnimation>();
        //GetButton((int)Buttons.BeginnerPackageButton).gameObject.BindEvent(OnClickBeginnerPackageButton);
        //GetButton((int)Buttons.BeginnerPackageButton).GetOrAddComponent<UI_ButtonAnimation>();

        // 가챠 상점
        //GetButton((int)Buttons.PickupGachaInfoButton).gameObject.BindEvent(OnClickPickupGachaInfoButton);
        //GetButton((int)Buttons.PickupGachaInfoButton).GetOrAddComponent<UI_ButtonAnimation>();
        //GetButton((int)Buttons.PickupGachaListButton).gameObject.BindEvent(OnClickGachaListButton);
        //GetButton((int)Buttons.PickupGachaListButton).GetOrAddComponent<UI_ButtonAnimation>();
        //GetButton((int)Buttons.OpenOneButton).gameObject.BindEvent(OnClickOpenOneButton);
        //GetButton((int)Buttons.OpenOneButton).GetOrAddComponent<UI_ButtonAnimation>();
        //GetButton((int)Buttons.OpenTenButton).gameObject.BindEvent(OnClickOpenTenButton);
        //GetButton((int)Buttons.OpenTenButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.CommonGachaOpenButton).gameObject.BindEvent(OnClickCommonGachaOpenButton);
        GetButton((int)Buttons.CommonGachaOpenButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.ADCommonGachaOpenButton).gameObject.BindEvent(OnClickADCommonGachaOpenButton);
        GetButton((int)Buttons.ADCommonGachaOpenButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.ADAdvancedGachaOpenButton).gameObject.BindEvent(OnClickADAdvancedGachaOpenButton);
        GetButton((int)Buttons.ADAdvancedGachaOpenButton).GetOrAddComponent<UI_ButtonAnimation>();

        GetButton((int)Buttons.CommonGachaListButton).gameObject.BindEvent(OnClickCommonGachaListButton);
        GetButton((int)Buttons.CommonGachaListButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.AdvancedGachaOpenButton).gameObject.BindEvent(OnClickAdvancedGachaOpenButton);
        GetButton((int)Buttons.AdvancedGachaOpenButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.AdvancedGachaOpenTenButton).gameObject.BindEvent(OnClickAdvancedGachaOpenTenButton);
        GetButton((int)Buttons.AdvancedGachaOpenTenButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.AdvancedGachaListButton).gameObject.BindEvent(OnClickAdvancedGachaListButton);
        GetButton((int)Buttons.AdvancedGachaListButton).GetOrAddComponent<UI_ButtonAnimation>();

        // 출시모델 제외

        //GetObject((int)GameObjects.DailyShopTitle).gameObject.SetActive(false);
        //GetObject((int)GameObjects.DailyShopGroup).gameObject.SetActive(false);
        //GetObject((int)GameObjects.PickupGachaGroup).gameObject.SetActive(false);
        //GetObject((int)GameObjects.AdvancedGuaranteedCountSliderObject).gameObject.SetActive(false);


        // 다이아 상점
        //GetObject((int)GameObjects.FreeDiaSoldOutObject).gameObject.SetActive(false);
        //GetObject((int)GameObjects.FreeDiaRedDotObject).gameObject.SetActive(false);
        //GetButton((int)Buttons.FreeDiaButton).gameObject.BindEvent(OnClickFreeDiaButton);
        //GetButton((int)Buttons.FreeDiaButton).GetOrAddComponent<UI_ButtonAnimation>();
        //GetButton((int)Buttons.FirstDiaProductButton).gameObject.BindEvent(OnClickFirstDiaProductButton);
        //GetButton((int)Buttons.FirstDiaProductButton).GetOrAddComponent<UI_ButtonAnimation>();
        //GetButton((int)Buttons.SecondDiaProductButton).gameObject.BindEvent(OnClickSecondDiaProductButton);
        //GetButton((int)Buttons.SecondDiaProductButton).GetOrAddComponent<UI_ButtonAnimation>();
        //GetButton((int)Buttons.ThirdDiaProductButton).gameObject.BindEvent(OnClickThirdDiaProductButton);
        //GetButton((int)Buttons.ThirdDiaProductButton).GetOrAddComponent<UI_ButtonAnimation>();
        //GetButton((int)Buttons.FourthDiaProductButton).gameObject.BindEvent(OnClickFourthDiaProductButton);
        //GetButton((int)Buttons.FourthDiaProductButton).GetOrAddComponent<UI_ButtonAnimation>();
        //GetButton((int)Buttons.FifthDiaProductButton).gameObject.BindEvent(OnClickFifthDiaProductButton);
        //GetButton((int)Buttons.FifthDiaProductButton).GetOrAddComponent<UI_ButtonAnimation>();

        // 첫구매 보너스 처리
        //GetObject((int)GameObjects.FirstDiaProductBonusObject).gameObject.SetActive(false); // 첫구매 이력이 있다면 비활성화
        //GetObject((int)GameObjects.SecondDiaProductBonusObject).gameObject.SetActive(false); // 첫구매 이력이 있다면 비활성화
        //GetObject((int)GameObjects.ThirdDiaProductBonusObject).gameObject.SetActive(false); // 첫구매 이력이 있다면 비활성화
        //GetObject((int)GameObjects.FourthDiaProductBonusObject).gameObject.SetActive(false); // 첫구매 이력이 있다면 비활성화
        //GetObject((int)GameObjects.FifthDiaProductBonusObject).gameObject.SetActive(false); // 첫구매 이력이 있다면 비활성화

        // 열쇠 상점
        GetObject((int)GameObjects.AdKeySoldOutObject).gameObject.SetActive(false);
        GetObject((int)GameObjects.AdKeyRedDotObject).gameObject.SetActive(false);
        GetButton((int)Buttons.AdKeyButton).gameObject.BindEvent(OnClickAdKeyButton);
        GetButton((int)Buttons.AdKeyButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.SilverKeyProductButton).gameObject.BindEvent(OnClickSilverKeyProductButton);
        GetButton((int)Buttons.SilverKeyProductButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.GoldKeyProductButton).gameObject.BindEvent(OnClickGoldKeyProductButton);
        GetButton((int)Buttons.GoldKeyProductButton).GetOrAddComponent<UI_ButtonAnimation>();

        // 골드 상점
        GetObject((int)GameObjects.FreeGoldSoldOutObject).gameObject.SetActive(false);
        GetObject((int)GameObjects.FreeGoldRedDotObject).gameObject.SetActive(false);
        GetButton((int)Buttons.FreeGoldButton).gameObject.BindEvent(OnClickFreeGoldButton);
        GetButton((int)Buttons.FreeGoldButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.FirstGoldProductButton).gameObject.BindEvent(OnClickFirstGoldProductButton);
        GetButton((int)Buttons.FirstGoldProductButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.SecondGoldProductButton).gameObject.BindEvent(OnClickSecondGoldProductButton);
        GetButton((int)Buttons.SecondGoldProductButton).GetOrAddComponent<UI_ButtonAnimation>();
    }

    public void SetInfo(ScrollRect scrollRect)
    {
        _scrollRect = scrollRect;
        RefreshUI();
    }

    private void RefreshUI()
    {
        // TODO ILHAK
        //Managers.Game.ItemDictionary.TryGetValue(Define.ID_GOLD_KEY, out int goldKeyCount);
        //Managers.Game.ItemDictionary.TryGetValue(Define.ID_SILVER_KEY, out int silverKeyCount);
        //Managers.Game.ItemDictionary.TryGetValue(Define.ID_BRONZE_KEY, out int bronzeKeyCount);

        // TODO ILHAK
        //GetText((int)Texts.CommonGachaCostValueText).text = $"{silverKeyCount}/1";
        //GetText((int)Texts.AdvancedGachaCostValueText).text = $"{goldKeyCount}/1";
        //GetText((int)Texts.AdvancedGachaTenCostValueText).text = $"{goldKeyCount}/10";
        //GetButton((int)Buttons.ADAdvancedGachaOpenButton).gameObject.SetActive(Managers.Game.GachaCountAdsAnvanced > 0);
        //GetButton((int)Buttons.ADCommonGachaOpenButton).gameObject.SetActive(Managers.Game.GachaCountAdsCommon > 0);
        //GetObject((int)GameObjects.FreeGoldSoldOutObject).SetActive(Managers.Game.GoldCountAds == 0); // 솔드아웃 표시
        //GetObject((int)GameObjects.AdKeySoldOutObject).SetActive(Managers.Game.BronzeKeyCountAds == 0);

        int goldAmount = 0;
        // TODO ILHAK
        //if (Managers.Data.OfflineRewardDataDic.TryGetValue(Managers.Game.GetMaxStageIndex(), out OfflineRewardData offlineReward))
        //{
        //    goldAmount = offlineReward.Reward_Gold;
        //}

        GetText((int)Texts.FreeGoldTitleText).text = $"{goldAmount}";
        GetText((int)Texts.FirstGoldProductTitleText).text = $"{goldAmount * 3}";
        GetText((int)Texts.SecondGoldProductTitleText).text = $"{goldAmount * 5}";

        #region 리프레스 버그 대응
        // 리프레시 버그 대응
        //LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.StagePackageCostObject).GetComponent<RectTransform>());
        //LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.OpenTenCostObject).GetComponent<RectTransform>());
        //LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.OpenOneCostObject).GetComponent<RectTransform>());
        //LayoutRebuilder.ForceRebuildLayoutImmediate(GetButton((int)Buttons.OpenTenButton).gameObject.GetComponent<RectTransform>());
        //LayoutRebuilder.ForceRebuildLayoutImmediate(GetButton((int)Buttons.OpenOneButton).gameObject.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.CommonGachaCostObject).GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.AdvancedGachaCostObject).GetComponent<RectTransform>());
        #endregion
    }

    #region EventHandler
    private void OnClickCommonGachaOpenButton(PointerEventData evt)
    {
        Managers.Sound.PlayButtonClick();
    }

    private void OnClickADCommonGachaOpenButton(PointerEventData evt)
    {

    }

    private void OnClickADAdvancedGachaOpenButton(PointerEventData evt)
    {

    }

    private void OnClickCommonGachaListButton(PointerEventData evt)
    {

    }    
    
    private void OnClickAdvancedGachaOpenButton(PointerEventData evt)
    {

    }

    private void OnClickAdvancedGachaOpenTenButton(PointerEventData evt)
    {

    }

    private void OnClickAdvancedGachaListButton(PointerEventData evt)
    {

    }

    private void OnClickAdKeyButton(PointerEventData evt)
    {

    }

    private void OnClickSilverKeyProductButton(PointerEventData evt)
    {

    }

    private void OnClickGoldKeyProductButton(PointerEventData evt)
    {

    }

    private void OnClickFreeGoldButton(PointerEventData evt)
    {

    }

    private void OnClickFirstGoldProductButton(PointerEventData evt)
    {

    }

    private void OnClickSecondGoldProductButton(PointerEventData evt)
    {

    }
    #endregion
}
