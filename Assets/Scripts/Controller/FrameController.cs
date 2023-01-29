using System;
using TMPro;
using UnityEngine;

public class FrameController : MonoBehaviour {
    public AbstractDataController data;

    public SpriteRenderer spriteRenderer;

    [SerializeField]
    public FrameData currentFrame;

    public float wait;
    public bool facingRight;
    public int externAction = -1;

    // Running
    private float RUNNING_COUNT = 4f;

    public float runningRightCount;
    public bool runningRightEnable;
    public bool countRightEnable;

    public float runningLeftCount;
    public bool runningLeftEnable;
    public bool countLeftEnable;

    public float sideDashUpCount;
    public float sideDashDownCount;

    //Movement
    public Vector2 inputDirection;

    //Hit
    public bool hitJump;
    public bool hitDefense;
    public bool hitAttack;
    public bool hitTaunt;

    //Hold
    public bool holdForwardAfter;

    //Team
    public TeamEnum team;
    public int ownerId;

    //Debug
    public string frameToStopForDebug = null;

    public TextMeshPro timeText;

    // Start is called before the first frame update
    void Start() {
        if (this.currentFrame == null) {
            this.currentFrame = this.data.frames[0];
        }
        this.wait = 0;
        if (this.facingRight) {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        } else {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }
        this.runningRightCount = 0;
        this.runningLeftCount = 0;
        this.sideDashUpCount = 0;
        this.sideDashDownCount = 0;
        this.inputDirection = Vector2.zero;
        this.runningRightEnable = false;
        this.runningLeftEnable = false;
        this.countRightEnable = false;
        this.countLeftEnable = false;
        this.holdForwardAfter = false;
    }

    // Update is called once per frame
    void Update() {
#if UNITY_EDITOR
        if (frameToStopForDebug != null && frameToStopForDebug.Trim().Length > 0) {
            int idToStop;
            if (int.TryParse(frameToStopForDebug, out idToStop)) {
                if (idToStop == this.currentFrame.id) {
                    Debug.Break();
                }
            }
        }
#endif
        if (externAction >= 0) {
            this.ChangeFrame(externAction, false);
            return;
        }

        if (hitJump) {
            this.ChangeFrame(currentFrame.hit_jump, false);
            hitJump = false;
            return;
        }

        if (hitAttack) {
            this.ChangeFrame(currentFrame.hit_attack, false);
            hitAttack = false;
            return;
        }

        if (hitDefense) {
            this.ChangeFrame(currentFrame.hit_defense, false);
            hitDefense = false;
            return;
        }

        if (hitTaunt) {
            this.ChangeFrame(currentFrame.hit_taunt, false);
            hitTaunt = false;
            return;
        }

        if (wait == 0) {
            wait = this.currentFrame.wait / 30;

            var centery = Mathf.Abs(this.currentFrame.centery / this.currentFrame.pic.rect.height - 1);
            var centerx = this.currentFrame.centerx / this.currentFrame.pic.rect.width;

            var pivot = new Vector2(centerx, centery);
            this.spriteRenderer.sprite = Sprite.Create(this.currentFrame.pic.texture, this.currentFrame.pic.rect, pivot);
        }

        if (wait > 0) {
            wait -= Time.deltaTime;
        } else {
            if (currentFrame.hold_forward_after != null && holdForwardAfter) {
                this.ChangeFrame(currentFrame.hold_forward_after, false);
            } else {
                this.ChangeFrame(currentFrame.next);
            }
        }

        switch (this.data.type) {
            case ObjectTypeEnum.CHARACTER:
                RunningCounter();
                break;
        }
    }

    void DisplayTime(float timeToDisplay) {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void ChangeFrame(Nullable<int> frameToGo, bool usingNextPattern = true) {
        if (frameToGo == null) {
            return;
        }
        var nonNullFrameToGo = frameToGo ?? default(int);
        this.ChangeFrame(nonNullFrameToGo, usingNextPattern);
    }

    public void ChangeFrame(int frameToGo, bool usingNextPattern = true) {
        if (currentFrame.next == (int)FrameSpecialValuesEnum.DELETE) {
            UnityEngine.Object.Destroy(this.gameObject);
            return;
        }
        wait = 0;
        externAction = -1;
        if (usingNextPattern) {
            currentFrame = currentFrame.next == (int)FrameSpecialValuesEnum.BACK_TO_STANDING ? this.data.frames[0] : this.data.frames[frameToGo];
        } else {
            currentFrame = this.data.frames[frameToGo];
        }
    }

    public void ChangeFrame(CharacterSpecialStartFrameEnum next, bool usingNextPattern = true) {
        this.ChangeFrame((int)next, usingNextPattern);
    }

    public void Flip() {
        float xScaleFacing = 0;
        if (transform.localScale.x > 0) {
            xScaleFacing = -1;
            this.facingRight = false;
        } else {
            xScaleFacing = 1;
            this.facingRight = true;
        }
        transform.localScale = new Vector3(xScaleFacing, transform.localScale.y, transform.localScale.z);
    }

    public void Flip(Vector3 inputDirection) {
        if (!facingRight && inputDirection.x > 0) {
            this.Flip();
            return;
        }
        if (facingRight && inputDirection.x < 0) {
            this.Flip();
            return;
        }
    }

    private void RunningCounter() {
        if (countRightEnable && !runningRightEnable) {
            runningRightEnable = true;
            runningRightCount = RUNNING_COUNT / 30;
            runningLeftCount = 0;
        }
        if (runningRightCount > 0) {
            runningRightCount -= Time.deltaTime;
        } else {
            runningRightCount = 0;
            runningRightEnable = false;
            countRightEnable = false;
        }


        if (countLeftEnable && !runningLeftEnable) {
            runningLeftEnable = true;
            runningLeftCount = RUNNING_COUNT / 30;
            runningRightCount = 0;
        }
        if (runningLeftCount > 0) {
            runningLeftCount -= Time.deltaTime;
        } else {
            runningLeftCount = 0;
            runningLeftEnable = false;
            countLeftEnable = false;
        }
    }
}
