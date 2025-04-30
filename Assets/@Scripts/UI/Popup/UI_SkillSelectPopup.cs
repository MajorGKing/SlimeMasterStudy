using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkillSelectPopup : UI_Popup
{
    enum GameObjects
    {
        ContentObject,
        SkillCardSelectListObject,
        ExpSliderObject,
        DisabledObject,
        CharacterLevelObject,
    }
    enum Buttons
    {
        CardRefreshButton,
        ADRefreshButton,
    }

    enum Texts
    {
        SkillSelectCommentText,
        SkillSelectTitleText,
        CardRefreshText,
        CardRefreshCountValueText,
        ADRefreshText,

        CharacterLevelupTitleText,

        CharacterLevelValueText,
        BeforeLevelValueText,
        AfterLevelValueText,
    }

    enum Images
    {
        BattleSkilI_Icon_0,
        BattleSkilI_Icon_1,
        BattleSkilI_Icon_2,
        BattleSkilI_Icon_3,
        BattleSkilI_Icon_4,
        BattleSkilI_Icon_5,
    }

    GameManager _game;

    protected override void Awake()
    {
        base.Awake();
        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));
        BindImages(typeof(Images));

        GetButton((int)Buttons.CardRefreshButton).gameObject.BindEvent(OnClickCardRefreshButton);
        GetButton((int)Buttons.ADRefreshButton).gameObject.BindEvent(OnClickADRefreshButton);

        GetObject((int)GameObjects.DisabledObject).gameObject.SetActive(false);

        _game = Managers.Game;

        RefreshUI();

        SetRecommendSkills();
        List<SkillBase> activeSkills = Managers.Game.Player.Skills.SkillList.Where(skill => skill.IsLearnedSkill).ToList();

        for (int i = 0; i < activeSkills.Count; i++)
        {
            SetCurrentSkill(i, activeSkills[i]);
        }
        Managers.Sound.Play(Define.ESound.Effect, "PopupOpen_SkillSelect");
    }

    private void OnEnable()
    {
        PopupOpenAnimation(GetObject((int)GameObjects.ContentObject));
    }

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            SetRecommendSkills();
        }

    }
#endif

    public void SetInfo()
    {

    }

    private void RefreshUI()
    {
        GetText((int)Texts.CharacterLevelValueText).text = $"{_game.Player.Level}";
        GetText((int)Texts.BeforeLevelValueText).text = $"Lv.{_game.Player.Level - 1}";
        GetText((int)Texts.AfterLevelValueText).text = $"Lv.{_game.Player.Level}";

        if (Managers.Game.Player.SkillRefreshCount > 0)
            GetText((int)Texts.CardRefreshCountValueText).text = $"남은 횟수 : {Managers.Game.Player.SkillRefreshCount}";
        else
            GetText((int)Texts.CardRefreshCountValueText).text = $"<color=red>남은 횟수 : 0</color>";


        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.CharacterLevelObject).GetComponent<RectTransform>());
    }

    private void SetRecommendSkills()
    {
        GameObject container = GetObject((int)GameObjects.SkillCardSelectListObject);
        //초기화
        container.DestroyChildren();
        List<SkillBase> List = Managers.Game.Player.Skills.RecommendSkills();

        foreach (SkillBase skill in List)
        {
            UI_SkillCardItem item = Managers.UI.MakeSubItem<UI_SkillCardItem>(container.transform);
            item.GetComponent<UI_SkillCardItem>().SetInfo(skill);
        }
    }

    private void SetCurrentSkill(int index, SkillBase skill)
    {
        GetImage(index).sprite = Managers.Resource.Load<Sprite>(skill.SkillData.IconLabel);
        GetImage(index).enabled = true;
    }

    private void OnClickCardRefreshButton(PointerEventData evt)
    {
        Managers.Sound.PlayButtonClick();
        if (Managers.Game.Player.SkillRefreshCount > 0)
        {
            SetRecommendSkills();
            Managers.Game.Player.SkillRefreshCount--;
        }
        RefreshUI();
    }

    private void OnClickADRefreshButton(PointerEventData evt)
    {
        // TODO ILHAK after ad
        //Managers.Sound.PlayButtonClick();
        //if (Managers.Game.SkillRefreshCountAds > 0)
        //{
        //    Managers.Ads.ShowRewardedAd(() =>
        //    {
        //        Managers.Game.SkillRefreshCountAds--;
        //        SetRecommendSkills();
        //    });
        //}
        //else
        //{
        //    Managers.UI.ShowToast("더이상 사용 할 수 없습니다.");
        //}
    }
}
