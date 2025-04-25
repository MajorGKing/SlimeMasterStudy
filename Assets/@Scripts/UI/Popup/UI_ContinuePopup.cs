using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_ContinuePopup : UI_Popup
{
    enum GameObjects
    {
        ContentObject
    }
    enum Texts
    {
        ContinuePopupTitleText,
        CountdownValueText,
        ContinueButtonText,
        ContinueCostValueText,
        ADContinueText,
    }

    enum Buttons
    {
        CloseButton,
        ContinueButton,
        ADContinueButton,
    }

    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));
    }

    protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindTexts(typeof(Texts));
        BindButtons(typeof(Buttons));
        GetButton((int)Buttons.CloseButton).gameObject.BindEvent(OnClickCloseButton);
        GetButton((int)Buttons.CloseButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.ContinueButton).gameObject.BindEvent(OnClickContinueButton);
        GetButton((int)Buttons.ContinueButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.ADContinueButton).gameObject.BindEvent(OnClickADContinueButton);
        GetButton((int)Buttons.ADContinueButton).GetOrAddComponent<UI_ButtonAnimation>();
        RefreshUI();
    }

    protected override void Start()
    {
        StartCoroutine(CountdownCoroutine());
    }

    public void SetInfo()
    {
        RefreshUI();
    }

    void RefreshUI()
    {
        if(Managers.Game.ItemDictionary.TryGetValue(Define.ID_BRONZE_KEY, out int keyCount) == true)
        {
            GetText((int)Texts.ContinueCostValueText).text = $"1/{keyCount}";
        }
        else
        {
            GetText((int)Texts.ContinueCostValueText).text = $"<color=red>0</color>";
        }

        // 리프레시 버그 대응
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetButton((int)Buttons.ADContinueButton).gameObject.GetComponent<RectTransform>());
    }

    private void OnClickCloseButton(PointerEventData evt)
    {
        Managers.UI.ClosePopupUI(this);
        Managers.Game.GameOver();
    }

    private void OnClickContinueButton(PointerEventData evt)
    {
        Managers.Sound.PlayButtonClick();

        if (Managers.Game.ItemDictionary.TryGetValue(Define.ID_BRONZE_KEY, out int keyCount) == true)
        {
            Managers.Game.RemovMaterialItem(Define.ID_BRONZE_KEY, 1);
            Managers.Game.Player.Resurrection(1);
            Managers.UI.ClosePopupUI(this);
        }
    }

    private void OnClickADContinueButton(PointerEventData evt)
    {
        // TODO ILHAK AD

        // TEMP
        Managers.UI.ClosePopupUI(this);
        Managers.Game.GameOver();
    }
}
