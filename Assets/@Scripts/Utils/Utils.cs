using System;
using System.Net;
using UnityEngine;
using static Define;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public static class Utils
{
    public static T GetOrAddComponent<T>(GameObject go) where T : Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();
        return component;
    }

    public static T FindAncestor<T>(GameObject go) where T : Object
    {
        Transform t = go.transform;
        while (t != null)
        {
            T component = t.GetComponent<T>();
            if (component != null)
                return component;
            t = t.parent;
        }
        return null; 
    }

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null)
            return null;

        return transform.gameObject;
    }

    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : Object
    {
        if (go == null)
            return null;

        if (recursive == false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }

    public static Vector2 RandomPointInAnnulus(Vector2 origin, float minRadius = 6, float maxRadius = 12)
    {
        float randomDist = Random.Range(minRadius, maxRadius);

        Vector2 randomDir = new Vector2(Random.Range(-100, 100), Random.Range(-100, 100)).normalized;
        //Debug.Log(randomDir);
        var point = origin + randomDir * randomDist;
        return point;
    }

    public static Vector2 GenerateMonsterSpawnPosition(Vector2 characterPosition, float minSpawnDistance = 20f, float maxSpawnDistance = 25f)
    {
        float angle = Random.Range(0, 360) * Mathf.Deg2Rad;
        float distance = Random.Range(minSpawnDistance, maxSpawnDistance);

        float xDist = Mathf.Cos(angle) * distance;
        float yDist = Mathf.Sin(angle) * distance;

        // 원 모양으로 생성
        Vector2 spawnPosition = characterPosition + new Vector2(xDist, yDist);

        // 맵 경계를 벗어나는 경우 타원 모양으로 생성
        float size = Managers.Game.CurrentMap.MapSize.x * 0.5f;
        if (Mathf.Abs(spawnPosition.x) > size || Mathf.Abs(spawnPosition.y) > size)
        {
            float ellipseFactorX = Mathf.Lerp(1f, 0.5f, Mathf.Abs(characterPosition.x) / size);
            float ellipseFactorY = Mathf.Lerp(1f, 0.5f, Mathf.Abs(characterPosition.y) / size);

            xDist *= ellipseFactorX;
            yDist *= ellipseFactorY;

            spawnPosition = Vector2.zero + new Vector2(xDist, yDist);

            // 생성 위치를 맵 사이즈 범위 내로 조정
            spawnPosition.x = Mathf.Clamp(spawnPosition.x, -size, size);
            spawnPosition.y = Mathf.Clamp(spawnPosition.y, -size, size);
        }

        return spawnPosition;
    }

    public static Color HexToColor(string color)
    {
        Color parsedColor;

        if (color.Contains("#") == false)
            ColorUtility.TryParseHtmlString("#" + color, out parsedColor);
        else
            ColorUtility.TryParseHtmlString(color, out parsedColor);

        return parsedColor;
    }

    // Animator 컴포넌트 내에 특정 애니메이션 클립이 존재하는지 확인하는 함수
    public static bool HasAnimationClip(Animator animator, string clipName)
    {
        if (animator.runtimeAnimatorController == null)
        {
            return false;
        }

        foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
        {
            if (clip.name == clipName)
            {
                return true;
            }
        }

        return false;
    }

    public static T ParseEnum<T>(string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }
    
    public static IPAddress GetIpv4Address(string hostAddress)
    {
        IPAddress[] ipAddr = Dns.GetHostAddresses(hostAddress);

        if (ipAddr.Length == 0)
        {
            Debug.LogError("AuthServer DNS Failed");
            return null;
        }

        foreach (IPAddress ip in ipAddr)
        {
            if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            {
                return ip;
            }
        }

        Debug.LogError("AuthServer IPv4 Failed");
        return null;
    }

    public static Define.ESkillType GetSkillTypeFromInt(int value)
    {
        foreach (Define.ESkillType skillType in Enum.GetValues(typeof(Define.ESkillType)))
        {
            int minValue = (int)skillType;
            int maxValue = minValue + 5; // 100501~ 100506 사이 값이면 100501값 리턴

            if (value >= minValue && value <= maxValue)
            {
                return skillType;
            }
        }

        Debug.LogError($" Faild add skill : {value}");
        return Define.ESkillType.None;
    }
}