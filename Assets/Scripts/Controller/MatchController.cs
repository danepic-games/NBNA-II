using UnityEngine;

public class MatchController : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        int CHARACTER_LAYER = LayerMask.NameToLayer("Character");
        int WEAPON_LAYER = LayerMask.NameToLayer("Weapon");
        int EFFECT_LAYER = LayerMask.NameToLayer("Effect");

        Physics.IgnoreLayerCollision(CHARACTER_LAYER, CHARACTER_LAYER);
        Physics.IgnoreLayerCollision(CHARACTER_LAYER, WEAPON_LAYER);
        Physics.IgnoreLayerCollision(CHARACTER_LAYER, EFFECT_LAYER);
        Physics.IgnoreLayerCollision(WEAPON_LAYER, EFFECT_LAYER);
        Physics.IgnoreLayerCollision(WEAPON_LAYER, WEAPON_LAYER);
        Physics.IgnoreLayerCollision(EFFECT_LAYER, EFFECT_LAYER);
    }

    // Update is called once per frame
    void Update() {

    }
}
