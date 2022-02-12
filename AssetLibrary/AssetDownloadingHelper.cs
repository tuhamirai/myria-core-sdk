namespace myria_core_sdk.AssetLibrary
{
    using Cysharp.Threading.Tasks;
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    public class AssetDownloadingHelper
    {
        private long totalDownloadSizeKb;
        
        private async void Download()
        {
            this.totalDownloadSizeKb = await Addressables.GetDownloadSizeAsync(AllKeys).Task;

            if (this.totalDownloadSizeKb == 0)
            {
                this.View.ImgLoadingFiller.fillAmount = 1;
                this.View.TxtPercent.text             = "100%";
                this.View.TxtProcessName.text         = LoadingProcessText;
                return;
            }

            var downloadedKb = 0f;
            this.View.ImgLoadingFiller.fillAmount = 0;
            this.View.TxtProcessName.text         = DownloadProcessText;
            foreach (var key in AllKeys)
            {
                var keyDownloadSizeKb = await Addressables.GetDownloadSizeAsync(key).Task;
                if (keyDownloadSizeKb <= 0) continue;

                var keyDownloadOperation = Addressables.DownloadDependenciesAsync(key);
                while (!keyDownloadOperation.IsDone)
                {
                    await UniTask.Yield();
                    var acquiredKb      = downloadedKb + (keyDownloadOperation.PercentComplete * keyDownloadSizeKb);
                    var percentDownload = acquiredKb / this.totalDownloadSizeKb;
                    //TODO: this is a temporary fixing, need to investigate why the progressPercentage go more than 100% after minimizing then resuming app. 
                    var totalProgressPercentage = Mathf.Min(percentDownload * 100, 100);

                    this.View.ImgLoadingFiller.fillAmount = totalProgressPercentage;
                    this.View.TxtPercent.text             = $"{(int)totalProgressPercentage}%";
                }

                downloadedKb += keyDownloadSizeKb;
            }

            this.View.TxtProcessName.text = LoadingProcessText;
        }
    }
}