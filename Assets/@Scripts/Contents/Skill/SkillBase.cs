using System;
using UnityEngine;

[Serializable]
public class SkillStat
{
    public Define.ESkillType SkillType;
    public int Level;
    public float MaxHp;
    public Data.SkillData SkillData;
}

public class SkillBase : BaseController
{
    public CreatureController Owner { get; set; }
    public Define.ESkillType SkillType { get; set; }

    public int Level { get; set; }

    [SerializeField]
    public Data.SkillData _skillData;
    public Data.SkillData SkillData
    {
        get
        {
            return _skillData;
        }
        set
        {
            _skillData = value;
        }
    }

    public float TotalDamage { get; set; } = 0;
    public bool IsLearnedSkill { get { return Level > 0; } }

    //protected override void Awake()
    //{
    //    base.Awake();
    //}

    public Data.SkillData UpdateSkillData(int dataId = 0)
    {

        int id = 0;
        if (dataId == 0)
            id = Level < 2 ? (int)SkillType : (int)SkillType + Level - 1;
        else
            id = dataId;

        Data.SkillData skillData = new Data.SkillData();

        if (Managers.Data.SkillDic.TryGetValue(id, out skillData) == false)
            return SkillData;

        foreach (Data.SupportSkillData support in Managers.Game.Player.Skills.SupportSkills)
        {
            if (SkillType.ToString() == support.SupportSkillName.ToString())
            {
                skillData.ProjectileSpacing += support.ProjectileSpacing;
                skillData.Duration += support.Duration;
                skillData.NumProjectiles += support.NumProjectiles;
                skillData.AttackInterval += support.AttackInterval;
                skillData.NumBounce += support.NumBounce;
                skillData.ProjRange += support.ProjRange;
                skillData.RoatateSpeed += support.RoatateSpeed;
                skillData.ScaleMultiplier += support.ScaleMultiplier;
                skillData.NumPenerations += support.NumPenerations;
            }
        }

        SkillData = skillData;
        OnChangedSkillData();
        return SkillData;
    }

    public virtual void OnChangedSkillData() { }

    // TODO ILHAK SKill 0->1
    public virtual void ActivateSkill()
    {
        UpdateSkillData();
    }

    public virtual void OnLevelUp()
    {
        if (Level == 0)
            ActivateSkill();
        Level++;
        UpdateSkillData();
    }

    protected virtual void GenerateProjectile(CreatureController Owner, string prefabName, Vector3 startPos, Vector3 dir, Vector3 targetPos, SkillBase skill)
    {
        ProjectileController pc = Managers.Object.Spawn<ProjectileController>(startPos, prefabName: prefabName);
        pc.SetInfo(Owner, startPos, dir, targetPos, skill);
    }

    protected void HitEvent(Collider2D collision)
    {

    }

}
