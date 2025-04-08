using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;

public class UI_ButtonAnimation : UI_Base
{
    protected override void Awake()
    {
        gameObject.BindEvent(ButtonPointerDownAnimation, Define.ETouchEvent.PointerDown);
        gameObject.BindEvent(ButtonPointerUpAnimation, Define.ETouchEvent.PointerUp);
    }

    public void ButtonPointerDownAnimation(PointerEventData evt)
    {
        transform.DOScale(0.85f, 0.1f).SetEase(Ease.InOutBack).SetUpdate(true); 
    }

    public void ButtonPointerUpAnimation(PointerEventData evt)
    {
        transform.DOScale(1f, 0.1f).SetEase(Ease.InOutSine).SetUpdate(true); 
    }
}
