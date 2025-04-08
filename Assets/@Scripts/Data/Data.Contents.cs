using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using static Define;

namespace Data
{
    //엑셀 파싱에 제외할 어트리뷰트 정의
    [AttributeUsage(AttributeTargets.Field)]
    public class ExcludeFieldAttribute : Attribute
    {
    }

    #region LevelData
    [Serializable]
    public class LevelData
    {
        public int Level;
        public int TotalExp;
        public int RequiredExp;
    }

    [Serializable]
    public class LevelDataLoader : ILoader<int, LevelData>
    {
        public List<LevelData> levels = new List<LevelData>();
        public Dictionary<int, LevelData> MakeDict()
        {
            Dictionary<int, LevelData> dict = new Dictionary<int, LevelData>();
            foreach (LevelData levelData in levels)
                dict.Add(levelData.Level, levelData);
            return dict;
        }

        public bool Validate()
        {
            return true;
        }
    }
    #endregion

    #region CreatureData
    [Serializable]
    public class CreatureDataInternal
    {
        public int DataId;
        public string DescriptionTextID;
        public string PrefabLabel;
        public float MaxHp;
        public float MaxHpBonus;
        public float Atk;
        public float AtkBonus;
        public float Def;
        public float MoveSpeed;
        public float TotalExp;
        public float HpRate;
        public float AtkRate;
        public float DefRate;
        public float MoveSpeedRate;
        public string IconLabel;
        public string SkillTypeList;//InGameSkills를 제외한 추가스킬들
    }

    public class CreatureData
    {
        public int DataId;
        public string DescriptionTextID;
        public string PrefabLabel;
        public float MaxHp;
        public float MaxHpBonus;
        public float Atk;
        public float AtkBonus;
        public float Def;
        public float MoveSpeed;
        public float TotalExp;
        public float HpRate;
        public float AtkRate;
        public float DefRate;
        public float MoveSpeedRate;
        public string IconLabel;
        public List<int> SkillTypeList;//InGameSkills를 제외한 추가스킬들
    }

    [Serializable]
    public class CreatureDataLoader : ILoader<int, CreatureData>
    {
        public List<CreatureDataInternal> creatures = new List<CreatureDataInternal>();
        public Dictionary<int, CreatureData> MakeDict()
        {
            Dictionary<int, CreatureData> dict = new Dictionary<int, CreatureData>();
            foreach (CreatureDataInternal creature in creatures)
            {
                var data = new CreatureData
                {
                    DataId = creature.DataId,
                    DescriptionTextID = creature.DescriptionTextID,
                    PrefabLabel = creature.PrefabLabel,
                    MaxHp = creature.MaxHp,
                    MaxHpBonus = creature.MaxHpBonus,
                    Atk = creature.Atk,
                    AtkBonus = creature.AtkBonus,
                    Def = creature.Def,
                    MoveSpeed = creature.MoveSpeed,
                    TotalExp = creature.TotalExp,
                    HpRate = creature.HpRate,
                    AtkRate = creature.AtkRate,
                    DefRate = creature.DefRate,
                    MoveSpeedRate = creature.MoveSpeedRate,
                    IconLabel = creature.IconLabel,
                    SkillTypeList = new List<int>()
                };

                if (string.IsNullOrEmpty(creature.SkillTypeList) != true)
                {
                    string[] skills = creature.SkillTypeList.Split('&');

                    if (skills != null && skills.Length > 0)
                    {
                        foreach (string skill in skills)
                        {
                            if (int.TryParse(skill, out int skillNumber))
                                data.SkillTypeList.Add(skillNumber);
                        }
                    }
                }

                dict.Add(data.DataId, data);
            }
            return dict;
        }

        public bool Validate()
        {
            return true;
        }
    }
    #endregion

