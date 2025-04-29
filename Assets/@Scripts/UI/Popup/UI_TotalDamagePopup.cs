using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_TotalDamagePopup : UI_Popup
{
    enum GameObjects
    {
        ContentObject,
        TotalDamageContentObject,
    }

    enum Buttons
    {
        BackgroundButton
    }

    enum Texts
    {
        BackgroundText,
        TotalDamagePopupTitleText,
    }

    enum Images
    {

    }

    protected override void Awake()
    {
        base.Awake();
        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));
        BindImages(typeof(Images));

        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickClosePopup);
    }

    public void SetInfo()
    {
        GetObject((int)GameObjects.TotalDamageContentObject).DestroyChildren();
        List<SkillBase> skillList = Managers.Game.Player.Skills.SkillList.ToList();
        foreach (SkillBase skill in skillList.FindAll(skill => skill.IsLearnedSkill))
        {
            UI_SkillDamageItem item = Managers.UI.MakeSubItem<UI_SkillDamageItem>(GetObject((int)GameObjects.TotalDamageContentObject).transform);
            item.SetInfo(skill);
            item.transform.localScale = Vector3.one;
        }
    }

    private void RefreshUI()
    {

    }

    public void OnClickClosePopup(PointerEventData evt)
    {
        Managers.UI.ClosePopupUI(this);
    }
}