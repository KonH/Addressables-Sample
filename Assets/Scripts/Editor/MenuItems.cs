using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;

// Static variant to change addressable settings (resource URL etc)
public static class MenuItems {
	[MenuItem("Custom/BuildAddressables/DEV")]
	public static void BuildAddressablesDev() {
		ChangeProfile("Default");
		AddressableAssetSettings.BuildPlayerContent();
	}

	[MenuItem("Custom/BuildAddressables/QA")]
	public static void BuildAddressablesQA() {
		ChangeProfile("Production");
		AddressableAssetSettings.BuildPlayerContent();
	}

	static void ChangeProfile(string profileName) {
		var profileId = AddressableAssetSettingsDefaultObject.Settings.profileSettings.GetProfileId(profileName);
		AddressableAssetSettingsDefaultObject.Settings.activeProfileId = profileId;
	}
}