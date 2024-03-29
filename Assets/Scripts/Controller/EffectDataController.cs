using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class EffectDataController : AbstractDataController {

    void Awake() {
        base.type = ObjectTypeEnum.EFFECT;

        var text_without_bmp_begin = dataFile.text.Replace("<bmp_begin>", "");

        string[] firstSplit = Regex.Split(text_without_bmp_begin, "<bmp_end>");

        var headerValue = firstSplit[0];
        this.header = new HeaderData();
        var headerRegex = new Regex("\n");

        var headerParams = headerRegex.Split(headerValue);

        var spriteFileNameHeaderParam = headerParams[(int)EffectHeaderKeyEnum.SPRITE_FILE_NAME];
        var spriteFileNameValueParam = spriteFileNameHeaderParam.Split(':')[1];
        var spriteFileNameRegex = new Regex("Resources\\\\(.*\\\\)");
        this.header.sprite_file_name = spriteFileNameRegex.Split(spriteFileNameValueParam.Trim())[2].Replace(".png  w", "");

        this.header.sprite_folder = GetHeaderParam(headerParams, EffectHeaderKeyEnum.SPRITE_FOLDER);
        this.header.invoke_limit = int.Parse(GetHeaderParam(headerParams, EffectHeaderKeyEnum.INVOKE_LIMIT));

        this.sprites = SpriteMapperUtil.SpriteToMapOfSprite(this.header.sprite_folder, this.header.sprite_file_name);

        var framesValue = firstSplit[1];

        DataMapperUtil.MapDataToObject(framesValue, out this.frames, this.sprites, header.sprite_file_name);

        //Add composer properties to frame

        foreach (KeyValuePair<int, FrameData> frame in this.frames) {
            BodyData bodyNew;
            if (bodysComposer.TryGetValue(frame.Key, out bodyNew)) {
                bodyNew.hasValue = true;
                frame.Value.body = bodyNew;
            }

            InteractionData itrNew;
            if (interactionsComposer.TryGetValue(frame.Key, out itrNew)) {
                itrNew.hasValue = true;
                frame.Value.itr = itrNew;
            }

            ObjectPointData opointNew;
            if (opointsComposer.TryGetValue(frame.Key, out opointNew)) {
                opointNew.hasValue = true;
                frame.Value.opoint = opointNew;
            }
        }
    }
}
