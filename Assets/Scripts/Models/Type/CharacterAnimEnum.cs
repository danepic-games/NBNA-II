using System;

namespace Back.Model.Type {
    public enum CharacterAnimEnum {
        Standing,
        Walking,
        Running,
        Running2,
        Defense,
        JumpDefense,
        DefenseMovementDebug,
        JumpDefenseMovementDebug,
        StopRunning,
        SideDash,
        Crouch,
        Jumping3,
        Jumping4,
        JumpingDash3,
        JumpingDash4,
        Jumping3WithCombo,
        Jumping4WithCombo,
        JumpingFrontBackDash,
        Punch,
    }

    static class Extensions {

        public static string Name(this CharacterAnimEnum animEnum) {
            return animEnum.ToString();
        }
    }
}