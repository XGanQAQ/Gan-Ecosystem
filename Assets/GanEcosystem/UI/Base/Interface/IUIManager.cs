using System.Collections.Generic;

namespace GanEcosystem.UI.Core
{
    public interface IUIManager
    {
        bool IsActive(string viewerName);
        IViewer OpenUI(string viewerName);
        void CloseUI(string viewerName);
        IEnumerable<KeyValuePair<string, IViewer>> GetLayerViewers(UILayer layer);
    }
}
