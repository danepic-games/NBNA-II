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
    }
}