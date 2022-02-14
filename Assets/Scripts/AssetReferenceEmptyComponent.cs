using System;
using UnityEngine.AddressableAssets;

[Serializable]
public class AssetReferenceEmptyComponent : AssetReferenceT<EmptyComponent> {
	public AssetReferenceEmptyComponent(string guid) : base(guid) {}
}