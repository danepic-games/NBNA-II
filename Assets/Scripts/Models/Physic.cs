using System;
using UnityEngine;

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

        public float inertiaHorizontal;
        public float inertiaVertical;
        public bool lockInertiaHorizontal;
        public bool lockInertiaVertical;
        public bool freeMovementAllSidesInertiaHorizontalValue;
        public bool freeMovementAllSidesInertiaVerticalValue;

        public float movementValueFixedVertical;
        public bool enableMovementFixedVertical;//enable movementValueFixedVertical

        public float externalForceX;
        public float externalForceY;
        public float externalForceZ;

        public Vector3 customGroundDetectionPosition;
        public Vector3 customGroundDetectionSize;

        public Vector3 Force() {
            return new Vector3(dvx, dvy, dvz);
        }
    }
}