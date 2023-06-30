using System;
using TMPro;
using UnityEngine;

using Random = UnityEngine.Random;

public class FrameController : MonoBehaviour
{
    public AbstractDataController data;

    public SpriteRenderer spriteRenderer;

    [SerializeField]
    public FrameData currentFrame;

    //Debug
    public bool debugFrameToGo;
    public string frameToStopForDebug = null;
    public int previousId;

    public float repeatAnimationAt;
    public bool repeatAnimationStart;
    public bool repeatAnimationReady;

    public float wait;
    public int currentHp;
    public int currentMp;
    public bool facingRight;
    public bool facingUp;

    // Cache
    public bool isCache = false;

    // Extern Interaction
    public int summonAction;

    // Extern Interaction
    public bool externAction;
    public InteractionData externItr;
    public bool enemyFacingRight;

    public bool attacked;
    public bool wasAttacked;

    // Running
    private float RUNNING_COUNT = 4f;
    private float SIDE_DASH_COUNT = 4f;

    public float runningRightCount;
    public bool runningRightEnable;
    public bool countRightEnable;

    public float runningLeftCount;
    public bool runningLeftEnable;
    public bool countLeftEnable;

    // Side Dash
    public float sideDashUpCount;
    public bool sideDashUpEnable;
    public bool countSideDashUpEnable;

    public float sideDashDownCount;
    public bool sideDashDownEnable;
    public bool countSideDashDownEnable;

    //Movement
    public Vector2 inputDirection;

    //Hit
    public bool hitJump;
    public bool hitDefense;
    public bool holdDefense;
    public bool hitAttack;
    public bool hitTaunt;
    public bool hitPower;

    //Hold
    public bool holdForwardAfter;
    public bool holdDefenseAfter;
    public bool holdPowerAfter;

    //Team
    public TeamEnum team;
    public int ownerId;
    public int selfId;
    public ObjectPointController ownerOpointController;
    public ObjectPointCache objectPointCache;

    //Injured
    public int injuredCount;
    public static int INJURED_COUNT_LIMIT = 5;
    public bool injuredCountOneTimePerState;

    public TextMeshPro timeText;

