using System;
using System.Collections.Generic;
using System.Reflection;

namespace GanEcosystem.UI.Core
{
    public abstract class UIManager : IUIManager
    {
        private readonly Dictionary<UILayer, Dictionary<string, IViewer>> _layerViewers = new();
        private static readonly Dictionary<string, Type> _viewerTypeCache = new();
        private static readonly Dictionary<string, ViewerAttribute> _viewerAttributeCache = new();

        protected ICursorController _cursorController;
        protected IUIResLoader _uiResLoader;
        protected IUIEventBus _uiEventBus;

        protected UIManager(IUIResLoader uiResLoader, IUIEventBus uiEventBus)
        {
            _uiResLoader = uiResLoader;
            _uiEventBus = uiEventBus;
            InitializeLayerViewers();
        }

        public bool IsActive(string viewerName)
        {
            var viewer = GetViewer(viewerName);
            return viewer != null && viewer.IsActive;
        }

        public IViewer OpenUI(string viewerName, object data = null)
        {
            IViewer viewer = GetViewer(viewerName);
            var attr = GetViewerAttribute(viewerName);
            if (viewer != null)
            {
                if (attr?.IsMutuallyExclusive == true)
                    CloseOtherLayerViewers(viewer.Layer, viewerName);

                viewer.Open(data);
                _cursorController?.UpdateCursorState();
                return viewer;
            }

            string assetKey = attr != null && !string.IsNullOrEmpty(attr.AssetKey)
                ? attr.AssetKey
                : "UI/" + viewerName;
            UILayer layer = attr?.Layer ?? UILayer.Normal;

            if (attr?.IsMutuallyExclusive == true)
                CloseOtherLayerViewers(layer, viewerName);

            viewer = CreateViewer(viewerName, assetKey, layer);
            if (viewer == null)
                return null;

            if (!_layerViewers.TryGetValue(layer, out var viewers))
            {
                viewers = new Dictionary<string, IViewer>();
                _layerViewers[layer] = viewers;
            }

            viewers.TryAdd(viewerName, viewer);

            viewer.Open(data);
            _uiEventBus.Publish(new OpenUIEvent(viewer));

            _cursorController?.UpdateCursorState();
            return viewer;
        }

        public void CloseUI(string viewerName)
        {
            IViewer viewer = GetViewer(viewerName);
            if (viewer != null)
            {
                viewer.Close();
                _uiEventBus.Publish(new CloseUIEvent(viewer));
            }

            _cursorController?.UpdateCursorState();
        }

        public IEnumerable<KeyValuePair<string, IViewer>> GetLayerViewers(UILayer layer)
        {
            if (!_layerViewers.TryGetValue(layer, out var viewers) || viewers == null)
                return Array.Empty<KeyValuePair<string, IViewer>>();

            return viewers;
        }

        protected bool TryGetLayerViewers(UILayer layer, out Dictionary<string, IViewer> viewers)
        {
            return _layerViewers.TryGetValue(layer, out viewers);
        }

        protected void SetCursorController(ICursorController cursorController)
        {
            _cursorController = cursorController;
        }

        protected abstract IViewer CreateViewer(string viewerName, string assetKey, UILayer layer);

        private void InitializeLayerViewers()
        {
            _layerViewers.Clear();
            foreach (UILayer layer in Enum.GetValues(typeof(UILayer)))
            {
                _layerViewers[layer] = new Dictionary<string, IViewer>();
            }
        }

        private IViewer GetViewer(string viewerName)
        {
            foreach (var layer in _layerViewers.Values)
            {
                if (layer.TryGetValue(viewerName, out IViewer existingUI))
                    return existingUI;
            }

            return null;
        }

        private static Type ResolveViewerType(string uiName)
        {
            if (_viewerTypeCache.TryGetValue(uiName, out var cached))
                return cached;

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.Name == uiName && typeof(IViewer).IsAssignableFrom(type) && !type.IsAbstract)
                    {
                        _viewerTypeCache[uiName] = type;
                        return type;
                    }
                }
            }

            _viewerTypeCache[uiName] = null;
            return null;
        }

        private static ViewerAttribute GetViewerAttribute(string viewerName)
        {
            if (_viewerAttributeCache.TryGetValue(viewerName, out var cached))
                return cached;

            var type = ResolveViewerType(viewerName);
            var attr = type?.GetCustomAttribute<ViewerAttribute>();
            _viewerAttributeCache[viewerName] = attr;
            return attr;
        }

        private void CloseOtherLayerViewers(UILayer layer, string viewerName)
        {
            if (!_layerViewers.TryGetValue(layer, out var viewers) || viewers == null)
                return;

            var targetNames = new List<string>();
            foreach (var pair in viewers)
            {
                if (pair.Key == viewerName || pair.Value == null || !pair.Value.IsActive)
                    continue;

                targetNames.Add(pair.Key);
            }

            foreach (var targetName in targetNames)
            {
                CloseUI(targetName);
            }
        }
    }
}