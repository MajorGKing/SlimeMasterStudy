using System;
using System.Collections;
using UnityEngine;

public class Charging : SequenceSkill
{
    Coroutine _coroutine;

    protected override void Awake()
    {
        base.Awake();
        SkillType = Define.ESkillType.Charging;
        AnimagtionName = "Charge";
    }

    public override void DoSkill(Action callback = null)
    {
        CreatureController owner = GetComponent<CreatureController>();
        if (owner.CreatureState != Define.ECreatureState.Skill)
            return;

        UpdateSkillData(DataId);

        _coroutine = null;
        _coroutine = StartCoroutine(CoSkill(callback));
    }

    public override void OnChangedSkillData()
    {
    }

    IEnumerator CoSkill(Action callback = null)
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        transform.GetChild(0).GetComponent<Animator>().Play(AnimagtionName);
        yield return new WaitForSeconds(SkillData.AttackInterval);
        callback?.Invoke();
    }
}
