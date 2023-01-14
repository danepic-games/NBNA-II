using UnityEngine;

public class AudioController : MonoBehaviour {
    public AudioSource audioSource;
    public FrameController frame;

    public bool playAudioOneTimePerFrame;

    public int currentFrameId;

    // Update is called once per frame
    void Update() {
        if (this.currentFrameId != this.frame.currentFrame.id) {
            this.playAudioOneTimePerFrame = true;
            this.currentFrameId = this.frame.currentFrame.id;
        }

        if (this.playAudioOneTimePerFrame) {
            if (this.frame.currentFrame.sound) {
                audioSource.clip = this.frame.currentFrame.sound;
                audioSource.Play();
            }
            this.playAudioOneTimePerFrame = false;
        }
    }
}
