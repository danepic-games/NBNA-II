using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class CharacterDataController : AbstractDataController {

    [SerializeField]
    public CharacterHeaderData header;

    void Awake() {

        var text_without_bmp_begin = base.dataFile.text.Replace("<bmp_begin>", "");

        string[] firstSplit = Regex.Split(text_without_bmp_begin, "<bmp_end>");

        var headerValue = firstSplit[0];
        this.header = new CharacterHeaderData();
        var headerRegex = new Regex("\n");

        var headerParams = headerRegex.Split(headerValue);

        var spriteFileNameHeaderParam = headerParams[(int)CharacterHeaderKeyEnum.SPRITE_FILE_NAME];
        var spriteFileNameValueParam = spriteFileNameHeaderParam.Split(':')[1];
        var spriteFileNameRegex = new Regex("Resources\\\\(.*\\\\)");
        this.header.sprite_file_name = spriteFileNameRegex.Split(spriteFileNameValueParam.Trim())[2].Replace(".png  w", "");

        this.header.sprite_folder = GetHeaderParam(headerParams, CharacterHeaderKeyEnum.SPRITE_FOLDER);

        this.sprites = new Dictionary<string, Sprite>();
        var allSprites = Resources.LoadAll<Sprite>($"{header.sprite_folder}");
        foreach (Sprite sprite in allSprites) {
            base.sprites.Add(sprite.name, sprite);
        }

        this.header.name = base.GetHeaderParam(headerParams, CharacterHeaderKeyEnum.NAME, ':');
        this.header.walking_speed = float.Parse(GetHeaderParam(headerParams, CharacterHeaderKeyEnum.WALKING_SPEED));
        this.header.walking_speedz = float.Parse(GetHeaderParam(headerParams, CharacterHeaderKeyEnum.WALKING_SPEEDZ));
        this.header.running_speed = float.Parse(GetHeaderParam(headerParams, CharacterHeaderKeyEnum.RUNNING_SPEED));
        this.header.running_speedz = float.Parse(GetHeaderParam(headerParams, CharacterHeaderKeyEnum.RUNNING_SPEEDZ));

        this.header.start_hp = int.Parse(GetHeaderParam(headerParams, CharacterHeaderKeyEnum.START_HP));
        this.header.start_mp = int.Parse(GetHeaderParam(headerParams, CharacterHeaderKeyEnum.START_MP));
        this.header.total_hp = int.Parse(GetHeaderParam(headerParams, CharacterHeaderKeyEnum.TOTAL_HP));
        this.header.total_mp = int.Parse(GetHeaderParam(headerParams, CharacterHeaderKeyEnum.TOTAL_MP));

        this.header.agressive = int.Parse(GetHeaderParam(headerParams, CharacterHeaderKeyEnum.AGRESSIVE));
        this.header.technique = int.Parse(GetHeaderParam(headerParams, CharacterHeaderKeyEnum.TECHNIQUE));
        this.header.inteligent = int.Parse(GetHeaderParam(headerParams, CharacterHeaderKeyEnum.INTELIGENT));
        this.header.speed = int.Parse(GetHeaderParam(headerParams, CharacterHeaderKeyEnum.SPEED));
        this.header.resistence = int.Parse(GetHeaderParam(headerParams, CharacterHeaderKeyEnum.RESISTENCE));
        this.header.stamina = int.Parse(GetHeaderParam(headerParams, CharacterHeaderKeyEnum.STAMINA));
        this.header.on_fly = int.Parse(GetHeaderParam(headerParams, CharacterHeaderKeyEnum.ON_FLY));

        var framesValue = firstSplit[1];

        DataMapperUtil.MapDataToObject(framesValue, out this.frames, this.sprites, header.sprite_file_name, out listOfFramesContent);
    }
}
