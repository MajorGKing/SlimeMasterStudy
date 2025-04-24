using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinShot : SequenceSkill
{
    public float RotateSpeed = 10;

    public GameObject Bullet;
    CreatureController _owner;
    public float SpawnInterval = 0.02f;
    private float _spawnTimer;
    private float _launchCount = 0;
    Vector3 _dir;

    protected override void Awake()
    {
        base.Awake();
        SkillType = Define.ESkillType.SpinShot;
        AnimagtionName = "Attack";
        _owner = GetComponent<CreatureController>();
    }

    public override void DoSkill(Action callback = null)
    {
        CreatureController owner = GetComponent<CreatureController>();
        if (owner.CreatureState != Define.ECreatureState.Skill)
            return;

        UpdateSkillData(DataId);

        _dir = Managers.Game.Player.CenterPosition - _owner.CenterPosition;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        transform.GetChild(0).GetComponent<Animator>().Play(AnimagtionName);
        StartCoroutine(CoSkill(callback));
    }

    public override void OnChangedSkillData()
    {
    }

    IEnumerator CoSkill(Action callback = null)
    {
        while (true)
        {
            _dir = Quaternion.Euler(0, 0, SkillData.RoatateSpeed) * _dir;
            _spawnTimer += Time.deltaTime;
            if (_spawnTimer < SpawnInterval) continue;

            _spawnTimer = 0f;

            Vector3 startPos = _owner.CenterPosition;
            GenerateProjectile(_owner, SkillData.PrefabLabel, startPos, _dir.normalized, Vector3.zero, this);
            _launchCount++;

            if (_launchCount > SkillData.NumProjectiles)
            {
                _launchCount = 0;
                break;
            }

            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(SkillData.AttackInterval);

        callback?.Invoke();
    }

}
