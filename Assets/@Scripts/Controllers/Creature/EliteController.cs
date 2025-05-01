using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class EliteController : MonsterController
{
    protected override void Awake()
    {
        base.Awake();

        CreatureState = Define.ECreatureState.Moving;

        _rigidBody.simulated = true;
        transform.localScale = new Vector3(2f, 2f, 2f);

        ObjectType = Define.EObjectType.EliteMonster;
    }

    private void OnEnable()
    {
        if (DataId != 0)
            SetInfo(DataId);
    }

    private void Start()
    {
        InvokeMonsterData();
    }

    public override void OnDead()
    {
        base.OnDead();
        Managers.Game.Player.SkillRefreshCount++;
        Managers.Game.Player.Skills.OnEliteDeadBonus();
        if (Managers.Game.DicMission.TryGetValue(Define.EMissionTarget.EliteMonsterKill, out MissionInfo mission))
            mission.Progress++;
        // TODO After Achievement
        // Managers.Game.TotalEliteKillCount++;
        DropItem();
    }

    private void DropItem()
    {
        int i = 0;
        foreach (int id in Managers.Game.CurrentWaveData.EliteDropItemId)
        {
            Data.DropItemData dropItem;
            if (Managers.Data.DropItemDataDic.TryGetValue(id, out dropItem) == true)
            {
                int dropCount = Managers.Game.CurrentWaveData.EliteDropItemId.Count;
                float angleInterval = 360f / dropCount;
                Vector3 dropPos;
                if (dropCount < 2)
                    dropPos = transform.position;
                else
                    dropPos = CalculateDropPotion(angleInterval * i);

                switch (dropItem.DropItemType)
                {
                    case Define.EDropItemType.Potion:
                        Managers.Object.Spawn<PotionController>(dropPos).SetInfo(dropItem);
                        break;
                    case Define.EDropItemType.DropBox:
                        Managers.Object.Spawn<EliteBoxController>(dropPos);
                        break;
                }
                i++;
            }
        }
    }

    Vector3 CalculateDropPotion(float angle)
    {
        float dropDistance = Random.Range(1f, 2f);

        Vector3 dropPos = transform.position;

        float x = Mathf.Cos(angle * Mathf.Deg2Rad);
        float y = Mathf.Sin(angle * Mathf.Deg2Rad);
        Vector3 offset = new Vector3(x, y, 0f) * dropDistance;
        Vector3 pos = dropPos + offset;

        return pos;
    }
}
