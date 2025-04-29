using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;

public class UI_GameScene : UI_Scene
{
    #region Enum
    enum GameObjects
    {
        WaveObject,
        SoulImage,
        OnDamaged,
        SoulShopObject,
        SupportSkillCardListObject,
        OwnBattleSkillInfoObject,
        //TotalDamageContentObject,
        SupportSkillListScrollObject,
        SupportSkillListScrollContentObject,
        WhiteFlash,
        MonsterAlarmObject,
        BossAlarmObject,

        EliteInfoObject,
        EliteHpSliderObject,
        BossInfoObject,
        BossHpSliderObject,

        BattleSkillSlotGroupObject,
    }
    enum Buttons
    {
        PauseButton,
        DevelopButton,
        SoulShopButton,
        CardListResetButton,
        SoulShopCloseButton,
        TotalDamageButton,
        SupportSkillListButton,
        SoulShopLeadButton,
        SoulShopBackgroundButton,
    }
    enum Texts
    {
        WaveValueText,
        TimeLimitValueText,
        SoulValueText,
        KillValueText,
        CharacterLevelValueText,
        CardListResetText,
        ResetCostValueText,
        //BattleSkillCountValueText,
        SupportSkillCountValueText,

        EliteNameValueText,
        BossNameValueText,

        MonsterCommentText,
        BossCommentText,
    }
    enum Images
    {
        BattleSkilI_Icon_0,
        BattleSkilI_Icon_1,
        BattleSkilI_Icon_2,
        BattleSkilI_Icon_3,
        BattleSkilI_Icon_4,
        BattleSkilI_Icon_5,
        SupportSkilI_Icon_0,
        SupportSkilI_Icon_1,
        SupportSkilI_Icon_2,
        SupportSkilI_Icon_3,
        SupportSkilI_Icon_4,
        SupportSkilI_Icon_5,
    }
    enum AlramType
    {
        wave,
        boss
    }

    enum Sliders
    {
        ExpSliderObject,
    }
    #endregion

    private float elapsedTime;
    private float updateInterval = 0.3f;

    GameManager _game;
    Coroutine _coWaveAlarm;

    bool _isSupportSkillListButton = false;

    protected override void Awake()
    {
        base.Awake();

        BindObjects(typeof(GameObjects));
        BindButtons(typeof(Buttons));
        BindTexts(typeof(Texts));
        BindImages(typeof(Images));
        BindSliders(typeof(Sliders));

        GetButton((int)Buttons.PauseButton).gameObject.BindEvent(OnClickPauseButton);
        GetButton((int)Buttons.PauseButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.SoulShopButton).gameObject.BindEvent(OnClickSoulShopButton);
        GetButton((int)Buttons.CardListResetButton).gameObject.BindEvent(OnClickCardListResetButton);
        GetButton((int)Buttons.CardListResetButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.SoulShopLeadButton).gameObject.BindEvent(OnClickSoulShopButton);
        GetButton((int)Buttons.SoulShopLeadButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.SoulShopCloseButton).gameObject.BindEvent(OnClickSoulShopCloseButton);
        GetButton((int)Buttons.SoulShopCloseButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.TotalDamageButton).gameObject.BindEvent(OnClickTotalDamageButton);
        GetButton((int)Buttons.TotalDamageButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.SupportSkillListButton).gameObject.BindEvent(OnClickSupportSkillListButton);
        GetButton((int)Buttons.SupportSkillListButton).GetOrAddComponent<UI_ButtonAnimation>();
        GetButton((int)Buttons.SoulShopCloseButton).gameObject.SetActive(false); // 영혼 상점 버튼 초기 상태
        GetButton((int)Buttons.SoulShopLeadButton).gameObject.SetActive(true); // 영혼 상점 버튼 초기 상태
        GetButton((int)Buttons.SoulShopBackgroundButton).gameObject.BindEvent(OnClickSoulShopCloseButton);
        GetButton((int)Buttons.SoulShopBackgroundButton).gameObject.SetActive(false); // 영혼 상점 배경
        GetObject((int)GameObjects.OwnBattleSkillInfoObject).gameObject.SetActive(false); // 배틀 스킬 리스트 초기 상태
        GetObject((int)GameObjects.SupportSkillListScrollObject).gameObject.SetActive(false); // 서포트 스킬 리스트 초기 상태
        _isSupportSkillListButton = false;

        GetObject((int)GameObjects.MonsterAlarmObject).gameObject.SetActive(false); // 몬스터 웨이브 알람 초기 상태
        GetObject((int)GameObjects.BossAlarmObject).gameObject.SetActive(false); // 보스 알람 초기 상태

        GetObject((int)GameObjects.EliteInfoObject).gameObject.SetActive(false); // 엘리트 정보(체력바) 초기 상태
        GetObject((int)GameObjects.BossInfoObject).gameObject.SetActive(false); // 보스 정보(체력바) 초기 상태

        _game = Managers.Game;

        Managers.Game.Player.OnPlayerDataUpdated = OnPlayerDataUpdated;
        //Managers.Game.Player.OnMonsterDataUpdated = OnOnMonsterDataUpdated;
        Managers.Game.Player.OnPlayerLevelUp = OnPlayerLevelUp;
        Managers.Game.Player.OnPlayerDamaged = OnDamaged;

        GetComponent<Canvas>().worldCamera = Camera.main;

        Managers.Game.Player.Skills.UpdateSkillUi += OnLevelUpSkill;
        Managers.Game.Player.OnPlayerMove += OnPlayerMove;

        RefreshUI();
    }

