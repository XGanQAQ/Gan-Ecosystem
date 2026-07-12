using System.Linq;

namespace GanEcosystem.UI.Core
{
    public static class IUIManagerExtension
    {
        public static bool IsActive<T>(this IUIManager uiManager) where T : class, IViewer
        {
            return uiManager.IsActive(typeof(T).Name);
        }

        public static bool IsActive(this IUIManager uiManager, UILayer layer)
        {
            return uiManager.GetLayerViewers(layer)
                .Any(kv => kv.Value != null && kv.Value.IsActive);
        }

        public static T OpenUI<T>(this IUIManager uiManager) where T : class, IViewer
        {
            return uiManager.OpenUI(typeof(T).Name) as T;
        }

        public static void CloseUI<T>(this IUIManager uiManager) where T : class, IViewer
        {
            string uiName = typeof(T).Name;
            uiManager.CloseUI(uiName);
        }

        public static void SwitchUI<T>(this IUIManager uiManager) where T : class, IViewer
        {
            if (uiManager.IsActive<T>())
                uiManager.CloseUI<T>();
            else
                uiManager.OpenUI<T>();
        }

        public static void CloseLayerUI(this IUIManager uiManager, UILayer layer)
        {
            var targets = uiManager.GetLayerViewers(layer)
                .Where(kv => kv.Value != null && kv.Value.IsActive)
                .Select(kv => kv.Key)
                .ToArray();

            foreach (var viewerName in targets)
            {
                uiManager.CloseUI(viewerName);
            }
        }
    }
}