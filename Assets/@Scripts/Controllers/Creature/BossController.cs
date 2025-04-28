using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonsterController
{
    public float chargingTime = 1f;
    public float rushingTime = 2.0f;
    public float attackingTime = 1.0f;
    private Queue<SkillBase> _skillQueue;

    public Vector2 DashPoint { get; set; }

    protected override void Awake()
    {
        base.Awake();
        transform.localScale = new Vector3(2f, 2f, 2f);
        ObjectType = Define.EObjectType.Boss;
        CreatureState = Define.ECreatureState.Skill;
    }

    public void Start()
    {
        CreatureState = Define.ECreatureState.Skill;
        Skills.StartNextSequenceSkill();
        InvokeMonsterData();
    }

    public override void UpdateAnimation()
    {
        switch (CreatureState)
        {
            case Define.ECreatureState.Idle:
                Anim.Play("Idle");
                break;
            case Define.ECreatureState.Moving:
                Anim.Play("Move");
                break;
            case Define.ECreatureState.Skill:
                break;
            case Define.ECreatureState.Dead:
                Skills.StopSkills();
                break;
        }
    }

    public override void InitCreatureStat(bool isFullHp = true)
    {
        //보스, 플레이어빼고  엘리트, 몬스터만
        MaxHp = (CreatureData.MaxHp + (CreatureData.MaxHpBonus * Managers.Game.CurrentStageData.StageLevel)) * CreatureData.HpRate;
        Atk = (CreatureData.Atk + (CreatureData.AtkBonus * Managers.Game.CurrentStageData.StageLevel)) * CreatureData.AtkRate;
        Hp = MaxHp;
        MoveSpeed = CreatureData.MoveSpeed * CreatureData.MoveSpeedRate;
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        PlayerController target = collision.gameObject.GetComponent<PlayerController>();

        if (target.IsValid() == false)
            return;
        if (this.IsValid() == false)
            return; ;
    }

    public override void OnCollisionExit2D(Collision2D collision)
    {
        base.OnCollisionExit2D(collision);
        PlayerController target = collision.gameObject.GetComponent<PlayerController>();

        if (target.IsValid() == false)
            return;
        if (this.IsValid() == false)
            return; ;
    }
}
