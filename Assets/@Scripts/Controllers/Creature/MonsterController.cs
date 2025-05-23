using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;
using Sequence = DG.Tweening.Sequence;
using Random = UnityEngine.Random;

public class MonsterController : CreatureController
{
    float _timer;

    Data.SkillData skillData;

    Coroutine _coMoving;
    Coroutine _coDotDamage;
    public event Action OnBossDead;

    public event Action<MonsterController> MonsterInfoUpdate;

    Vector3 _moveDir;

    Coroutine _coroutineKnockback;

    protected override void Awake()
    {
        base.Awake();

        ObjectType = Define.EObjectType.Monster;
        CreatureState = Define.ECreatureState.Moving;

        _rigidBody.simulated = true;
        transform.localScale = new Vector3(1f, 1f, 1f);

        SetMonsterPosition();
        //꺼졌다 켜졌을때 초기화
        if (Skills)
        {
            foreach (SkillBase skill in Skills.SkillList)
            {
                skill.Level = 0;
                skill.UpdateSkillData();
            }
        }

        if (CreatureData != null)
        {
            if (CreatureData.SkillTypeList.Count != 0)
            {
                InitSkill();
                Skills.LevelUpSkill((Define.ESkillType)CreatureData.SkillTypeList[0]);
            }
        }
    }

    private void OnEnable()
    {
        if (DataId != 0)
            SetInfo(DataId);
    }

    void FixedUpdate()
    {
        PlayerController pc = Managers.Object.Player;

        if (pc.IsValid() == false)
            return;

        _moveDir = pc.transform.position - transform.position;
        CreatureSprite.flipX = _moveDir.x > 0;

        if (CreatureState != Define.ECreatureState.Moving)
            return;

        Vector3 newPos = transform.position + _moveDir.normalized * Time.deltaTime * MoveSpeed;
        _rigidBody.MovePosition(newPos);

        _timer += Time.deltaTime;
    }

    public void SetMonsterPosition()
    {
        Vector2 randCirclePos = Utils.GenerateMonsterSpawnPosition(Managers.Game.Player.PlayerCenterPos);
        transform.position = (randCirclePos);
    }

    public override void UpdateAnimation()
    {
        base.UpdateAnimation();

        switch (CreatureState)
        {
            case Define.ECreatureState.Idle:
                UpdateIdle();
                break;
            case Define.ECreatureState.Skill:
                UpdateSkill();
                break;
            case Define.ECreatureState.Moving:
                UpdateMoving();
                break;
            case Define.ECreatureState.Dead:
                UpdateDead();
                break;
        }
    }

    public override void OnDead()
    {
        base.OnDead();

        InvokeMonsterData();

        Managers.Game.Player.KillCount++;
        // TODO After Achievement
        // Managers.Game.TotalMonsterKillCount++;

        //gem
        if (Random.value >= Managers.Game.CurrentWaveData.nonDropRate)
        {
            GemController gem = Managers.Object.Spawn<GemController>(transform.position);
            gem.SetInfo(Managers.Game.GetGemInfo());
        }

        //영혼획득량 확률 
        if (Random.value < Define.STAGE_SOULDROP_RATE)
        {
            SoulController soul = Managers.Object.Spawn<SoulController>(transform.position);
        }

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(0f, 0.2f).SetEase(Ease.InOutBounce)).OnComplete(() =>
        {
            StopAllCoroutines();
            _coroutineKnockback = null;
            _rigidBody.velocity = Vector2.zero;
            OnBossDead?.Invoke();
            Managers.Object.Despawn(this);
        });
    }

    public override void OnDamaged(BaseController attacker, SkillBase skill, float damage = 0)
    {
        if (skill != null)
        {
            Managers.Sound.Play(Define.ESound.Effect, skill.SkillData.HitSoundLabel);
        }
        float totalDmg = Managers.Game.Player.Atk * skill.SkillData.DamageMultiplier;
        base.OnDamaged(attacker, skill, totalDmg);
        InvokeMonsterData();
        if (ObjectType == Define.EObjectType.Monster)
        {
            if (_coroutineKnockback == null)
            {
                _coroutineKnockback = StartCoroutine(CoKnockBack());
            }
        }
    }

    IEnumerator CoKnockBack()
    {
        float elapsed = 0;
        CreatureState = Define.ECreatureState.OnDamaged;
        while (true)
        {
            elapsed += Time.deltaTime;
            if (elapsed > Define.KNOCKBACK_TIME)
                break;

            Vector3 dir = _moveDir * -1f;
            Vector2 nextVec = dir.normalized * Define.KNOCKBACK_SPEED * Time.fixedDeltaTime;
            _rigidBody.MovePosition(_rigidBody.position + nextVec);

            yield return null;
        }
        CreatureState = Define.ECreatureState.Moving;

        yield return new WaitForSeconds(Define.KNOCKBACK_COOLTIME);
        _coroutineKnockback = null;
        yield break;
    }

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
        Managers.Object.Despawn(this);
    }
}
