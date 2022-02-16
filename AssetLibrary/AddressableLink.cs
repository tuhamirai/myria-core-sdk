namespace myria_core_sdk.AssetLibrary
{
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    public class AddressableLink : MonoBehaviour
    {
        public AssetReference asset;

        private IGameAssets gameAssets;
        public void Link(AssetReference obj)
        {
            this.asset = obj;
        }
        private void OnDestroy()
        {
            this.gameAssets.ReleaseAsset(this.asset);
        }
    }
}