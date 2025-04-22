using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureController : BaseController
{
    [SerializeField]
    protected SpriteRenderer CreatureSprite;
    protected string SpriteName;
    public Material DefaultMat;
    public Material HitEffectmat;
    [SerializeField]
    protected bool isPlayDamagedAnim = false;

    public Rigidbody2D _rigidBody { get; set; }
    public Animator Anim { get; set; }
    public CreatureData CreatureData;

    public virtual int DataId { get; set; }
    public virtual float Hp { get; set; }
    public virtual float MaxHp { get; set; }
    public virtual float MaxHpBonusRate { get; set; } = 1;
    public virtual float HealBonusRate { get; set; } = 1;
    public virtual float HpRegen { get; set; }
    public virtual float Atk { get; set; }
    public virtual float AttackRate { get; set; } = 1;
    public virtual float Def { get; set; }
    public virtual float DefRate { get; set; }
    public virtual float CriRate { get; set; }
    public virtual float CriDamage { get; set; } = 1.5f;
    public virtual float DamageReduction { get; set; }
    public virtual float MoveSpeedRate { get; set; } = 1;
    public virtual float MoveSpeed { get; set; }
    //public virtual SkillBook Skills { get; set; }

    public Vector3 CenterPosition
    {
        get
        {
            return _offset.bounds.center;
        }
    }
    public float ColliderRadius { get; set; }

    private Collider2D _offset;
    Define.ECreatureState _creatureState = Define.ECreatureState.Moving;
    public virtual Define.ECreatureState CreatureState
    {
        get { return _creatureState; }
        set
        {
            _creatureState = value;
            UpdateAnimation();
        }
    }

    public virtual void UpdateAnimation() { }

    public virtual void OnDamaged(BaseController attacker, SkillBase skill = null, float damage = 0)
    {

    }

    public virtual void Healing(float amount, bool isEffect = true) { }

    public bool IsMonster()
    {
        switch (ObjectType)
        {
            case Define.EObjectType.Boss:
            case Define.EObjectType.Monster:
            case Define.EObjectType.EliteMonster:
                return true;
            case Define.EObjectType.Player:
            case Define.EObjectType.Projectile:
                return false; ;
            default:
                return false;
        }
    }
}
