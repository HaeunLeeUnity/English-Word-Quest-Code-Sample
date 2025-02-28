using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace LHEHelper.Editor
{
#if UNITY_EDITOR
    public class ListDivider : EditorWindow
    {
        private string _targetString;
        private string[] _dividedStrings;
        private string _rawFilePath;
        private int _targetFileCount = 2;

        [MenuItem("LHEHelper/텍스트 파일 분배기")]
        public static void ShowWindow()
        {
            // Show the window
            var window = GetWindow<ListDivider>("텍스트 파일 분배기");

            window.minSize = new Vector2(500, 500); // 최소 크기
            window.maxSize = new Vector2(500, 1000); // 최대 크기
        }

        private void LoadFile(string path)
        {
            _rawFilePath = path;
            _targetString = File.ReadAllText(_rawFilePath);
            DivideString();
        }

        private void CreateFile()
        {
            var targetStrings = SplitArrayIntoStringChunks(_dividedStrings, _targetFileCount);
            var filePath = Path.GetDirectoryName(_rawFilePath);
            var fileName = Path.GetFileNameWithoutExtension(_rawFilePath);

            for (var i = 0; i < targetStrings.Length; i++)
            {
                var newPath = Path.Combine(filePath, $"{fileName}-{i + 1}.txt");
                File.WriteAllText(newPath, targetStrings[i]);
                Debug.Log($"저장됨: {newPath}");
            }
        }

        private string[] SplitArrayIntoStringChunks(string[] inputArray, int numberOfChunks)
        {
            var totalElements = inputArray.Length;
            var baseSize = totalElements / numberOfChunks; // 기본 크기
            var remainder = totalElements % numberOfChunks; // 나머지

            var currentIndex = 0;
            var result = new string[numberOfChunks];

            for (var i = 0; i < numberOfChunks; i++)
            {
                var chunkSize = baseSize + (i < remainder ? 1 : 0); // 나머지를 앞에서부터 하나씩 분배
                var chunk = new string[chunkSize];
                Array.Copy(inputArray, currentIndex, chunk, 0, chunkSize);
                result[i] = string.Join("\n", chunk); // '\\n'으로 연결하여 하나의 문자열로 저장
                currentIndex += chunkSize;
            }

            return result;
        }

        private void DivideString()
        {
            _dividedStrings = _targetString.Split('\n');
        }

        private int GetRawLineCount()
        {
            return _dividedStrings.Length;
        }

        private int GetDividedLineCount()
        {
            return GetRawLineCount() / _targetFileCount;
        }

        private void OnGUI()
        {
            #region 설명

            var centeredStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter
            };

            GUILayout.Space(15);
            EditorGUILayout.HelpBox(
                "여러 줄의 긴 텍스트 파일을 여러 파일로 나눕니다.\n\n예시) A.txt (30줄) -> A-1.txt, A-2.txt, A-3.txt (10줄)",
                MessageType.Info);
            GUILayout.Space(15);

            #endregion

            #region 파일 선택

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("\n파일 찾기\n", GUILayout.Width(300)))
            {
                var path = EditorUtility.OpenFilePanel("Select a File", "", "txt");
                if (!string.IsNullOrEmpty(path))
                {
                    Debug.Log($"선택된 파일 경로 : {path}");
                    LoadFile(path);
                }
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(20);

            if (!string.IsNullOrEmpty(_rawFilePath))
            {
                GUILayout.Label($"{_rawFilePath}", centeredStyle);
            }
            else
            {
                GUILayout.Label($"파일을 선택해주세요.", centeredStyle);
            }

            #endregion

            #region 파일 개수 선택

            GUILayout.Space(50);
            GUILayout.Label($"파일 개수", centeredStyle);

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button("-", GUILayout.Width(50), GUILayout.Height(50)))
            {
                if (1 < _targetFileCount)
                {
                    _targetFileCount--;
                }
            }

            GUILayout.Space(20);
            var centeredStyleMiddle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = 30
            };

            GUILayout.Label(_targetFileCount.ToString(), centeredStyleMiddle);

            GUILayout.Space(20);
            if (GUILayout.Button("+", GUILayout.Width(50), GUILayout.Height(50)))
            {
                _targetFileCount++;
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(20);

            #endregion

            #region 줄 수 표시

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label($"변환 전", centeredStyle);
            GUILayout.FlexibleSpace();
            GUILayout.Label($"변환 후", centeredStyle);
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (_dividedStrings != null)
            {
                GUILayout.Label($"{GetRawLineCount()}", centeredStyleMiddle);
            }
            else
            {
                GUILayout.Label($"X", centeredStyleMiddle);
            }

            GUILayout.FlexibleSpace();

            if (_dividedStrings != null)
            {
                GUILayout.Label($"{GetDividedLineCount()}", centeredStyleMiddle);
            }
            else
            {
                GUILayout.Label($"X", centeredStyleMiddle);
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();

            #endregion

            #region 파일 생성

            GUILayout.FlexibleSpace();

            GUI.enabled = _dividedStrings != null;
            if (GUILayout.Button("\n파일 생성\n", GUILayout.Height(50)))
            {
                CreateFile();
            }

            #endregion
        }
    }
#endif
}