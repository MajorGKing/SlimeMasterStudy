using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

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
}
