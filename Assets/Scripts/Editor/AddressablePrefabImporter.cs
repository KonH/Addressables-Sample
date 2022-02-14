using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;

public class AddressablePrefabImporter : AssetPostprocessor {
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
		return (parts[0] == "Assets") && (parts[1] == "Prefabs_AB");
	}

	static string CreateGroupNameFromPath(string[] parts) {
		var innerParts = parts
			.Take(parts.Length - 1)
			.Skip(2);
		return string.Join("_", innerParts);
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