using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_MonsterInfoItem : UI_SubItem
{
    #region Enum
    enum Buttons
    {
        MonsterInfoButton
    }
    enum Texts
    {
        MonsterLevelValueText,
    }

    enum Images
    {
        MonsterImage,
    }
    #endregion

    CreatureData _creature;
    Transform _makeSubItemParents;
    int _level;

    protected override void Awake()
    {
        base.Awake();

        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));
        BindImages(typeof(Images));

        GetButton((int)Buttons.MonsterInfoButton).gameObject.BindEvent(OnClickMonsterInfoButton);
    }

    public void SetInfo(int monsterId, int level, Transform makeSubItemParents)
    {
        _makeSubItemParents = makeSubItemParents;
        transform.localScale = Vector3.one;

        if (Managers.Data.CreatureDic.TryGetValue(monsterId, out _creature))
        {
            _creature = Managers.Data.CreatureDic[monsterId];
            _level = level;
        }

        RefreshUI();
    }

    private void RefreshUI()
    {
        if (GetText((int)Texts.MonsterLevelValueText) == null)
            return;

        if (_creature == null)
        {
            gameObject.SetActive(false);
            return;
        }

        GetText((int)Texts.MonsterLevelValueText).text = $"Lv. {_level}";
        GetImage((int)Images.MonsterImage).sprite = Managers.Resource.Load<Sprite>(_creature.IconLabel);
    }

    private void OnClickMonsterInfoButton(PointerEventData evt)
    {
        Managers.Sound.PlayButtonClick();
        // UI_ToolTipItem 프리팹 생성
        UI_ToolTipItem item = Managers.UI.MakeSubItem<UI_ToolTipItem>(_makeSubItemParents);
        item.transform.localScale = Vector3.one;
        RectTransform targetPos = this.gameObject.GetComponent<RectTransform>();
        RectTransform parentsCanvas = _makeSubItemParents.gameObject.GetComponent<RectTransform>();
        item.SetInfo(_creature, targetPos, parentsCanvas);
        item.transform.SetAsLastSibling();
    }
}
