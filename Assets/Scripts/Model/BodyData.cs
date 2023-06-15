using UnityEngine;

[System.Serializable]
public class BodyData {
    public BodyKindEnum kind;
    public float x;
    public float y;
    public float z;
    public float w;
    public float h;
    public float zwidth;
    public bool wallCheck;
    public int bodyNumber;
    public int frameId;
    public int beginFrameId;
    public int endFrameId;
}
