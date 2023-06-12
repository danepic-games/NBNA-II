using System.Text.RegularExpressions;
using SerializableHelper;
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

        this.sprites = SpriteMapperUtil.SpriteToMapOfSprite(this.header.sprite_folder, this.header.sprite_file_name);

        Debug.Log(sprites.Count);

        var framesValue = firstSplit[1];

        DataMapperUtil.MapDataToObject(framesValue, out this.frames, this.sprites, header.sprite_file_name);
    }
}
