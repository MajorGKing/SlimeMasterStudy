using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO ILHAK
public class MonsterController : CreatureController
{
    float _timer;

    Data.SkillData skillData;

    Coroutine _coMoving;
    Coroutine _coDotDamage;
    public event Action OnBossDead;

    public event Action<MonsterController> MonsterInfoUpdate;

    public void InvokeMonsterData()
    {
        if (this.IsValid() && gameObject.IsValid() && ObjectType != Define.EObjectType.Monster)
        {
            MonsterInfoUpdate?.Invoke(this);
        }
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController target = collision.gameObject.GetComponent<PlayerController>();

        if (target.IsValid() == false)
            return;
        if (this.IsValid() == false)
            return; ;

        if (_coDotDamage != null)
            StopCoroutine(_coDotDamage);

        _coDotDamage = StartCoroutine(CoStartDotDamage(target));
    }

    public virtual void OnCollisionExit2D(Collision2D collision)
    {
        PlayerController target = collision.gameObject.GetComponent<PlayerController>();
        if (target.IsValid() == false)
            return;
        if (this.IsValid() == false)
            return;

        if (_coDotDamage != null)
            StopCoroutine(_coDotDamage);
        _coDotDamage = null;
    }

    public IEnumerator CoStartDotDamage(PlayerController target)
    {
        while (true)
        {
            target.OnDamaged(this, null, Atk);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public override void OnDeathAnimationEnd()
    {
        throw new NotImplementedException();
    }
}
