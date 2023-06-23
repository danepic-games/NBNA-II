using Model;

[System.Serializable]
public class BodyData : Data {
    public BodyKindEnum kind;
    public float x;
    public float y;
    public float z;
    public float w;
    public float h;
    public float zwidth;
    public bool wallCheck;
    public int frameId;
    public int beginFrameId;
    public int endFrameId;
    public bool hasValue = false;

    public bool HasValue() {
        return hasValue;
    }
}
