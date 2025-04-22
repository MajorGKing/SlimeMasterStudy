using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : SkillBase
{
    public SkillBase Skill;
    Vector2 _spawnPos;
    Vector3 _dir = Vector3.zero;
    Vector3 _target = Vector3.zero;

    Rigidbody2D _rigid;
    int _numPenerations;
    public int _bounceCount = 1;
    GameObject _meteorShadow;

    List<CreatureController> _enteredColliderList = new List<CreatureController>();
    Coroutine _coDotDamage;

    float _timer = 0;
    private float _rotateAmount = 1000;

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public override void OnChangedSkillData()
    {
        //base.OnChangedSkillData();
    }

    protected override void Awake()
    {
        base.Awake();

        ObjectType = Define.EObjectType.Projectile;
    }

    public void SetInfo(CreatureController owner, Vector2 position, Vector2 dir, Vector2 target, SkillBase skill)
    {
        Owner = owner;
        _spawnPos = position;
        _dir = dir;
        Skill = skill;
        _rigid = GetComponent<Rigidbody2D>();
        _target = target;
        transform.localScale = Vector3.one * Skill.SkillData.ScaleMultiplier;
        _numPenerations = skill.SkillData.NumPenerations;
        _bounceCount = skill.SkillData.NumBounce;

        switch (skill.SkillType)
        {
            case Define.ESkillType.ChainLightning:
                StartCoroutine(CoChainLightning(_spawnPos, _target, true));
                break;
            case Define.ESkillType.PhotonStrike:
                StartCoroutine(CoPhotonStrike());
                break;
            case Define.ESkillType.Shuriken:
                _bounceCount = Skill.SkillData.NumBounce;
                _rigid.velocity = _dir * Skill.SkillData.ProjSpeed;
                break;
            case Define.ESkillType.ComboShot:
                LaunchComboShot();
                break;
            case Define.ESkillType.WindCutter:
                if (gameObject.activeInHierarchy)
                    StartCoroutine(CoWindCutter());
                break;
            case Define.ESkillType.Meteor:
                _dir = (_target - transform.position).normalized;
                transform.rotation = Quaternion.FromToRotation(Vector3.up, _dir);
                _rigid.velocity = _dir * Skill.SkillData.ProjSpeed;
                _meteorShadow = Managers.Resource.Instantiate("MeteorShadow", pooling: true);
                _meteorShadow.transform.position = target; //+ new Vector3(-0.5f, -0.45f, 1);
                if (gameObject.activeInHierarchy)
                    StartCoroutine(CoMeteor());
                break;
            case Define.ESkillType.PoisonField:
                if (gameObject.activeInHierarchy)
                    StartCoroutine(CoPosionField(skill));
                break;
            case Define.ESkillType.EgoSword:
            case Define.ESkillType.StormBlade:
                StartCoroutine(CoDestroy());
                transform.rotation = Quaternion.FromToRotation(Vector3.up, _dir);
                _rigid.velocity = _dir * Skill.SkillData.ProjSpeed;
                break;
            default:
                transform.rotation = Quaternion.FromToRotation(Vector3.up, _dir);
                _numPenerations = Skill.SkillData.NumPenerations;
                _rigid.velocity = _dir * Skill.SkillData.ProjSpeed;
                break;
        }

        if (gameObject.activeInHierarchy)
            StartCoroutine(CoCheckDestory());
    }

    IEnumerator CoChainLightning(Vector3 startPos, Vector3 endPos, bool isFollow = false)
    {
        SetParticleSize(startPos, endPos);
        yield return new WaitForSeconds(0.25f);
        DestroyProjectile();
    }

    IEnumerator CoPhotonStrike()
    {
        List<MonsterController> target = Managers.Object.GetMonsterWithinCamera(1);
        while (true)
        {
            _timer += Time.deltaTime;
            if (_timer > 3 || target == null)
            {
                DestroyProjectile();
                _timer = 0;
                break;
            }

            if (target[0].IsValid() == false)
                break;

            Vector2 direction = (Vector2)target[0].CenterPosition - _rigid.position;
            float rotateSpeed = Vector3.Cross(direction.normalized, transform.up).z;
            _rigid.angularVelocity = -_rotateAmount * rotateSpeed;
            _rigid.velocity = transform.up * Skill.SkillData.ProjSpeed;

            //if (Vector2.Distance(_rigid.position, targetPos) < 0.3f)
            //    ExplosionMeteor();
            yield return new WaitForFixedUpdate();
        }
    }

    void LaunchComboShot()
    {
        Vector3 targePoint = Owner.CenterPosition + _dir * Skill.SkillData.ProjRange;
        float angle = Mathf.Atan2(_dir.y, _dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        Sequence seq = DOTween.Sequence();
        float duration = Skill.SkillData.Duration;

        seq.Append(transform.DOMove(targePoint, 0.5f).SetEase(Ease.Linear)).AppendInterval(duration - 0.5f).OnComplete(() =>
        {
            Vector3 targetDir = Managers.Game.Player.CenterPosition - transform.position;
            angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
            _rigid.velocity = targetDir.normalized * Skill.SkillData.ProjSpeed;
        });
    }

    IEnumerator CoWindCutter()
    {
        Vector3 targePoint = Managers.Game.Player.PlayerCenterPos + _dir * Skill.SkillData.ProjSpeed;
        transform.localScale = Vector3.zero;
        transform.localScale = Vector3.one * Skill.SkillData.ScaleMultiplier;

        Sequence seq = DOTween.Sequence();
        // 1. 목표지점까지 빠르게 도착
        // 2. 도착수 약간 더 전진
        // 3. 되돌아옴

        float projectileTravelTime = 1f; // 발사체가 목표지점까지 가는데 걸리는시간
        float secondSeqStartTime = 0.7f; // 두번쨰 시퀀스 시작시간
        float secondSeqDuringTime = 1.8f; //두번째 시퀀스 유지시간

        seq.Append(transform.DOMove(targePoint, projectileTravelTime).SetEase(Ease.OutExpo))
            .Insert(secondSeqStartTime, transform.DOMove(targePoint + _dir, secondSeqDuringTime).SetEase(Ease.Linear));

        yield return new WaitForSeconds(Skill.SkillData.Duration);

        while (true)
        {
            transform.position = Vector2.MoveTowards(this.transform.position, Managers.Game.Player.PlayerCenterPos, Time.deltaTime * Skill.SkillData.ProjSpeed * 4f);
            if (Managers.Game.Player.PlayerCenterPos == transform.position)
            {
                DestroyProjectile();
                break;
            }
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator CoMeteor()
    {
        while (true)
        {
            if (_meteorShadow != null)
            {
                Vector2 shadowPosition = _meteorShadow.transform.position;

                float distance = Vector2.Distance(shadowPosition, transform.position);
                float scale = Mathf.Lerp(0f, 2.5f, 1 - distance / 10f);
                _meteorShadow.transform.position = shadowPosition;
                _meteorShadow.transform.localScale = new Vector3(scale, scale, 1f);
            }
            if (Vector2.Distance(_rigid.position, _target) < 0.3f)
                ExplosionMeteor();
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator CoPosionField(SkillBase skill)
    {
        while (true)
        {
            // TODO ILHAK after PoisonFieldEffect
            //transform.position = Vector2.MoveTowards(this.transform.position, _target, Time.deltaTime * Skill.SkillData.ProjSpeed);

            //if (transform.position == _target)
            //{
            //    string effectName = skill.Level == 6 ? "PoisonFieldEffect_Final" : "PoisonFieldEffect";

            //    GameObject fireEffect = Managers.Resource.Instantiate(effectName, pooling: true);
            //    fireEffect.GetComponent<PoisonFieldEffect>().SetInfo(Managers.Game.Player, skill);
            //    fireEffect.transform.position = _target;
            //    DestroyProjectile();
            //}

            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator CoDestroy()
    {
        yield return new WaitForSeconds(Skill.SkillData.Duration);
        DestroyProjectile();
    }

    IEnumerator CoCheckDestory()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            DestroyProjectile();
        }
    }

    void SetParticleSize(Vector3 startPos, Vector3 endPos)
    {
        ParticleSystem particle = GetComponent<ParticleSystem>();
        ParticleSystem childParticle = Utils.FindChild<ParticleSystem>(gameObject);
        var main = particle.main;
        var main2 = childParticle.main;

        // Scale
        transform.position = startPos;
        float dist = Vector3.Distance(startPos, endPos);
        main.startSizeX = main2.startSizeX = dist;
        main.startSizeY = main2.startSizeY = 8;
        // rotatate
        Vector3 dir = (endPos - startPos).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x);
        main.startRotation = main2.startRotation = angle * -1f;

        // Cast box
        List<Transform> listMonster = new List<Transform>();
        LayerMask targetLayer = LayerMask.GetMask("Monster", "Boss");
        float boxWidth = 1f;
        Vector3 midPos = (startPos + endPos) / 2f; // 시작점과 끝점 사이의 중간 지점
        Vector2 boxSize = new Vector2(boxWidth, boxWidth);
        //Vector2 boxSize = new Vector2(300, 399);
        float angleRad = angle * Mathf.Deg2Rad;

        RaycastHit2D[] colliders = Physics2D.BoxCastAll(midPos, boxSize, 0, dir, dist * 1.3f, targetLayer);

        foreach (RaycastHit2D hit in colliders)
        {
            MonsterController monster = hit.transform.GetComponent<MonsterController>();
            if (monster != null)
            {
                monster.OnDamaged(Owner, Skill);
            }
        }
    }

    public void DestroyProjectile()
    {
        Managers.Object.DespawnGameObject(this);
        //
    }

    void ExplosionMeteor()
    {
        Managers.Resource.Destroy(_meteorShadow);
        float scanRange = 1.5f;
        string prefabName = Level == 6 ? "MeteorHitEffect_Final" : "MeteorHitEffect";
        GameObject obj = Managers.Resource.Instantiate(prefabName, pooling: true);
        obj.transform.position = transform.position;

        RaycastHit2D[] _targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0);

        foreach (RaycastHit2D _target in _targets)
        {
            CreatureController creature = _target.transform.GetComponent<CreatureController>();
            if (creature?.IsMonster() == true)
                creature.OnDamaged(Owner, Skill);
        }
        DestroyProjectile();
    }

    // TODO ILHAK after MonsterSkill_01, FrozenHeart
    //void BounceProjectile(CreatureController creature)
    //{
    //    List<Transform> list = new List<Transform>();
    //    list = Managers.Object.GetFindMonstersInFanShape(creature.CenterPosition, _dir, 5.5f, 240);

    //    List<Transform> sortedList = (from t in list
    //                                  orderby Vector3.Distance(t.position, transform.position) descending
    //                                  select t).ToList();

    //    if (sortedList.Count == 0)
    //    {
    //        DestroyProjectile();
    //    }
    //    else
    //    {
    //        int index = Random.Range(sortedList.Count / 2, sortedList.Count);
    //        _dir = (sortedList[index].position - transform.position).normalized;
    //        _rigid.velocity = _dir * Skill.SkillData.BounceSpeed;
    //    }
    //}

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    FrozenHeart frozenHeart = collision.transform.GetComponent<FrozenHeart>();
    //    if (frozenHeart != null)
    //    {
    //        DestroyProjectile();
    //    }
    //}

    //void OnTriggerEnter2D(Collider2D collision)
    //{
    //    MonsterSkill_01 monsterProj = GetComponent<MonsterSkill_01>();

    //    if (collision.transform.parent != null && monsterProj != null)
    //    {
    //        FrozenHeart frozenHeart = collision.transform.parent.transform.GetComponent<FrozenHeart>();
    //        if (frozenHeart != null)
    //        {
    //            DestroyProjectile();
    //        }
    //    }


    //    CreatureController creature = collision.transform.GetComponent<CreatureController>();
    //    if (creature.IsValid() == false)
    //        return;
    //    if (this.IsValid() == false)
    //        return;

    //    switch (Skill.SkillType)
    //    {
    //        case Define.ESkillType.IcicleArrow:
    //        case Define.ESkillType.MonsterSkill_01:
    //        case Define.ESkillType.SpinShot:
    //        case Define.ESkillType.CircleShot:
    //        case Define.ESkillType.PhotonStrike:
    //            _numPenerations--;
    //            if (_numPenerations < 0)
    //            {
    //                _rigid.velocity = Vector3.zero;
    //                DestroyProjectile();
    //            }
    //            break;
    //        case Define.ESkillType.Shuriken:
    //        case Define.ESkillType.EnergyBolt:
    //            _bounceCount--;
    //            BounceProjectile(creature);
    //            if (_bounceCount < 0)
    //            {
    //                _rigid.velocity = Vector3.zero;
    //                DestroyProjectile();
    //            }
    //            break;
    //        case Define.ESkillType.WindCutter:
    //            _enteredColliderList.Add(creature);
    //            if (_coDotDamage == null)
    //                _coDotDamage = StartCoroutine(CoStartDotDamage());
    //            break;
    //        default:
    //            break;
    //    }
    //    creature.OnDamaged(Owner, Skill);
    //}

    //void OnTriggerExit2D(Collider2D collision)
    //{
    //    CreatureController target = collision.transform.GetComponent<CreatureController>();
    //    if (target.IsValid() == false)
    //        return;

    //    if (this.IsValid() == false)
    //        return;

    //    _enteredColliderList.Remove(target);

    //    if (_enteredColliderList.Count == 0 && _coDotDamage != null)
    //    {
    //        StopCoroutine(_coDotDamage);
    //        _coDotDamage = null;
    //    }
    //}
}
