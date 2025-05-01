using Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class StageClearInfo
{
    public int StageIndex = 1;
    public int MaxWaveIndex = 0;
    public bool isOpenFirstBox = false;
    public bool isOpenSecondBox = false;
    public bool isOpenThirdBox = false;
    public bool isClear = false;
}

[Serializable]
public class MissionInfo
{
    public int Progress;
    public bool IsRewarded;
}

/// <summary>
/// 계정에관한 모든 정보 
[Serializable]
public class GameData
{
    public int UserLevel = 1;
    public string UserName = "Player";

    public int Stamina = Define.MAX_STAMINA;
    public int Gold = 0;
    public int Dia = 0;

    #region 업적
    public int CommonGachaOpenCount = 0;
    public int AdvancedGachaOpenCount = 0;
    public int FastRewardCount = 0;
    public int OfflineRewardCount = 0;
    public int TotalMonsterKillCount = 0;
    public int TotalEliteKillCount = 0;
    public int TotalBossKillCount = 0;
    public List<Data.AchievementData> Achievements = new List<AchievementData>();  // 업적 목록
    #endregion

    #region 하루마다 초기화
    public int GachaCountAdsAnvanced = 1;
    public int GachaCountAdsCommon = 1;
    public int GoldCountAds = 1;
    public int RebirthCountAds = 3;
    public int DiaCountAds = 3;
    public int StaminaCountAds = 1;
    public int FastRewardCountAds = 1;
    public int FastRewardCountStamina = 3;
    public int SkillRefreshCountAds = 3;
    public int RemainsStaminaByDia = 3;
    public int BronzeKeyCountAds = 1;
    #endregion

    public bool[] AttendanceReceived = new bool[30];
    public bool BGMOn = true;
    public bool EffectSoundOn = true;
    public Define.EJoystickType JoystickType = Define.EJoystickType.Flexible;
    public List<Character> Characters = new List<Character>();
    public List<Equipment> OwnedEquipments = new List<Equipment>();
    public ContinueData ContinueInfo = new ContinueData();
    public StageData CurrentStage = new StageData();
    public Dictionary<int, int> ItemDictionary = new Dictionary<int, int>();//<ID, 갯수>
    public Dictionary<Define.EEquipmentType, Equipment> EquippedEquipments = new Dictionary<Define.EEquipmentType, Equipment>();
    public Dictionary<int, StageClearInfo> DicStageClearInfo = new Dictionary<int, StageClearInfo>();
    public Dictionary<Define.EMissionTarget, MissionInfo> DicMission = new Dictionary<Define.EMissionTarget, MissionInfo>()
    {
        {Define.EMissionTarget.StageEnter, new MissionInfo() { Progress = 0, IsRewarded = false }},
        {Define.EMissionTarget.StageClear, new MissionInfo() { Progress = 0, IsRewarded = false }},
        {Define.EMissionTarget.EquipmentLevelUp, new MissionInfo() { Progress = 0, IsRewarded = false }},
        {Define.EMissionTarget.OfflineRewardGet, new MissionInfo() { Progress = 0, IsRewarded = false }},
        {Define.EMissionTarget.EquipmentMerge, new MissionInfo() { Progress = 0, IsRewarded = false }},
        {Define.EMissionTarget.MonsterKill, new MissionInfo() { Progress = 0, IsRewarded = false }},
        {Define.EMissionTarget.EliteMonsterKill, new MissionInfo() { Progress = 0, IsRewarded = false }},
        {Define.EMissionTarget.GachaOpen, new MissionInfo() { Progress = 0, IsRewarded = false }},
        {Define.EMissionTarget.ADWatchIng, new MissionInfo() { Progress = 0, IsRewarded = false }},
    };
}

[Serializable]
public class ContinueData
{
    public bool isContinue { get { return SavedBattleSkill.Count > 0; } }
    public int PlayerDataId;
    public float Hp;
    public float MaxHp;
    public float MaxHpBonusRate = 1;
    public float HealBonusRate = 1;
    public float HpRegen;
    public float Atk;
    public float AttackRate = 1;
    public float Def;
    public float DefRate;
    public float MoveSpeed;
    public float MoveSpeedRate = 1;
    public float TotalExp;
    public int Level = 1;
    public float Exp;
    public float CriRate;
    public float CriDamage = 1.5f;
    public float DamageReduction;
    public float ExpBonusRate = 1;
    public float SoulBonusRate = 1;
    public float CollectDistBonus = 1;
    public int KillCount;
    public int SkillRefreshCount = 3;
    public float SoulCount;

