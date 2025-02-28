using UnityEditor;
using UnityEngine;

namespace LHEHelper.Editor
{
    #if UNITY_EDITOR
    public static class VersionManager
    {
        [MenuItem("LHEHelper/Major 버전 코드 증가")]
        public static void IncreaseMajorVersion()
        {
            var version = PlayerSettings.bundleVersion;
            var versionParts = version.Split('.');

            if (versionParts.Length >= 2)
            {
                var major = int.Parse(versionParts[0]);
                major++;
                PlayerSettings.bundleVersion = $"{major}.0.0";
                Debug.Log($"Updated Major Version: {PlayerSettings.bundleVersion}");
            }
            else
            {
                Debug.LogError("Invalid version format. Make sure it follows 'MAJOR.MINOR.PATCH'.");
            }
        }

        [MenuItem("LHEHelper/Minor 버전 코드 증가")]
        public static void IncreaseMinorVersion()
        {
            var version = PlayerSettings.bundleVersion;
            var versionParts = version.Split('.');

            if (versionParts.Length >= 2)
            {
                var major = int.Parse(versionParts[0]);
                var minor = int.Parse(versionParts[1]);
                minor++;
                PlayerSettings.bundleVersion = $"{major}.{minor}.0";
                Debug.Log($"Updated Minor Version: {PlayerSettings.bundleVersion}");
            }
            else
            {
                Debug.LogError("Invalid version format. Make sure it follows 'MAJOR.MINOR.PATCH'.");
            }
        }

        [MenuItem("LHEHelper/Patch 버전 코드 증가")]
        public static void IncreasePatchVersion()
        {
            var version = PlayerSettings.bundleVersion;
            var versionParts = version.Split('.');

            if (versionParts.Length >= 3)
            {
                var major = int.Parse(versionParts[0]);
                var minor = int.Parse(versionParts[1]);
                var patch = int.Parse(versionParts[2]);
                patch++;
                PlayerSettings.bundleVersion = $"{major}.{minor}.{patch}";
                Debug.Log($"Updated Patch Version: {PlayerSettings.bundleVersion}");
            }
            else
            {
                Debug.LogError("Invalid version format. Make sure it follows 'MAJOR.MINOR.PATCH'.");
            }
        }
    }
    #endif
}