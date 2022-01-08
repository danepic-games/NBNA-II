using System;

namespace Model {
    [Serializable]
    public class Physic {
        public float dvx;
        public float dvy;
        public float dvz;
        public bool teleportToEnemy;
        public bool teleportToAlly;
        public bool hasHorizontalMovement; //Get value
        public bool resetInertiaMoveHorizontal = false;
        public bool useHorizontalInertia; //Lock value used
        public bool hasVerticalMovement; //Get value
        public bool useVerticalInertia; //Lock value used
        public bool useConstantGravity; //Lock gravity value used
        public bool lockDirectionForce;
        public bool useEnemyForceInNextFrame;
        public bool stopGravity;
        public bool resetExternalGravityForce;
        public bool resetExternalForce;

        public float movementValueFixedVertical;
        public bool enableMovementFixedVertical;//enable movementValueFixedVertical

        public float externalForceX;
        public float externalForceY;
        public float externalForceZ;
    }
}