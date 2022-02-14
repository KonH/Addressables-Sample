using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;

public class AddressableSceneImporter : AssetPostprocessor {
	static void OnPostprocessAllAssets(
		string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths) {
		var settings = AddressableAssetSettingsDefaultObject.Settings;
		foreach ( var assetPath in importedAssets ) {
			if ( !File.Exists(assetPath) ) {
				continue;
			}
			var parts = assetPath.Split('/', '\\');
			if ( !IsValidPath(parts) ) {
				continue;
			}
			var groupName = CreateGroupNameFromPath(parts);
			var group = GetOrCreateGroup(settings, groupName);
			var guid = AssetDatabase.AssetPathToGUID(assetPath);
			var entry = settings.CreateOrMoveEntry(guid, group);
			if ( !settings.GetLabels().Contains(groupName) ) {
				settings.AddLabel(groupName);
			}
			entry.SetLabel(groupName, true);
		}
	}

	static bool IsValidPath(string[] parts) {
		if ( parts.Length < 3 ) {
			return false;
		}
		var fileName = parts[parts.Length - 1];
		if ( fileName == "EntryPoint.unity" ) {
			return false;
		}
		return (parts[0] == "Assets") && (parts[1] == "Scenes");
	}

	static string CreateGroupNameFromPath(string[] parts) {
		return Path.ChangeExtension(parts[parts.Length - 1], null);
	}

	static AddressableAssetGroup GetOrCreateGroup(AddressableAssetSettings settings, string groupName) {
		var group = settings.groups.Find(g => g.name == groupName);
		if ( group ) {
			return group;
		}
		return settings.CreateGroup(
			groupName, false, false, false,
			new List<AddressableAssetGroupSchema> { settings.DefaultGroup.Schemas[0] });
	}
}