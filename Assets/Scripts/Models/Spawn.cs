using System;
using Back.Model.Type;
using UnityEngine;
using Models;

namespace Model {
    [Serializable]
    public class Spawn {
        public Vector3 position;
        public int quantity;
        public DirectionEnum direction;
        public GameObject obj;
        public bool flipX;

        //Start frame of class; empty is default.
        public string startAnim;

        //Use position of spawn opoint with relative position of owner
        public bool useOwnerRelativePosition;

        //Destroy obj oppoint in the specified frame of owner
        public string destroyInOwnerAnimation;

        //Enable disable opoint if a frame where the owner check "Disable Opoint" field.
        public bool disableOpoint;

        //Enable direction up/down for opoint
        public bool enableZDirection;

        public bool useSharedOpointPosition;
    }
}