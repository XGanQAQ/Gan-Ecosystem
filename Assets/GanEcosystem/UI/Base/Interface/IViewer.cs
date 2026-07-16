using System;

namespace GanEcosystem.UI.Core
{
    public interface IViewer : IInitializable
    {
        string UIName { get; }
        UILayer Layer { get; }
        string AssetKey { get; }// 资源路径，若为空则使用UI/ViewerName作为资源路径
        bool IsActive { get; }
        bool IsMutuallyExclusive { get; }  // 是否与其他同层级UI互斥，互斥的UI在打开时会关闭其他同层级UI
        bool CloseableByEscape { get; } // 是否可以通过ESC键关闭，若为true则在按下ESC键时会关闭该UI

        event Action OnOpen;
        event Action OnClose;

        void Open(object data = null);
        void Close();
    }
}
