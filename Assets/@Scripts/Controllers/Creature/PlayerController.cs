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

    public override int DataId
    {
        get { return Managers.Game.ContinueInfo.PlayerDataId; }
        set { Managers.Game.ContinueInfo.PlayerDataId = StatViewer.DataId = value; }
    }

    public override float Hp
    {
        get { return Managers.Game.ContinueInfo.Hp; }
        set
        {
            if (value > MaxHp)
                Managers.Game.ContinueInfo.Hp = StatViewer.Hp = MaxHp;
            else
                Managers.Game.ContinueInfo.Hp = StatViewer.Hp = value;
        }
    }

    public override float MaxHp
    {
        get { return Managers.Game.ContinueInfo.MaxHp; }
        set { Managers.Game.ContinueInfo.MaxHp = StatViewer.MaxHp = value; }
    }

    public override float MaxHpBonusRate
    {
        get { return Managers.Game.ContinueInfo.MaxHpBonusRate; }
        set { Managers.Game.ContinueInfo.MaxHpBonusRate = StatViewer.MaxHpBonusRate = value; }
    }

    public override float HealBonusRate
    {
        get { return Managers.Game.ContinueInfo.HealBonusRate; }
        set { Managers.Game.ContinueInfo.HealBonusRate = StatViewer.HealBonusRate = value; }
    }
    public override float HpRegen
    {
        get { return Managers.Game.ContinueInfo.HpRegen; }
        set { Managers.Game.ContinueInfo.HpRegen = StatViewer.HpRegen = value; }
    }

    public override float Atk
    {
        get { return Managers.Game.ContinueInfo.Atk; }
        set { Managers.Game.ContinueInfo.Atk = StatViewer.Atk = value; }
    }
    public override float AttackRate
    {
        get { return Managers.Game.ContinueInfo.AttackRate; }
        set { Managers.Game.ContinueInfo.AttackRate = StatViewer.AttackRate = value; }
    }

    public override float Def
    {
        get { return Managers.Game.ContinueInfo.Def; }
        set { Managers.Game.ContinueInfo.Def = StatViewer.Def = value; }
    }
    public override float DefRate
    {
        get { return Managers.Game.ContinueInfo.DefRate; }
        set { Managers.Game.ContinueInfo.DefRate = StatViewer.DefRate = value; }
    }
    public override float CriRate
    {
        get { return Managers.Game.ContinueInfo.CriRate; }
        set { Managers.Game.ContinueInfo.CriRate = StatViewer.CriRate = value; }
    }
    public override float CriDamage
    {
        get { return Managers.Game.ContinueInfo.CriDamage; }
        set { Managers.Game.ContinueInfo.CriDamage = StatViewer.CriDamage = value; }
    }

    public override float DamageReduction
    {
        get { return Managers.Game.ContinueInfo.DamageReduction; }
        set { Managers.Game.ContinueInfo.DamageReduction = StatViewer.DamageReduction = value; }
    }
    public override float MoveSpeedRate
    {
        get { return Managers.Game.ContinueInfo.MoveSpeedRate; }
        set { Managers.Game.ContinueInfo.MoveSpeedRate = StatViewer.MoveSpeedRate = value; }
    }
    public override float MoveSpeed
    {
        get { return Managers.Game.ContinueInfo.MoveSpeed; }
        set { Managers.Game.ContinueInfo.MoveSpeed = StatViewer.MoveSpeed = value; }
    }
    public int Level
    {
        get { return Managers.Game.ContinueInfo.Level; }
        set { Managers.Game.ContinueInfo.Level = value; }
    }

    public float Exp
    {
        get { return Managers.Game.ContinueInfo.Exp; }
        set { }// TODO ILHAK
    }

    public float TotalExp
    {
        get { return Managers.Game.ContinueInfo.TotalExp; }
        set { Managers.Game.ContinueInfo.TotalExp = value; }
    }
    public float ExpBonusRate
    {
        get { return Managers.Game.ContinueInfo.ExpBonusRate; }
        set { Managers.Game.ContinueInfo.ExpBonusRate = value; }
    }
    public float SoulBonusRate
    {
        get { return Managers.Game.ContinueInfo.SoulBonusRate; }
        set { Managers.Game.ContinueInfo.SoulBonusRate = value; }
    }
    public float CollectDistBonus
    {
        get { return Managers.Game.ContinueInfo.CollectDistBonus; }
        set { Managers.Game.ContinueInfo.CollectDistBonus = value; }
    }
    public int SkillRefreshCount
    {
        get { return Managers.Game.ContinueInfo.SkillRefreshCount; }
        set { Managers.Game.ContinueInfo.SkillRefreshCount = value; }
    }

    public float SoulCount
    {
        get { return Managers.Game.ContinueInfo.SoulCount; }

        set
        {
            Managers.Game.ContinueInfo.SoulCount = Mathf.Round(value);

            OnPlayerDataUpdated?.Invoke();
        }
    }

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
