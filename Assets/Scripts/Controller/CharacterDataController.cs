using System.Collections.Generic;
using System.Text.RegularExpressions;
using SerializableHelper;
using UnityEngine;

[System.Serializable]
public class CharacterDataController : AbstractDataController {

    [SerializeField]
    public HeaderData header;

    public float walking_speedz;
    public float running_speedz;
    public float walking_speed;

    private static int SPRITE_FILE_1 = 0;
    private static int SPRITE_FILE_2 = 64;
    private static int SPRITE_FILE_3 = 128;
    private static int SPRITE_FILE_4 = 192;
    private static int SPRITE_FILE_5 = 256;
    private static int SPRITE_FILE_6 = 320;
    private static int SPRITE_FILE_7 = 384;
    private static int SPRITE_FILE_8 = 448;

    void Awake() {
        base.type = ObjectTypeEnum.CHARACTER;

        var text_without_bmp_begin = base.dataFile.text.Replace("<bmp_begin>", "");

        string[] firstSplit = Regex.Split(text_without_bmp_begin, "<bmp_end>");

        var headerValue = firstSplit[0];
        this.header = new HeaderData();
        var headerRegex = new Regex("\n");

        var headerParams = headerRegex.Split(headerValue);

        var spriteFileNameHeaderParam = headerParams[(int)CharacterHeaderKeyEnum.SPRITE_FILE_NAME];
        var spriteFileNameValueParam = spriteFileNameHeaderParam.Split(':')[1];
        var spriteFileNameRegex = new Regex("Resources\\\\(.*\\\\)");
        this.header.sprite_file_name = spriteFileNameRegex.Split(spriteFileNameValueParam.Trim())[2].Replace(".png  w", "");

        this.header.sprite_folder = GetHeaderParam(headerParams, CharacterHeaderKeyEnum.SPRITE_FOLDER);

        this.sprites = new Map<int, Sprite>();
        var allSprites = Resources.LoadAll<Sprite>($"{header.sprite_folder}");

        foreach (Sprite sprite in allSprites) {
            var spriteKeyValue = sprite.name.Replace(this.header.sprite_file_name, "").Substring(1).Split("_");
            if (spriteKeyValue.Length == 1) {
                base.sprites.Add(SPRITE_FILE_1 + int.Parse(spriteKeyValue[0]), sprite);
                continue;
            }
            if (spriteKeyValue[0] == "2") {
                base.sprites.Add(SPRITE_FILE_2 + int.Parse(spriteKeyValue[1]), sprite);
                continue;
            }
            if (spriteKeyValue[0] == "3") {
                base.sprites.Add(SPRITE_FILE_3 + int.Parse(spriteKeyValue[1]), sprite);
                continue;
            }
            if (spriteKeyValue[0] == "4") {
                base.sprites.Add(SPRITE_FILE_4 + int.Parse(spriteKeyValue[1]), sprite);
                continue;
            }
            if (spriteKeyValue[0] == "5") {
                base.sprites.Add(SPRITE_FILE_5 + int.Parse(spriteKeyValue[1]), sprite);
                continue;
            }
            if (spriteKeyValue[0] == "6") {
                base.sprites.Add(SPRITE_FILE_6 + int.Parse(spriteKeyValue[1]), sprite);
                continue;
            }
            if (spriteKeyValue[0] == "7") {
                base.sprites.Add(SPRITE_FILE_7 + int.Parse(spriteKeyValue[1]), sprite);
                continue;
            }
            if (spriteKeyValue[0] == "8") {
                base.sprites.Add(SPRITE_FILE_8 + int.Parse(spriteKeyValue[1]), sprite);
                continue;
            }
        }

        this.header.name = base.GetHeaderParam(headerParams, CharacterHeaderKeyEnum.NAME, ':');

        this.header.start_hp = int.Parse(GetHeaderParam(headerParams, CharacterHeaderKeyEnum.START_HP));
        this.header.start_mp = int.Parse(GetHeaderParam(headerParams, CharacterHeaderKeyEnum.START_MP));

        this.header.agressive = int.Parse(GetHeaderParam(headerParams, CharacterHeaderKeyEnum.AGRESSIVE));
        this.header.technique = int.Parse(GetHeaderParam(headerParams, CharacterHeaderKeyEnum.TECHNIQUE));
        this.header.inteligent = int.Parse(GetHeaderParam(headerParams, CharacterHeaderKeyEnum.INTELIGENT));
        this.header.speed = int.Parse(GetHeaderParam(headerParams, CharacterHeaderKeyEnum.SPEED));
        this.header.resistence = int.Parse(GetHeaderParam(headerParams, CharacterHeaderKeyEnum.RESISTENCE));
        this.header.stamina = int.Parse(GetHeaderParam(headerParams, CharacterHeaderKeyEnum.STAMINA));
        this.header.on_fly = int.Parse(GetHeaderParam(headerParams, CharacterHeaderKeyEnum.ON_FLY));

        this.header.walking_speed = this.walking_speed;
        this.header.walking_speedz = this.walking_speedz;
        this.header.running_speed = StatsDefinitionUtil.Speed(this.header.speed);
        this.header.running_speedz = this.running_speedz;

        this.header.total_hp = StatsDefinitionUtil.TotalHP(this.header.resistence);
        this.header.total_mp = StatsDefinitionUtil.TotalHP(this.header.stamina);
        this.header.first_attack = StatsDefinitionUtil.Agressive(this.header.agressive);

        var framesValue = firstSplit[1];

        DataMapperUtil.MapDataToObject(framesValue, out this.frames, this.sprites, header.sprite_file_name);

        //Add composer properties to frame

        foreach (KeyValuePair<int, FrameData> frame in this.frames) {
            BodyData bodyNew;
            if (bodysComposer.TryGetValue(frame.Key, out bodyNew)) {
                var frameToUpdate = frame.Value.bodys[bodyNew.bodyNumber - 1];
                frameToUpdate.x = bodyNew.x;
                frameToUpdate.y = bodyNew.y;
                frameToUpdate.z = bodyNew.z;
                frameToUpdate.w = bodyNew.w;
                frameToUpdate.h = bodyNew.h;
                frameToUpdate.zwidth = bodyNew.zwidth;
                frameToUpdate.bodyNumber = bodyNew.bodyNumber;
            }
            if (bodysComposer2.TryGetValue(frame.Key, out bodyNew)) {
                var frameToUpdate = frame.Value.bodys[bodyNew.bodyNumber - 1];
                frameToUpdate.x = bodyNew.x;
                frameToUpdate.y = bodyNew.y;
                frameToUpdate.z = bodyNew.z;
                frameToUpdate.w = bodyNew.w;
                frameToUpdate.h = bodyNew.h;
                frameToUpdate.zwidth = bodyNew.zwidth;
                frameToUpdate.bodyNumber = bodyNew.bodyNumber;
            }
            if (bodysComposer3.TryGetValue(frame.Key, out bodyNew)) {
                var frameToUpdate = frame.Value.bodys[bodyNew.bodyNumber - 1];
                frameToUpdate.x = bodyNew.x;
                frameToUpdate.y = bodyNew.y;
                frameToUpdate.z = bodyNew.z;
                frameToUpdate.w = bodyNew.w;
                frameToUpdate.h = bodyNew.h;
                frameToUpdate.zwidth = bodyNew.zwidth;
                frameToUpdate.bodyNumber = bodyNew.bodyNumber;
            }

            InteractionData itrNew;
            if (interactionsComposer.TryGetValue(frame.Key, out itrNew)) {
                var frameToUpdate = frame.Value.itrs[itrNew.itrNumber - 1];
                frameToUpdate.x = itrNew.x;
                frameToUpdate.y = itrNew.y;
                frameToUpdate.z = itrNew.z;
                frameToUpdate.w = itrNew.w;
                frameToUpdate.h = itrNew.h;
                frameToUpdate.zwidthz = itrNew.zwidthz;
                frameToUpdate.itrNumber = itrNew.itrNumber;
            }
            if (interactionsComposer2.TryGetValue(frame.Key, out itrNew)) {
                var frameToUpdate = frame.Value.itrs[itrNew.itrNumber - 1];
                frameToUpdate.x = itrNew.x;
                frameToUpdate.y = itrNew.y;
                frameToUpdate.z = itrNew.z;
                frameToUpdate.w = itrNew.w;
                frameToUpdate.h = itrNew.h;
                frameToUpdate.zwidthz = itrNew.zwidthz;
                frameToUpdate.itrNumber = itrNew.itrNumber;
            }
            if (interactionsComposer3.TryGetValue(frame.Key, out itrNew)) {
                var frameToUpdate = frame.Value.itrs[itrNew.itrNumber - 1];
                frameToUpdate.x = itrNew.x;
                frameToUpdate.y = itrNew.y;
                frameToUpdate.z = itrNew.z;
                frameToUpdate.w = itrNew.w;
                frameToUpdate.h = itrNew.h;
                frameToUpdate.zwidthz = itrNew.zwidthz;
                frameToUpdate.itrNumber = itrNew.itrNumber;
            }

            ObjectPointData opointNew;
            if (opointsComposer.TryGetValue(frame.Key, out opointNew)) {
                var frameToUpdate = frame.Value.opoints[opointNew.opointNumber - 1];
                frameToUpdate.x = opointNew.x;
                frameToUpdate.y = opointNew.y;
                frameToUpdate.z = opointNew.z;
                frameToUpdate.opointNumber = opointNew.opointNumber;
            }
        }
    }


}
