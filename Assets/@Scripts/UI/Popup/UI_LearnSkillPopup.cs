using UnityEngine;
using UnityEngine.EventSystems;

public class UI_LearnSkillPopup : UI_Popup
{
    enum GameObjects
    {
        ContentObject,
    }
    enum Buttons
    {
        BackgroundButton,
    }

    enum Texts
    {
        BackgroundText,
        LearnSkillCommentText,
        SkillDescriptionText,
        CardNameText
    }
    enum Images
    {
        SkillCardBackgroundImage,
        SkillImage,
        StarOn_0,
        StarOn_1,
        StarOn_2,
        StarOn_3,
        StarOn_4,
        StarOn_5
    }

    SkillBase _skill;

    protected override void Awake()
    {
        base.Awake();
        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));
        BindImages(typeof(Images));
        GetButton((int)Buttons.BackgroundButton).gameObject.BindEvent(OnClickBackgroundButton);
    }

    public void SetInfo()
    {
        //배우고있는 스킬 중 하나 레벨업 시켜준다.
        int index = UnityEngine.Random.Range(0, Managers.Game.Player.Skills.ActivatedSkills.Count);
        _skill = Managers.Game.Player.Skills.RecommandDropSkill();

        if (_skill != null)
        {
            Managers.Game.Player.Skills.LevelUpSkill(_skill.SkillType);
        }
        else
        {
            Managers.UI.ClosePopupUI(this);
        }

        GetImage((int)Images.SkillImage).sprite = Managers.Resource.Load<Sprite>(_skill.SkillData.IconLabel);
        GetText((int)Texts.CardNameText).text = _skill.SkillData.Name;
        GetText((int)Texts.SkillDescriptionText).text = _skill.SkillData.Description;
        GetImage((int)Images.StarOn_1).gameObject.SetActive(_skill.Level >= 2);
        GetImage((int)Images.StarOn_2).gameObject.SetActive(_skill.Level >= 3);
        GetImage((int)Images.StarOn_3).gameObject.SetActive(_skill.Level >= 4);
        GetImage((int)Images.StarOn_4).gameObject.SetActive(_skill.Level >= 5);
        GetImage((int)Images.StarOn_5).gameObject.SetActive(_skill.Level >= 6);

        RefreshUI();
    }

    private void RefreshUI()
    {

    }

    private void OnClickBackgroundButton(PointerEventData evt)
    {
        Managers.UI.ClosePopupUI(this);
    }
}
