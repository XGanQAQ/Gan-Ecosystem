using GanEcosystem.UI.Core;
using UnityEngine;
namespace GanEcosystem.UI.UnityRuntime
{
    public class UnitySimpleUIResLoader : IUIResLoader
    {
        public T Load<T>(string assetKey) where T : Object
        {
            return Resources.Load<T>(assetKey);
        }
    }
}