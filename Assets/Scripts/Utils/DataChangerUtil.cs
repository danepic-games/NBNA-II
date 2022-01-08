using System;
using System.IO;
using System.Linq;
using Model;
using UnityEngine;

namespace Util {
    public class DataChangerUtil : MonoBehaviour {

        public static Data GetDataFromJson(string dataPath) {
            Data data = new Data();

            var fileNames = new string[]{"Standing", "Walking"};

            GetFramesByDataFileName(data, dataPath, fileNames);

            return data;
        }

        private static void GetFramesByDataFileName(Data data, string dataPath, string[] fileNames){
            foreach (string fileName in fileNames) {
                var framesDataChanger = Resources.Load<TextAsset>($"{dataPath}{fileName}");
                JsonUtility.FromJsonOverwrite(framesDataChanger.text, data);
            }
        }
    }
}