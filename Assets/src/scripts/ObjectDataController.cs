using System.Collections.Generic;
using SerializableHelper;
using UnityEngine;

[System.Serializable]
public class DataController : MonoBehaviour {
    [SerializeField]
    public TextAsset dataFile;

    [SerializeField]
    public HeaderData header;

    [SerializeField]
    public ObjectTypeEnum type;

    [SerializeField]
    public Map<int, FrameData> frames;

    [SerializeField]
    public Map<int, BodyData> bodysComposer;

    [SerializeField]
    public Map<int, ObjectPointData> opointsComposer;

    [SerializeField]
    public Map<int, InteractionData> interactionsComposer;

    [SerializeField]
    protected Map<int, Sprite> sprites;

    [SerializeField]
    public string assetPath;

    public string GetHeaderParam(string[] headerParams, CharacterHeaderKeyEnum paramKey, char separator = ' ') {
        return this.GetHeaderParam(headerParams, (int)paramKey, separator);
    }

    public string GetHeaderParam(string[] headerParams, EffectHeaderKeyEnum paramKey, char separator = ' ') {
        return this.GetHeaderParam(headerParams, (int)paramKey, separator);
    }

    public string GetHeaderParam(string[] headerParams, PowerHeaderKeyEnum paramKey, char separator = ' ') {
        return this.GetHeaderParam(headerParams, (int)paramKey, separator);
    }

    public string GetHeaderParam(string[] headerParams, int paramKey, char separator = ' ') {
        var headerParam = headerParams[paramKey];
        var valueParam = headerParam.Split(separator)[1];
        
        return valueParam.Trim();
    }
}
