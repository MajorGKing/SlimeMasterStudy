using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemInfo
{
    public Define.EGemType Type;
    public string SpriteName;
    public Vector3 GemScale;
    public int ExpAmount;

    public GemInfo(Define.EGemType type, Vector3 gemScale)
    {
        Type = type;
        SpriteName = $"{type}Gem";
        GemScale = gemScale;
        switch (type)
        {
            case Define.EGemType.Small:
                ExpAmount = Define.SMALL_EXP_AMOUNT;
                break;
            case Define.EGemType.Green:
                ExpAmount = Define.GREEN_EXP_AMOUNT;
                break;
            case Define.EGemType.Blue:
                ExpAmount = Define.BLUE_EXP_AMOUNT;
                break;
            case Define.EGemType.Yellow:
                ExpAmount = Define.YELLOW_EXP_AMOUNT;
                break;
        }
    }
}

public class GemController : DropItemController
{
    GemInfo _gemInfo;
    Coroutine _coMoveToPlayer;

    protected override void Awake()
    {
        base.Awake();
        itemType = Define.EObjectType.Gem;
    }

    public override void OnDisable()
    {
        base.OnDisable();

        if (_coMoveToPlayer != null)
        {
            StopCoroutine(_coMoveToPlayer);
            _coMoveToPlayer = null;
        }
    }

    public void SetInfo(GemInfo gemInfo)
    {
        _gemInfo = gemInfo;
        Sprite spr = Managers.Resource.Load<Sprite>($"{_gemInfo.SpriteName}");
        GetComponent<SpriteRenderer>().sprite = spr;
        transform.localScale = _gemInfo.GemScale;
    }

    public override void GetItem()
    {
        base.GetItem();
        if (_coMoveToPlayer == null && this.IsValid())
        {
            Sequence seq = DOTween.Sequence();
            Vector3 dir = (transform.position - Managers.Game.Player.PlayerCenterPos).normalized;
            Vector3 target = gameObject.transform.position + dir * 1.5f;
            seq.Append(transform.DOMove(target, 0.3f).SetEase(Ease.Linear)).OnComplete(() =>
            {
                _coMoveToPlayer = StartCoroutine(CoMoveToPlayer());
            });
        }
    }

    public IEnumerator CoMoveToPlayer()
    {
        while (this.IsValid() == true)
        {
            float dist = Vector3.Distance(gameObject.transform.position, Managers.Game.Player.PlayerCenterPos);

            transform.position = Vector3.MoveTowards(transform.position, Managers.Game.Player.PlayerCenterPos, Time.deltaTime * 30.0f);

            if (dist < 0.4f)
            {
                string soundName = UnityEngine.Random.value > 0.5 ? "ExpGet_01" : "ExpGet_02";
                Managers.Sound.Play(Define.ESound.Effect, soundName);
                Managers.Game.Player.Exp += _gemInfo.ExpAmount * Managers.Game.Player.ExpBonusRate;
                Managers.Object.Despawn(this);
                yield break;
            }

            yield return new WaitForFixedUpdate();
        }
    }
}
