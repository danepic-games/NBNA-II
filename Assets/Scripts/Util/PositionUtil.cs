using UnityEngine;

public class PositionUtil {

    public static float FacingByX (float x, bool facingRight) {
        if (!facingRight) {
            if (x < 0) {
                return Mathf.Abs(x);
            } else {
                return -x;
            }
        }
        return x;
    }
}