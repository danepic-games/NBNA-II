using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using SerializableHelper;
using UnityEngine;

public class DataMapperUtil {

    public static void MapDataToObject(string framesValue, out Map<int, FrameData> frames, Map<int, Sprite> sprites, string sprite_file_name) {
        var listFrames = Regex.Split(framesValue, "<frame_end>");

        frames = new Map<int, FrameData>();

        for (int i = 0; i < listFrames.Length; i++) {
            var frameData = new FrameData();
            var frameLines = listFrames[i].Split("\n");

            for (int lineNumber = 0; lineNumber < frameLines.Length; lineNumber++) {
                var currentFrameLine = frameLines[lineNumber].Trim();
                try {
                    if (string.IsNullOrEmpty(currentFrameLine)) {
                        continue;
                    }

#region <frame>
                    if (currentFrameLine.StartsWith("<frame>")) {
                        var idAndName = currentFrameLine.Replace("<frame> ", "").Split(' ');
                        frameData.id = int.Parse(idAndName[0]);
                        frameData.name = idAndName[1];
                        continue;
                    }
#endregion

#region pic
                    if (currentFrameLine.StartsWith(FrameKeyEnum.pic.ToString())) {
                        var configProps = currentFrameLine.Split("  ");
                        foreach (string configProp in configProps) {
                            if (string.IsNullOrEmpty(configProp)) {
                                continue;
                            }
                            var keyValue = configProp.Split(':');
                            var key = keyValue[0].Trim();
                            var value = keyValue[1].Trim();
                            if (key.Equals(FrameKeyEnum.pic.ToString())) {
                                if (sprites.ContainsKey(int.Parse(value))) {
                                    frameData.pic = sprites[int.Parse(value)];
                                } else {
                                    throw new NullReferenceException($"Sprite key with value {value} not found in map!");
                                }
                                continue;
                            }
                            if (key.Equals(FrameKeyEnum.state.ToString())) {
                                frameData.state = (StateFrameEnum)int.Parse(value);
                                continue;
                            }
                            if (key.Equals(FrameKeyEnum.wait.ToString())) {
                                frameData.wait = float.Parse(value);
                                continue;
                            }
                            if (key.Equals(FrameKeyEnum.waitz.ToString())) {
                                frameData.wait = float.Parse(value);
                                continue;
                            }
                            if (key.Equals(FrameKeyEnum.next.ToString())) {
                                frameData.next = int.Parse(value);
                                continue;
                            }
                            if (key.Equals(FrameKeyEnum.dvx.ToString())) {
                                frameData.dvx = float.Parse(value);
                                continue;
                            }
                            if (key.Equals(FrameKeyEnum.dvy.ToString())) {
                                frameData.dvy = float.Parse(value);
                                continue;
                            }
                            if (key.Equals(FrameKeyEnum.dvz.ToString())) {
                                frameData.dvz = float.Parse(value);
                                continue;
                            }
                            if (key.Equals(FrameKeyEnum.dx.ToString())) {
                                frameData.dx = float.Parse(value);
                                continue;
                            }
                            if (key.Equals(FrameKeyEnum.dy.ToString())) {
                                frameData.dy = float.Parse(value);
                                continue;
                            }
                            if (key.Equals(FrameKeyEnum.dz.ToString())) {
                                frameData.dz = float.Parse(value);
                                continue;
                            }
                            if (key.Equals(FrameKeyEnum.centerx.ToString())) {
                                frameData.centerx = int.Parse(value);
                                continue;
                            }
                            if (key.Equals(FrameKeyEnum.centery.ToString())) {
                                frameData.centery = int.Parse(value);
                                continue;
                            }
                            if (key.Equals(FrameKeyEnum.hit_attack.ToString())) {
                                frameData.hit_attack = int.Parse(value);
                                continue;
                            }
                            if (key.Equals(FrameKeyEnum.hit_defense.ToString())) {
                                frameData.hit_defense = int.Parse(value);
                                continue;
                            }
                            if (key.Equals(FrameKeyEnum.hit_jump.ToString())) {
                                frameData.hit_jump = int.Parse(value);
                                continue;
                            }
                            if (key.Equals(FrameKeyEnum.hit_power.ToString())) {
                                frameData.hit_power = int.Parse(value);
                                continue;
                            }
                            if (key.Equals(FrameKeyEnum.hit_super.ToString())) {
                                frameData.hit_super = int.Parse(value);
                                continue;
                            }
                            if (key.Equals(FrameKeyEnum.hit_taunt.ToString())) {
                                frameData.hit_taunt = int.Parse(value);
                                continue;
                            }
                            if (key.Equals(FrameKeyEnum.hit_jump_defense.ToString())) {
                                frameData.hit_jump_defense = int.Parse(value);
                                continue;
                            }
                            if (key.Equals(FrameKeyEnum.hit_defense_attack.ToString())) {
                                frameData.hit_defense_attack = int.Parse(value);
                                continue;
                            }
                            if (key.Equals(FrameKeyEnum.hit_power_attack_up.ToString())) {
                                frameData.hit_power_attack_up = int.Parse(value);
                                continue;
                            }
                            if (key.Equals(FrameKeyEnum.hit_power_attack_forward.ToString())) {
                                frameData.hit_power_attack_forward = int.Parse(value);
                                continue;
                            }
                            if (key.Equals(FrameKeyEnum.hit_power_attack_down.ToString())) {
                                frameData.hit_power_attack_down = int.Parse(value);
                                continue;
                            }
                            if (key.Equals(FrameKeyEnum.hit_power_defense_up.ToString())) {
                                frameData.hit_power_defense_up = int.Parse(value);
                                continue;
                            }
                            if (key.Equals(FrameKeyEnum.hit_power_defense_forward.ToString())) {
                                frameData.hit_power_defense_forward = int.Parse(value);
                                continue;
                            }
                            if (key.Equals(FrameKeyEnum.hit_power_defense_down.ToString())) {
                                frameData.hit_power_defense_down = int.Parse(value);
                                continue;
                            }
                            if (key.Equals(FrameKeyEnum.hit_power_jump_up.ToString())) {
                                frameData.hit_power_jump_up = int.Parse(value);
                                continue;
                            }
                            if (key.Equals(FrameKeyEnum.hit_power_jump_forward.ToString())) {
                                frameData.hit_power_jump_forward = int.Parse(value);
                                continue;
                            }
                            if (key.Equals(FrameKeyEnum.hit_power_jump_down.ToString())) {
                                frameData.hit_power_jump_down = int.Parse(value);
                                continue;
                            }
                            if (key.Equals(FrameKeyEnum.mp.ToString())) {
                                frameData.mp = int.Parse(value);
                                continue;
                            }
                            if (key.Equals(FrameKeyEnum.hp.ToString())) {
                                frameData.hp = int.Parse(value);
                                continue;
                            }
                            if (key.Equals(FrameKeyEnum.sound.ToString())) {
                                frameData.sound = Resources.Load<AudioClip>(value);
                                continue;
                            }
                            if (key.Equals(FrameKeyEnum.hit_ground.ToString())) {
                                frameData.hit_ground = int.Parse(value);
                                continue;
                            }
                            if (key.Equals(FrameKeyEnum.hit_wall.ToString())) {
                                frameData.hit_wall = int.Parse(value);
                                continue;
                            }
                            if (key.Equals(FrameKeyEnum.hit_off_ground.ToString())) {
                                frameData.hit_off_ground = int.Parse(value);
                                continue;
                            }
                            if (key.Equals(FrameKeyEnum.hidden.ToString())) {
                                frameData.hidden = bool.Parse(value);
                                continue;
                            }
                            if (key.Equals(FrameKeyEnum.hold_forward_after.ToString())) {
                                frameData.hold_forward_after = int.Parse(value);
                                continue;
                            }
                            if (key.Equals(FrameKeyEnum.hold_defense_after.ToString())) {
                                frameData.hold_defense_after = int.Parse(value);
                                continue;
                            }
                            if (key.Equals(FrameKeyEnum.hold_power_after.ToString())) {
                                frameData.hold_power_after = int.Parse(value);
                                continue;
                            }
                            if (key.Equals(FrameKeyEnum.repeat_at.ToString())) {
                                frameData.repeat_at = float.Parse(value);
                                continue;
                            }
                            if (key.Equals(FrameKeyEnum.repeat_next.ToString())) {
                                frameData.repeat_next = int.Parse(value);
                                continue;
                            }
                            if (key.Equals(FrameKeyEnum.scale.ToString())) {
                                frameData.scale = float.Parse(value);
                                continue;
                            }
                            if (key.Equals(FrameKeyEnum.fadeOut.ToString())) {
                                frameData.fadeOut = float.Parse(value);
                                continue;
                            }
                        }
                    }
#endregion

                    if (frames.ContainsKey(frameData.id)) {
                        continue;
                    }
                    frames.Add(frameData.id, frameData);

                } catch (Exception ex) {
                    Debug.LogException(ex);
                    Debug.LogError($"Error in line {lineNumber} with frame id {frameData.id}");
                    throw ex;
                }
            }
        }
    }

}