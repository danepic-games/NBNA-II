using Back.Model.Type;
using Model.Type;
using UnityEngine;
using Models;

namespace Model {
    [System.Serializable]
    public class Core {
        public int frameCount;
        public StateEnum state;
        public string anim;
        public HurtboxEnum hurtBoxType;

        public int recoverHP;
        public int usageHP;
        public int recoverMP;
        public int usageMP;

        public bool resetRunningInterval = false;
        public bool resetCombinations = false;
        public bool resetAnimation = false;

        public bool isWalkingEnabled;
        public bool isRunningEnabled;
        public bool isSideDashEnabled;
        public bool isInjured;

        //Debug
        public bool pauseBreak;

        public string touchHurtBoxNextAnim;

        public string nextAnim;

        public AudioClip audio;
    }
}