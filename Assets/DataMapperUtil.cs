using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class DataMapperUtil {

    public static void MapDataToObject(string framesValue, out Dictionary<int, FrameData> frames, Dictionary<string, Sprite> sprites, string sprite_file_name, out List<FrameData> listOfFramesContent) {
        var listFrames = Regex.Split(framesValue, "<frame_end>");

        listOfFramesContent = new List<FrameData>();
        frames = new Dictionary<int, FrameData>();

        for (int i = 0; i < listFrames.Length; i++) {
            var frameData = new FrameData();
            var frameLines = listFrames[i].Split("\n");

            bool isBdy = false;
            bool isItr = false;
            bool isOpoint = false;

            for (int lineNumber = 0; lineNumber < frameLines.Length; lineNumber++) {
                var currentFrameLine = frameLines[lineNumber].Trim();
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
                            if (sprites.ContainsKey($"{sprite_file_name}_{value}")) {
                                frameData.pic = sprites[$"{sprite_file_name}_{value}"];
                            } else {
                                frameData.pic = null;
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
                        if (key.Equals(FrameKeyEnum.hit_off_ground.ToString())) {
                            frameData.hit_off_ground = int.Parse(value);
                            continue;
                        }
                        if (key.Equals(FrameKeyEnum.hidden.ToString())) {
                            frameData.hidden = bool.Parse(value);
                            continue;
                        }
                    }
                }
#endregion

#region bdy
                if (currentFrameLine.StartsWith(FrameKeyEnum.bdy.ToString() + ":")) {
                    isBdy = true;
                    continue;
                }

                if (isBdy) {
                    isBdy = false;

                    var bdyProps = currentFrameLine.Split("  ");

                    var bdy = new BodyData();

                    foreach (string bdyProp in bdyProps) {

                        if (string.IsNullOrEmpty(bdyProp)) {
                            continue;
                        }
                        var keyValue = bdyProp.Split(':');
                        var key = keyValue[0].Trim();
                        var value = keyValue[1].Trim();

                        if (key.Equals(FrameKeyEnum.kind.ToString())) {
                            bdy.kind = (BodyKindEnum)int.Parse(value);
                            continue;
                        }

                        if (key.Equals(FrameKeyEnum.x.ToString())) {
                            bdy.x = float.Parse(value);
                            continue;
                        }

                        if (key.Equals(FrameKeyEnum.y.ToString())) {
                            bdy.y = float.Parse(value);
                            continue;
                        }

                        if (key.Equals(FrameKeyEnum.z.ToString())) {
                            bdy.z = float.Parse(value);
                            continue;
                        }

                        if (key.Equals(FrameKeyEnum.w.ToString())) {
                            bdy.w = float.Parse(value);
                            continue;
                        }

                        if (key.Equals(FrameKeyEnum.h.ToString())) {
                            bdy.h = float.Parse(value);
                            continue;
                        }

                        if (key.Equals(FrameKeyEnum.zwidth.ToString())) {
                            bdy.zwidth = float.Parse(value);
                            continue;
                        }

                        continue;
                    }

                    frameData.bodys.Add(bdy);
                }
#endregion

#region opoint
                if (currentFrameLine.StartsWith(FrameKeyEnum.opoint.ToString() + ":")) {
                    isOpoint = true;
                    continue;
                }

                if (isOpoint) {
                    isOpoint = false;

                    var opointProps = currentFrameLine.Split("  ");

                    var opoint = new ObjectPointData();

                    foreach (string opointProp in opointProps) {

                        if (string.IsNullOrEmpty(opointProp)) {
                            continue;
                        }
                        var keyValue = opointProp.Split(':');
                        var key = keyValue[0].Trim();
                        var value = keyValue[1].Trim();

                        if (key.Equals(FrameKeyEnum.kind.ToString())) {
                            opoint.kind = (ObjectPointKindEnum)int.Parse(value);
                            continue;
                        }

                        if (key.Equals(FrameKeyEnum.x.ToString())) {
                            opoint.x = float.Parse(value);
                            continue;
                        }

                        if (key.Equals(FrameKeyEnum.y.ToString())) {
                            opoint.y = float.Parse(value);
                            continue;
                        }

                        if (key.Equals(FrameKeyEnum.z.ToString())) {
                            opoint.z = float.Parse(value);
                            continue;
                        }

                        if (key.Equals(FrameKeyEnum.action.ToString())) {
                            opoint.action = int.Parse(value);
                            continue;
                        }

                        if (key.Equals(FrameKeyEnum.dvx.ToString())) {
                            opoint.dvx = float.Parse(value);
                            continue;
                        }

                        if (key.Equals(FrameKeyEnum.dvy.ToString())) {
                            opoint.dvy = float.Parse(value);
                            continue;
                        }

                        if (key.Equals(FrameKeyEnum.dvz.ToString())) {
                            opoint.dvz = float.Parse(value);
                            continue;
                        }

                        if (key.Equals(FrameKeyEnum.object_id.ToString())) {
                            opoint.object_id = value;
                            continue;
                        }

                        if (key.Equals(FrameKeyEnum.facing.ToString())) {
                            opoint.facing = (int.Parse(value)) >= 0;
                            continue;
                        }

                        if (key.Equals(FrameKeyEnum.quantity.ToString())) {
                            opoint.quantity = int.Parse(value);
                            continue;
                        }

                        if (key.Equals(FrameKeyEnum.z_div_per_quantity.ToString())) {
                            opoint.z_division_per_quantity = float.Parse(value);
                            continue;
                        }

                        continue;
                    }

                    frameData.opoints.Add(opoint);
                }
#endregion

#region itr
                if (currentFrameLine.StartsWith(FrameKeyEnum.itr.ToString() + ":")) {
                    isItr = true;
                    continue;
                }

                if (isItr) {
                    isItr = false;

                    var itrProps = currentFrameLine.Split("  ");

                    var itr = new InteractionData();

                    foreach (string itrProp in itrProps) {

                        if (string.IsNullOrEmpty(itrProp)) {
                            continue;
                        }
                        var keyValue = itrProp.Split(':');
                        var key = keyValue[0].Trim();
                        var value = keyValue[1].Trim();

                        if (key.Equals(FrameKeyEnum.kind.ToString())) {
                            itr.kind = (ItrKindEnum)int.Parse(value);
                            continue;
                        }

                        if (key.Equals(FrameKeyEnum.x.ToString())) {
                            itr.x = float.Parse(value);
                            continue;
                        }

                        if (key.Equals(FrameKeyEnum.y.ToString())) {
                            itr.y = float.Parse(value);
                            continue;
                        }

                        if (key.Equals(FrameKeyEnum.z.ToString())) {
                            itr.z = float.Parse(value);
                            continue;
                        }

                        if (key.Equals(FrameKeyEnum.w.ToString())) {
                            itr.w = float.Parse(value);
                            continue;
                        }

                        if (key.Equals(FrameKeyEnum.h.ToString())) {
                            itr.h = float.Parse(value);
                            continue;
                        }

                        if (key.Equals(FrameKeyEnum.zwidth.ToString())) {
                            itr.zwidth = float.Parse(value);
                            continue;
                        }

                        if (key.Equals(FrameKeyEnum.dvx.ToString())) {
                            itr.dvx = float.Parse(value);
                            continue;
                        }

                        if (key.Equals(FrameKeyEnum.dvy.ToString())) {
                            itr.dvy = float.Parse(value);
                            continue;
                        }

                        if (key.Equals(FrameKeyEnum.dvz.ToString())) {
                            itr.dvz = float.Parse(value);
                            continue;
                        }

                        if (key.Equals(FrameKeyEnum.arest.ToString())) {
                            itr.arest = float.Parse(value);
                            continue;
                        }

                        if (key.Equals(FrameKeyEnum.vrest.ToString())) {
                            itr.vrest = float.Parse(value);
                            continue;
                        }

                        if (key.Equals(FrameKeyEnum.injury.ToString())) {
                            itr.injury = int.Parse(value);
                            continue;
                        }

                        if (key.Equals(FrameKeyEnum.sound.ToString())) {
                            itr.sound = Resources.Load<AudioClip>(value);
                            continue;
                        }

                        if (key.Equals(FrameKeyEnum.confuse.ToString())) {
                            itr.confuse = float.Parse(value);
                            continue;
                        }

                        if (key.Equals(FrameKeyEnum.silence.ToString())) {
                            itr.silence = float.Parse(value);
                            continue;
                        }

                        if (key.Equals(FrameKeyEnum.slow.ToString())) {
                            itr.slow = float.Parse(value);
                            continue;
                        }

                        if (key.Equals(FrameKeyEnum.stun.ToString())) {
                            itr.stun = float.Parse(value);
                            continue;
                        }

                        if (key.Equals(FrameKeyEnum.ignite.ToString())) {
                            itr.ignite = float.Parse(value);
                            continue;
                        }

                        if (key.Equals(FrameKeyEnum.root.ToString())) {
                            itr.root = float.Parse(value);
                            continue;
                        }

                        if (key.Equals(FrameKeyEnum.charm.ToString())) {
                            itr.charm = float.Parse(value);
                            continue;
                        }

                        if (key.Equals(FrameKeyEnum.fear.ToString())) {
                            itr.fear = float.Parse(value);
                            continue;
                        }

                        if (key.Equals(FrameKeyEnum.taunt.ToString())) {
                            itr.taunt = float.Parse(value);
                            continue;
                        }

                        if (key.Equals(FrameKeyEnum.blind.ToString())) {
                            itr.blind = float.Parse(value);
                            continue;
                        }

                        if (key.Equals(FrameKeyEnum.paralysis.ToString())) {
                            itr.paralysis = float.Parse(value);
                            continue;
                        }

                        if (key.Equals(FrameKeyEnum.freeze.ToString())) {
                            itr.freeze = float.Parse(value);
                            continue;
                        }

                        continue;
                    }

                    frameData.itrs.Add(itr);
                }
#endregion

                if (frames.ContainsKey(frameData.id)) {
                    continue;
                }
                frames.Add(frameData.id, frameData);

#if (UNITY_EDITOR)
                listOfFramesContent.Add(frameData);
#endif
            }
        }
    }

}