using System;
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
            try {
                var currentAnimType = EnumUtils.ParseEnum<CharacterAnimEnum>(currentAnim);
                switch (currentAnimType) {
                    case CharacterAnimEnum.Standing:
                        return GetFrameSafety(data.standing, animationIndex, currentAnim);
                    case CharacterAnimEnum.Walking:
                        return GetFrameSafety(data.walking, animationIndex, currentAnim);
                    case CharacterAnimEnum.Punch:
                        return GetFrameSafety(data.punch, animationIndex, currentAnim);
                    case CharacterAnimEnum.SimpleDash:
                        return GetFrameSafety(data.simpleDash, animationIndex, currentAnim);
                    case CharacterAnimEnum.StopRunning:
                        return GetFrameSafety(data.stopRunning, animationIndex, currentAnim);
                    case CharacterAnimEnum.Running:
                        return GetFrameSafety(data.running, animationIndex, currentAnim);
                    case CharacterAnimEnum.Jumping:
                        return GetFrameSafety(data.jumping, animationIndex, currentAnim);
                    case CharacterAnimEnum.Crouch:
                        return GetFrameSafety(data.crouch, animationIndex, currentAnim);
                    case CharacterAnimEnum.FallJumping:
                        return GetFrameSafety(data.fallJumping, animationIndex, currentAnim);
                    default:
                        Debug.LogWarning($"Frame of current animation {currentAnim} not mapped yet to extract actual frame!");
                        return null;
                }
            } catch (Exception ex) {
                Debug.LogError(ex);
                return null;
            }
        }

        private static Frame GetFrameSafety(Frame[] frames, int animationIndex, string currentAnim) {
            if (frames.Length - 1 < animationIndex) {
                Debug.LogWarning($"Frame of current animation {currentAnim} - {animationIndex} not mapped yet to extract actual frame!");
                return null;
            }
            return frames[animationIndex];
        }
    }
}