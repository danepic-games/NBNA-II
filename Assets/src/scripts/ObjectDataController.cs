using SerializableHelper;
using Unity.Collections;
using UnityEngine;

[System.Serializable]
public class ObjectDataController : MonoBehaviour {
    [SerializeField]
    public TextAsset dataFile;

    [ReadOnly]
    [SerializeField]
    private HeaderData header;

    [ReadOnly]
    [SerializeField]
    private ObjectTypeEnum type;

    [SerializeField]
    private Map<int, FrameData> frames;

    [SerializeField]
    private Map<int, Sprite> sprites;
}
