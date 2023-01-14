using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        camera.transparencySortMode = TransparencySortMode.Orthographic;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
