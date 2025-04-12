
using Unity.VisualScripting;
using UnityEngine.EventSystems;

public class UI_BackToBattlePopup : UI_Popup
{
    #region Enum
    enum GameObjects
    {
        ContentObject,
    }
    enum Buttons
    {
        ConfirmButton,
        CancelButton,
    }

    enum Texts
    {
        BackToBattleTitleText,
        BackToBattleContentText,
        ConfirmText,
        CancelText,
    }
    #endregion

    protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));

        GetButton((int)Buttons.ConfirmButton).gameObject.BindEvent(OnClickConfirmButton);
        GetButton((int)Buttons.ConfirmButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.CancelButton).gameObject.BindEvent(OnClickCancelButton);
        GetButton((int)Buttons.CancelButton).GetOrAddComponent<UI_ButtonAnimation>();
    }

    #region EventHandler
    private void OnClickConfirmButton(PointerEventData evt)
    {
        Managers.Sound.PlayButtonClick();
        // 이전 플레이하던 게임으로 되돌아가기\
        Managers.Scene.LoadScene(Define.EScene.GameScene, transform);
    }

    private void OnClickCancelButton(PointerEventData evt)
    {
        Managers.Sound.PlayButtonClick();
        Managers.Game.ClearContinueData();
        Managers.UI.ClosePopupUI(this);
    }
    #endregion
}