    #region SkillData
    [Serializable]
    public class SkillData
    {
        public int DataId;
        public string Name;
        public string Description;
        public string PrefabLabel; //프리팹 경로
        public string IconLabel;//아이콘 경로
        public string SoundLabel;// 발동사운드 경로
        public string Category;//스킬 카테고리
        public float CoolTime; // 쿨타임
        public float DamageMultiplier; //스킬데미지 (곱하기)
        public float ProjectileSpacing;// 발사체 사이 간격
        public float Duration; //스킬 지속시간
        public float RecognitionRange;//인식범위
        public int NumProjectiles;// 회당 공격횟수
        public string CastingSound; // 시전사운드
        public float AngleBetweenProj;// 발사체 사이 각도
        public float AttackInterval; //공격간격
        public int NumBounce;//바운스 횟수
        public float BounceSpeed;// 바운스 속도
        public float BounceDist;//바운스 거리
        public int NumPenerations; //관통 횟수
        public int CastingEffect; // 스킬 발동시 효과
        public string HitSoundLabel; // 히트사운드
        public float ProbCastingEffect; // 스킬 발동 효과 확률
        public int HitEffect;// 적중시 이펙트
        public float ProbHitEffect; // 스킬 발동 효과 확률
        public float ProjRange; //투사체 사거리
        public float MinCoverage; //최소 효과 적용 범위
        public float MaxCoverage; // 최대 효과 적용 범위
        public float RoatateSpeed; // 회전 속도
        public float ProjSpeed; //발사체 속도
        public float ScaleMultiplier;
    }
    [Serializable]
    public class SkillDataLoader : ILoader<int, SkillData>
    {
        public List<SkillData> skills = new List<SkillData>();

        public Dictionary<int, SkillData> MakeDict()
        {
            Dictionary<int, SkillData> dict = new Dictionary<int, SkillData>();
            foreach (SkillData skill in skills)
                dict.Add(skill.DataId, skill);
            return dict;
        }

        public bool Validate()
        {
            return true;
        }
    }
    #endregion

    #region SupportSkilllData
    [Serializable]
    public class SupportSkillDataInternal
    {
        public int DataId;
        public Define.ESupportSkillType SupportSkillType;
        public Define.ESupportSkillName SupportSkillName;
        public Define.ESupportSkillGrade SupportSkillGrade;
        public string Name;
        public string Description;
        public string IconLabel;
        public float HpRegen;
        public float HealRate; // 회복량 (최대HP%)
        public float HealBonusRate; // 회복량 증가
        public float MagneticRange; // 아이템 습득 범위
        public int SoulAmount; // 영혼획득
        public float HpRate;
        public float AtkRate;
        public float DefRate;
        public float MoveSpeedRate;
        public float CriRate;
        public float CriDmg;
        public float DamageReduction;
        public float ExpBonusRate;
        public float SoulBonusRate;
        public float ProjectileSpacing;// 발사체 사이 간격
        public float Duration; //스킬 지속시간
        public int NumProjectiles;// 회당 공격횟수
        public float AttackInterval; //공격간격
        public int NumBounce;//바운스 횟수
        public int NumPenerations; //관통 횟수
        public float ProjRange; //투사체 사거리
        public float RoatateSpeed; // 회전 속도
        public float ScaleMultiplier;
        public float Price;
    }

    public class SupportSkillData
    {
        public int AcquiredLevel;
        public int DataId;
        public Define.ESupportSkillType SupportSkillType;
        public Define.ESupportSkillName SupportSkillName;
        public Define.ESupportSkillGrade SupportSkillGrade;
        public string Name;
        public string Description;
        public string IconLabel;
        public float HpRegen;
        public float HealRate; // 회복량 (최대HP%)
        public float HealBonusRate; // 회복량 증가
        public float MagneticRange; // 아이템 습득 범위
        public int SoulAmount; // 영혼획득
        public float HpRate;
        public float AtkRate;
        public float DefRate;
        public float MoveSpeedRate;
        public float CriRate;
        public float CriDmg;
        public float DamageReduction;
        public float ExpBonusRate;
        public float SoulBonusRate;
        public float ProjectileSpacing;// 발사체 사이 간격
        public float Duration; //스킬 지속시간
        public int NumProjectiles;// 회당 공격횟수
        public float AttackInterval; //공격간격
        public int NumBounce;//바운스 횟수
        public int NumPenerations; //관통 횟수
        public float ProjRange; //투사체 사거리
        public float RoatateSpeed; // 회전 속도
        public float ScaleMultiplier;
        public float Price;
        public bool IsLocked = false;
        public bool IsPurchased = false;

