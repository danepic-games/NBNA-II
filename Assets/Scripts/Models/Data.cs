namespace Models {
    [System.Serializable]
    public class Data {
        public HeaderData headerData;
        public Frame[] standing;
        public Frame[] walking;
        public Frame[] punch;
        public Frame[] simpleDash;
        public Frame[] running;
        public Frame[] stopRunning;
        public Frame[] jumping;
        public Frame[] fallJumping;
        public Frame[] crouch;
        public Frame[] runningPunch;
        public Frame[] runningDash;
        public Frame[] taunt;
        public Frame[] jumpingDash;
        public Frame[] stopCharge;
        public Frame[] charge;
        public Frame[] startCharge;
        public Frame[] sideDash;
    }
}