using System;
using Spine;
using Spine.Unity;
using UnityEngine;
using UnityEngine.Rendering;
using Event = Spine.Event;

public class BaseController : MonoBehaviour
{
    public Define.EObjectType ObjectType { get; protected set; }
}

