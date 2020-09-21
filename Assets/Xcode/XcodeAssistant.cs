#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IOS
using System.IO;
using UnityEditor.iOS.Xcode;
#endif

public class XcodeAssistant
{
	[PostProcessBuild]
	public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
	{
#if UNITY_IOS
		// Read plist
		var plistPath = Path.Combine(path, "Info.plist");
		var plist = new PlistDocument();
		plist.ReadFromFile(plistPath);

		PlistElementDict rootDict = plist.root;
		// Add Permissions
		rootDict.SetString("NSCameraUsageDescription", "要求相機權限");
		// Write plist
		File.WriteAllText(plistPath, plist.WriteToString());
#endif
	}

}

#endif
