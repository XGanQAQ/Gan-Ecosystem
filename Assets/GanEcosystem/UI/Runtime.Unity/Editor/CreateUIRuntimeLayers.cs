#if UNITY_EDITOR
using GanEcosystem.UI.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GanEcosystem.UI.UnityRuntime
{
    public static class CreateUIRuntimeLayers
    {
        private const string MenuRoot = "GameObject/GanFramework/UI/";
        private const int MenuPriority = 10;
        private const string UIRootName = "[UIRoot]";

        [MenuItem(MenuRoot + "Create Mock UI Root", false, MenuPriority)]
        public static void CreateMockUIRoot()
        {
            var parent = Selection.activeTransform;
            var uiRoot = new GameObject(UIRootName);
            Undo.RegisterCreatedObjectUndo(uiRoot, "Create Mock UI Root");
            GameObjectUtility.SetParentAndAlign(uiRoot, parent != null ? parent.gameObject : null);

            foreach (UILayer layer in System.Enum.GetValues(typeof(UILayer)))
            {
                CreateLayerCanvas(layer, uiRoot.transform);
            }

            EnsureEventSystem(uiRoot.transform);

            Selection.activeGameObject = uiRoot;
        }

        [MenuItem(MenuRoot + "Create Mock UI Root", true)]
        public static bool ValidateCreateMockUIRoot()
        {
            return !EditorApplication.isPlaying;
        }

        [MenuItem(MenuRoot + "Create Layer Under Selection", false, MenuPriority + 1)]
        public static void CreateLayerUnderSelection()
        {
            var parent = Selection.activeTransform;
            if (parent == null)
            {
                EditorUtility.DisplayDialog("Create UI Layer", "请先在层级面板中选择一个父节点。", "OK");
                return;
            }

            GenericMenu menu = new GenericMenu();
            foreach (UILayer layer in System.Enum.GetValues(typeof(UILayer)))
            {
                var capturedLayer = layer;
                menu.AddItem(new GUIContent(layer.ToString()), false, () =>
                {
                    CreateLayerCanvas(capturedLayer, parent);
                    Selection.activeGameObject = parent.Find(capturedLayer + "_Canvas")?.gameObject;
                });
            }

            menu.ShowAsContext();
        }

        [MenuItem(MenuRoot + "Create Layer Under Selection", true)]
        public static bool ValidateCreateLayerUnderSelection()
        {
            return !EditorApplication.isPlaying && Selection.activeTransform != null;
        }

        private static GameObject CreateLayerCanvas(UILayer layer, Transform parent)
        {
            string objectName = layer + "_Canvas";
            Transform existingChild = parent.Find(objectName);
            if (existingChild != null)
            {
                Selection.activeGameObject = existingChild.gameObject;
                return existingChild.gameObject;
            }

            GameObject layerRootObj = new GameObject(objectName);
            Undo.RegisterCreatedObjectUndo(layerRootObj, "Create UI Layer Canvas");
            GameObjectUtility.SetParentAndAlign(layerRootObj, parent.gameObject);

            var canvas = Undo.AddComponent<Canvas>(layerRootObj);
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = (int)layer * 100;

            Undo.AddComponent<CanvasScaler>(layerRootObj);
            Undo.AddComponent<CanvasRenderer>(layerRootObj);
            Undo.AddComponent<GraphicRaycaster>(layerRootObj);

            return layerRootObj;
        }

        private static void EnsureEventSystem(Transform parent)
        {
            if (Object.FindObjectOfType<EventSystem>() != null)
                return;

            var eventSystemObj = new GameObject("[EventSystem]");
            Undo.RegisterCreatedObjectUndo(eventSystemObj, "Create EventSystem");
            GameObjectUtility.SetParentAndAlign(eventSystemObj, parent.gameObject);
            Undo.AddComponent<EventSystem>(eventSystemObj);
            Undo.AddComponent<StandaloneInputModule>(eventSystemObj);
        }
    }
}
#endif