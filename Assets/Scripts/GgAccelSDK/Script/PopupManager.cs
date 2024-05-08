using System.Collections.Generic;
using GgAccel;
using UnityEngine;

public class PopupManager : MonoSingleton<PopupManager>
{
    public const string FolderName = "Popups";
    public Transform CacheTransform { get; private set; }

    protected override void Start()
    {
        base.Start();
        CacheTransform = transform;
    }
}