    public List<SupportSkillData> SoulShopList = new List<SupportSkillData>();
    public List<SupportSkillData> SavedSupportSkill = new List<SupportSkillData>();
    public Dictionary<Define.ESkillType, int> SavedBattleSkill = new Dictionary<Define.ESkillType, int>();

    public int WaveIndex;
    public void Clear()
    {
        // 각 변수의 초기값 설정
        PlayerDataId = 0;
        Hp = 0f;
        MaxHp = 0f;
        MaxHpBonusRate = 1f;
        HealBonusRate = 1f;
        HpRegen = 0f;
        Atk = 0f;
        AttackRate = 1f;
        Def = 0f;
        DefRate = 0f;
        MoveSpeed = 0f;
        MoveSpeedRate = 1f;
        TotalExp = 0f;
        Level = 1;
        Exp = 0f;
        CriRate = 0f;
        CriDamage = 1.5f;
        DamageReduction = 0f;
        ExpBonusRate = 1f;
        SoulBonusRate = 1f;
        CollectDistBonus = 1f;

        KillCount = 0;
        SoulCount = 0f;
        SkillRefreshCount = 3;

        SoulShopList.Clear();
        SavedSupportSkill.Clear();
        SavedBattleSkill.Clear();

    }
}

public class GameManager
{
    #region GameData
    private GameData _gameData = new GameData();
    public List<Equipment> OwnedEquipments
    {
        get { return _gameData.OwnedEquipments; }
        set
        {
            _gameData.OwnedEquipments = value;
            //갱신이 빈번하게 발생하여 렉 발생, Sorting시 무한루프 발생으로 인하여 주석처리
            //EquipInfoChanged?.Invoke();
        }
    }

    public List<SupportSkillData> SoulShopList
    {
        get { return _gameData.ContinueInfo.SoulShopList; }
        set
        {
            _gameData.ContinueInfo.SoulShopList = value;
            SaveGame();
        }
    }

    public Dictionary<int, int> ItemDictionary
    {
        get { return _gameData.ItemDictionary; }
        set
        {
            _gameData.ItemDictionary = value;
        }
    }

    public Dictionary<Define.EMissionTarget, MissionInfo> DicMission
    {
        get { return _gameData.DicMission; }
        set
        {
            _gameData.DicMission = value;
        }
    }

    public List<Character> Characters
    {
        get { return _gameData.Characters; }
        set
        {
            _gameData.Characters = value;
            EquipInfoChanged?.Invoke();
        }
    }
    public Dictionary<Define.EEquipmentType, Equipment> EquippedEquipments
    {
        get { return _gameData.EquippedEquipments; }
        set
        {
            _gameData.EquippedEquipments = value;
            EquipInfoChanged?.Invoke();
        }
    }

    public Dictionary<int, StageClearInfo> DicStageClearInfo
    {
        get { return _gameData.DicStageClearInfo; }
        set
        {
            _gameData.DicStageClearInfo = value;
            // TODO ILHAK
            // Managers.Achievement.StageClear();
            SaveGame();
        }
    }

    public int UserLevel
    {
        get { return _gameData.UserLevel; }
        set { _gameData.UserLevel = value; }
    }
    public string UserName
    {
        get { return _gameData.UserName; }
        set { _gameData.UserName = value; }
    }
    public int Stamina
    {
        get { return _gameData.Stamina; }
        set
        {
            _gameData.Stamina = value;
            SaveGame();
            OnResourcesChagned?.Invoke();
        }
    }
    public int Gold
    {
        get { return _gameData.Gold; }
        set
        {
            _gameData.Gold = value;
            SaveGame();
            OnResourcesChagned?.Invoke();
        }
    }
    public int Dia
    {
        get { return _gameData.Dia; }
        set
        {
            _gameData.Dia = value;
            SaveGame();
            OnResourcesChagned?.Invoke();
        }
    }

