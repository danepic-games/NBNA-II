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

    public Nullable<int> hit_attack;
    public Nullable<int> hit_defense;
    public Nullable<int> hit_jump;
    public Nullable<int> hit_power;
    public Nullable<int> hit_super;
    public Nullable<int> hit_taunt;

    public Nullable<int> hit_jump_defense;
    public Nullable<int> hit_defense_attack;
    public Nullable<int> hit_defense_power;

    public Nullable<int> hit_power_attack_up;
    public Nullable<int> hit_power_attack_forward;
    public Nullable<int> hit_power_attack_down;

    public Nullable<int> hit_power_defense_up;
    public Nullable<int> hit_power_defense_forward;
    public Nullable<int> hit_power_defense_down;

    public Nullable<int> hit_power_jump_up;
    public Nullable<int> hit_power_jump_forward;
    public Nullable<int> hit_power_jump_down;

    public Nullable<int> hold_forward_after;
    public Nullable<int> hold_defense_after;
    public Nullable<int> hold_power_after;

    public int mp;
    public int hp;
    public AudioClip sound;

    public Nullable<int> hit_ground = null;
    public bool hidden;
    public Nullable<int> hit_off_ground;

    public List<BodyData> bodys = new List<BodyData>();

    public List<ObjectPointData> opoints = new List<ObjectPointData>();

    public List<InteractionData> itrs = new List<InteractionData>();
}