        //public bool CheckRecommendationCondition()
        //{
        //    if (IsLocked == true || Managers.Game.SoulShopList.Contains(this) == true)
        //    {
        //        return false;
        //    }

        //    if (SupportSkillType == Define.ESupportSkillType.Special)
        //    {
        //        //내가 가지고 있는 장비스킬이 아니면 false
        //        if (Managers.Game.EquippedEquipments.TryGetValue(Define.EEquipmentType.Weapon, out Equipment myWeapon))
        //        {
        //            int skillId = myWeapon.EquipmentData.BasicSkill;
        //            Define.ESkillType type = Utils.GetSkillTypeFromInt(skillId);

        //            switch (SupportSkillName)
        //            {
        //                case Define.ESupportSkillName.ArrowShot:
        //                case Define.ESupportSkillName.SavageSmash:
        //                case Define.ESupportSkillName.PhotonStrike:
        //                case Define.ESupportSkillName.Shuriken:
        //                case Define.ESupportSkillName.EgoSword:
        //                    if (SupportSkillName.ToString() != type.ToString())
        //                        return false;
        //                    break;
        //            }

        //        }
        //    }
        //    #region 서포트 스킬 중복 방지 모드 보류
        //    //if (Managers.Game.Player.Skills.SupportSkills.TryGetValue(SupportSkillName, out var existingSkill))
        //    //{
        //    //    if (existingSkill == null)
        //    //        return true;

        //    //    if (DataId <= existingSkill.DataId)
        //    //    {
        //    //        return false;
        //    //    }
        //    //}
        //    #endregion

        //    return true;
        //}
    }
    [Serializable]
    public class SupportSkillDataLoader : ILoader<int, SupportSkillData>
    {
        public List<SupportSkillDataInternal> supportSkills = new List<SupportSkillDataInternal>();

        public Dictionary<int, SupportSkillData> MakeDict()
        {
            Dictionary<int, SupportSkillData> dict = new Dictionary<int, SupportSkillData>();
            foreach (SupportSkillDataInternal skill in supportSkills)
            {
                var data = new SupportSkillData()
                {
                    DataId = skill.DataId,
                    SupportSkillType = skill.SupportSkillType,
                    SupportSkillName = skill.SupportSkillName,
                    SupportSkillGrade = skill.SupportSkillGrade,
                    Name = skill.Name,
                    Description = skill.Description,
                    IconLabel = skill.IconLabel,
                    HpRegen = skill.HpRegen,
                    HealRate = skill.HealRate,
                    HealBonusRate = skill.HealBonusRate,
                    MagneticRange = skill.MagneticRange,
                    SoulAmount = skill.SoulAmount,
                    HpRate = skill.HpRate,
                    AtkRate = skill.AtkRate,
                    DefRate = skill.DefRate,
                    MoveSpeedRate = skill.MoveSpeedRate,
                    CriRate = skill.CriRate,
                    CriDmg = skill.CriDmg,
                    DamageReduction = skill.DamageReduction,
                    ExpBonusRate = skill.ExpBonusRate,
                    SoulBonusRate = skill.SoulBonusRate,
                    ProjectileSpacing = skill.ProjectileSpacing,
                    Duration = skill.Duration,
                    NumProjectiles = skill.NumProjectiles,
                    AttackInterval = skill.AttackInterval,
                    NumBounce = skill.NumBounce,
                    NumPenerations = skill.NumPenerations,
                    ProjRange = skill.ProjRange,
                    RoatateSpeed = skill.RoatateSpeed,
                    ScaleMultiplier = skill.ScaleMultiplier,
                    Price = skill.Price,
                    //AcquiredLevel = 0,
                    //IsLocked = false,
                    //IsPurchased = false
                };
                dict.Add(data.DataId, data);
            }
            return dict;
        }

