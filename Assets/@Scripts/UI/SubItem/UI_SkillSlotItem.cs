using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SkillSlotItem : UI_SubItem
{
    enum SkillLevelObjects
    {
        SkillLevel_1,
        SkillLevel_2,
        SkillLevel_3,
        SkillLevel_4,
        SkillLevel_5,
        SkillLevel_6,
    }
    enum Texts
    {
        SkillDescriptionText
    }

    enum Images
    {
        BattleSkilIImage,
    }

    protected override void Awake()
    {
        base.Awake();
        BindObjects(typeof(SkillLevelObjects));
        BindImages(typeof(Images));
        gameObject.GetComponent<RectTransform>().localScale = Vector3.one;
    }

    public void SetInfo(string iconLabel, int skillLevel = 1)
    {
        GetImage((int)Images.BattleSkilIImage).sprite = Managers.Resource.Load<Sprite>(iconLabel);

        //별 모두 끄기
        for (int i = 0; i < 6; i++)
        {
            GetObject(i).SetActive(false);
        }

        //스킬레벨만큼 별 켜기
        for (int i = 0; i < skillLevel; i++)
            GetObject(i).SetActive(true);
    }
}
