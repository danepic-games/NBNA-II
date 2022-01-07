using System.Collections.Generic;
using System.IO;
using Model;
using UnityEngine;

namespace Util {
    public class DataChangerUtil {

        public static Data GetDataFromJson(string dataPath) {
            StreamReader streamReader = new StreamReader(dataPath, true);

            var jsonContent = streamReader.ReadToEnd();

            Data data = JsonUtility.FromJson<Data>(jsonContent);

            streamReader.Close();

            return data;
        }
    }
}