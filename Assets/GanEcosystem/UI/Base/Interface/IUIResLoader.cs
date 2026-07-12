namespace GanEcosystem.UI.Core
{
    public interface IUIResLoader
    {
        T Load<T>(string assetKey) where T : UnityEngine.Object;
    }
}