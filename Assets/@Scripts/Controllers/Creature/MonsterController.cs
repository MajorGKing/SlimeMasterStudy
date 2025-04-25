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
}
