
using UnityEngine.EventSystems;

public class UI_BeginnerSupportRewardPopup : UI_Popup
{
    #region Enum
    enum GameObjects
    {
        ContentObject,
    }
    enum Buttons
    {
        BackgroundButton,
    }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));

        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);
    }

    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));
    }

    #region EventHandler
    void OnClickBackgroundButton(PointerEventData evt)
    {
        Managers.UI.ClosePopupUI(this);
    }
    #endregion
}
