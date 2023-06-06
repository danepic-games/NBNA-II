using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

[System.Serializable]
public class CharacterDataController : AbstractDataController {

    public float walking_speedz;
    public float running_speedz;
    public float walking_speed;

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

        this.sprites = SpriteMapperUtil.CharacterSpriteToMapOfSprite(this.header.sprite_folder, this.header.sprite_file_name);

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
                bodyNew.bodyNumber = 1;
                frame.Value.bodys.Add(bodyNew);
            }
            if (bodysComposer2.TryGetValue(frame.Key, out bodyNew)) {
                bodyNew.bodyNumber = 2;
                frame.Value.bodys.Add(bodyNew);
            }
            if (bodysComposer3.TryGetValue(frame.Key, out bodyNew)) {
                bodyNew.bodyNumber = 3;
                frame.Value.bodys.Add(bodyNew);
            }

            InteractionData itrNew;
            if (interactionsComposer.TryGetValue(frame.Key, out itrNew)) {
                itrNew.itrNumber = 1;
                frame.Value.itrs.Add(itrNew);
            }
            if (interactionsComposer2.TryGetValue(frame.Key, out itrNew)) {
                itrNew.itrNumber = 2;
                frame.Value.itrs.Add(itrNew);
            }
            if (interactionsComposer3.TryGetValue(frame.Key, out itrNew)) {
                itrNew.itrNumber = 3;
                frame.Value.itrs.Add(itrNew);
            }

            ObjectPointData opointNew;
            if (opointsComposer.TryGetValue(frame.Key, out opointNew)) {
                opointNew.opointNumber = 1;
                frame.Value.opoints.Add(opointNew);
            }
            if (opointsComposer2.TryGetValue(frame.Key, out opointNew)) {
                opointNew.opointNumber = 2;
                frame.Value.opoints.Add(opointNew);
            }
            if (opointsComposer3.TryGetValue(frame.Key, out opointNew)) {
                opointNew.opointNumber = 3;
                frame.Value.opoints.Add(opointNew);
            }
        }
    }


}
