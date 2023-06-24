using UnityEngine;

[System.Serializable]
public class ObjectPointCache {

    public string key;

    public GameObject gameObject;

    public PhysicController physicController;

    public FrameController frameController;

    public Vector3 originalPosition;
}
