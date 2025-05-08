using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Define;
//using static Define;

public class SkillBook : MonoBehaviour
{
    [SerializeField]
    
    public List<SkillBase> SkillList { get; private set; } = new List<SkillBase>();
    public List<SequenceSkill> SequenceSkills { get; } = new List<SequenceSkill>();
    public List<SupportSkillData> LockedSupportSkills { get; } = new List<SupportSkillData>();
    public List<SupportSkillData> SupportSkills = new List<SupportSkillData>();

    public List<SkillBase> ActivatedSkills
    {
        get { return SkillList.Where(skill => skill.IsLearnedSkill).ToList(); }
    }

    [SerializeField]
    public Dictionary<Define.ESkillType, int> SavedBattleSkill = new Dictionary<Define.ESkillType, int>();

    public event Action UpdateSkillUi;
    public Define.EObjectType _ownerType;

    int _sequenceIndex = 0;
    bool _stopped = false;

    public void Awake()
    {
        _ownerType = GetComponent<CreatureController>().ObjectType;
    }

    public void SetInfo(Define.EObjectType type)
    {
        _ownerType = type;
    }

    public void LoadSkill(Define.ESkillType skillType, int level)
    {
        //모든스킬은 0으로 시작함. 레벨 수 만큼 레벨업ㅎ ㅏ기
        AddSkill(skillType);
        for (int i = 0; i < level; i++)
            LevelUpSkill(skillType);
    }

    public void AddSkill(Define.ESkillType skillType, int skillId = 0)
    {
        string className = skillType.ToString();

        if (skillType == Define.ESkillType.FrozenHeart || skillType == Define.ESkillType.SavageSmash || skillType == Define.ESkillType.EletronicField)
        {
            GameObject go = Managers.Resource.Instantiate(skillType.ToString(), gameObject.transform);
            if (go != null)
            {
                SkillBase skill = go.GetOrAddComponent<SkillBase>();
                SkillList.Add(skill);
                if (SavedBattleSkill.ContainsKey(skillType))
                    SavedBattleSkill[skillType] = skill.Level;
                else
                    SavedBattleSkill.Add(skillType, skill.Level);
            }
        }
        else
        {
            // AddComponent만 하면됌
            SequenceSkill skill = gameObject.AddComponent(Type.GetType(className)) as SequenceSkill;
            if (skill != null)
            {
                skill.ActivateSkill();
                skill.Owner = GetComponent<CreatureController>();
                skill.DataId = skillId;
                SkillList.Add(skill);
                SequenceSkills.Add(skill);
            }
            else
            {
                RepeatSkill skillbase = gameObject.GetComponent(Type.GetType(className)) as RepeatSkill;
                SkillList.Add(skillbase);
                if (SavedBattleSkill.ContainsKey(skillType))
                    SavedBattleSkill[skillType] = skillbase.Level;
                else
                    SavedBattleSkill.Add(skillType, skillbase.Level);
            }
        }
    }

    public void LevelUpSkill(Define.ESkillType skillType)
    {
        for (int i = 0; i < SkillList.Count; i++)
        {
            if (SkillList[i].SkillType == skillType)
            {
                SkillList[i].OnLevelUp();
                if (SavedBattleSkill.ContainsKey(skillType))
                {
                    SavedBattleSkill[skillType] = SkillList[i].Level;
                }
                UpdateSkillUi?.Invoke();
            }
        }
    }

    public void AddActiavtedSkills(SkillBase skill)
    {
        ActivatedSkills.Add(skill);
    }

    public void StartNextSequenceSkill()
    {
        if (_stopped)
            return;
        if (SequenceSkills.Count == 0)
            return;

        SequenceSkills[_sequenceIndex].DoSkill(OnFinishedSequenceSkill);
    }

    void OnFinishedSequenceSkill()
    {
        _sequenceIndex = (_sequenceIndex + 1) % SequenceSkills.Count;
        StartNextSequenceSkill();
    }

    public void StopSkills()
    {
        _stopped = true;

        foreach (var skill in ActivatedSkills)
        {
            skill.StopAllCoroutines();
        }
    }

    public void AddSupportSkill(SupportSkillData skill, bool isLoadSkill = false)
    {
        #region 서포트스킬 중복가능
        skill.IsPurchased = true;

        //1. 스킬 등록 없이 바로 끝내는 것들
        if (skill.SupportSkillName == Define.ESupportSkillName.Healing)
        {
            Managers.Game.Player.Healing(skill.HealRate);
            return;
        }

        SupportSkills.Add(skill);
        UpdateSkillUi?.Invoke();


        // 이미 적용된 값을 가지고 있으니 스킬데이타를 업데이트 하지않고 Add 시킨후 UI에만 추가한다.
        if (isLoadSkill == true)
            return;

        if (skill.SupportSkillType == Define.ESupportSkillType.General)
        {
            GeneralSupportSkillBonus(skill);
        }
        else if (skill.SupportSkillType == Define.ESupportSkillType.Special)
        {
            //배틀스킬에 영향을 미치는 스킬인경우 UpdateskilLData();
            foreach (SkillBase playerSkill in SkillList)
            {
                if (skill.SupportSkillName.ToString() == playerSkill.SkillType.ToString())
                {
                    playerSkill.UpdateSkillData();
                }
            }
        }

        #endregion
    }

