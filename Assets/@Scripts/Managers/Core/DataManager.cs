using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public interface IValidate
{
    bool Validate();
}

public interface ILoader<Key, Value> : IValidate
{
    Dictionary<Key, Value> MakeDict();
}

public class DataManager
{
    private HashSet<IValidate> _loaders = new HashSet<IValidate>();

    public Dictionary<int, Data.AchievementData> AchievementDataDic { get; private set; } = new Dictionary<int, Data.AchievementData>();
    public Dictionary<int, Data.MaterialData> MaterialDic { get; private set; } = new Dictionary<int, Data.MaterialData>();
    public Dictionary<int, Data.SupportSkillData> SupportSkillDic { get; private set; } = new Dictionary<int, Data.SupportSkillData>();
    public Dictionary<int, Data.StageData> StageDic { get; private set; } = new Dictionary<int, Data.StageData>();
    public Dictionary<int, Data.SkillData> SkillDic { get; private set; } = new Dictionary<int, Data.SkillData>();
    public Dictionary<int, Data.CreatureData> CreatureDic { get; private set; } = new Dictionary<int, Data.CreatureData>();
    public Dictionary<int, Data.LevelData> LevelDataDic { get; private set; } = new Dictionary<int, Data.LevelData>();
    public Dictionary<string, Data.EquipmentData> EquipDataDic { get; private set; } = new Dictionary<string, Data.EquipmentData>();
    public Dictionary<int, Data.EquipmentLevelData> EquipLevelDataDic { get; private set; } = new Dictionary<int, Data.EquipmentLevelData>();
    public Dictionary<Define.EGachaType, Data.GachaTableData> GachaTableDataDic { get; private set; } = new Dictionary<Define.EGachaType, Data.GachaTableData>();
    public Dictionary<int, Data.MissionData> MissionDataDic { get; private set; } = new Dictionary<int, Data.MissionData>();
    
    public Dictionary<int, Data.DropItemData> DropItemDataDic { get; private set; } = new Dictionary<int, Data.DropItemData>();
    public Dictionary<int, Data.CheckOutData> CheckOutDataDic { get; private set; } = new Dictionary<int, Data.CheckOutData>();
    public Dictionary<int, Data.OfflineRewardData> OfflineRewardDataDic { get; private set; } = new Dictionary<int, Data.OfflineRewardData>();

    
    public Dictionary<int, Data.WaveData> WaveDataDic { get; private set; } = new Dictionary<int, Data.WaveData>();

    public void Init()
    {
        MaterialDic = LoadJson<Data.MaterialDataLoader, int, Data.MaterialData>("MaterialData").MakeDict();
        SupportSkillDic = LoadJson<Data.SupportSkillDataLoader, int, Data.SupportSkillData>("SupportSkillData").MakeDict();
        StageDic = LoadJson<Data.StageDataLoader, int, Data.StageData>("StageData").MakeDict();
        CreatureDic = LoadJson<Data.CreatureDataLoader, int, Data.CreatureData>("CreatureData").MakeDict();
        SkillDic = LoadJson<Data.SkillDataLoader, int, Data.SkillData>("SkillData").MakeDict();
        LevelDataDic = LoadJson<Data.LevelDataLoader, int, Data.LevelData>("LevelData").MakeDict();
        EquipDataDic = LoadJson<Data.EquipmentDataLoader, string, Data.EquipmentData>("EquipmentData").MakeDict();
        EquipLevelDataDic = LoadJson<Data.EquipmentLevelDataLoader, int, Data.EquipmentLevelData>("EquipmentLevelData").MakeDict();
        GachaTableDataDic = LoadJson<Data.GachaDataLoader, Define.EGachaType, Data.GachaTableData>("GachaTableData").MakeDict();
        MissionDataDic = LoadJson<Data.MissionDataLoader, int, Data.MissionData>("MissionData").MakeDict();
        AchievementDataDic = LoadJson<Data.AchievementDataLoader, int, Data.AchievementData>("AchievementData").MakeDict();
        DropItemDataDic = LoadJson<Data.DropItemDataLoader, int, Data.DropItemData>("DropItemData").MakeDict();
        CheckOutDataDic = LoadJson<Data.CheckOutDataLoader, int, Data.CheckOutData>("CheckOutData").MakeDict();
        OfflineRewardDataDic = LoadJson<Data.OfflineRewardDataLoader, int, Data.OfflineRewardData>("OfflineRewardData").MakeDict();

        WaveDataDic = LoadJson<Data.WaveDataLoader, int, Data.WaveData>("WaveData").MakeDict();

        Validate();

        // ILHAK
        Debug.Log("Data Init Sucess");
    }

    private Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
		TextAsset textAsset = Managers.Resource.Load<TextAsset>($"{path}");
        Loader loader = JsonConvert.DeserializeObject<Loader>(textAsset.text);
        _loaders.Add(loader);
        return loader;
	}

    private bool Validate()
    {
        bool success = true;

        foreach (var loader in _loaders)
        {
            if (loader.Validate() == false)
                success = false;
        }

        _loaders.Clear();

        return success;
    }

}
