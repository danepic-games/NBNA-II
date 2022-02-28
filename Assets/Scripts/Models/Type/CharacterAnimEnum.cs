namespace Back.Model.Type {
    public enum CharacterAnimEnum {
        Standing,
        Walking,
        SimpleDash,
        Running,
        Defense,
        JumpDefense,
        DefenseMovementDebug,
        JumpDefenseMovementDebug,
        StopRunning,
        SideDash,
        Crouch,
        Jumping,
        FallJumping,
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