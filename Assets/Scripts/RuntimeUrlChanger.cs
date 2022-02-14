using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;

// Runtime variant to change resource URL
public static class RuntimeUrlChanger {

	[RuntimeInitializeOnLoadMethod]
	static void SetInternalIdTransform() {
		Addressables.InternalIdTransformFunc = CustomUrlTransformer;
	}

	static string CustomUrlTransformer(IResourceLocation location) {
		if ((location.ResourceType == typeof(IAssetBundleResource)) && location.InternalId.StartsWith("http")) {
			// Debug.Log($"Resource URL can be changed: '{location.InternalId}'");
		}
		return location.InternalId;
	}
}