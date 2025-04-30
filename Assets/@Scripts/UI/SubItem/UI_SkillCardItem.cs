using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_SkillCardItem : UI_SubItem
{
    enum GameObjects
    {
        UI_SkillCardItem,
        NewIImageObject,
    }

    enum Texts
    {
        SkillDescriptionText,
        CardNameText
    }

    enum Images
    {
        SkillCardBackgroundImage,
        SkillImage,
        //EvoSkillImage,
        StarOn_0,
        StarOn_1,
        StarOn_2,
        StarOn_3,
        StarOn_4,
        StarOn_5,
    }

    private SkillBase _skill;

    protected override void Awake()
    {
        base.Awake();
        BindObjects(typeof(GameObjects));
        BindTexts(typeof(Texts));
        BindImages(typeof(Images));

        gameObject.BindEvent(OnClicked);
    }

    public void SetInfo(SkillBase skill)
    {
        transform.localScale = Vector3.one;
        GetObject((int)GameObjects.NewIImageObject).gameObject.SetActive(false);

        _skill = skill;
        GetImage((int)Images.SkillImage).sprite = Managers.Resource.Load<Sprite>(skill.UpdateSkillData().IconLabel);
        GetText((int)Texts.CardNameText).text = _skill.SkillData.Name;
        GetText((int)Texts.SkillDescriptionText).text = _skill.SkillData.Description;

        GetImage((int)Images.StarOn_1).gameObject.SetActive(_skill.Level + 1 >= 2);
        GetImage((int)Images.StarOn_2).gameObject.SetActive(_skill.Level + 1 >= 3);
        GetImage((int)Images.StarOn_3).gameObject.SetActive(_skill.Level + 1 >= 4);
        GetImage((int)Images.StarOn_4).gameObject.SetActive(_skill.Level + 1 >= 5);
        GetImage((int)Images.StarOn_5).gameObject.SetActive(_skill.Level + 1 >= 6);
    }

    private void RefreshUI()
    {

    }

    private void OnClicked(PointerEventData evt)
    {
        Managers.Game.Player.Skills.LevelUpSkill(_skill.SkillType);
        Managers.UI.ClosePopupUI();
    }
}