        public bool Validate()
        {
            return true;
        }
    }
    #endregion

    #region StageData
    [Serializable]
    public class StageDataInternal
    {
        public int StageIndex = 1;
        public string StageName;
        public int StageLevel = 1;
        public string MapName;
        public int StageSkill;

        public int FirstWaveCountValue;
        public int FirstWaveClearRewardItemId;
        public int FirstWaveClearRewardItemValue;

        public int SecondWaveCountValue;
        public int SecondWaveClearRewardItemId;
        public int SecondWaveClearRewardItemValue;

        public int ThirdWaveCountValue;
        public int ThirdWaveClearRewardItemId;
        public int ThirdWaveClearRewardItemValue;

        public int ClearReward_Gold;
        public int ClearReward_Exp;
        public string StageImage;
        public string AppearingMonsters;
    }

    public class StageData
    {
        public int StageIndex = 1;
        public string StageName;
        public int StageLevel = 1;
        public string MapName;
        public int StageSkill;

        public int FirstWaveCountValue;
        public int FirstWaveClearRewardItemId;
        public int FirstWaveClearRewardItemValue;

        public int SecondWaveCountValue;
        public int SecondWaveClearRewardItemId;
        public int SecondWaveClearRewardItemValue;

        public int ThirdWaveCountValue;
        public int ThirdWaveClearRewardItemId;
        public int ThirdWaveClearRewardItemValue;

        public int ClearReward_Gold;
        public int ClearReward_Exp;
        public string StageImage;
        [ExcludeField]
        public List<int> AppearingMonsters;
        [ExcludeField]
        public List<WaveData> WaveArray;
    }

    public class StageDataLoader : ILoader<int, StageData>
    {
        public List<StageDataInternal> stages = new List<StageDataInternal>();

        public Dictionary<int, StageData> MakeDict()
        {
            Dictionary<int, StageData> dict = new Dictionary<int, StageData>();
            foreach (StageDataInternal stage in stages)
            {
                StageData data = new StageData()
                {
                    StageIndex = stage.StageIndex,
                    StageName = stage.StageName,
                    StageLevel = stage.StageLevel,
                    MapName = stage.MapName,
                    StageSkill = stage.StageSkill,
                    FirstWaveCountValue = stage.FirstWaveCountValue,
                    FirstWaveClearRewardItemId = stage.FirstWaveClearRewardItemId,
                    FirstWaveClearRewardItemValue = stage.FirstWaveClearRewardItemValue,
                    SecondWaveCountValue = stage.SecondWaveCountValue,
                    SecondWaveClearRewardItemId = stage.SecondWaveClearRewardItemId,
                    SecondWaveClearRewardItemValue = stage.SecondWaveClearRewardItemValue,
                    ThirdWaveCountValue = stage.ThirdWaveCountValue,
                    ThirdWaveClearRewardItemId = stage.ThirdWaveClearRewardItemId,
                    ThirdWaveClearRewardItemValue = stage.ThirdWaveClearRewardItemValue,
                    ClearReward_Gold = stage.ClearReward_Gold,
                    ClearReward_Exp = stage.ClearReward_Exp,
                    StageImage = stage.StageImage,
                    AppearingMonsters = new List<int>(),
                    WaveArray = new List<WaveData>()
                };

                if (string.IsNullOrEmpty(stage.AppearingMonsters) != false)
                {
                    string[] monsters = stage.AppearingMonsters.Split('&');
                    foreach (string monster in monsters)
                    {
                        if (int.TryParse(monster, out int monsterNumber))
                            data.AppearingMonsters.Add(monsterNumber);
                    }
                }

                dict.Add(data.StageIndex, data);
            }
            return dict;
        }

