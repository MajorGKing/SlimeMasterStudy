using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// for Editor
[Serializable]
public class PlayerStat
{
    public int DataId;
    public float Hp;
    public float MaxHp;
    public float MaxHpBonusRate = 1;
    public float HealBonusRate = 1;
    public float HpRegen;
    public float Atk;
    public float AttackRate = 1;
    public float Def;
    public float DefRate = 1;
    public float CriRate;
    public float CriDamage = 1.5f;
    public float DamageReduction;
    public float MoveSpeedRate = 1;
    public float MoveSpeed;
}

public class PlayerController : CreatureController
{
    [SerializeField]
    public GameObject Indicator;
    [SerializeField]
    public GameObject IndicatorSprite;
    Vector2 _moveDir = Vector2.zero;
    public PlayerStat StatViewer = new PlayerStat();

    public Action OnPlayerDataUpdated;
    public Action OnPlayerLevelUp;
    public Action OnPlayerDead;
    public Action OnPlayerDamaged;
    public Action OnPlayerMove;



    public Vector3 PlayerCenterPos { get { return Indicator.transform.position; } }
    public Vector3 PlayerDirection { get { return (IndicatorSprite.transform.position - PlayerCenterPos).normalized; } }

    public override void Healing(float amount, bool isEffect = true)
    {
        //if (amount == 0) return;
        //float res = ((MaxHp * amount) * HealBonusRate);
        //if (res == 0) return;
        //Hp = Hp + res;
        //Managers.Object.ShowDamageFont(CenterPosition, 0, res, transform);
        //if (isEffect)
        //    Managers.Resource.Instantiate("HealEffect", transform);
    }
}
