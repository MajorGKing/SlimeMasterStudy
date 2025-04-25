using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : DropItemController
{
    Data.DropItemData _dropItemData;

    protected override void Awake()
    {
        base.Awake();
        itemType = Define.EObjectType.Bomb;
    }

    public override void GetItem()
    {
        base.GetItem();
        if (_coroutine == null && this.IsValid())
        {
            _coroutine = StartCoroutine(CoCheckDistance());
        }
    }

    public void SetInfo(Data.DropItemData data)
    {
        _dropItemData = data;
        CollectDist = Define.BOX_COLLECT_DISTANCE;        
    }

    public override void CompleteGetItem()
    {
        Managers.Object.KillAllMonsters();
        Managers.Object.Despawn(this);
    }
}