    //public int CommonGachaOpenCount
    //{
    //    get { return _gameData.CommonGachaOpenCount; }
    //    set
    //    {
    //        _gameData.CommonGachaOpenCount = value;
    //        Managers.Achievement.CommonOpen();
    //    }
    //}
    //public int AdvancedGachaOpenCount
    //{
    //    get { return _gameData.AdvancedGachaOpenCount; }
    //    set
    //    {
    //        _gameData.AdvancedGachaOpenCount = value;
    //        Managers.Achievement.AdvancedOpen();
    //    }
    //}
    //public int FastRewardCount
    //{
    //    get { return _gameData.FastRewardCount; }
    //    set
    //    {
    //        _gameData.FastRewardCount = value;
    //        Managers.Achievement.FastReward();
    //    }
    //}
    //public int OfflineRewardCount
    //{
    //    get { return _gameData.OfflineRewardCount; }
    //    set
    //    {
    //        _gameData.OfflineRewardCount = value;
    //        Managers.Achievement.OfflineReward();
    //    }
    //}
    //public int TotalMonsterKillCount
    //{
    //    get { return _gameData.TotalMonsterKillCount; }
    //    set
    //    {
    //        _gameData.TotalMonsterKillCount = value;
    //        if (value % 100 == 0)
    //            Managers.Achievement.MonsterKill();
    //    }
    //}
    //public int TotalEliteKillCount
    //{
    //    get { return _gameData.TotalEliteKillCount; }
    //    set
    //    {
    //        _gameData.TotalEliteKillCount = value;
    //        Managers.Achievement.EliteKill();
    //    }
    //}
    //public int TotalBossKillCount
    //{
    //    get { return _gameData.TotalBossKillCount; }
    //    set
    //    {
    //        _gameData.TotalBossKillCount = value;
    //        Managers.Achievement.BossKill();
    //    }
    //}

    public ContinueData ContinueInfo
    {
        get { return _gameData.ContinueInfo; }
        set { _gameData.ContinueInfo = value; }
    }

    public StageData CurrentStageData
    {
        get { return _gameData.CurrentStage; }
        set { _gameData.CurrentStage = value; }
    }

    public WaveData CurrentWaveData
    {
        get { return CurrentStageData.WaveArray[CurrentWaveIndex]; }
    }

    public int CurrentWaveIndex
    {
        get { return _gameData.ContinueInfo.WaveIndex; }
        set { _gameData.ContinueInfo.WaveIndex = value; }
    }

    public Map CurrentMap { get; set; }

    #endregion

    #region Option
    public bool BGMOn
    {
        get { return _gameData.BGMOn; }
        set
        {
            if (_gameData.BGMOn == value)
                return;
            _gameData.BGMOn = value;
            if (_gameData.BGMOn == false)
            {
                Managers.Sound.Stop(Define.ESound.Bgm);
            }
            else
            {
                string name = "Bgm_Lobby";
                if (Managers.Scene.CurrentScene.SceneType == Define.EScene.GameScene)
                    name = "Bgm_Game";

                Managers.Sound.Play(Define.ESound.Bgm, name);
            }
        }
    }

    public bool EffectSoundOn
    {
        get { return _gameData.EffectSoundOn; }
        set { _gameData.EffectSoundOn = value; }
    }

    public Define.EJoystickType JoystickType
    {
        get { return _gameData.JoystickType; }
        set { _gameData.JoystickType = value; }
    }
    #endregion

    public int RemainsStaminaByDia
    {
        get { return _gameData.RemainsStaminaByDia; }
        set
        {
            _gameData.RemainsStaminaByDia = value;
        }
    }

    public int StaminaCountAds
    {
        get { return _gameData.StaminaCountAds; }
        set
        {
            _gameData.StaminaCountAds = value;
        }
    }

    #region Character
    public Character CurrentCharacter
    {
        get
        {
            return _gameData.Characters.Find(c => c.isCurrentCharacter == true);
        }
    }
    #endregion

    #region Player
    public PlayerController Player { get; set; }
    Vector2 _moveDir;
    public Vector2 MoveDir
    {
        get { return _moveDir; }
        set
        {
            _moveDir = value;
            OnMoveDirChanged?.Invoke(_moveDir);
        }
    }
    #endregion

    #region Action
    public event Action<Vector2> OnMoveDirChanged;
    public event Action EquipInfoChanged;
    public event Action OnResourcesChagned;
    //public Action OnMonsterDataUpdated;
    #endregion

    public float TimeRemaining = 60;
    public Vector3 SoulDestination { get; set; }
    public bool IsLoaded = false;
    public bool IsGameEnd = false;

    public CameraController CameraController { get; set; }