    public void OnSkillBookChanged()
    {
        UpdateSkillUi?.Invoke();
    }

    public void Clear()
    {
        SavedBattleSkill.Clear();
        SupportSkills.Clear();
    }


    #region 서포트스킬 보너스 추가 
    public void GeneralSupportSkillBonus(SupportSkillData skill)
    {
        List<SupportSkillData> generalList = SupportSkills.Where(skill => skill.SupportSkillType == Define.ESupportSkillType.General).ToList();

        PlayerController player = Managers.Game.Player;
        player.CriRate += skill.CriRate;
        player.MaxHpBonusRate += skill.HpRate;
        player.ExpBonusRate += skill.ExpBonusRate;
        player.AttackRate += skill.AtkRate;
        player.DefRate += skill.DefRate;
        player.DamageReduction += skill.DamageReduction;
        player.SoulBonusRate += skill.SoulBonusRate;
        player.HealBonusRate += skill.HealBonusRate;
        player.MoveSpeedRate += skill.MoveSpeedRate;
        player.HpRegen += skill.HpRegen;
        player.CriDamage += skill.CriDmg;
        player.CollectDistBonus += skill.MagneticRange;

        player.UpdatePlayerStat();
    }

    public void OnPlayerLevelUpBonus()
    {
        List<SupportSkillData> passiveSkills = SupportSkills.Where(skill => skill.SupportSkillType == Define.ESupportSkillType.LevelUp).ToList();

        float moveRate = 0;
        float atkRate = 0;
        float criRate = 0;
        float cridmg = 0;
        float reduceDamage = 0;

        foreach (SupportSkillData passive in passiveSkills)
        {
            if (passive.SupportSkillName == Define.ESupportSkillName.Resurrection)
                continue;
            moveRate += passive.MoveSpeedRate;
            atkRate += passive.AtkRate;
            criRate += passive.CriRate;
            cridmg += passive.CriDmg;
            reduceDamage += passive.DamageReduction;
        }

        PlayerController player = Managers.Game.Player;
        player.MoveSpeedRate += moveRate;
        player.AttackRate += atkRate;
        player.CriRate += criRate;
        player.CriDamage += cridmg;
        player.DamageReduction += reduceDamage;

        player.UpdatePlayerStat();
    }

    public void OnMonsterKillBonus()
    {
        List<SupportSkillData> passiveSkills = SupportSkills.Where(skill => skill.SupportSkillType == Define.ESupportSkillType.MonsterKill).ToList();
        float dmgReduction = 0;
        float atkRate = 0;
        float healAmount = 0;
        foreach (SupportSkillData passive in passiveSkills)
        {
            if (passive.SupportSkillName == Define.ESupportSkillName.Resurrection)
                continue;
            dmgReduction += passive.DamageReduction;
            atkRate += passive.AtkRate;
            healAmount += passive.HealRate;
        }

        PlayerController player = Managers.Game.Player;
        player.DamageReduction += dmgReduction;
        player.AttackRate += atkRate;

        player.UpdatePlayerStat();
        Managers.Game.Player.Healing(healAmount);
    }

    public void OnEliteDeadBonus()
    {
        List<SupportSkillData> passiveSkills = SupportSkills.Where(skill => skill.SupportSkillType == Define.ESupportSkillType.EliteKill).ToList();

        float soulCount = 0;
        float expBonus = 0;

        foreach (SupportSkillData passive in passiveSkills)
        {
            if (passive.SupportSkillName == Define.ESupportSkillName.Resurrection)
                continue;
            soulCount += passive.SoulAmount;
            expBonus += passive.ExpBonusRate;
        }

        PlayerController player = Managers.Game.Player;
        player.SoulCount += soulCount;
        player.ExpBonusRate += expBonus;
    }
    #endregion

    #region 스킬 가챠
    public SkillBase RecommandDropSkill()
    {
        List<SkillBase> skillList = Managers.Game.Player.Skills.SkillList.ToList();
        List<SkillBase> activeSkills = skillList.FindAll(skill => skill.IsLearnedSkill);

        List<SkillBase> recommendSkills = activeSkills.FindAll(s => s.Level < 5);
        recommendSkills.Shuffle();
        
        return recommendSkills[0];
    }

