namespace myria_core_sdk.AssetLibrary
{
    using System;
    using System.Collections;
    using System.Linq;
    using Cysharp.Threading.Tasks;
    using UnityEngine.AddressableAssets;
    using UnityEngine.Events;

    public class AssetDownloadingHelper
    {
        private const float DownloadingProgressActionTimeStep = 0.02f;

        /// <summary>
        /// Wrap https://docs.unity3d.com/Packages/com.unity.addressables@1.15/manual/DownloadDependenciesAsync.html with download progress
        /// </summary>
        public async void DownloadDependenciesAsync(object key, UnityAction<float> progressAction, bool autoReleaseHandle = false)
        {
            var downloadSize = await Addressables.GetDownloadSizeAsync(key);
            if (downloadSize > 0)
            {
                var downloadDependenciesAsync = Addressables.DownloadDependenciesAsync(key, autoReleaseHandle);
                while (!downloadDependenciesAsync.IsDone)
                {
                    progressAction(downloadDependenciesAsync.PercentComplete);
                    await UniTask.Delay(TimeSpan.FromSeconds(DownloadingProgressActionTimeStep));
                }
            }

            progressAction(1f);
        }

        /// <summary>
        /// Wrap https://docs.unity3d.com/Packages/com.unity.addressables@1.15/manual/DownloadDependenciesAsync.html with download progress
        /// don't use public static AsyncOperationHandle<long> GetDownloadSizeAsync(IList<object> keys) because it's Obsolete
        /// </summary>
        public async void DownloadDependenciesAsync(IEnumerable keys, UnityAction<float> progressAction, Addressables.MergeMode mergeMode = Addressables.MergeMode.Union,
            bool autoReleaseHandle = false)
        {
            var downloadSize = await Addressables.GetDownloadSizeAsync(keys);
            if (downloadSize > 0)
            {
                var downloadDependenciesAsync = Addressables.DownloadDependenciesAsync(keys, Addressables.MergeMode.Union, autoReleaseHandle);
                while (!downloadDependenciesAsync.IsDone)
                {
                    progressAction(downloadDependenciesAsync.PercentComplete);
                    await UniTask.Delay(TimeSpan.FromSeconds(DownloadingProgressActionTimeStep));
                }
            }

            progressAction(1f);
        }

        public async void DownloadAllAssetsAsync(UnityAction<float> progressAction)
        {
            var resourceLocator     = await Addressables.InitializeAsync();
            var allKeys             = resourceLocator.Keys.ToList();
            var totalDownloadSizeKb = await Addressables.GetDownloadSizeAsync(allKeys);

            if (totalDownloadSizeKb > 0)
            {
                this.DownloadDependenciesAsync(allKeys, progressAction);
            }
            
            progressAction(1f);
        }
    }
}