    private void OnDestroy()
    {
        if (Managers.Game.Player != null)
        {
            Managers.Game.Player.Skills.UpdateSkillUi -= OnLevelUpSkill;
            Managers.Game.Player.OnPlayerMove -= OnPlayerMove;
        }
    }

    public void SetInfo()
    {
        RefreshUI();
    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= updateInterval)
        {
            float fps = 1.0f / Time.deltaTime;
            float ms = Time.deltaTime * 1000.0f;
            string text = string.Format("{0:N1} FPS ({1:N1}ms)", fps, ms);
            // GetText((int)Texts.FpsText).text = text;

            elapsedTime = 0;
        }
    }

    private void RefreshUI()
    {
        SetBattleSkill();
        GetSlider((int)Sliders.ExpSliderObject).value = _game.Player.ExpRatio;
        GetText((int)Texts.CharacterLevelValueText).text = $"{_game.Player.Level}";
        GetText((int)Texts.KillValueText).text = $"{_game.Player.KillCount}";
        GetText((int)Texts.SoulValueText).text = _game.Player.SoulCount.ToString();
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetObject((int)GameObjects.WaveObject).GetComponent<RectTransform>());
    }

    private void OnClickPauseButton(PointerEventData evt)
    {
        Managers.Sound.PlayButtonClick();
        Managers.UI.ShowPopupUI<UI_PausePopup>();
    }

    private void OnClickSoulShopButton(PointerEventData evt)
    {
        Managers.Sound.Play(Define.ESound.Effect, "PopupOpen_SoulShop");
        //상점 활성화
        SetActiveSoulShop(true);

        if (Managers.Game.ContinueInfo.SoulShopList.Count == 0)
            ResetSupportCard();
        else
            LoadSupportCard();

        RefreshUI();
    }

    private void OnClickCardListResetButton(PointerEventData evt) // 영혼상점 리스트 리셋 버튼
    {
        Managers.Sound.PlayButtonClick();
        // 서포트 카드 리스트 내용 리셋
        int cardListResetCost = (int)Define.SOUL_SHOP_COST_PROB[6];
        if (Managers.Game.Player.SoulCount >= cardListResetCost)
        {
            Managers.Game.Player.SoulCount -= cardListResetCost;
            ResetSupportCard();
        }
    }

    private void OnClickSoulShopCloseButton(PointerEventData evt)
    {
        Managers.Sound.PlayButtonClick();

        //상점 비활성화
        SetActiveSoulShop(false);
    }

    private void OnClickTotalDamageButton(PointerEventData evt) // 토탈 데미지 버튼
    {
        Managers.Sound.PlayButtonClick();
        Managers.UI.ShowPopupUI<UI_TotalDamagePopup>().SetInfo();
    }

    private void OnClickSupportSkillListButton(PointerEventData evt) // 서포트 스킬 리스트 버튼
    {
        Managers.Sound.PlayButtonClick();
        if (_isSupportSkillListButton) // 이미 눌렀다면 닫기
            SoulShopInit(); // 영혼 상점 초기 상태
        else
        {
            GetObject((int)GameObjects.SoulShopObject).GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1);
            GetObject((int)GameObjects.SoulShopObject).GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0);
            GetObject((int)GameObjects.SoulShopObject).GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0);
            GetObject((int)GameObjects.SoulShopObject).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 1120);

            PopupOpenAnimation(GetObject((int)GameObjects.SoulShopObject));
            GetObject((int)GameObjects.OwnBattleSkillInfoObject).gameObject.SetActive(false); // 배틀 스킬 리스트 비활성화
            GetObject((int)GameObjects.SupportSkillListScrollObject).gameObject.SetActive(true); // 서포트 스킬 리스트 활성화
            _isSupportSkillListButton = true;

            ResetSupportSkillList();
        }
    }

    private void OnPlayerDataUpdated()
    {
        GetSlider((int)Sliders.ExpSliderObject).value = _game.Player.ExpRatio;
        GetText((int)Texts.KillValueText).text = $"{_game.Player.KillCount}";
        GetText((int)Texts.SoulValueText).text = _game.Player.SoulCount.ToString();
    }

    private void OnPlayerLevelUp()
    {
        if (Managers.Game.IsGameEnd) return;

        List<SkillBase> list = Managers.Game.Player.Skills.RecommendSkills();

        if (list.Count > 0)
        {
            //Managers.UI.ShowPopupUI<UI_SkillSelectPopup>();
        }
    }

    private void OnLevelUpSkill()
    {

    }

    private void OnPlayerMove()
    {

    }

    private void SetBattleSkill()
    {

    }

    public void OnDamaged()
    {
        StartCoroutine(CoBloodScreen());
    }
    public void DoWhiteFlash()
    {
        StartCoroutine(CoWhiteScreen());
    }

    IEnumerator CoBloodScreen()
    {
        Color targetColor = new Color(1, 0, 0, 0.3f);

        yield return null;
        Sequence seq = DOTween.Sequence();

        seq.Append(GetObject((int)GameObjects.OnDamaged).GetComponent<Image>().DOColor(targetColor, 0.3f))
            .Append(GetObject((int)GameObjects.OnDamaged).GetComponent<Image>().DOColor(Color.clear, 0.3f)).OnComplete(() => { });
    }

    IEnumerator CoWhiteScreen()
    {
        Color targetColor = new Color(1, 1, 1, 1f);

        yield return null;
        Sequence seq = DOTween.Sequence();

        seq.Append(GetObject((int)GameObjects.WhiteFlash).GetComponent<Image>().DOFade(1, 0.1f))
            .Append(GetObject((int)GameObjects.WhiteFlash).GetComponent<Image>().DOFade(0, 0.2f)).OnComplete(() => { });
    }

    public void MonsterInfoUpdate(MonsterController monster)
    {
        if (monster.ObjectType == Define.EObjectType.EliteMonster)
        {
            if (monster.CreatureState != Define.ECreatureState.Dead)
            {
                GetObject((int)GameObjects.EliteInfoObject).SetActive(true);
                GetObject((int)GameObjects.EliteHpSliderObject).GetComponent<Slider>().value = monster.Hp / monster.MaxHp;
                GetText((int)Texts.EliteNameValueText).text = monster.CreatureData.DescriptionTextID;
            }
            else
            {
                GetObject((int)GameObjects.EliteInfoObject).SetActive(false);
            }
        }
        else if (monster.ObjectType == Define.EObjectType.Boss)
        {
            if (monster.CreatureState != Define.ECreatureState.Dead)
            {
                GetObject((int)GameObjects.BossInfoObject).SetActive(true);
                GetObject((int)GameObjects.BossHpSliderObject).GetComponent<Slider>().value = monster.Hp / monster.MaxHp;
                GetText((int)Texts.BossNameValueText).text = monster.CreatureData.DescriptionTextID;
            }
            else
            {
                GetObject((int)GameObjects.BossInfoObject).SetActive(false);
            }
        }
    }

    private void SetActiveSoulShop(bool isActive)
    {
        if (isActive)
        {
            SoulShopInit(); // 영혼 상점 초기 상태
            GetComponent<Canvas>().sortingOrder = Define.UI_GAMESCENE_SORT_OPEN;
        }
        else
        {
            GetComponent<Canvas>().sortingOrder = Define.UI_GAMESCENE_SORT_CLOSED;

            // 영혼 상점 초기 상태
            GetObject((int)GameObjects.SoulShopObject).GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1);
            GetObject((int)GameObjects.SoulShopObject).GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0);
            GetObject((int)GameObjects.SoulShopObject).GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0);
            GetObject((int)GameObjects.SoulShopObject).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);

            PopupOpenAnimation(GetObject((int)GameObjects.SoulShopObject));
            GetButton((int)Buttons.SoulShopCloseButton).gameObject.SetActive(false); // 영혼 상점 버튼 비활성화
            GetButton((int)Buttons.SoulShopLeadButton).gameObject.SetActive(true);
            GetButton((int)Buttons.SoulShopBackgroundButton).gameObject.SetActive(false); // 영혼 상점 배경
            GetObject((int)GameObjects.OwnBattleSkillInfoObject).gameObject.SetActive(false); // 배틀 스킬 리스트 비활성화
            GetObject((int)GameObjects.SupportSkillListScrollObject).gameObject.SetActive(false); // 서포트 스킬 리스트 비활성화
            _isSupportSkillListButton = false;
            Managers.UI.IsActiveSoulShop = false;
        }
    }

    private void SoulShopInit()
    {

    }

    private void ResetSupportCard()
    {

    }

    private void LoadSupportCard()
    {

    }

    private void ResetSupportSkillList()
    {
        GameObject container = GetObject((int)GameObjects.SupportSkillListScrollContentObject);
        container.DestroyChildren();

        List<Data.SupportSkillData> temp = Managers.Game.Player.Skills.SupportSkills.OrderByDescending(x => x.DataId).ToList();
        foreach (Data.SupportSkillData skill in temp)
        {
            UI_SupportSkillItem item = Managers.UI.MakeSubItem<UI_SupportSkillItem>(container.transform);
            ScrollRect scrollRect = GetObject((int)GameObjects.SupportSkillListScrollObject).GetComponent<ScrollRect>();
            item.SetInfo(skill, this.transform, scrollRect);
            Managers.Game.SoulShopList.Add(skill);
        }
    }
}