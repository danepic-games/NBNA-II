using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class AbstractDataController : MonoBehaviour {

    [SerializeField]
    protected TextAsset dataFile;

    public ObjectTypeEnum type;

    [SerializeField]
    protected List<FrameData> listOfFramesContent;

    public Dictionary<int, FrameData> frames;

    [SerializeField]
    protected Dictionary<string, Sprite> sprites;

    protected string GetHeaderParam(string[] headerParams, CharacterHeaderKeyEnum paramKey, char separator = ' ') {
        var headerParam = headerParams[(int)paramKey];
        var valueParam = headerParam.Split(separator)[1];
        return valueParam.Trim();
    }
}
