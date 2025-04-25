using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static Define;

public class ObjectManager
{
    #region Roots
    public Transform GetRootTransform(string name)
    {
        GameObject root = GameObject.Find(name);
        if (root == null)
            root = new GameObject { name = name };

        return root.transform;
    }

    public Transform HeroRoot { get { return GetRootTransform("@Heroes"); } }
    public Transform MonsterRoot { get { return GetRootTransform("@Monsters"); } }
    public Transform ProjectileRoot { get { return GetRootTransform("@Projectiles"); } }
    public Transform EnvRoot { get { return GetRootTransform("@Envs"); } }
    public Transform EffectRoot { get { return GetRootTransform("@Effects"); } }
    public Transform NpcRoot { get { return GetRootTransform("@Npc"); } }
    public Transform ItemHolderRoot { get { return GetRootTransform("@ItemHolders"); } }
    #endregion

    public PlayerController Player { get; private set; }
    public HashSet<MonsterController> Monsters { get; } = new HashSet<MonsterController>();
    public HashSet<GemController> Gems { get; } = new HashSet<GemController>();
    public HashSet<SoulController> Souls { get; } = new HashSet<SoulController>();
    //public HashSet<DropItemController> DropItems { get; } = new HashSet<DropItemController>();
    public HashSet<ProjectileController> Projectiles { get; } = new HashSet<ProjectileController>();

    public ObjectManager()
    {
    }

    public void Clear()
    {
        Monsters.Clear();
        //Gems.Clear();
        //Souls.Clear();
        Projectiles.Clear();
    }

    public void LoadMap(string mapName)
    {
        GameObject objMap = Managers.Resource.Instantiate(mapName);
        objMap.transform.position = Vector3.zero;
        objMap.name = "@Map";

        objMap.GetComponent<Map>().Init();
    }

    public GameObject SpawnGameObject(Vector3 position, string prefabName)
    {
        GameObject go = Managers.Resource.Instantiate(prefabName, pooling: true);
        go.transform.position = position;
        
        return go;
    }

    public void DespawnGameObject<T>(T obj) where T : BaseController
    {

    }

