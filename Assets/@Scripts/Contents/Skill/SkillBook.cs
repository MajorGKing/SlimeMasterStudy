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
    
    public List<SkillBase> SkillList { get; private set; }  = new List<SkillBase>();
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


    #region 서포트스킬 보너스 추가 
    public void GeneralSupportSkillBonus(SupportSkillData skill)
    {
        //List<SupportSkillData> generalList = SupportSkills.Where(skill => skill.SupportSkillType == SupportSkillType.General).ToList();

        //PlayerController player = Managers.Game.Player;
        //player.CriRate += skill.CriRate;
        //player.MaxHpBonusRate += skill.HpRate;
        //player.ExpBonusRate += skill.ExpBonusRate;
        //player.AttackRate += skill.AtkRate;
        //player.DefRate += skill.DefRate;
        //player.DamageReduction += skill.DamageReduction;
        //player.SoulBonusRate += skill.SoulBonusRate;
        //player.HealBonusRate += skill.HealBonusRate;
        //player.MoveSpeedRate += skill.MoveSpeedRate;
        //player.HpRegen += skill.HpRegen;
        //player.CriDamage += skill.CriDmg;
        //player.CollectDistBonus += skill.MagneticRange;

        //player.UpdatePlayerStat();
    }

    // TODO ILHAK
    public void OnPlayerLevelUpBonus()
    {

    }

    // TODI ILHAK
    public void OnMonsterKillBonus()
    {

    }

    // TODO ILHAK
    public void OnEliteDeadBonus()
    {

    }
    #endregion

    // TODO ILHAK
    #region 스킬 가챠
    public List<SkillBase> RecommendSkills()
    {
        return SkillList;
    }
    #endregion
}
