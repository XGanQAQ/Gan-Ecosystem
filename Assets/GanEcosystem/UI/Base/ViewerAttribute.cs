using System;

namespace GanEcosystem.UI.Core
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ViewerAttribute : Attribute
    {
        // UI所在的层级
        public UILayer Layer { get; }

        // 资源路径，若为空则使用UI/ViewerName作为资源路径
        public string AssetKey { get; }

        // 是否与其他同层级UI互斥，互斥的UI在打开时会关闭其他同层级UI
        public bool IsMutuallyExclusive { get; } 

        public ViewerAttribute(UILayer layer = UILayer.Normal, string assetKey = "", bool isMutuallyExclusive = false)
        {
            Layer = layer;
            AssetKey = assetKey;
            IsMutuallyExclusive = isMutuallyExclusive;
        }
    }
}
