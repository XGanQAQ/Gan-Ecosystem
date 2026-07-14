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
        var cursorController = new UnityCursorController(
            new HashSet<UILayer> { UILayer.Normal, UILayer.Popup },
            true
        );
        _uiManager = new UnityUIManager(
            new UnitySimpleUIResLoader(),
            new UnitySimpleUIEventBus(),
            cursorController
        );
        cursorController.SetUIManager(_uiManager);
    }
}