using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EgoSword : RepeatSkill
{
    [SerializeField]
    private ParticleSystem[] _swingParticle;
    float _radian;

    protected override void Awake()
    {
        base.Awake();
        SkillType = Define.ESkillType.EgoSword;
    }

    public override void ActivateSkill()
    {
        base.ActivateSkill();
    }

    public override void OnLevelUp()
    {
        base.OnLevelUp();
    }

    IEnumerator SwingSword()
    {
        if (Managers.Game.Player != null)
        {
            Vector3 dir = Managers.Game.Player.PlayerDirection;
            Shoot(dir);
        }
        yield return null;
    }

    private void Shoot(Vector3 dir)
    {
        string prefabName = SkillData.PrefabLabel;
        Vector3 startPos = Managers.Game.Player.PlayerCenterPos;

        for (int i = 0; i < SkillData.NumProjectiles; i++)
        {
            float angle = SkillData.AngleBetweenProj * (i - (SkillData.NumProjectiles - 1) / 2f);
            Vector3 res = Quaternion.AngleAxis(angle, Vector3.forward) * dir;
            GenerateProjectile(Managers.Game.Player, prefabName, startPos, res.normalized, Vector3.zero, this);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.IsLearnedSkill == false)
            return;
        CreatureController creature = collision.transform.GetComponent<CreatureController>();
        if (creature?.IsMonster() == true)
            creature.OnDamaged(Managers.Game.Player, this);
    }

    protected override void DoSkillJob()
    {
        StartCoroutine(SwingSword());
    }

    public override void OnChangedSkillData()
    {
    }
}
