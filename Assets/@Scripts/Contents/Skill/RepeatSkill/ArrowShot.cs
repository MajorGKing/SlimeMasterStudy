using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShot : RepeatSkill
{
    protected override void Awake()
    {
        base.Awake();
        SkillType = Define.ESkillType.ArrowShot;
    }

    IEnumerator SetArrowShot()
    {
        string prefabName = SkillData.PrefabLabel;

        if (Managers.Game.Player != null)
        {
            List<MonsterController> target = Managers.Object.GetNearestMonsters(SkillData.NumProjectiles);
            if (target == null)
                yield break;

            for (int i = 0; i < target.Count; i++)
            {
                Vector3 dir = Managers.Game.Player.PlayerDirection;
                Vector3 startPos = Managers.Game.Player.CenterPosition;
                GenerateProjectile(Managers.Game.Player, prefabName, startPos, dir, Vector3.zero, this);
                yield return new WaitForSeconds(SkillData.ProjectileSpacing);
            }
        }
    }

    protected override void DoSkillJob()
    {
        StartCoroutine(SetArrowShot());
    }

    public override void OnChangedSkillData()
    {
    }
}
