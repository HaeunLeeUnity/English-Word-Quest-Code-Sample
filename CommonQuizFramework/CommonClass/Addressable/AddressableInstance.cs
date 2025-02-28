using UnityEngine;

namespace CommonQuizFramework.CommonClass.Addressable
{
    public class AddressableInstance : MonoBehaviour
    {
        private void OnDestroy()
        {
            AssetProvider.Instance.ReleaseInstance(gameObject);
        }
    }
}