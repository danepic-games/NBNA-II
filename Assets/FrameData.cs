using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FrameData {
    public int id;
    public string name;
    public Sprite pic;
    public StateFrameEnum state;
    public float wait;
    public int next;
    public float dvx;
    public float dvy;
    public float dvz;
    public float dx;
    public float dy;
    public float dz;
    public int centerx;
    public int centery;

    public int hit_attack;
    public int hit_defense;
    public int hit_jump;
    public int hit_power;

    public int hit_power_attack_up;
    public int hit_power_attack_forward;
    public int hit_power_attack_down;

    public int hit_power_defense_up;
    public int hit_power_defense_forward;
    public int hit_power_defense_down;

    public int hit_power_jump_up;
    public int hit_power_jump_forward;
    public int hit_power_jump_down;

    public int mp;
    public int hp;
    public AudioClip sound;

    public Nullable<int> hit_ground = null;
    public bool hidden;
    public int hit_off_ground;

    public List<BodyData> bodys = new List<BodyData>();

    public List<ObjectPointData> opoints = new List<ObjectPointData>();

    public List<InteractionData> itrs = new List<InteractionData>();
}
