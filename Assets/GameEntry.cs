using UnityEngine;
using GanEcosystem.UI.Core;
using GanEcosystem.UI.UnityRuntime;
using System.Collections.Generic;

public static class GameEntry
{
    private static UnityUIManager _uiManager;
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void BeforeSceneLoad()
    {
        _uiManager = new UnityUIManager(
            new UnitySimpleUIResLoader(),
            new UnitySimpleUIEventBus(),
            new HashSet<UILayer>() { UILayer.Popup, UILayer.Top }
            );
    }
}