        public bool Validate()
        {
            foreach(var data in Managers.Data.StageDic.Values)
            {
                data.WaveArray = Managers.Data.WaveDataDic.Values.Where(wave => wave.StageIndex == data.StageIndex).ToList();
            }

            return true;
        }
    }
    #endregion

    #region WaveData
    [System.Serializable]
    public class WaveDataInternal
    {
        public int StageIndex = 1;
        public int WaveIndex = 1;
        public float SpawnInterval = 0.5f;
        public int OnceSpawnCount;
        public string MonsterId;
        public string EleteId;
        public string BossId;
        public float RemainsTime;
        public Define.EWaveType WaveType;
        public float FirstMonsterSpawnRate;
        public float HpIncreaseRate;
        public float nonDropRate;
        public float SmallGemDropRate;
        public float GreenGemDropRate;
        public float BlueGemDropRate;
        public float YellowGemDropRate;
        public string EliteDropItemId;
    }

    public class WaveData
    {
        public int StageIndex = 1;
        public int WaveIndex = 1;
        public float SpawnInterval = 0.5f;
        public int OnceSpawnCount;
        public List<int> MonsterId;
        public List<int> EleteId;
        public List<int> BossId;
        public float RemainsTime;
        public Define.EWaveType WaveType;
        public float FirstMonsterSpawnRate;
        public float HpIncreaseRate;
        public float nonDropRate;
        public float SmallGemDropRate;
        public float GreenGemDropRate;
        public float BlueGemDropRate;
        public float YellowGemDropRate;
        public List<int> EliteDropItemId;
    }

    public class WaveDataLoader : ILoader<int, WaveData>
    {
        public List<WaveDataInternal> waves = new List<WaveDataInternal>();

        public Dictionary<int, WaveData> MakeDict()
        {
            Dictionary<int, WaveData> dict = new Dictionary<int, WaveData>();
            int i = 0;
            foreach (WaveDataInternal wave in waves)
            {
                var data = new WaveData()
                {
                    StageIndex = wave.StageIndex,
                    WaveIndex = wave.WaveIndex,
                    SpawnInterval = wave.SpawnInterval,
                    OnceSpawnCount = wave.OnceSpawnCount,
                    MonsterId = new List<int>(),
                    EleteId = new List<int>(),
                    BossId = new List<int>(),
                    RemainsTime = wave.RemainsTime,
                    WaveType = wave.WaveType,
                    FirstMonsterSpawnRate = wave.FirstMonsterSpawnRate,
                    HpIncreaseRate = wave.HpIncreaseRate,
                    nonDropRate = wave.nonDropRate,
                    SmallGemDropRate = wave.SmallGemDropRate,
                    GreenGemDropRate = wave.GreenGemDropRate,
                    BlueGemDropRate = wave.BlueGemDropRate,
                    YellowGemDropRate = wave.YellowGemDropRate,
                    EliteDropItemId = new List<int>()
                };

                {
                    if (string.IsNullOrEmpty(wave.MonsterId) == false)
                    {
                        string[] monsters = wave.MonsterId.Split('&');
                        foreach (string monster in monsters)
                        {
                            if (int.TryParse(monster, out int monsterNumber))
                                data.MonsterId.Add(monsterNumber);
                        }
                    }
                }

                {
                    if (string.IsNullOrEmpty(wave.EleteId) == false)
                    {
                        string[] eletes = wave.EleteId.Split('&');
                        foreach (string elete in eletes)
                        {
                            if (int.TryParse(elete, out int eleteNumber))
                                data.EleteId.Add(eleteNumber);
                        }
                    }
                }

                {
                    if (string.IsNullOrEmpty(wave.BossId) == false)
                    {
                        string[] bosses = wave.BossId.Split('&');
                        foreach (string boss in bosses)
                        {
                            if (int.TryParse(boss, out int bossNumber))
                                data.BossId.Add(bossNumber);
                        }
                    }
                }

                {
                    if (string.IsNullOrEmpty(wave.EliteDropItemId) == false)
                    {
                        string[] eleteDrops = wave.EliteDropItemId.Split('&');
                        foreach (string eleteDrop in eleteDrops)
                        {
                            if (int.TryParse(eleteDrop, out int eleteDropNumber))
                                data.EliteDropItemId.Add(eleteDropNumber);
                        }
                    }
                }

                dict.Add(i, data);
                i++;
            }
            return dict;
        }

