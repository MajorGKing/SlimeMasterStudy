using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_RewardPopup : UI_Popup
{
    #region Enum
    enum GameObjects
    {
        RewardItemScrollContentObject,
    }

    enum Buttons
    {
        BackgroundButton,
    }

    enum Texts
    {
        RewardPopupTitleText,
        BackgroundText
    }
    #endregion

    public Action OnClosed;
    string[] _spriteName;
    int[] _count;

    protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);

        Managers.Sound.Play(Define.ESound.Effect, "PopupOpen_Reward");
    }

    public void SetInfo(string[] spriteName, int[] count, Action callback = null)
    {
        _spriteName = spriteName;
        _count = count;
        OnClosed = callback;
        RefreshUI();
    }

    private void RefreshUI()
    {
        GetObject((int)GameObjects.RewardItemScrollContentObject).DestroyChildren();
        for (int i = 0; i < _spriteName.Length; i++)
        {
            Debug.Log(_spriteName[i]);
            UI_MaterialItem item = Managers.UI.MakeSubItem<UI_MaterialItem>(GetObject((int)GameObjects.RewardItemScrollContentObject).transform);
            item.SetInfo(_spriteName[i], _count[i]);
        }
    }

    void OnClickBackgroundButton(PointerEventData evt) // 화면 터치하여 닫기
    {
        Managers.Sound.PlayPopupClose();
        Managers.UI.ClosePopupUI(this);
        OnClosed?.Invoke();
    }
}
