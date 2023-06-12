using SerializableHelper;
using UnityEngine;

public class SpriteMapperUtil {

    private static int CHARACTER_SPRITE_FILE_1 = 0;
    private static int CHARACTER_SPRITE_FILE_2 = 64;
    private static int CHARACTER_SPRITE_FILE_3 = 128;
    private static int CHARACTER_SPRITE_FILE_4 = 192;
    private static int CHARACTER_SPRITE_FILE_5 = 256;
    private static int CHARACTER_SPRITE_FILE_6 = 320;
    private static int CHARACTER_SPRITE_FILE_7 = 384;
    private static int CHARACTER_SPRITE_FILE_8 = 448;

    public static Map<int, Sprite> SpriteToMapOfSprite(string spriteFolder, string spriteFileName) {
        Map<int, Sprite> result = new Map<int, Sprite>();
        var allSprites = Resources.LoadAll<Sprite>($"{spriteFolder}");
        foreach (Sprite sprite in allSprites) {
            var spriteKeyValue = sprite.name.Replace(spriteFileName, "").Substring(1).Split("_");
            result.Add(int.Parse(spriteKeyValue[0]), sprite);
        }
        return result;
    }

    public static Map<int, Sprite> CharacterSpriteToMapOfSprite(string spriteFolder, string spriteFileName) {
        Map<int, Sprite> sprites = new Map<int, Sprite>();
        var allSprites = Resources.LoadAll<Sprite>($"{spriteFolder}");

        foreach (Sprite sprite in allSprites) {
            var spriteKeyValue = sprite.name.Replace(spriteFileName, "").Substring(1).Split("_");
            if (spriteKeyValue.Length == 1) {
                sprites.Add(CHARACTER_SPRITE_FILE_1 + int.Parse(spriteKeyValue[0]), sprite);
                continue;
            }
            if (spriteKeyValue[0] == "2") {
                sprites.Add(CHARACTER_SPRITE_FILE_2 + int.Parse(spriteKeyValue[1]), sprite);
                continue;
            }
            if (spriteKeyValue[0] == "3") {
                sprites.Add(CHARACTER_SPRITE_FILE_3 + int.Parse(spriteKeyValue[1]), sprite);
                continue;
            }
            if (spriteKeyValue[0] == "4") {
                sprites.Add(CHARACTER_SPRITE_FILE_4 + int.Parse(spriteKeyValue[1]), sprite);
                continue;
            }
            if (spriteKeyValue[0] == "5") {
                sprites.Add(CHARACTER_SPRITE_FILE_5 + int.Parse(spriteKeyValue[1]), sprite);
                continue;
            }
            if (spriteKeyValue[0] == "6") {
                sprites.Add(CHARACTER_SPRITE_FILE_6 + int.Parse(spriteKeyValue[1]), sprite);
                continue;
            }
            if (spriteKeyValue[0] == "7") {
                sprites.Add(CHARACTER_SPRITE_FILE_7 + int.Parse(spriteKeyValue[1]), sprite);
                continue;
            }
            if (spriteKeyValue[0] == "8") {
                sprites.Add(CHARACTER_SPRITE_FILE_8 + int.Parse(spriteKeyValue[1]), sprite);
                continue;
            }
        }
        return sprites;
    }

}