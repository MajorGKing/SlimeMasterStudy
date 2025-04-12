using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StaminaChargePopup : UI_Popup
{
    #region Enum
    enum GameObjects
    {
        ContentObject,
    }

    enum Buttons
    {
        BackgroundButton,
        BuyDiaButton,
        BuyADButton,
    }

    enum Texts
    {
        BackgroundText,
        BuyADText,
        StaminaChargePopupTitleText,
        DiaRemainingValueText,
        ADRemainingValueText,
        ChargeInfoText,
        ChargeInfoValueText,
        HaveStaminaValueText,
    }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);
        GetButton((int)Buttons.BuyDiaButton).gameObject.BindEvent(OnClickBuyDiaButton);
        GetButton((int)Buttons.BuyDiaButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.BuyADButton).gameObject.BindEvent(OnClickBuyADButton);
        GetButton((int)Buttons.BuyADButton).GetOrAddComponent<UI_ButtonAnimation>();

        RefreshUI();
    }

    private void RefreshUI()
    {
        GetText((int)Texts.HaveStaminaValueText).text = "+1";
        GetText((int)Texts.DiaRemainingValueText).text = $"오늘 남은 횟수 : {Managers.Game.RemainsStaminaByDia}";
        GetText((int)Texts.ADRemainingValueText).text = $"오늘 남은 횟수 : {Managers.Game.StaminaCountAds}";
    }

    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));
        StartCoroutine(CoTimeCheck());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator CoTimeCheck()
    {
        while (true)
        {
            // TODO ILHAK
            //TimeSpan timeSpan = TimeSpan.FromSeconds(Managers.Time.StaminaTime);

            //string formattedTime = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);

            //GetText((int)Texts.ChargeInfoValueText).text = formattedTime;

            yield return new WaitForSeconds(1);
        }
    }


    private void OnClickBackgroundButton(PointerEventData evt)
    {
        Managers.UI.ClosePopupUI(this);
    }

    private void OnClickBuyDiaButton(PointerEventData evt)
    {
        Managers.Sound.PlayButtonClick();
        if (Managers.Game.RemainsStaminaByDia > 0 && Managers.Game.Dia >= 100)
        {
            string[] spriteName = new string[1];
            int[] count = new int[1];

            spriteName[0] = Managers.Data.MaterialDic[Define.ID_STAMINA].SpriteName;
            count[0] = 15;

            UI_RewardPopup rewardPopup = (Managers.UI.SceneUI as UI_LobbyScene).RewardPopupUI;
            rewardPopup.gameObject.SetActive(true);
            Managers.Game.RemainsStaminaByDia--;
            Managers.Game.Dia -= 100;
            Managers.Game.Stamina += 15;
            rewardPopup.SetInfo(spriteName, count);
        }
    }

    private void OnClickBuyADButton(PointerEventData evt)
    {
        Managers.Sound.PlayButtonClick();
        if (Managers.Game.StaminaCountAds > 0)
        {
            // TODO ILHAK
            //Managers.Ads.ShowRewardedAd(() =>
            //{
            //    string[] spriteName = new string[1];
            //    int[] count = new int[1];

            //    spriteName[0] = Managers.Data.MaterialDic[Define.ID_STAMINA].SpriteName;
            //    count[0] = 15;

            //    UI_RewardPopup rewardPopup = (Managers.UI.SceneUI as UI_LobbyScene).RewardPopupUI;
            //    rewardPopup.gameObject.SetActive(true);
            //    Managers.Game.StaminaCountAds--;
            //    Managers.Game.Stamina += 5;
            //    rewardPopup.SetInfo(spriteName, count);
            //});
        }
    }
}