    // Start is called before the first frame update
    void Start()
    {
        if (this.currentFrame == null)
        {
            this.currentFrame = this.data.frames[0];
        }

        if (ownerOpointController != null)
        {
            this.ResetValues(ownerOpointController.frame.facingRight);
        }

        this.currentHp = data.header.start_hp;
        this.currentMp = data.header.start_mp;
        this.selfId = GetInstanceID();

        if (isCache)
        {
            gameObject.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (frameToStopForDebug != null && frameToStopForDebug.Trim().Length > 0)
        {
            int idToStop;
            if (int.TryParse(frameToStopForDebug, out idToStop))
            {
                if (idToStop == this.currentFrame.id)
                {
                    Debug.Break();
                }
            }
        }
#endif

        if (debugFrameToGo)
        {
            //            Debug.Log(name + " : " + previousId + " -> " + currentFrame.id);
        }

        if (this.currentFrame.state == StateFrameEnum.NONE)
        {
            return;
        }

        if (externAction)
        {
            if (externItr.action >= 0)
            {
                if (this.data.type == ObjectTypeEnum.CHARACTER)
                {
                    if (externItr.defensable && currentFrame.state == StateFrameEnum.DEFEND)
                    {
                        this.currentHp -= externItr.injury / 3;
                        this.ChangeFrame(CharacterSpecialStartFrameEnum.HIT_DEFENSE, false);
                    }
                    else if (externItr.defensable && currentFrame.state == StateFrameEnum.JUMP_DEFEND)
                    {
                        this.currentHp -= externItr.injury / 3;
                        this.ChangeFrame(CharacterSpecialStartFrameEnum.HIT_JUMP_DEFENSE, false);
                    }
                    else
                    {
                        int externActionUpdate = externItr.action;
                        if (externItr.action == (int)FrameSpecialValuesEnum.INJURED_RANDOM)
                        {
                            externActionUpdate = Random.value > 0.5f ? (int)CharacterSpecialStartFrameEnum.INJURED_1 : (int)CharacterSpecialStartFrameEnum.INJURED_2;
                        }
                        this.ChangeFrame(externActionUpdate, false);
                        this.currentHp -= externItr.injury;
                    }

                    if (this.currentHp <= 0)
                    {
                        this.ChangeFrame(CharacterSpecialStartFrameEnum.DEATH, false);
                    }
                }
                else if (this.data.type == ObjectTypeEnum.POWER)
                {
                    this.currentHp -= externItr.injury;

                    if (this.currentHp <= 0)
                    {
                        this.ChangeFrame(CharacterSpecialStartFrameEnum.DEATH, false);
                    }
                }
                externAction = false;
            }
            return;
        }

        if (summonAction >= 0)
        {
            this.ChangeFrame(summonAction, false);
            return;
        }

        this.ApplyInjuredCount();

        if (hitJump && hitDefense)
        {
            this.ChangeFrame(currentFrame.hit_jump_defense, false);
            hitJump = false;
            hitDefense = false;
            return;
        }

        if (hitDefense && hitAttack)
        {
            this.ChangeFrame(currentFrame.hit_defense_attack, false);
            hitDefense = false;
            hitAttack = false;
            return;
        }

        if (hitPower && hitDefense)
        {
            this.ChangeFrame(currentFrame.hit_defense_power, false);
            hitPower = false;
            hitDefense = false;
            return;
        }

        if (hitJump)
        {
            this.ChangeFrame(currentFrame.hit_jump, false);
            hitJump = false;
            return;
        }

        if (hitAttack)
        {
            this.ChangeFrame(currentFrame.hit_attack, false);
            hitAttack = false;
            return;
        }

        if (hitPower)
        {
            this.ChangeFrame(currentFrame.hit_power, false);
            hitPower = false;
            return;
        }

        if (hitDefense)
        {
            this.ChangeFrame(currentFrame.hit_defense, false);
            hitDefense = false;
            return;
        }

        if (hitTaunt)
        {
            this.ChangeFrame(currentFrame.hit_taunt, false);
            hitTaunt = false;
            return;
        }

        if (repeatAnimationReady)
        {
            this.ChangeFrame(currentFrame.repeat_next, false);
            repeatAnimationReady = false;
            return;
        }

        if (repeatAnimationAt > 0)
        {
            repeatAnimationAt -= Time.deltaTime;
            repeatAnimationStart = true;
        }
        else
        {
            repeatAnimationAt = 0;

            if (repeatAnimationStart)
            {
                repeatAnimationReady = true;
                repeatAnimationStart = false;
            }
        }

        if (this.currentFrame.scale.HasValue)
        {
            transform.localScale = new Vector3(this.currentFrame.scale.Value, this.currentFrame.scale.Value, this.currentFrame.scale.Value);
        }

        if (wait == 0)
        {
            wait = this.currentFrame.wait / 30;
            this.spriteRenderer.sprite = this.currentFrame.pic;
        }

        if (wait > 0)
        {
            wait -= Time.deltaTime;
        }
        else
        {
            if (currentFrame.hold_forward_after != null && holdForwardAfter)
            {
                this.ChangeFrame(currentFrame.hold_forward_after, false);

            }
            else if (currentFrame.hold_defense_after != null && holdDefenseAfter)
            {
                this.ChangeFrame(currentFrame.hold_defense_after, false);

            }
            else if (currentFrame.hold_power_after != null && holdPowerAfter)
            {
                this.ChangeFrame(currentFrame.hold_power_after, false);

            }
            else if (currentFrame.hit_defense != null && holdDefense)
            {
                this.ChangeFrame(currentFrame.hit_defense, false);

            }
            else
            {
                this.ChangeFrame(currentFrame.next);
            }
        }

        switch (this.data.type)
        {
            case ObjectTypeEnum.CHARACTER:
                RunningCounter();
                SideDashCounter();
                break;
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void ChangeFrame(Nullable<int> frameToGo, bool usingNextPattern = true)
    {
        if (frameToGo == null)
        {
            return;
        }
        var nonNullFrameToGo = frameToGo ?? default(int);
        this.ChangeFrame(nonNullFrameToGo, usingNextPattern);
    }

    public void ChangeFrame(int frameToGo, bool usingNextPattern = true)
    {
        previousId = currentFrame.id;
        if (currentFrame.next == (int)FrameSpecialValuesEnum.DELETE)
        {
            if (ownerOpointController != null)
            {
                this.transform.parent = this.ownerOpointController.gameObjectOpoint.transform;
                this.transform.localPosition = this.objectPointCache.originalPosition;
                this.gameObject.SetActive(false);
                this.ownerOpointController.opoints[objectPointCache.key].Enqueue(objectPointCache);
            }
            else
            {
                Destroy(this.gameObject);
            }
            return;
        }
        summonAction = -1;
        wait = 0;
        if (usingNextPattern)
        {
            currentFrame = currentFrame.next == (int)FrameSpecialValuesEnum.BACK_TO_STANDING ? this.data.frames[0] : this.data.frames[frameToGo];
        }
        else
        {
            currentFrame = this.data.frames[frameToGo];
        }
    }

    public void ChangeFrame(CharacterSpecialStartFrameEnum next, bool usingNextPattern = true)
    {
        this.ChangeFrame((int)next, usingNextPattern);
    }

    public void Flip()
    {
        float xScaleFacing = 0;
        if (transform.localScale.x > 0)
        {
            xScaleFacing = -transform.localScale.x;
            this.facingRight = false;
        }
        else
        {
            xScaleFacing = Mathf.Abs(transform.localScale.x);
            this.facingRight = true;
        }
        transform.localScale = new Vector3(xScaleFacing, transform.localScale.y, transform.localScale.z);
    }

    public void Flip(Vector3 inputDirection)
    {
        if (!facingRight && inputDirection.x > 0)
        {
            this.Flip();
            return;
        }
        if (facingRight && inputDirection.x < 0)
        {
            this.Flip();
            return;
        }
    }

    private void RunningCounter()
    {
        if (countRightEnable && !runningRightEnable)
        {
            runningRightEnable = true;
            runningRightCount = RUNNING_COUNT / 30;
            runningLeftCount = 0;
        }
        if (runningRightCount > 0)
        {
            runningRightCount -= Time.deltaTime;
        }
        else
        {
            runningRightCount = 0;
            runningRightEnable = false;
            countRightEnable = false;
        }


        if (countLeftEnable && !runningLeftEnable)
        {
            runningLeftEnable = true;
            runningLeftCount = RUNNING_COUNT / 30;
            runningRightCount = 0;
        }
        if (runningLeftCount > 0)
        {
            runningLeftCount -= Time.deltaTime;
        }
        else
        {
            runningLeftCount = 0;
            runningLeftEnable = false;
            countLeftEnable = false;
        }
    }

    private void SideDashCounter()
    {
        if (countSideDashUpEnable && !sideDashUpEnable)
        {
            sideDashUpEnable = true;
            sideDashUpCount = SIDE_DASH_COUNT / 30;
            sideDashDownCount = 0;
        }
        if (sideDashUpCount > 0)
        {
            sideDashUpCount -= Time.deltaTime;
        }
        else
        {
            sideDashUpCount = 0;
            sideDashUpEnable = false;
            countSideDashUpEnable = false;
        }


        if (countSideDashDownEnable && !sideDashDownEnable)
        {
            sideDashDownEnable = true;
            sideDashDownCount = SIDE_DASH_COUNT / 30;
            sideDashUpCount = 0;
        }
        if (sideDashDownCount > 0)
        {
            sideDashDownCount -= Time.deltaTime;
        }
        else
        {
            sideDashDownCount = 0;
            sideDashDownEnable = false;
            countSideDashDownEnable = false;
        }
    }

    private void ApplyInjuredCount()
    {
        if (this.currentFrame.id != (int)CharacterSpecialStartFrameEnum.INJURED_1 && this.currentFrame.id != (int)CharacterSpecialStartFrameEnum.INJURED_2)
        {
            this.injuredCountOneTimePerState = true;
        }

        if (this.injuredCountOneTimePerState)
        {
            if (this.currentFrame.id == (int)CharacterSpecialStartFrameEnum.INJURED_1 || this.currentFrame.id == (int)CharacterSpecialStartFrameEnum.INJURED_2)
            {
                this.injuredCount += 1;
                this.injuredCountOneTimePerState = false;
            }
        }

        if (injuredCount >= INJURED_COUNT_LIMIT)
        {
            this.ChangeFrame(CharacterSpecialStartFrameEnum.FALLING, false);
            return;
        }
    }

    public void ResetValues(bool facingRight)
    {
        ResetValues(facingRight ? Vector3.right : Vector3.left);
    }

    public void ResetValues(Vector3 inputDirection)
    {
        this.currentFrame = this.data.frames[0];
        this.wait = 0;
        this.Flip(inputDirection);
        this.runningRightCount = 0;
        this.runningLeftCount = 0;
        this.sideDashUpCount = 0;
        this.sideDashDownCount = 0;
        this.inputDirection = Vector2.zero;
        this.runningRightEnable = false;
        this.runningLeftEnable = false;
        this.countRightEnable = false;
        this.countLeftEnable = false;
        this.sideDashDownEnable = false;
        this.sideDashUpEnable = false;
        this.countSideDashUpEnable = false;
        this.countSideDashDownEnable = false;
        this.holdForwardAfter = false;
        this.holdDefenseAfter = false;
        this.holdPowerAfter = false;
        this.externAction = false;
        this.injuredCount = 0;
        this.injuredCountOneTimePerState = true;

        switch (this.data.type)
        {
            case ObjectTypeEnum.CHARACTER:
                this.currentHp = ((CharacterDataController)this.data).header.start_hp;
                break;
            case ObjectTypeEnum.POWER:
                this.currentHp = ((PowerDataController)this.data).header.start_hp;
                break;
        }
    }
}
