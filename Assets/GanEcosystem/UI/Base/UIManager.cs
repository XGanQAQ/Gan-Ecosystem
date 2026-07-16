using System;
using System.Collections.Generic;
using System.Reflection;

namespace GanEcosystem.UI.Core
{
    public abstract class UIManager : IUIManager
    {
        private readonly Dictionary<UILayer, Dictionary<string, IViewer>> _layerViewers = new();
        private static readonly Dictionary<string, Type> _viewerTypeCache = new();

        protected ICursorController _cursorController;
        protected IUIResLoader _uiResLoader;
        protected IUIEventBus _uiEventBus;

        protected UIManager(IUIResLoader uiResLoader, IUIEventBus uiEventBus, ICursorController uiCursorController)
        {
            _uiResLoader = uiResLoader;
            _uiEventBus = uiEventBus;
            _cursorController = uiCursorController;
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
            if (viewer != null)
            {
                if (viewer.IsMutuallyExclusive == true)
                    CloseOtherLayerViewers(viewer.Layer, viewerName);

                viewer.Open(data);
                _cursorController?.UpdateCursorState();
                return viewer;
            }

            if (viewer.IsMutuallyExclusive == true)
                CloseOtherLayerViewers(viewer.Layer, viewerName);

            viewer = CreateViewer(viewerName, viewer.AssetKey, viewer.Layer);
            if (viewer == null)
                return null;

            if (!_layerViewers.TryGetValue(viewer.Layer, out var viewers))
            {
                viewers = new Dictionary<string, IViewer>();
                _layerViewers[viewer.Layer] = viewers;
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