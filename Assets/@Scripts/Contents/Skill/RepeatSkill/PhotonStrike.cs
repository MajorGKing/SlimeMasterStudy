using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonStrike : RepeatSkill
{
    protected override void Awake()
    {
        base.Awake();
        SkillType = Define.ESkillType.PhotonStrike;
    }

    public override void OnChangedSkillData()
    {
    }

    protected override void DoSkillJob()
    {
        StartCoroutine(SetPhotonStrike());
    }

    IEnumerator SetPhotonStrike()
    {
        if (Managers.Game.Player == null)
            yield return null;

        string prefabName = SkillData.PrefabLabel;

        for (int i = 0; i < SkillData.NumProjectiles; i++)
        {
            Vector3 dir = Vector3.one;
            Vector3 startPos = Managers.Game.Player.CenterPosition;
            GenerateProjectile(Managers.Game.Player, prefabName, startPos, dir, Vector3.zero, this);

            yield return new WaitForSeconds(SkillData.ProjectileSpacing);
        }
    }
}