    public void ExchangeMaterial(MaterialData data, int count)
    {
        switch (data.MaterialType)
        {
            case Define.EMaterialType.Dia:
                Dia += count;
                break;
            case Define.EMaterialType.Gold:
                Gold += count;
                break;
            case Define.EMaterialType.Stamina:
                Stamina += count;
                break;
            case Define.EMaterialType.BronzeKey:
            case Define.EMaterialType.SilverKey:
            case Define.EMaterialType.GoldKey:
                AddMaterialItem(data.DataId, count);
                break;
            case Define.EMaterialType.RandomScroll:
                int randScroll = UnityEngine.Random.Range(50101, 50106);
                AddMaterialItem(randScroll, count);
                break;
            case Define.EMaterialType.WeaponScroll:
                AddMaterialItem(Define.ID_WEAPON_SCROLL, count);
                break;
            case Define.EMaterialType.GlovesScroll:
                AddMaterialItem(Define.ID_GLOVES_SCROLL, count);
                break;
            case Define.EMaterialType.RingScroll:
                AddMaterialItem(Define.ID_RING_SCROLL, count);
                break;
            case Define.EMaterialType.BeltScroll:
                AddMaterialItem(Define.ID_BELT_SCROLL, count);
                break;
            case Define.EMaterialType.ArmorScroll:
                AddMaterialItem(Define.ID_ARMOR_SCROLL, count);
                break;
            case Define.EMaterialType.BootsScroll:
                AddMaterialItem(Define.ID_BOOTS_SCROLL, count);
                break;
            default:
                //TODO 
                break;
        }
    }

    public void SetNextStage()
    {
        CurrentStageData = Managers.Data.StageDic[CurrentStageData.StageIndex + 1];
    }

    public void AddMaterialItem(int id, int quantity)
    {
        if (ItemDictionary.ContainsKey(id))
        {
            ItemDictionary[id] += quantity;
        }
        else
        {
            ItemDictionary[id] = quantity;
        }
        SaveGame();
    }

    public void RemovMaterialItem(int id, int quantity)
    {
        if (ItemDictionary.ContainsKey(id))
        {
            ItemDictionary[id] -= quantity;
            SaveGame();
        }
    }


    #region InGame
    public GemInfo GetGemInfo()
    {
        float smallGemChance = CurrentWaveData.SmallGemDropRate;
        float greenGemChance = CurrentWaveData.GreenGemDropRate + smallGemChance;
        float blueGemChance = CurrentWaveData.BlueGemDropRate + greenGemChance;
        float yellowGemChance = CurrentWaveData.YellowGemDropRate + blueGemChance;
        float rand = Random.value;

        if (rand < smallGemChance)
            return new GemInfo(Define.EGemType.Small, new Vector3(0.65f, 0.65f, 0.65f));
        else if (rand < greenGemChance)
            return new GemInfo(Define.EGemType.Green, Vector3.one);
        else if (rand < blueGemChance)
            return new GemInfo(Define.EGemType.Blue, Vector3.one);
        else if (rand < yellowGemChance)
            return new GemInfo(Define.EGemType.Yellow, Vector3.one);

        return null;
    }

    public GemInfo GetGemInfo(Define.EGemType type)
    {
        if (type == Define.EGemType.Small)
            return new GemInfo(Define.EGemType.Small, new Vector3(0.65f, 0.65f, 0.65f));

        return new GemInfo(type, Vector3.one);
    }

    public void GameOver()
    {
        IsGameEnd = true;
        Player.StopAllCoroutines();
        // TODO ILHAK
        //Managers.UI.ShowPopupUI<UI_GameoverPopup>().SetInfo();
    }

    public (int hp, int atk) GetCurrentChracterStat()
    {
        int hpBonus = 0;
        int AtkBonus = 0;
        // TODO ILHAK
        //var (equipHpBonus, equipAtkBonus) = GetEquipmentBonus();

        //Character ch = CurrentCharacter;

        //hpBonus = (equipHpBonus);
        //AtkBonus = (equipAtkBonus);

        return (hpBonus, AtkBonus);
    }
    #endregion

    #region Save&Load
    string _path;

    public void SaveGame()
    {
        // TODO ILHAK
        //if (Player != null)
        //{
        //    _gameData.ContinueInfo.SavedBattleSkill = Player.Skills?.SavedBattleSkill;
        //    _gameData.ContinueInfo.SavedSupportSkill = Player.Skills?.SupportSkills;
        //}
        string jsonStr = JsonConvert.SerializeObject(_gameData);
        //File.WriteAllText(_path, jsonStr);
    }

    public void ClearContinueData()
    {
        // TODO ILHAK
        //Managers.Game.SoulShopList.Clear();
        //ContinueInfo.Clear();
        //CurrentWaveIndex = 0;
        SaveGame();
    }

    public float GetTotalDamage()
    {
        float result = 0;

        foreach (SkillBase skill in Player.Skills.SkillList)
        {
            result += skill.TotalDamage;
        }

        return result;
    }
    #endregion
}