using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteBoxController : DropItemController
{
    public int _soudCount = 5;
    Coroutine _coMoveToPlayer;

    protected override void Awake()
    {
        base.Awake();
        CollectDist = Define.BOX_COLLECT_DISTANCE;
        itemType = Define.EObjectType.DropBox;
    }

    public void SetInfo()
    {

    }

    public override void GetItem()
    {
        base.GetItem();
        if (_coMoveToPlayer == null && this.IsValid())
        {
            _coroutine = StartCoroutine(CoCheckDistance());
        }
    }

    public override void CompleteGetItem()
    {
        //스킬 습득
        UI_LearnSkillPopup popup = Managers.UI.ShowPopupUI<UI_LearnSkillPopup>();
        popup.SetInfo();

        Managers.Object.Despawn(this);
    }
}
