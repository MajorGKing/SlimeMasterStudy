using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : SequenceSkill
{
    Rigidbody2D _rb;
    Coroutine _coroutine;

    CreatureController _owner;

    protected override void Awake()
    {
        base.Awake();
        _owner = GetComponent<CreatureController>();
    }

    public override void DoSkill(Action callback = null)
    {
        UpdateSkillData(DataId);
        if (_coroutine != null)
            StopCoroutine(_coroutine);
        float speed = GetComponent<CreatureController>().MoveSpeed;
        _coroutine = StartCoroutine(CoMove(callback));
    }

    public override void OnChangedSkillData()
    {
    }

    IEnumerator CoMove(Action callback = null)
    {
        _rb = GetComponent<Rigidbody2D>();
        transform.GetChild(0).GetComponent<Animator>().Play(AnimagtionName);
        float elapsed = 0;

        while (true)
        {
            elapsed += Time.deltaTime;
            if (elapsed > 3.0f)
                break;

            Vector3 dir = (Managers.Game.Player.CenterPosition - _owner.CenterPosition).normalized;
            Vector2 targetPosition = Managers.Game.Player.CenterPosition + dir * UnityEngine.Random.Range(SkillData.MinCoverage, SkillData.MaxCoverage);

            if (Vector3.Distance(_rb.position, targetPosition) <= 0.1f)
                continue;

            Vector2 dirVec = targetPosition - _rb.position;
            Vector2 nextVec = dirVec.normalized * SkillData.ProjSpeed * Time.fixedDeltaTime;
            _rb.MovePosition(_rb.position + nextVec);

            yield return null;
        }
        callback?.Invoke();
    }
}
