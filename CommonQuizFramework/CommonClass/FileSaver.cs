using System;
using System.IO;
using UnityEngine;

namespace CommonQuizFramework.CommonClass
{
    public static class FileSaver
    {
        public static void SaveFileInPersistentDataPath(string fileName, string data, Action onComplete = null)
        {
            var filePath = Path.Combine(Application.persistentDataPath, fileName);
            File.WriteAllText(filePath, data);
            onComplete?.Invoke();
        }
    }
}