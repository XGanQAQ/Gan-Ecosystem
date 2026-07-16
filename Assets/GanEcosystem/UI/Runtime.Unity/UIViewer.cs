using System;
using GanEcosystem.UI.Core;
using UnityEngine;

namespace GanEcosystem.UI.UnityRuntime
{
    public abstract class UIViewer : MonoBehaviour, IViewer
    {
        [SerializeField] private string viewerName;
        public virtual string UIName => string.IsNullOrEmpty(viewerName) ? GetType().Name : viewerName;
        public virtual UILayer Layer { get; set; } = UILayer.Normal;
        public virtual string AssetKey => $"UI/{UIName}";
        public virtual bool IsMutuallyExclusive => true;
        public virtual bool CloseableByEscape { get; set; } = true;
        public bool IsActive => gameObject.activeSelf;

        public event Action OnOpen;
        public event Action OnClose;

        public virtual void Open(object data = null)
        {
            gameObject.SetActive(true);
            OnOpen?.Invoke();
        }

        public virtual void Close()
        {
            gameObject.SetActive(false);
            OnClose?.Invoke();
        }

        public virtual void Init()
        {
        }
    }
}