    public List<SkillBase> RecommendSkills()
    {
        List<SkillBase> skillList = Managers.Game.Player.Skills.SkillList.ToList();
        List<SkillBase> activeSkills = skillList.FindAll(skill => skill.IsLearnedSkill);

        //1. 이미 6개의 스킬을 배웠으면 배운 스킬중 5렙 미만인 스킬을 추천
        if (activeSkills.Count == MAX_SKILL_COUNT)
        {
            List<SkillBase> recommendSkills = activeSkills.FindAll(s => s.Level < MAX_SKILL_LEVEL);
            recommendSkills.Shuffle();
            //Util.Shuffle(recommandSkills);
            return recommendSkills.Take(3).ToList();
        }
        else
        {
            // 레벨이 5 미만인 스킬 
            List<SkillBase> recommendSkills = skillList.FindAll(s => s.Level < MAX_SKILL_LEVEL);
            recommendSkills.Shuffle();
            //Util.Shuffle(recommandSkills);
            return recommendSkills.Take(3).ToList();
        }
    }

    public List<SupportSkillData> RecommendSupportkills()
    {
        GameManager game = Managers.Game;
        game.SoulShopList.Clear();

        //1. Lock 된 스킬이 있는지 확인하고 추가하기
        foreach (SupportSkillData skill in LockedSupportSkills)
        {
            skill.IsLocked = true;
            game.SoulShopList.Add(skill);
        }

        int recommandCount = 4 - game.SoulShopList.Count;

        for (int i = 0; i < recommandCount; i++)
        {
            //1. 등급 가챠
            Define.ESupportSkillGrade grade = GetRandomGrade();
            // 2. 해당 등급 스킬 목록 가져오기
            List<SupportSkillData> skills = GetSupportSkills(grade);

            if (skills.Count > 0)
                game.SoulShopList.Add(skills[UnityEngine.Random.Range(0, skills.Count)]);
            else
                AddRecommendSkills(grade);
        }

        return game.SoulShopList;
    }

    public static Define.ESupportSkillGrade GetRandomGrade()
    {
        float randomValue = UnityEngine.Random.value;
        if (randomValue < SUPPORTSKILL_GRADE_PROB[(int)Define.ESupportSkillGrade.Common])
        {
            return Define.ESupportSkillGrade.Common;
        }
        else if (randomValue < SUPPORTSKILL_GRADE_PROB[(int)Define.ESupportSkillGrade.Common] + SUPPORTSKILL_GRADE_PROB[(int)Define.ESupportSkillGrade.Uncommon])
        {
            return Define.ESupportSkillGrade.Uncommon;
        }
        else if (randomValue < SUPPORTSKILL_GRADE_PROB[(int)Define.ESupportSkillGrade.Common] + SUPPORTSKILL_GRADE_PROB[(int)Define.ESupportSkillGrade.Uncommon] + SUPPORTSKILL_GRADE_PROB[(int)Define.ESupportSkillGrade.Epic])
        {
            return Define.ESupportSkillGrade.Epic;
        }
        else if (randomValue < SUPPORTSKILL_GRADE_PROB[(int)Define.ESupportSkillGrade.Common] + SUPPORTSKILL_GRADE_PROB[(int)Define.ESupportSkillGrade.Uncommon] + SUPPORTSKILL_GRADE_PROB[(int)Define.ESupportSkillGrade.Epic] + SUPPORTSKILL_GRADE_PROB[(int)Define.ESupportSkillGrade.Rare])
        {
            return Define.ESupportSkillGrade.Rare;
        }
        else
        {
            return Define.ESupportSkillGrade.Legend;
        }
    }



    private List<SupportSkillData> GetSupportSkills(Define.ESupportSkillGrade grade)
    {
        return Managers.Data.SupportSkillDic.Values
                .Where(skill => skill.SupportSkillGrade == grade && skill.CheckRecommendationCondition())
                .ToList();
    }

    private void AddRecommendSkills(Define.ESupportSkillGrade grade)
    {
        if ((int)grade > Enum.GetValues(typeof(Define.ESupportSkillGrade)).Length)
            return;
        List<SupportSkillData> commonSkills = new List<SupportSkillData>();
        Define.ESupportSkillGrade nextGrade = grade + 1;

        // 2. 해당 등급 스킬 목록 가져오기
        commonSkills = GetSupportSkills(nextGrade);

        if (commonSkills.Count > 0)
            Managers.Game.SoulShopList.Add(commonSkills[UnityEngine.Random.Range(0, commonSkills.Count)]);
        else
            AddRecommendSkills(nextGrade);
    }
    #endregion
}
