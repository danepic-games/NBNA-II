using Model;

[System.Serializable]
public class ObjectPointData : Data {
    public ObjectPointKindEnum kind;
    public float x;
    public float y;
    public float z;
    public int action;
    public float dvx;
    public float dvy;
    public float dvz;
    public string object_id;
    public bool facing;
    public int quantity;
    public float z_division_per_quantity;
    public bool enable_dvz_invocation;

    public int frameId;
    public bool hasValue = false;

    public bool HasValue() {
        return hasValue;
    }
}
