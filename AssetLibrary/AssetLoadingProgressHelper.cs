namespace myria_core_sdk.AssetLibrary
{
    using System;
    using Cysharp.Threading.Tasks;
    using UnityEngine.Events;
    using UnityEngine.SceneManagement;

    public class AssetLoadingHelper
    {
        private const float LoadingProgressActionTimeStep = 0.02f;

        /// <summary>
        /// Load Asset and it's dependencies by key, also do progressAction during that process
        /// </summary>
        public async void LoadAssetAsync<T>(object key, UnityAction<float> progressAction, bool isAutoUnload = true)
        {
            var asyncOperationHandle = GameAssets.LoadAssetAsync<T>(key, isAutoUnload);

            while (!asyncOperationHandle.IsDone)
            {
                progressAction(asyncOperationHandle.PercentComplete);
                await UniTask.Delay(TimeSpan.FromSeconds(LoadingProgressActionTimeStep));
            }

            progressAction(1f);
        }
        
        /// <summary>
        /// Load Scene and it's dependencies by key, also do progressAction during that process
        /// </summary>
        public async void LoadSceneAsync(object key, UnityAction<float> progressAction, LoadSceneMode loadMode = LoadSceneMode.Single, bool activeOnLoad = true)
        {
            var asyncOperationHandle = GameAssets.LoadSceneAsync(key, loadMode, activeOnLoad);

            while (!asyncOperationHandle.IsDone)
            {
                progressAction(asyncOperationHandle.PercentComplete);
                await UniTask.Delay(TimeSpan.FromSeconds(LoadingProgressActionTimeStep));
            }

            progressAction(1f);
        }
    }
}