using System;
using System.Collections;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Cysharp.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;
using Progress = Cysharp.Threading.Tasks.Progress;

public class EntryPointStarter : MonoBehaviour {
	[SerializeField]
	TMP_Text _statusText;

	[SerializeField]
	AssetReference _sceneReference;

	async void Start() {
		WriteLine("Starting...");
		var labels = (IEnumerable) new[] {
			"Sample", "Style1", "Style2"
		};
		var downloadSizeHandle = Addressables.GetDownloadSizeAsync(labels);
		var downloadSize = await downloadSizeHandle.ToUniTask();
		if ( downloadSizeHandle.Status == AsyncOperationStatus.Failed ) {
			WriteLine("Failed to get download size");
			return;
		}
		WriteLine($"Download size is {downloadSize} bytes");
		var downloadHandle = Addressables.DownloadDependenciesAsync(labels, Addressables.MergeMode.None, autoReleaseHandle: true);
		var downloadTask = downloadHandle.ToUniTask(Progress.Create<float>(percent => Write($"{percent} ")));
		await downloadTask;
		if ( downloadSizeHandle.Status == AsyncOperationStatus.Failed ) {
			WriteLine($"Failed to download dependencies: {downloadSizeHandle.OperationException}");
			return;
		}
		WriteLine("Dependencies downloaded");
		await Task.Delay(TimeSpan.FromSeconds(1.5));
		await _sceneReference.LoadSceneAsync();
	}

	void WriteLine(string line) {
		Write($"{line}\n");
	}

	void Write(string text) {
		_statusText.text += $"{text}";
	}
}