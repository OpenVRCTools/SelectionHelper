using UnityEngine;
using UnityEditor;
using System.Linq;
using System.IO;

namespace OpenVRCTools.SelectionHelper
{
    [System.Serializable]
    public class SaveSelection : ScriptableObject
    {
        [SerializeReference]
        public Object[] oldSelection;

        public string[] s = {"Boi","Huh"};

        [MenuItem("Assets/Selection Helper/Save Selection")]
        [MenuItem("GameObject/Selection Helper/Save\\Load/Save Selection", false, 0)]
        static void SaveSelected()
        {
            instance.oldSelection = Selection.objects;
            Save();
        }

        [MenuItem("Assets/Selection Helper/Load Selection")]
        [MenuItem("GameObject/Selection Helper/Save\\Load/Load Selection", false, 1)]
        static void LoadSelected()
        {
            if (instance.oldSelection != null)
                Selection.objects = Selection.objects.Concat(instance.oldSelection).ToArray();
        }

        private static SaveSelection _instance;
        private static SaveSelection instance =>
            _instance ? _instance : GetInstance();

        public static string folderPath = "OpenVRCTools/Saved Data/SaveSelection";
        private static string SavePath =>
            folderPath + "/SaveSelectionData.txt";

        public static SaveSelection GetInstance()
        {
            if (_instance == null && Exists())
            {
                _instance = CreateInstance<SaveSelection>();
                using (StreamReader reader = new StreamReader(SavePath))
                    JsonUtility.FromJsonOverwrite(reader.ReadToEnd(),_instance);
            }

            if (_instance == null)
            {
                _instance = CreateInstance<SaveSelection>();
                string directoryPath = Path.GetDirectoryName(SavePath);

                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);

                string json = JsonUtility.ToJson(_instance);
                using (StreamWriter writer = File.CreateText(SavePath))
                    writer.Write(json);
            }

            return _instance;
        }

        public static void Save()
        {
            string json = EditorJsonUtility.ToJson(_instance);
            using (StreamWriter writer = new StreamWriter(SavePath))
                writer.Write(json);
        }
        public static bool Exists() =>
            File.Exists(SavePath);
    }
}