        public bool Validate()
        {
            return true;
        }
    }
    #endregion

    #region EquipmentData
    [Serializable]
    public class EquipmentData
    {
        public string DataId;
        public Define.EGachaRarity GachaRarity;
        public Define.EEquipmentType EquipmentType;
        public Define.EEquipmentGrade EquipmentGrade;
        public string NameTextID;
        public string DescriptionTextID;
        public string SpriteName;
        public string HpRegen;
        public int MaxHpBonus;
        public int MaxHpBonusPerUpgrade;
        public int AtkDmgBonus;
        public int AtkDmgBonusPerUpgrade;
        public int MaxLevel;
        public int UncommonGradeSkill;
        public int RareGradeSkill;
        public int EpicGradeSkill;
        public int LegendaryGradeSkill;
        public int BasicSkill;
        public Define.EMergeEquipmentType MergeEquipmentType1;
        public string MergeEquipment1;
        public Define.EMergeEquipmentType MergeEquipmentType2;
        public string MergeEquipment2;
        public string MergedItemCode;
        public int LevelupMaterialID;
        public string DowngradeEquipmentCode;
        public string DowngradeMaterialCode;
        public int DowngradeMaterialCount;
    }

    [Serializable]
    public class EquipmentDataLoader : ILoader<string, EquipmentData>
    {
        public List<EquipmentData> Equipments = new List<EquipmentData>();
        public Dictionary<string, EquipmentData> MakeDict()
        {
            Dictionary<string, EquipmentData> dict = new Dictionary<string, EquipmentData>();
            foreach (EquipmentData equip in Equipments)
                dict.Add(equip.DataId, equip);
            return dict;
        }

        public bool Validate()
        {
            return true;
        }
    }
    #endregion

    #region MaterialtData
    [Serializable]
    public class MaterialData
    {
        public int DataId;
        public Define.EMaterialType MaterialType;
        public Define.EMaterialGrade MaterialGrade;
        public string NameTextID;
        public string DescriptionTextID;
        public string SpriteName;
    }

    [Serializable]
    public class MaterialDataLoader : ILoader<int, MaterialData>
    {
        public List<MaterialData> Materials = new List<MaterialData>();
        public Dictionary<int, MaterialData> MakeDict()
        {
            Dictionary<int, MaterialData> dict = new Dictionary<int, MaterialData>();
            foreach (MaterialData mat in Materials)
                dict.Add(mat.DataId, mat);
            return dict;
        }

        public bool Validate()
        {
            return true;
        }
    }
    #endregion

    #region LevelData
    [Serializable]
    public class EquipmentLevelData
    {
        public int Level;
        public int UpgradeCost;
        public int UpgradeRequiredItems;
    }

    [Serializable]
    public class EquipmentLevelDataLoader : ILoader<int, EquipmentLevelData>
    {
        public List<EquipmentLevelData> levels = new List<EquipmentLevelData>();
        public Dictionary<int, EquipmentLevelData> MakeDict()
        {
            Dictionary<int, EquipmentLevelData> dict = new Dictionary<int, EquipmentLevelData>();

            foreach (EquipmentLevelData levelData in levels)
                dict.Add(levelData.Level, levelData);
            return dict;
        }

        public bool Validate()
        {
            return true;
        }
    }
    #endregion

