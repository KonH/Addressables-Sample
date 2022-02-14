using UnityEngine;
using UnityEngine.AddressableAssets;

public class SampleStarter : MonoBehaviour {
	[SerializeField]
	Transform _parent;

	[SerializeField]
	ComponentReference<EmptyComponent> _reference;

	[SerializeField]
	string _variantBaseName = "Assets/Prefabs/BaseImage Variant{0}.prefab";

	void Start() {
		_reference.InstantiateAsync(_parent);
	}

	public void LoadVariant(int index) {
		var finalName = string.Format(_variantBaseName, index);
		Addressables.InstantiateAsync(finalName, _parent);
	}
}