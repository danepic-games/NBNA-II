using Model;
using UnityEngine;

[System.Serializable]
public class InteractionData : Data {
    public ItrKindEnum kind;
    public float x;
    public float y;
    public float z;
    public float w;
    public float h;
    public float zwidthz;
    public float dvx;
    public float dvy;
    public float dvz;

    public float damageRest;
    public bool applyForEntireFrame;

    public int action;
    public int power;
    public bool defensable;
    public int injury;
    public int nextIfHit;

    public AudioClip sound;
    public float confuse;
    public float silence;
    public float slow;
    public float stun;
    public float ignite;
    public float poison;
    public float root;
    public float charm;
    public float fear;
    public float taunt;
    public float blind;
    public float paralysis;
    public float freeze;

    public int frameId;
    public bool hasValue = false;

    public bool HasValue() {
        return hasValue;
    }
}