    #region DropItemData
    public class DropItemData
    {
        public int DataId;
        public Define.EDropItemType DropItemType;
        public string NameTextID;
        public string DescriptionTextID;
        public string SpriteName;
    }
    [Serializable]
    public class DropItemDataLoader : ILoader<int, DropItemData>
    {
        public List<DropItemData> DropItems = new List<DropItemData>();
        public Dictionary<int, DropItemData> MakeDict()
        {
            Dictionary<int, DropItemData> dict = new Dictionary<int, DropItemData>();
            foreach (DropItemData dtm in DropItems)
                dict.Add(dtm.DataId, dtm);
            return dict;
        }

        public bool Validate()
        {
            return true;
        }
    }
    #endregion

    #region GachaData
    public class GachaTableData
    {
        public Define.EGachaType Type;
        public List<GachaRateData> GachaRateTable = new List<GachaRateData>();
    }


    [Serializable]
    public class GachaDataLoader : ILoader<Define.EGachaType, GachaTableData>
    {
        public List<GachaTableData> GachaTable = new List<GachaTableData>();
        public Dictionary<Define.EGachaType, GachaTableData> MakeDict()
        {
            Dictionary<Define.EGachaType, GachaTableData> dict = new Dictionary<Define.EGachaType, GachaTableData>();
            foreach (GachaTableData gacha in GachaTable)
            {
                if(dict.ContainsKey(gacha.Type) == false)
                {
                    var value = new GachaTableData();
                    dict.Add(gacha.Type, value);
                }

                dict[gacha.Type].GachaRateTable.Add(gacha.GachaRateTable[0]);
            }
            return dict;
        }

        public bool Validate()
        {
            return true;
        }
    }
    #endregion

    #region GachaRateData
    public class GachaRateData
    {
        public string EquipmentID;
        public float GachaRate;
        public Define.EEquipmentGrade EquipGrade;

    }
    #endregion

    #region StagePackageData
    public class StagePackageData
    {
        public int StageIndex;
        public int DiaValue;
        public int GoldValue;
        public int RandomScrollValue;
        public int GoldKeyValue;
        public int ProductCostValue;
    }

    [Serializable]
    public class StagePackageDataLoader : ILoader<int, StagePackageData>
    {
        public List<StagePackageData> stagePackages = new List<StagePackageData>();
        public Dictionary<int, StagePackageData> MakeDict()
        {
            Dictionary<int, StagePackageData> dict = new Dictionary<int, StagePackageData>();
            foreach (StagePackageData stp in stagePackages)
                dict.Add(stp.StageIndex, stp);
            return dict;
        }

        public bool Validate()
        {
            return false;
        }
    }
    #endregion

    #region MissionData
    public class MissionData
    {
        public int MissionId;
        public Define.EMissionType MissionType;
        public string DescriptionTextID;
        public Define.EMissionTarget MissionTarget;
        public int MissionTargetValue;
        public int ClearRewardItmeId;
        public int RewardValue;
    }

    [Serializable]
    public class MissionDataLoader : ILoader<int, MissionData>
    {
        public List<MissionData> missions = new List<MissionData>();
        public Dictionary<int, MissionData> MakeDict()
        {
            Dictionary<int, MissionData> dict = new Dictionary<int, MissionData>();
            foreach (MissionData mis in missions)
                dict.Add(mis.MissionId, mis);
            return dict;
        }

        public bool Validate()
        {
            return true;
        }
    }
    #endregion

    #region AchievementData
    [Serializable]
    public class AchievementData
    {
        public int AchievementID;
        public string DescriptionTextID;
        public Define.EMissionTarget MissionTarget;
        public int MissionTargetValue;
        public int ClearRewardItmeId;
        public int RewardValue;
        [ExcludeField]
        public bool IsCompleted;
        [ExcludeField]
        public bool IsRewarded;
        [ExcludeField]
        public int ProgressValue;
    }

    [Serializable]
    public class AchievementDataLoader : ILoader<int, AchievementData>
    {
        public List<AchievementData> Achievements = new List<AchievementData>();
        public Dictionary<int, AchievementData> MakeDict()
        {
            Dictionary<int, AchievementData> dict = new Dictionary<int, AchievementData>();
            foreach (AchievementData ach in Achievements)
                dict.Add(ach.AchievementID, ach);
            return dict;
        }

