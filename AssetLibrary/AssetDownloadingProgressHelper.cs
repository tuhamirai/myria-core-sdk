namespace myria_core_sdk.AssetLibrary
{
    using System;
    using System.Collections;
    using Cysharp.Threading.Tasks;
    using UnityEngine;
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
            Debug.Log($"Download {downloadSize / 1000} Kbs!!");
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
        public async void DownloadDependenciesAsync(IEnumerable keys, Addressables.MergeMode mergeMode, UnityAction<float> progressAction,
            bool autoReleaseHandle = false)
        {
            var downloadSize = await Addressables.GetDownloadSizeAsync(keys);
            Debug.Log($"Download {downloadSize / 1000} Kbs!!");
            if (downloadSize > 0)
            {
                var downloadDependenciesAsync = Addressables.DownloadDependenciesAsync(keys, mergeMode, autoReleaseHandle);
                while (!downloadDependenciesAsync.IsDone)
                {
                    progressAction(downloadDependenciesAsync.PercentComplete);
                    await UniTask.Delay(TimeSpan.FromSeconds(DownloadingProgressActionTimeStep));
                }
            }

            progressAction(1f);
        }

        /// <summary>
        /// Download all remote asset in game 
        /// </summary>
        public async void DownloadAllAssetsAsync(UnityAction<float> progressAction)
        {
            var resourceLocator = await Addressables.InitializeAsync();
            var allKeys         = resourceLocator.Keys;

            this.DownloadDependenciesAsync(allKeys, Addressables.MergeMode.Union, progressAction);
        }
    }
}