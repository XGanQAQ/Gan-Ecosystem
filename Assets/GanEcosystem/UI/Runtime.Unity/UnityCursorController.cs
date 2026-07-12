using System.Collections.Generic;
using UnityEngine;
using GanEcosystem.UI.Core;

namespace GanEcosystem.UI.UnityRuntime
{
    public class UnityCursorController : ICursorController
    {
        public HashSet<UILayer> UnLockedCursorLayers;

        public bool IsNeedAutoLockCursor = true;
        private IUIManager _uiManager;

        public UnityCursorController(IUIManager uiManager, HashSet<UILayer> unLockedCursorLayers, bool isNeedAutoLockCursor = true)
        {
            UnLockedCursorLayers = unLockedCursorLayers;
            IsNeedAutoLockCursor = isNeedAutoLockCursor;
            _uiManager = uiManager;
        }

        public bool IsShouldLockCursor()
        {

            if (IsNeedAutoLockCursor == false)
                return false;
            foreach (var layer in UnLockedCursorLayers)
            {
                if (_uiManager.IsActive(layer))
                    return false;
            }
            return true;
        }

        public void UpdateCursorState()
        {
            bool shouldLockCursor = IsShouldLockCursor();
            Cursor.lockState = shouldLockCursor ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !shouldLockCursor;
        }
    }
}