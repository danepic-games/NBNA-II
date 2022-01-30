using Back.Model.Type;
using Models;
using UnityEngine;

namespace Utils {
    public class DataChangerUtil : MonoBehaviour {

        public static Data GetDataFromJson(string dataPath) {
            Data data = new Data();

            var headerData = Resources.Load<TextAsset>($"{dataPath}main");
            if (headerData != null) {
                JsonUtility.FromJsonOverwrite(headerData.text, data);
            }

            GetFramesByDataFileName(data, dataPath, data.headerData.loadedAnimations);

            return data;
        }

        private static void GetFramesByDataFileName(Data data, string dataPath, string[] fileNames) {
            foreach (string fileName in fileNames) {
                var framesDataChanger = Resources.Load<TextAsset>($"{dataPath}{fileName}");
                if (framesDataChanger != null) {
                    JsonUtility.FromJsonOverwrite(framesDataChanger.text, data);
                }
            }
        }

        public static Frame GetActualFrameFromData(int animationIndex, string currentAnim, Data data) {
//            Debug.Log($"{animationIndex} | {currentAnim}");
            var currentAnimType = EnumUtils.ParseEnum<CharacterAnimEnum>(currentAnim);
            switch (currentAnimType) {
                case CharacterAnimEnum.Standing:
                    return data.standing[animationIndex];
                case CharacterAnimEnum.Walking:
                    return data.walking[animationIndex];
                case CharacterAnimEnum.Punch:
                    return data.punch[animationIndex];
                case CharacterAnimEnum.SimpleDash:
                    return data.simpleDash[animationIndex];
                case CharacterAnimEnum.StopRunning:
                    return data.stopRunning[animationIndex];
                case CharacterAnimEnum.Running:
                    return data.running[animationIndex];
                default:
                    Debug.LogError($"Frame of current animation {currentAnim} not mapped yet to extract actual frame!");
                    return null;
            }
        }
    }
}