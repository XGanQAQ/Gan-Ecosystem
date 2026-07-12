using System;

namespace GanEcosystem.UI.Core
{
    public interface IViewer: IInitializable
    {
        string UIName { get; }
        UILayer Layer { get; }
        bool IsActive { get; }
        bool CloseableByEscape { get; set; }

        event Action OnOpen;
        event Action OnClose;

        void Open();
        void Close();
    }
}
