using myria_core_sdk.AssetLibrary;
using UnityEngine;
using UnityEngine.UI;

public class TestAddressable : MonoBehaviour
{
   public Button downloadDemoSceneButton;
   public Button downloadAllAssetButton;
   public Button loadDemoSceneButton;

   private readonly AssetDownloadingHelper assetDownloadingHelper = new();
   private readonly AssetLoadingHelper     assetLoadingHelper     = new();
   
   public void Awake()
   {
      this.downloadDemoSceneButton.onClick.AddListener(this.OnClickDownloadDemoScene);
      this.downloadAllAssetButton.onClick.AddListener(this.OnClickDownloadAllAssets);
      this.loadDemoSceneButton.onClick.AddListener(this.OnClickLoadDemoScene);
   }
   private void OnClickLoadDemoScene()
   {
      Caching.ClearCache();
      this.assetLoadingHelper.LoadSceneAsync("Demo", progressPercentage => Debug.Log(progressPercentage));
   }

   private void OnClickDownloadDemoScene()
   {
      Caching.ClearCache();
      this.assetDownloadingHelper.DownloadDependenciesAsync("Demo", progressPercentage => Debug.Log(progressPercentage));
   }
   
   private void OnClickDownloadAllAssets()
   {
      Caching.ClearCache();
      this.assetDownloadingHelper.DownloadAllAssetsAsync(progressPercentage => Debug.Log(progressPercentage));
   }
}