    public T Spawn<T>(Vector3 position, int templateID = 0, string prefabName = "") where T : BaseController
    {
        System.Type type = typeof(T);

        if (type == typeof(PlayerController))
        {
            //GameObject go = Managers.Resource.Instantiate(Managers.Data.CreatureDic[templateID].PrefabLabel);
            //go.transform.position = position;
            //PlayerController pc = go.GetOrAddComponent<PlayerController>();
            //pc.SetInfo(templateID);
            //Player = pc;
            //Managers.Game.Player = pc;

            //return pc as T;
        }
        //else if (type == typeof(MonsterController))
        //{
        //    Data.CreatureData cd = Managers.Data.CreatureDic[templateID];
        //    GameObject go = Managers.Resource.Instantiate($"{cd.PrefabLabel}", pooling: true);
        //    MonsterController mc = go.GetOrAddComponent<MonsterController>();
        //    go.transform.position = position;
        //    mc.SetInfo(templateID);
        //    go.name = cd.PrefabLabel;
        //    Monsters.Add(mc);

        //    return mc as T;
        //}
        //else if (type == typeof(EliteController))
        //{
        //    Data.CreatureData cd = Managers.Data.CreatureDic[templateID];
        //    GameObject go = Managers.Resource.Instantiate($"{cd.PrefabLabel}", pooling: true);
        //    EliteController mc = go.GetOrAddComponent<EliteController>();
        //    go.transform.position = position;
        //    mc.SetInfo(templateID);
        //    go.name = cd.PrefabLabel;
        //    Monsters.Add(mc);

        //    return mc as T;
        //}

        //else if (type == typeof(BossController))
        //{
        //    Data.CreatureData cd = Managers.Data.CreatureDic[templateID];

        //    GameObject go = Managers.Resource.Instantiate($"{cd.PrefabLabel}");
        //    BossController mc = go.GetOrAddComponent<BossController>();
        //    mc.enabled = true; // Disabled 상태로 Attatch됨
        //    go.transform.position = position;
        //    mc.SetInfo(templateID);
        //    go.name = cd.PrefabLabel;
        //    Monsters.Add(mc);

        //    return mc as T;
        //}
        //else if (type == typeof(GemController))
        //{
        //    GameObject go = Managers.Resource.Instantiate("ExpGem", pooling: true);
        //    GemController gc = go.GetOrAddComponent<GemController>();
        //    go.transform.position = position;
        //    Gems.Add(gc);
        //    Managers.Game.CurrentMap.Grid.Add(gc);

        //    return gc as T;
        //}
        //else if (type == typeof(SoulController))
        //{
        //    GameObject go = Managers.Resource.Instantiate("Soul", pooling: true);
        //    SoulController gc = go.GetOrAddComponent<SoulController>();
        //    go.transform.position = position;
        //    Souls.Add(gc);
        //    Managers.Game.CurrentMap.Grid.Add(gc);

        //    return gc as T;
        //}
        //else if (type == typeof(PotionController))
        //{
        //    GameObject go = Managers.Resource.Instantiate("Potion", pooling: true);
        //    PotionController pc = go.GetOrAddComponent<PotionController>();
        //    go.transform.position = position;
        //    DropItems.Add(pc);
        //    Managers.Game.CurrentMap.Grid.Add(pc);

        //    return pc as T;
        //}
        //else if (type == typeof(BombController))
        //{
        //    GameObject go = Managers.Resource.Instantiate("Bomb", pooling: true);
        //    BombController bc = go.GetOrAddComponent<BombController>();
        //    go.transform.position = position;
        //    DropItems.Add(bc);
        //    Managers.Game.CurrentMap.Grid.Add(bc);

        //    return bc as T;
        //}
        //else if (type == typeof(MagnetController))
        //{
        //    GameObject go = Managers.Resource.Instantiate("Magnet", pooling: true);
        //    MagnetController mc = go.GetOrAddComponent<MagnetController>();
        //    go.transform.position = position;
        //    DropItems.Add(mc);
        //    Managers.Game.CurrentMap.Grid.Add(mc);

        //    return mc as T;
        //}
        //else if (type == typeof(EliteBoxController))
        //{
        //    GameObject go = Managers.Resource.Instantiate("DropBox", pooling: true);
        //    EliteBoxController bc = go.GetOrAddComponent<EliteBoxController>();
        //    go.transform.position = position;
        //    DropItems.Add(bc);
        //    Managers.Game.CurrentMap.Grid.Add(bc);
        //    Managers.Sound.Play(Sound.Effect, "Drop_Box");
        //    return bc as T;
        //}
        else if (type == typeof(ProjectileController))
        {
            GameObject go = Managers.Resource.Instantiate(prefabName, pooling: true);
            ProjectileController pc = go.GetOrAddComponent<ProjectileController>();
            go.transform.position = position;
            Projectiles.Add(pc);

            return pc as T;
        }

        return null;
    }

    public void Despawn<T>(T obj) where T : BaseController
    {

    }

    public List<MonsterController> GetNearestMonsters(int count = 1, int distanceThreshold = 0)
    {
        List<MonsterController> monsterList = Monsters.OrderBy(monster => (Player.CenterPosition - monster.CenterPosition).sqrMagnitude).ToList();

        if (distanceThreshold > 0)
            monsterList = monsterList.Where(monster => (Player.CenterPosition - monster.CenterPosition).magnitude > distanceThreshold).ToList();

        int min = Mathf.Min(count, monsterList.Count);

        List<MonsterController> nearestMonsters = monsterList.Take(min).ToList();

        if (nearestMonsters.Count == 0) return null;

        // 요소 개수가 count와 다른 경우 마지막 요소 반복해서 추가
        while (nearestMonsters.Count < count)
        {
            nearestMonsters.Add(nearestMonsters.Last());
        }

        return nearestMonsters;
    }

    // TODO ILHAK
    public List<MonsterController> GetMonsterWithinCamera(int count = 1)
    {
        return null;
    }

    public void KillAllMonsters()
    {
        UI_GameScene scene = Managers.UI.SceneUI as UI_GameScene;

        if (scene != null)
            scene.DoWhiteFlash();
        foreach (MonsterController monster in Monsters.ToList())
        {
            if (monster.ObjectType == Define.EObjectType.Monster)
                monster.OnDead();
        }
        DespawnAllMonsterProjectiles();
    }

    public void DespawnAllMonsterProjectiles()
    {
        foreach (ProjectileController proj in Projectiles.ToList())
        {
            if (proj.Skill.SkillType == Define.ESkillType.MonsterSkill_01)
                Despawn(proj);
        }
    }

    public void CollectAllItems()
    {
        foreach (GemController gem in Gems.ToList())
        {
            gem.GetItem();
        }

        foreach (SoulController soul in Souls.ToList())
        {
            soul.GetItem();
        }
    }

}