        public bool Validate()
        {
            return true;
        }
    }
    #endregion

    #region CheckOutData
    public class CheckOutData
    {
        public int Day;
        public int RewardItemId;
        public int MissionTarRewardItemValuegetValue;
    }

    [Serializable]
    public class CheckOutDataLoader : ILoader<int, CheckOutData>
    {
        public List<CheckOutData> checkouts = new List<CheckOutData>();
        public Dictionary<int, CheckOutData> MakeDict()
        {
            Dictionary<int, CheckOutData> dict = new Dictionary<int, CheckOutData>();
            foreach (CheckOutData chk in checkouts)
                dict.Add(chk.Day, chk);
            return dict;
        }

        public bool Validate()
        {
            return true;
        }
    }
    #endregion

    #region OfflineRewardData
    public class OfflineRewardData
    {
        public int StageIndex;
        public int Reward_Gold;
        public int Reward_Exp;
        public int FastReward_Scroll;
        public int FastReward_Box;
    }

    [Serializable]
    public class OfflineRewardDataLoader : ILoader<int, OfflineRewardData>
    {
        public List<OfflineRewardData> offlines = new List<OfflineRewardData>();
        public Dictionary<int, OfflineRewardData> MakeDict()
        {
            Dictionary<int, OfflineRewardData> dict = new Dictionary<int, OfflineRewardData>();
            foreach (OfflineRewardData ofr in offlines)
                dict.Add(ofr.StageIndex, ofr);
            return dict;
        }

        public bool Validate()
        {
            return true;
        }
    }
    #endregion

    #region BattlePassData
    public class BattlePassData
    {
        public int PassLevel;
        public int FreeRewardItemId;
        public int FreeRewardItemValue;
        public int RareRewardItemId;
        public int RareRewardItemValue;
        public int EpicRewardItemId;
        public int EpicRewardItemValue;
    }

    [Serializable]
    public class BattlePassDataLoader : ILoader<int, BattlePassData>
    {
        public List<BattlePassData> battles = new List<BattlePassData>();
        public Dictionary<int, BattlePassData> MakeDict()
        {
            Dictionary<int, BattlePassData> dict = new Dictionary<int, BattlePassData>();
            foreach (BattlePassData bts in battles)
                dict.Add(bts.PassLevel, bts);
            return dict;
        }

        public bool Validate()
        {
            return true;
        }
    }
    #endregion

    #region DailyShopData
    public class DailyShopData
    {
        public int Index;
        public int BuyItemId;
        public int CostItemId;
        public int CostValue;
        public float DiscountValue;
    }

    [Serializable]
    public class DailyShopDataLoader : ILoader<int, DailyShopData>
    {
        public List<DailyShopData> dailys = new List<DailyShopData>();
        public Dictionary<int, DailyShopData> MakeDict()
        {
            Dictionary<int, DailyShopData> dict = new Dictionary<int, DailyShopData>();
            foreach (DailyShopData dai in dailys)
                dict.Add(dai.Index, dai);
            return dict;
        }

        public bool Validate()
        {
            return true;
        }
    }
    #endregion

    #region AccountPassData
    public class AccountPassData
    {
        public int AccountLevel;
        public int FreeRewardItemId;
        public int FreeRewardItemValue;
        public int RareRewardItemId;
        public int RareRewardItemValue;
        public int EpicRewardItemId;
        public int EpicRewardItemValue;
    }

    [Serializable]
    public class AccountPassDataLoader : ILoader<int, AccountPassData>
    {
        public List<AccountPassData> accounts = new List<AccountPassData>();
        public Dictionary<int, AccountPassData> MakeDict()
        {
            Dictionary<int, AccountPassData> dict = new Dictionary<int, AccountPassData>();
            foreach (AccountPassData aps in accounts)
                dict.Add(aps.AccountLevel, aps);
            return dict;
        }

        public bool Validate()
        {
            return true;
        }
    }
    #endregion
}