using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillDamageItem : UI_SubItem
{
    enum GameObjects
    {
        DamageSliderObject
    }

    enum Texts
    {
        SkillNameValueText,
        SkillDamageValueText,
        DamageProbabilityValueText,
    }

    enum Images
    {
        SkillImage,
    }

    protected override void Awake()
    {
        base.Awake();
        BindObjects(typeof(GameObjects));
        BindTexts(typeof(Texts));
        BindImages(typeof(Images));

        RefreshUI();
    }

    public void SetInfo(SkillBase skill)
    {
        GetImage((int)Images.SkillImage).sprite = Managers.Resource.Load<Sprite>(skill.SkillData.IconLabel);
        GetText((int)Texts.SkillNameValueText).text = $"{skill.SkillData.Name}";
        GetText((int)Texts.SkillDamageValueText).text = $"{skill.TotalDamage}";

        float allSkillDamage = Managers.Game.GetTotalDamage();
        float percentage = skill.TotalDamage / Managers.Game.GetTotalDamage();

        //총 스킬 데미지가 0일때 100%로 표기
        if (allSkillDamage == 0)
            percentage = 1;

        GetText((int)Texts.DamageProbabilityValueText).text = (percentage * 100).ToString("F2") + "%";
        GetObject((int)GameObjects.DamageSliderObject).GetComponent<Slider>().value = percentage;
        
        RefreshUI();
    }

    private void RefreshUI()
    {

    }
}
