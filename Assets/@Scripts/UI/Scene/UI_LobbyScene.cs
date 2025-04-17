using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    public UI_BattlePopup BattlePopupUI { get; private set; }
    bool _isSelectedBattle = false;
    //UI_EvolvePopup _evolvePopupUI;
    public UI_EquipmentPopup EquipmentPopupUI { get; private set; }
    bool _isSelectedEquipment = false;
    public UI_ShopPopup ShopPopupUI { get; private set; }
    bool _isSelectedShop = false;
    //UI_ChallengePopup _challengePopupUI;
    public UI_MergePopup MergePopupUI { get; private set; }
    public UI_EquipmentInfoPopup EquipmentInfoPopupUI { get; private set; }
    public UI_EquipmentResetPopup EquipmentResetPopupUI {  get; private set; }
    public UI_RewardPopup RewardPopupUI { get; private set; }
    public UI_MergeResultPopup MergeResultPopupUI { get; private set; }

    public void OnDestroy()
    {
        if (Managers.Game != null)
            Managers.Game.OnResourcesChagned -= RefreshUI;
    }

    protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));
        BindToggles(typeof(Toggles));
        BindImages(typeof(Images));

        // 토글 클릭 시 행동
        GetToggle((int)Toggles.ShopToggle).gameObject.BindEvent(OnClickShopToggle);
        GetToggle((int)Toggles.EquipmentToggle).gameObject.BindEvent(OnClickEquipmentToggle);
        GetToggle((int)Toggles.BattleToggle).gameObject.BindEvent(OnClickBattleToggle);

        ShopPopupUI = Managers.UI.ShowPopupUI<UI_ShopPopup>();
        EquipmentPopupUI = Managers.UI.ShowPopupUI<UI_EquipmentPopup>();
        BattlePopupUI = Managers.UI.ShowPopupUI<UI_BattlePopup>();
        //_challengePopupUI = Managers.UI.ShowPopupUI<UI_ChallengePopup>();
        //_evolvePopupUI = Managers.UI.ShowPopupUI<UI_EvolvePopup>();
        EquipmentInfoPopupUI = Managers.UI.ShowPopupUI<UI_EquipmentInfoPopup>();
        MergePopupUI = Managers.UI.ShowPopupUI<UI_MergePopup>();
        EquipmentResetPopupUI = Managers.UI.ShowPopupUI<UI_EquipmentResetPopup>();
        RewardPopupUI = Managers.UI.ShowPopupUI<UI_RewardPopup>();
        MergeResultPopupUI = Managers.UI.ShowPopupUI<UI_MergeResultPopup>();

        //토글에 따른 ContentObject.SetActive()를 위한 오브젝트
        TogglesInit();
        GetToggle((int)Toggles.BattleToggle).gameObject.GetComponent<Toggle>().isOn = true;
        OnClickBattleToggle();


        Managers.Game.OnResourcesChagned += RefreshUI;
        RefreshUI();
    }

    public void SetInfo()
    {

    }

    private void RefreshUI()
    {
        GetText((int)Texts.StaminaValueText).text = $"{Managers.Game.Stamina}/{Define.MAX_STAMINA}";
        GetText((int)Texts.DiaValueText).text = Managers.Game.Dia.ToString();
        GetText((int)Texts.GoldValueText).text = Managers.Game.Gold.ToString();

        // 토글 선택 시 리프레시 버그 대응
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.MenuToggleGroup).GetComponent<RectTransform>());
    }

    private void TogglesInit()
    {
        ShopPopupUI.gameObject.SetActive(false);
        EquipmentPopupUI.gameObject.SetActive(false);
        BattlePopupUI.gameObject.SetActive(false);
        //_challengePopupUI.gameObject.SetActive(false);
        //_evolvePopupUI.gameObject.SetActive(false);
        EquipmentInfoPopupUI.gameObject.SetActive(false);
        MergePopupUI.gameObject.SetActive(false);
        EquipmentResetPopupUI.gameObject.SetActive(false);
        RewardPopupUI.gameObject.SetActive(false);
        MergeResultPopupUI.gameObject.SetActive(false);

        // 재 클릭 방지 트리거 초기화
        _isSelectedEquipment = false;
        _isSelectedShop = false;
        _isSelectedBattle = false;

        // 버튼 레드닷 초기화
        GetObject((int)GameObjects.ShopToggleRedDotObject).SetActive(false);
        GetObject((int)GameObjects.EquipmentToggleRedDotObject).SetActive(false);
        GetObject((int)GameObjects.BattleToggleRedDotObject).SetActive(false);

        // 선택 토글 아이콘 초기화
        GetObject((int)GameObjects.CheckShopImageObject).SetActive(false);
        GetObject((int)GameObjects.CheckEquipmentImageObject).SetActive(false);
        GetObject((int)GameObjects.CheckBattleImageObject).SetActive(false);

        GetObject((int)GameObjects.CheckShopImageObject).GetComponent<RectTransform>().sizeDelta = new Vector2(200, 155);
        GetObject((int)GameObjects.CheckEquipmentImageObject).GetComponent<RectTransform>().sizeDelta = new Vector2(200, 155);
        GetObject((int)GameObjects.CheckBattleImageObject).GetComponent<RectTransform>().sizeDelta = new Vector2(200, 155);

        // 메뉴 텍스트 초기화
        GetText((int)Texts.ShopToggleText).gameObject.SetActive(false);
        GetText((int)Texts.EquipmentToggleText).gameObject.SetActive(false);
        GetText((int)Texts.BattleToggleText).gameObject.SetActive(false);

        // 토글 크기 초기화
        GetToggle((int)Toggles.ShopToggle).GetComponent<RectTransform>().sizeDelta = new Vector2(200, 150);
        GetToggle((int)Toggles.EquipmentToggle).GetComponent<RectTransform>().sizeDelta = new Vector2(200, 150);
        GetToggle((int)Toggles.BattleToggle).GetComponent<RectTransform>().sizeDelta = new Vector2(200, 150);
    }

    void ShowUI(GameObject contentPopup, Toggle toggle, TMP_Text text, GameObject obj2, float duration = 0.1f)
    {
        TogglesInit();

        contentPopup.SetActive(true);
        toggle.GetComponent<RectTransform>().sizeDelta = new Vector2(280, 150);
        text.gameObject.SetActive(true);
        obj2.SetActive(true);
        obj2.GetComponent<RectTransform>().DOSizeDelta(new Vector2(200, 180), duration).SetEase(Ease.InOutQuad);

        RefreshUI();
    }

    private void OnClickBattleToggle()
    {
        Managers.Sound.PlayButtonClick();
        GetImage((int)Images.Backgroundimage).color = Utils.HexToColor("1F5FA0"); // 배경 색상 변경
        if (_isSelectedBattle == true) // 활성화 후 토글 클릭 방지
            return;
        ShowUI(BattlePopupUI.gameObject, GetToggle((int)Toggles.BattleToggle), GetText((int)Texts.BattleToggleText), GetObject((int)GameObjects.CheckBattleImageObject));
        _isSelectedBattle = true;
    }

    private void OnClickShopToggle(PointerEventData evt)
    {
        Managers.Sound.PlayButtonClick();
        GetImage((int)Images.Backgroundimage).color = Utils.HexToColor("525DAD"); // 배경 색상 변경
        if (_isSelectedShop == true) // 활성화 후 토글 클릭 방지
            return;
        ShowUI(ShopPopupUI.gameObject, GetToggle((int)Toggles.ShopToggle), GetText((int)Texts.ShopToggleText), GetObject((int)GameObjects.CheckShopImageObject));
        _isSelectedShop = true;
    }

    private void OnClickEquipmentToggle(PointerEventData evt)
    {
        Managers.Sound.PlayButtonClick();
        GetImage((int)Images.Backgroundimage).color = Utils.HexToColor("5C254B"); // 배경 색상 변경
        if (_isSelectedEquipment == true) // 활성화 후 토글 클릭 방지
            return;

        ShowUI(EquipmentPopupUI.gameObject, GetToggle((int)Toggles.EquipmentToggle), GetText((int)Texts.EquipmentToggleText), GetObject((int)GameObjects.CheckEquipmentImageObject));
        _isSelectedEquipment = true;

        EquipmentPopupUI.SetInfo();
    }

    private void OnClickBattleToggle(PointerEventData evt)
    {
        OnClickBattleToggle();
    }
}
