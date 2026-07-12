namespace GanEcosystem.UI.Core
{
    /// <summary>
    /// UI层级定义，数值越小越靠后渲染
    /// </summary>
    public enum UILayer
    {
        /// <summary>全屏背景UI（主菜单背景、场景过渡背景等）</summary>
        Background = 0,
        /// <summary>3D场景UI、世界空间UI（场景内交互提示、怪物头顶血条等）</summary>
        Scene = 1,
        /// <summary>主界面UI、HUD（背包、任务面板、角色信息等）</summary>
        Normal = 2,
        /// <summary>浮动提示、信息显示（拾取提示、经验值提示等短暂信息）</summary>
        Info = 3,
        /// <summary>弹窗、对话框（确认框、设置面板等需要用户交互的窗口）</summary>
        Popup = 4,
        /// <summary>短时通知（Toast提示、成就解锁等自动消失的通知）</summary>
        Toast = 5,
        /// <summary>系统级UI（加载界面、断线重连提示等全局覆盖UI）</summary>
        Top = 6,
        /// <summary>调试UI（FPS显示、开发调试面板，仅开发模式使用）</summary>
        Debug = 7,
    }
}
