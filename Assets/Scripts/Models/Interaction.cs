using Back.Model.Type;
using Model.Type;
using UnityEngine;
using Models;

namespace Model {
    [System.Serializable]
    public class Interaction {
        public Vector3 position;
        public Vector3 size;
        public InteractionKindEnum kind;
        public Vector3 force;
        public bool inverseForceDvx;
        public float damageRestTU;
        public bool ignoreDamageRestTU;
        public bool resetDamageRestTime;
        public string affectedAnimation;
        public string nextAnimation;
        public string nextIfPressedAttackAnimation;
        public int injury;
        public InteractionEffectEnum effect;
        public LevelDamageEnum level;
        public GameObject origin;
    }
}