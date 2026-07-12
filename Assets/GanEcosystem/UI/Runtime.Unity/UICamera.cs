using UnityEngine;

namespace GanEcosystem.UI.UnityRuntime
{
    [RequireComponent(typeof(Camera))]
    public class UICamera : MonoBehaviour
    {
        public static UICamera Instance { get; private set; }
        private Camera uiCamera;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);

            uiCamera = GetComponent<Camera>();
            uiCamera.clearFlags = CameraClearFlags.Depth;
            int uiLayer = LayerMask.NameToLayer("UI");
            if (uiLayer >= 0)
                uiCamera.cullingMask = 1 << uiLayer;
            else
                Debug.LogWarning("[UICamera]: UI layer not found, please add a 'UI' layer in the project settings.");
            uiCamera.orthographic = false;
            uiCamera.depth = 100;
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }

        public Camera GetCamera()
        {
            return uiCamera;
        }
    }
}
