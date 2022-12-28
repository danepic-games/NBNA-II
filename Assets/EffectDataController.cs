using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class EffectDataController : MonoBehaviour {

    [SerializeField]
    private TextAsset dataFile;

    public ObjectTypeEnum type;

    [SerializeField]
    public EffectHeaderData header;

    [SerializeField]
    private List<FrameData> listOfFramesContent;

    public Dictionary<int, FrameData> frames;

    [SerializeField]
    private Dictionary<string, Sprite> sprites;

    void Awake() {

        var text_without_bmp_begin = dataFile.text.Replace("<bmp_begin>", "");

        string[] firstSplit = Regex.Split(text_without_bmp_begin, "<bmp_end>");

        var headerValue = firstSplit[0];
        this.header = new EffectHeaderData();
        var headerRegex = new Regex("\n");

        var headerParams = headerRegex.Split(headerValue);

        var spriteFileNameHeaderParam = headerParams[(int)EffectHeaderKeyEnum.SPRITE_FILE_NAME];
        var spriteFileNameValueParam = spriteFileNameHeaderParam.Split(':')[1];
        var spriteFileNameRegex = new Regex("Resources\\\\(.*\\\\)");
        this.header.sprite_file_name = spriteFileNameRegex.Split(spriteFileNameValueParam.Trim())[2].Replace(".png  w", "");

        this.header.sprite_folder = GetHeaderParam(headerParams, CharacterHeaderKeyEnum.SPRITE_FOLDER);

        this.sprites = new Dictionary<string, Sprite>();
        var allSprites = Resources.LoadAll<Sprite>($"{header.sprite_folder}");
        foreach (Sprite sprite in allSprites) {
            this.sprites.Add(sprite.name, sprite);
        }

        this.header.type = type;

        var framesValue = firstSplit[1];

        this.MapDataToObject(framesValue);
    }

    private void MapDataToObject(string framesValue) {
        var listFrames = Regex.Split(framesValue, "<frame_end>");

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
                            if (this.sprites.ContainsKey($"{this.header.sprite_file_name}_{value}")) {
                                frameData.pic = this.sprites[$"{this.header.sprite_file_name}_{value}"];
                            } else {
                                frameData.pic = null;
                            }
                            continue;
                        }
                        if (key.Equals(FrameKeyEnum.state.ToString())) {
                            frameData.state = (CharacterStateFrameEnum)int.Parse(value);
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
#endregion
                }

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


    private string GetHeaderParam(string[] headerParams, CharacterHeaderKeyEnum paramKey, char separator = ' ') {
        var headerParam = headerParams[(int)paramKey];
        var valueParam = headerParam.Split(separator)[1];
        return valueParam.Trim();
    }
}
