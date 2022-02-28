using Model;
using UnityEngine;

namespace Models {
    [System.Serializable]
    public class Frame {
        public string name;
        public int id;
        public Core core;
        public Body[] bodies;
        public Flip flip;
        public Spawn[] spawns;
        public Physic physic;
        public ChildObjects childObjects;
        public Trigger trigger;
        public Combination combination;
        public Interaction[] interactions;
    }
}