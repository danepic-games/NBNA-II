using SerializableHelper;
using UnityEngine;

public class ComposerUtil {

    public static int GetTypeFromAbstract(ObjectTypeEnum type) {
        switch (type) {
            case ObjectTypeEnum.CHARACTER:
                return (int)CharacterHeaderKeyEnum.SPRITE_FILE_NAME;
            case ObjectTypeEnum.EFFECT:
                return (int)EffectHeaderKeyEnum.SPRITE_FILE_NAME;
            case ObjectTypeEnum.POWER:
                return (int)PowerHeaderKeyEnum.SPRITE_FILE_NAME;
            default:
                return 0;
        }
    }

    public static Map<int, Sprite> GetSpriteMapper(AbstractDataController dataController) {
        if (dataController.type == ObjectTypeEnum.CHARACTER) {
            return SpriteMapperUtil.CharacterSpriteToMapOfSprite(dataController.header.sprite_folder, dataController.header.sprite_file_name);
        }
        return SpriteMapperUtil.SpriteToMapOfSprite(dataController.header.sprite_folder, dataController.header.sprite_file_name);
    }
}