using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : RepeatSkill
{
    protected override void Awake()
    {
        base.Awake();
        SkillType = Define.ESkillType.Shuriken;
    }

    public override void ActivateSkill()
    {
        base.ActivateSkill();
    }

    public override void OnChangedSkillData()
    {
    }

    protected override void DoSkillJob()
    {
        StartCoroutine(SetShuriken(SkillType));
    }

    IEnumerator SetShuriken(Define.ESkillType type)
    {
        string prefabName = SkillData.PrefabLabel;

        if (Managers.Game.Player != null)
        {
            List<MonsterController> target = Managers.Object.GetMonsterWithinCamera(SkillData.NumProjectiles);

            if (target == null)
                yield break;
            for (int i = 0; i < target.Count; i++)
            {
                if (target != null)
                {
                    if (target[i].IsValid() == false)
                        continue;
                    Vector3 dir = target[i].CenterPosition - Managers.Game.Player.CenterPosition;
                    Vector3 startPos = Managers.Game.Player.CenterPosition;
                    GenerateProjectile(Managers.Game.Player, prefabName, startPos, dir.normalized, Vector3.zero, this);
                }

                yield return new WaitForSeconds(SkillData.ProjectileSpacing);
            }
        }
    }
}
