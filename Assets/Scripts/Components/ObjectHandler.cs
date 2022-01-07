using System.Collections.Generic;
using Back.Model.Type;
using Model;
using Model.Type;
using UnityEngine;
using Util;

namespace Components {

    public class ObjectHandler : MonoBehaviour {

        //Parametros de definição de algumas ações
        [SerializeField]
        private float walking_speed;
        [SerializeField]
        private float walking_speedz;
        [SerializeField]
        private float running_speed;
        [SerializeField]
        private float running_speedz;
        [SerializeField]
        private float jump_distance;
        [SerializeField]
        private float dash_distance;
        [SerializeField]
        private float sideDash_distance;
        [SerializeField]
        private int totalHP;
        [SerializeField]
        private int totalMP;
        [SerializeField]
        private int initialHP;
        [SerializeField]
        private int initialMP;
        [SerializeField]
        private int currentHP;
        [SerializeField]
        private int currentMP;

        //Dono do objeto
        [SerializeField]
        private ObjectHandler owner;

        //Animações e Sprites
        [SerializeField]
        private Sprite lastSprite;
        [SerializeField]
        private Sprite currentSprite;
        [SerializeField]
        private string currentAnim;
        [SerializeField]
        private string lastAttackAnim;

        // Tipo do objeto e quem controla
        [SerializeField]
        public ObjectEnum objectType;
        [SerializeField]
        public PlayerEnum playerType;

        //Components
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private SpriteRenderer spriteRenderer;
        [SerializeField]
        private AudioSource audioSource;
        public BoxCollider boxCollider;
        [SerializeField]
        private HurtboxManager hurtboxManager;
        public Transform mainHurtbox;
        public Transform additionalHurtBox1;
        public Transform additionalHurtBox2;

        [SerializeField]
        private HitboxManager hitboxManager;
        public Transform mainHitbox;
        public Transform additionalHitbox1;
        public Transform additionalHitbox2;

        [SerializeField]
        private int agressive;
        [SerializeField]
        private int technique;
        [SerializeField]
        private int inteligent;
        [SerializeField]
        private int speed;
        [SerializeField]
        private int resistence;
        [SerializeField]
        private int stamina;

        //Execução de algum componente
        [SerializeField]
        private bool execOpointOneTime = true;
        [SerializeField]
        private bool execRecoverManaOneTime;
        [SerializeField]
        private bool execUsageManaOneTime;
        [SerializeField]
        private bool execAudioOneTime;

        //Alguns parametros de fisica
        [SerializeField]
        private float inertiaMoveHorizontal;
        [SerializeField]
        private float inertiaMoveVertical;
        [SerializeField]
        private float constantGravity;
        [SerializeField]
        private bool lockRightForce;
        [SerializeField]
        private float fixedValueForDirection = 0f;

        [SerializeField]
        private Data data;
        public Frame actualFrame;

        [SerializeField]
        private string dataPath;

        //RUNNING
        [SerializeField] private float intervalDoubleTapRunning;
        [SerializeField] private bool stepOneRunningRightEnabled;
        [SerializeField] private bool stepOneRunningLeftEnabled;
        [SerializeField] private float runningCountTapRight;
        [SerializeField] private float runningCountTapLeft;

        //SIDE DASH
        [SerializeField] private float intervalDoubleTapSideDash;
        [SerializeField] private bool stepOneSideDashUpEnabled;
        [SerializeField] private bool stepOneSideDashDownEnabled;
        [SerializeField] private float sideDashCountTapUp;
        [SerializeField] private float sideDashCountTapDown;

        //Disabled variables
        [SerializeField] private bool isRunningEnabled;
        [SerializeField] private bool isWalkingEnabled;
        [SerializeField] private bool isSideDashEnabled;

        //Catálogo de teclas
        [SerializeField]
        private float moveHorizontal;
        [SerializeField]
        private float moveVertical;
        [SerializeField]
        private bool pressJumpDown;
        [SerializeField]
        private bool moveHorizontalDown;
        [SerializeField]
        private bool moveHorizontalUp;
        [SerializeField]
        private bool moveVerticalUp;
        [SerializeField]
        private bool pressForwardAttackDown;
        [SerializeField]
        private bool pressUpAttackDown;
        [SerializeField]
        private bool pressDownAttackDown;
        [SerializeField]
        private bool pressDefenseDown;
        [SerializeField]
        private bool pressWeaponDown;
        [SerializeField]
        private bool pressAttackDown;
        [SerializeField]
        private bool pressTauntDown;
        [SerializeField]
        private float lastMoveVerticalUpValue;

        //Combinações
        //Combination: <^v> ADJW Lembrar de Desabilitar no 'DisableCombination'
        [SerializeField]
        private bool defense_up_attack;
        [SerializeField]
        private bool defense_up_jump;
        [SerializeField]
        private bool defense_up_weapon;
        [SerializeField]
        private bool defense_forward_attack;
        [SerializeField]
        private bool defense_forward_jump;
        [SerializeField]
        private bool defense_forward_weapon;
        [SerializeField]
        private bool defense_down_attack;
        [SerializeField]
        private bool defense_down_jump;
        [SerializeField]
        private bool defense_down_weapon;
        [SerializeField]
        private bool defense_jump_attack;//Charge

        //Gatilhos na execução de alguma animação especifica
        [SerializeField]
        private bool isLyingDown;
        public bool isFacingRight;
        [SerializeField]
        private bool isDead = false;
        [SerializeField]
        private bool isAttacking;
        [SerializeField]
        private bool hasTouchedHurtBox = false;
        public bool hasAttacked;

        //Gatilhos externos
        [SerializeField]
        private bool onWall;
        [SerializeField]
        private bool onWallDebug;
        [SerializeField]
        private bool onCeiling;
        [SerializeField]
        public bool onGround;

        //Eventos de interação com outro objeto
        [SerializeField]
        private bool enableInjured;
        [SerializeField]
        private bool isInjured;
        [SerializeField]
        private bool executeExternalForce;

        //Identificação
        public TeamEnum team;
        [SerializeField]
        private List<Transform> enemies = new List<Transform>();
        [SerializeField]
        private List<Transform> alies = new List<Transform>();

        //Parametros auxiliares
        [SerializeField]
        private float highY;
        [SerializeField]
        private bool isDummy;

        //Interação externa
        public Interaction externalItr;
        [SerializeField]
        private bool sameExternalItr;

        //Teclas pressionadas
        [SerializeField]
        private ButtonEnum lastButtonPressed = ButtonEnum.NONE;
        [SerializeField]
        private string[] buttonsPressed = new string[3];

        //Frame Eventos
        [SerializeField]
        private bool selfOpointAffectedFrame;
        [SerializeField]
        private string ownerFrame;
        [SerializeField]
        private bool flipOneTimeForFrame = true;

        //Para Objetos invocados
        [SerializeField]
        private Vector3 opointPosition;
        [SerializeField]
        private DirectionEnum spawnDirection;
        [SerializeField]
        private bool disableOpoint;
        [SerializeField]
        private Vector3 relativePositionFromOwner;
        [SerializeField]
        private string destroyInOwnerAnimation;
        [SerializeField]
        private string startAnimation;

        //Default Anims
        [SerializeField] private string defaultStandingAnim;
        [SerializeField] private string defaultWalkingAnim;
        [SerializeField] private string defaultRunningAnim;
        [SerializeField] private string defaultRunning2Anim;
        [SerializeField] private string defaultDefenseAnim;
        [SerializeField] private string defaultJumpDefenseAnim;
        [SerializeField] private string defaultDefenseMovementDebugAnim;
        [SerializeField] private string defaultJumpDefenseMovementDebugAnim;
        [SerializeField] private string defaultStopRunningAnim;
        [SerializeField] private string defaultSideDashAnim;
        [SerializeField] private string defaultCrouchAnim;
        [SerializeField] private string defaultJumping3Anim;
        [SerializeField] private string defaultJumping4Anim;
        [SerializeField] private string defaultJumpingDash3Anim;
        [SerializeField] private string defaultJumpingDash4Anim;
        [SerializeField] private string defaultJumping3WithComboAnim;
        [SerializeField] private string defaultJumping4WithComboAnim;
        [SerializeField] private string defaultJumpingFrontBackDashAnim;

        [SerializeField] private  string defaultDisableCombinationAnim;

        [SerializeField] private  List<string> defaultMovementAnims;
        [SerializeField] private  List<string> defaultInjuredAnims;

        void Start() {
            data = DataChangerUtil.GetDataFromJson(dataPath);

            var transformHurtboxManager = transform.Find("HurtboxManager");
            GetComponentInChild(transformHurtboxManager, hurtboxManager);

            mainHurtbox = transformHurtboxManager.Find("MainHurtbox");
            additionalHurtBox1 = transformHurtboxManager.Find("AdditionalHurtbox1");
            additionalHurtBox2 = transformHurtboxManager.Find("AdditionalHurtbox2");

            var transformHitboxManager = transform.Find("HitboxManager");
            GetComponentInChild(transformHitboxManager, hitboxManager);

            mainHitbox = transformHitboxManager.Find("MainHitbox");
            additionalHurtBox1 = transformHitboxManager.Find("AdditionalHitbox1");
            additionalHurtBox2 = transformHitboxManager.Find("AdditionalHitbox2");

            currentHP = initialHP;
            currentMP = initialMP;

            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent <SpriteRenderer>();
            audioSource = GetComponent <AudioSource>();
            boxCollider = GetComponent <BoxCollider>();

            if (team == null) {
                team = TeamEnum.INDEPENDENT;
            }

            buttonsPressed[0] = ButtonEnum.NONE.ToString();
            buttonsPressed[1] = ButtonEnum.NONE.ToString();
            buttonsPressed[2] = ButtonEnum.NONE.ToString();
        }

        private void GetComponentInChild<T>(Transform transform, T outObject) {
            if (transform) {
                if (!transform.TryGetComponent<T>(out outObject)) {
                    ExceptionThrowUtil.MissingComponent();
                }
            } else {
                ExceptionThrowUtil.MissingGameObject();
            }
        }

        void Update() {
#if UNITY_EDITOR
            ExecutePauseBreak();
#endif

            SetupAnimResets();

            isFacingRight = spriteRenderer.flipX ? false : true;

            Flip(actualFrame.flip.canFlip);

            SetupDead();

            StartCoroutine(SetupButtons());

            SetupPhysicResets();

            SetupChildActions();

            CheckEvents();

            ToggleTriggersForState();

            ResetRunningInterval();

            Walking();

            Running();

            SideDash();

            JumpingFrontBackDash();

            SetupGravity();

            ExecUsageManaOneTime(actualFrame.core.usageMP);

            ForceFlip();

            SharePositionToOwner();

            lastSprite = currentSprite;


            if (actualFrame.physic.enableMovementFixedVertical && fixedValueForDirection.Equals(0f)) {
                fixedValueForDirection = moveVertical;
            } else if (!actualFrame.physic.enableMovementFixedVertical) {
                fixedValueForDirection = 0f;
            }

            Debug.Log(actualFrame.physic.externalForceX);
        }

        void FixedUpdate() {

        }

        void OnCollisionStay(Collision hit) {
            if (objectType.Equals(ObjectEnum.CHARACTER)) {
                if (hit.collider.tag.Equals("Ground")) {
                    onGround = true;
                    constantGravity = 0f;
                } else if (hit.collider.tag.Equals("WallRight") || hit.collider.tag.Equals("WallLeft")) {
                    //Detecta Parede com o Collider do objeto
                    onWall = true;

                } else if (hit.collider.tag.Equals("WallDebug")) {
                    onWallDebug = true;
                }
            }
        }

        void OnCollisionExit(Collision hit) {
            if (hit.collider.tag.Equals("Ground")) {
                onGround = false;
                lockRightForce = isFacingRight;
            } else if (hit.collider.tag.Equals("WallRight") || hit.collider.tag.Equals("WallLeft")) {
                onWall = false;

            } else if (hit.collider.tag.Equals("WallDebug")) {
                onWallDebug = false;
            }
        }

        void GetFrameData(AnimationEvent animationEvent) {
            switch (animationEvent.stringParameter.ToLower()) {
                case "standing":
                    actualFrame = data.standing[animationEvent.intParameter];
                    break;
            }
        }

        void ExecOpoint() {
            if (execOpointOneTime) {
                execOpointOneTime = ExecOpoint(actualFrame.spawns);
            }
        }

        void ExecRecoverManaOneTime(int manaPoints) {
            if (manaPoints != 0) {
                currentMP += manaPoints;

                if (currentMP > totalMP) {
                    currentMP = totalMP;
                }
            }
        }

        void SetupAudio() {
            if (actualFrame.core.audio != null) {
                audioSource.PlayOneShot(actualFrame.core.audio);
            }
        }

        private void ExecutePauseBreak() {
            if (actualFrame.core.pauseBreak) {
                Debug.Break();
            }
        }

        private void SetupAnimResets() {
            currentSprite = this.spriteRenderer.sprite;
            if (lastSprite != null && !lastSprite.name.Equals(currentSprite.name)) {
                execOpointOneTime = true;
                execAudioOneTime = true;
            }

            if (this.animator.GetCurrentAnimatorClipInfo(0).Length > 0) {
                currentAnim = this.animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
            }

            if (lastAttackAnim != null && !lastAttackAnim.Equals(currentAnim)) {
                hasAttacked = false;
            }
        }

        private void Flip(bool canFlip) {
            if (canFlip) {
                //Flip
                if (moveHorizontal != 0) {
                    bool walkingLeft = moveHorizontal < 0;
                    bool walkingRight = moveHorizontal > 0;

                    if (walkingLeft) {
                        spriteRenderer.flipX = true;
                    } else if (walkingRight) {
                        spriteRenderer.flipX = false;
                    }
                }
            }
        }

        private void SetupDead() {
            if (currentHP <= 0) {
                isDead = true;
            }
        }

        IEnumerator<bool> SetupButtons() {
            if (!isDummy) {
                SetupControlsForPlayer(playerType);
                DisableCombination();
            }

            yield return true;
        }

        private void SetupControlsForPlayer(PlayerEnum playerType) {
            switch (playerType) {
                case PlayerEnum.PLAYER1:
                    moveHorizontal = Input.GetAxisRaw(ButtonEnum.Horizontal.ToString()) / 25;
                    moveVertical = Input.GetAxisRaw(ButtonEnum.Vertical.ToString()) / 25;
                    moveHorizontalDown = Input.GetButtonDown(ButtonEnum.Horizontal.ToString());
                    moveHorizontalUp = Input.GetButtonUp(ButtonEnum.Horizontal.ToString());
                    moveVerticalUp = Input.GetButtonUp(ButtonEnum.Vertical.ToString());
                    pressDefenseDown = Input.GetButtonDown(ButtonEnum.Defense.ToString());
                    pressAttackDown = Input.GetButtonDown(ButtonEnum.Attack.ToString());
                    pressWeaponDown = Input.GetButtonDown(ButtonEnum.Weapon.ToString());
                    pressJumpDown = Input.GetButtonDown(ButtonEnum.Jump.ToString());
                    pressTauntDown = Input.GetButtonDown(ButtonEnum.Taunt.ToString());

                    if (!defense_forward_jump) {
                        defense_forward_jump = buttonsPressed[2].Equals(ButtonEnum.Defense.ToString())
                        && buttonsPressed[1].Equals(ButtonEnum.Horizontal.ToString())
                        && buttonsPressed[0].Equals(ButtonEnum.Jump.ToString());
                    }
                    if (!defense_forward_attack) {
                        defense_forward_attack = buttonsPressed[2].Equals(ButtonEnum.Defense.ToString())
                        && buttonsPressed[1].Equals(ButtonEnum.Horizontal.ToString())
                        && buttonsPressed[0].Equals(ButtonEnum.Attack.ToString());
                    }
                    if (!defense_forward_weapon) {
                        defense_forward_weapon = buttonsPressed[2].Equals(ButtonEnum.Defense.ToString())
                        && buttonsPressed[1].Equals(ButtonEnum.Horizontal.ToString())
                        && buttonsPressed[0].Equals(ButtonEnum.Weapon.ToString());
                    }

                    if (!defense_up_jump) {
                        defense_up_jump = buttonsPressed[2].Equals(ButtonEnum.Defense.ToString())
                        && buttonsPressed[1].Equals(ButtonEnum.Vertical_Up.ToString())
                        && buttonsPressed[0].Equals(ButtonEnum.Jump.ToString());
                    }
                    if (!defense_up_weapon) {
                        defense_up_weapon = buttonsPressed[2].Equals(ButtonEnum.Defense.ToString())
                        && buttonsPressed[1].Equals(ButtonEnum.Vertical_Up.ToString())
                        && buttonsPressed[0].Equals(ButtonEnum.Weapon.ToString());
                    }
                    if (!defense_up_attack) {
                        defense_up_attack = buttonsPressed[2].Equals(ButtonEnum.Defense.ToString())
                        && buttonsPressed[1].Equals(ButtonEnum.Vertical_Up.ToString())
                        && buttonsPressed[0].Equals(ButtonEnum.Attack.ToString());
                    }

                    if (!defense_down_jump) {
                        defense_down_jump = buttonsPressed[2].Equals(ButtonEnum.Defense.ToString())
                        && buttonsPressed[1].Equals(ButtonEnum.Vertical_Down.ToString())
                        && buttonsPressed[0].Equals(ButtonEnum.Jump.ToString());
                    }
                    if (!defense_down_weapon) {
                        defense_down_weapon = buttonsPressed[2].Equals(ButtonEnum.Defense.ToString())
                        && buttonsPressed[1].Equals(ButtonEnum.Vertical_Down.ToString())
                        && buttonsPressed[0].Equals(ButtonEnum.Weapon.ToString());
                    }
                    if (!defense_down_attack) {
                        defense_down_attack = buttonsPressed[2].Equals(ButtonEnum.Defense.ToString())
                        && buttonsPressed[1].Equals(ButtonEnum.Vertical_Down.ToString())
                        && buttonsPressed[0].Equals(ButtonEnum.Attack.ToString());
                    }

                    if (!defense_jump_attack) {
                        defense_jump_attack = buttonsPressed[2].Equals(ButtonEnum.Defense.ToString())
                        && buttonsPressed[1].Equals(ButtonEnum.Jump.ToString())
                        && buttonsPressed[0].Equals(ButtonEnum.Attack.ToString());
                    }

                    pressForwardAttackDown = pressAttackDown && !moveHorizontal.Equals(0f);
                    pressUpAttackDown = pressAttackDown && moveVertical > 0;
                    pressDownAttackDown = pressAttackDown && moveVertical < 0;


                    //Obter botões
                    lastButtonPressed = !Input.GetAxisRaw(ButtonEnum.Horizontal.ToString()).Equals(0) ? ButtonEnum.Horizontal : lastButtonPressed;
                    lastButtonPressed = Input.GetAxisRaw(ButtonEnum.Vertical.ToString()) > 0 ? ButtonEnum.Vertical_Up : lastButtonPressed;
                    lastButtonPressed = Input.GetAxisRaw(ButtonEnum.Vertical.ToString()) < 0 ? ButtonEnum.Vertical_Down : lastButtonPressed;
                    lastButtonPressed = Input.GetButtonUp(ButtonEnum.Defense.ToString()) ? ButtonEnum.Defense : lastButtonPressed;
                    lastButtonPressed = Input.GetButtonUp(ButtonEnum.Attack.ToString()) ? ButtonEnum.Attack : lastButtonPressed;
                    lastButtonPressed = Input.GetButtonUp(ButtonEnum.Weapon.ToString()) ? ButtonEnum.Weapon : lastButtonPressed;
                    lastButtonPressed = Input.GetButtonUp(ButtonEnum.Jump.ToString()) ? ButtonEnum.Jump : lastButtonPressed;

                    if (!lastButtonPressed.Equals(ButtonEnum.NONE) && !buttonsPressed[0].Equals(lastButtonPressed.ToString()))
                    {
                        buttonsPressed[2] = buttonsPressed[1];
                        buttonsPressed[1] = buttonsPressed[0];
                        buttonsPressed[0] = lastButtonPressed.ToString();
                    }
                    break;
                case PlayerEnum.PLAYER2:
                    moveHorizontal = Input.GetAxisRaw(ButtonEnum.Horizontal_P2.ToString()) / 25;
                    moveVertical = Input.GetAxisRaw(ButtonEnum.Vertical_P2.ToString()) / 25;
                    moveHorizontalDown = Input.GetButtonDown(ButtonEnum.Horizontal_P2.ToString());
                    moveHorizontalUp = Input.GetButtonUp(ButtonEnum.Horizontal_P2.ToString());
                    moveVerticalUp = Input.GetButtonUp(ButtonEnum.Vertical.ToString());
                    pressDefenseDown = Input.GetButtonDown(ButtonEnum.Defense_P2.ToString());
                    pressAttackDown = Input.GetButtonDown(ButtonEnum.Attack_P2.ToString());
                    pressWeaponDown = Input.GetButtonDown(ButtonEnum.Weapon_P2.ToString());
                    pressJumpDown = Input.GetButtonDown(ButtonEnum.Jump_P2.ToString());
                    pressTauntDown = Input.GetButtonDown(ButtonEnum.Taunt_P2.ToString());

                    if (!defense_forward_jump) {
                        defense_forward_jump = buttonsPressed[2].Equals(ButtonEnum.Defense_P2.ToString())
                        && buttonsPressed[1].Equals(ButtonEnum.Horizontal_P2.ToString())
                        && buttonsPressed[0].Equals(ButtonEnum.Jump_P2.ToString());
                    }
                    if (!defense_forward_attack) {
                        defense_forward_attack = buttonsPressed[2].Equals(ButtonEnum.Defense_P2.ToString())
                        && buttonsPressed[1].Equals(ButtonEnum.Horizontal_P2.ToString())
                        && buttonsPressed[0].Equals(ButtonEnum.Attack_P2.ToString());
                    }
                    if (!defense_forward_weapon) {
                        defense_forward_weapon = buttonsPressed[2].Equals(ButtonEnum.Defense_P2.ToString())
                        && buttonsPressed[1].Equals(ButtonEnum.Horizontal_P2.ToString())
                        && buttonsPressed[0].Equals(ButtonEnum.Weapon_P2.ToString());
                    }

                    if (!defense_up_jump) {
                        defense_up_jump = buttonsPressed[2].Equals(ButtonEnum.Defense_P2.ToString())
                        && buttonsPressed[1].Equals(ButtonEnum.Vertical_Up_P2.ToString())
                        && buttonsPressed[0].Equals(ButtonEnum.Jump_P2.ToString());
                    }
                    if (!defense_up_weapon) {
                        defense_up_weapon = buttonsPressed[2].Equals(ButtonEnum.Defense_P2.ToString())
                        && buttonsPressed[1].Equals(ButtonEnum.Vertical_Up_P2.ToString())
                        && buttonsPressed[0].Equals(ButtonEnum.Weapon_P2.ToString());
                    }
                    if (!defense_up_attack) {
                        defense_up_attack = buttonsPressed[2].Equals(ButtonEnum.Defense_P2.ToString())
                        && buttonsPressed[1].Equals(ButtonEnum.Vertical_Up_P2.ToString())
                        && buttonsPressed[0].Equals(ButtonEnum.Attack_P2.ToString());
                    }

                    if (!defense_down_jump) {
                        defense_down_jump = buttonsPressed[2].Equals(ButtonEnum.Defense_P2.ToString())
                        && buttonsPressed[1].Equals(ButtonEnum.Vertical_Down_P2.ToString())
                        && buttonsPressed[0].Equals(ButtonEnum.Jump_P2.ToString());
                    }
                    if (!defense_down_weapon) {
                        defense_down_weapon = buttonsPressed[2].Equals(ButtonEnum.Defense_P2.ToString())
                        && buttonsPressed[1].Equals(ButtonEnum.Vertical_Down_P2.ToString())
                        && buttonsPressed[0].Equals(ButtonEnum.Weapon_P2.ToString());
                    }
                    if (!defense_down_attack) {
                        defense_down_attack = buttonsPressed[2].Equals(ButtonEnum.Defense_P2.ToString())
                        && buttonsPressed[1].Equals(ButtonEnum.Vertical_Down_P2.ToString())
                        && buttonsPressed[0].Equals(ButtonEnum.Attack_P2.ToString());
                    }

                    if (!defense_jump_attack) {
                        defense_jump_attack = buttonsPressed[2].Equals(ButtonEnum.Defense_P2.ToString())
                        && buttonsPressed[1].Equals(ButtonEnum.Jump_P2.ToString())
                        && buttonsPressed[0].Equals(ButtonEnum.Attack_P2.ToString());
                    }

                    pressForwardAttackDown = pressAttackDown && !moveHorizontal.Equals(0f);
                    pressUpAttackDown = pressAttackDown && moveVertical > 0;
                    pressDownAttackDown = pressAttackDown && moveVertical < 0;


                    //Obter botões
                    lastButtonPressed = !Input.GetAxisRaw(ButtonEnum.Horizontal_P2.ToString()).Equals(0) ? ButtonEnum.Horizontal_P2 : lastButtonPressed;
                    lastButtonPressed = Input.GetAxisRaw(ButtonEnum.Vertical_P2.ToString()) > 0 ? ButtonEnum.Vertical_Up_P2 : lastButtonPressed;
                    lastButtonPressed = Input.GetAxisRaw(ButtonEnum.Vertical_P2.ToString()) < 0 ? ButtonEnum.Vertical_Down_P2 : lastButtonPressed;
                    lastButtonPressed = Input.GetButtonUp(ButtonEnum.Defense_P2.ToString()) ? ButtonEnum.Defense_P2 : lastButtonPressed;
                    lastButtonPressed = Input.GetButtonUp(ButtonEnum.Attack_P2.ToString()) ? ButtonEnum.Attack_P2 : lastButtonPressed;
                    lastButtonPressed = Input.GetButtonUp(ButtonEnum.Weapon_P2.ToString()) ? ButtonEnum.Weapon_P2 : lastButtonPressed;
                    lastButtonPressed = Input.GetButtonUp(ButtonEnum.Jump_P2.ToString()) ? ButtonEnum.Jump_P2 : lastButtonPressed;

                    if (!lastButtonPressed.Equals(ButtonEnum.NONE) && !buttonsPressed[0].Equals(lastButtonPressed.ToString()))
                    {
                        buttonsPressed[2] = buttonsPressed[1];
                        buttonsPressed[1] = buttonsPressed[0];
                        buttonsPressed[0] = lastButtonPressed.ToString();
                    }
                    break;
            }
        }

        private void DisableCombination() {
            //Disable Combination
            if (actualFrame.core.resetCombinations) {
                defense_up_attack = false;
                defense_up_jump = false;
                defense_up_weapon = false;
                defense_forward_attack = false;
                defense_forward_jump = false;
                defense_forward_weapon = false;
                defense_down_attack = false;
                defense_down_jump = false;
                defense_down_weapon = false;
                defense_jump_attack = false;

                SetupPressedButtons();
            }
        }

        bool ExecOpoint(Spawn[] opoints) {
            if (opoints != null && opoints.Length > 0) {
                if (opoints[0].obj == null && opoints[1].obj == null && opoints[2].obj == null) {
                    return true;
                }

                foreach (Spawn opoint in opoints) {
                    if (opoint.obj == null) {
                        continue;
                    }

                    float xLeft;
                    float xRight;
                    bool flipX = opoint.flipX ? isFacingRight : !isFacingRight;

                    if (opoint.direction.Equals(DirectionEnum.FORWARD)) {
                        xLeft = -opoint.position.x;
                        xRight = opoint.position.x;
                    } else {
                        xLeft = opoint.position.x;
                        xRight = -opoint.position.x;
                    }

                    float xCorr = !isFacingRight ? xLeft : xRight;
                    var position = transform.position;
                    float x = position.x + xCorr;
                    float y = position.y + opoint.position.y;
                    float z = position.z + opoint.position.z;

                    GameObject objInstatiate = null;
                    Vector3 objSpawnPositon = new Vector3(x, y, z);

                    ObjectHandler objectHandler;
                    if (opoint.obj.TryGetComponent<ObjectHandler>(out objectHandler)) {
                        if (opoint.enableZDirection)
                        {
                            if (moveVertical > 0)
                            {
                                objectHandler.spawnDirection = DirectionEnum.UP;
                            } else if (moveVertical < 0)
                            {
                                objectHandler.spawnDirection = DirectionEnum.DOWN;
                            }
                            else
                            {
                                objectHandler.spawnDirection = DirectionEnum.FORWARD;
                            }
                        }

                        objectHandler.disableOpoint = opoint.disableOpoint;
                        objectHandler.relativePositionFromOwner = opoint.useOwnerRelativePosition ? new Vector3(xCorr, opoint.position.y, opoint.position.z) : Vector3.zero;
                        objectHandler.destroyInOwnerAnimation = opoint.destroyInOwnerAnimation;
                        objectHandler.team = this.team;
                        objectHandler.owner = this;
                        objectHandler.execOpointOneTime = true;
                        objectHandler.execOpointOneTime = true;
                        objectHandler.startAnimation = opoint.startAnim;
                        objInstatiate = objectHandler.gameObject;
                    } else {
                        ObjectHandler scriptChar = opoint.obj.GetComponent<ObjectHandler>();
                        if (scriptChar != null) {
                            scriptChar.owner = this;
                            scriptChar.execOpointOneTime = true;
                            objInstatiate = scriptChar.gameObject;
                        }
                        else {
                            objInstatiate = opoint.obj;
                        }
                    }

                    execOpointOneTime = false;
                    Instantiate(objInstatiate, objSpawnPositon, Quaternion.identity).GetComponent<SpriteRenderer>().flipX = flipX;
                }
                return false;

            } else {
                return true;
            }
        }

        private void SetupPressedButtons() {
            buttonsPressed[0] = ButtonEnum.NONE.ToString();
            buttonsPressed[1] = ButtonEnum.NONE.ToString();
            buttonsPressed[2] = ButtonEnum.NONE.ToString();
            lastButtonPressed = ButtonEnum.NONE;
        }

        private void SetupChildActions() {
            if (objectType.Equals(ObjectEnum.STATIC_MAP_OBJECT) || objectType.Equals(ObjectEnum.MOBILE_MAP_OBJECT)) {
                if (actualFrame.childObjects != null && actualFrame.childObjects.removeWalls && actualFrame.childObjects.walls.Count > 0) {
                    gameObject.layer = 11;
                    actualFrame.childObjects.walls.ForEach(wall => wall.layer = 11);
                }
            }
        }

        private void SetupPhysicResets() {
            if (actualFrame.physic.lockDirectionForce) {
                lockRightForce = !isFacingRight ? false : true;
            }

            if (actualFrame.physic.resetExternalForce) {
                actualFrame.physic.externalForceX = 0f;
                actualFrame.physic.externalForceY = 0f;
                actualFrame.physic.externalForceZ = 0f;
            }

            if (actualFrame.physic.resetExternalGravityForce) {
                actualFrame.physic.externalForceY = 0f;
            }
        }

        void CheckEvents() {

            string eventNextAnim = null;

            if (startAnimation != null && startAnimation.Length > 0) {
                eventNextAnim = startAnimation;
            }

            if (moveHorizontal != 0 && actualFrame.trigger.holdHorizontalAnim != null) {
                if (moveVertical > 0 && moveHorizontal > 0) {
                    eventNextAnim = actualFrame.trigger.holdUpRightAnim.ToString();

                } else if (moveVertical > 0 && moveHorizontal < 0) {
                    eventNextAnim = actualFrame.trigger.holdUpLeftAnim.ToString();

                } else if (moveVertical < 0 && moveHorizontal > 0) {
                    eventNextAnim = actualFrame.trigger.holdDownRightAnim.ToString();

                } else if (moveVertical < 0 && moveHorizontal < 0) {
                    eventNextAnim = actualFrame.trigger.holdDownLeftAnim.ToString();

                } else {
                    eventNextAnim = actualFrame.trigger.holdHorizontalAnim.ToString();
                }
            } else if (moveVertical != 0) {
                if (moveVertical > 0 && actualFrame.trigger.holdUpAnim != null) {
                    eventNextAnim = actualFrame.trigger.holdUpAnim.ToString();

                } else if (moveVertical < 0 && actualFrame.trigger.holdDownAnim != null) {
                    eventNextAnim = actualFrame.trigger.holdDownAnim.ToString();
                }
            }

            if (pressAttackDown) {
                if (actualFrame.trigger.attackButtonAnim != null) {
                    //Attack Button
                    eventNextAnim = actualFrame.trigger.attackButtonAnim.ToString();
                }
                if (actualFrame.trigger.upAttackAnim != null && pressUpAttackDown) {
                    eventNextAnim = actualFrame.trigger.upAttackAnim.ToString();
                }
                if (actualFrame.trigger.forwardAttackAnim != null && pressForwardAttackDown) {
                    eventNextAnim = actualFrame.trigger.forwardAttackAnim.ToString();
                }
                if (actualFrame.trigger.downAttackAnim != null && pressDownAttackDown) {
                    eventNextAnim = actualFrame.trigger.downAttackAnim.ToString();
                }

            } else if (pressDefenseDown && actualFrame.trigger.defenseButtonAnim != null) {
                //Defense Button
                eventNextAnim = actualFrame.trigger.defenseButtonAnim.ToString();

            } else if (pressJumpDown && actualFrame.trigger.jumpButtonAnim != null) {
                //Jump Button
                eventNextAnim = actualFrame.trigger.jumpButtonAnim.ToString();

            } else if (pressWeaponDown && actualFrame.trigger.weaponButtonAnim != null) {
                //Weapon Button
                eventNextAnim = actualFrame.trigger.weaponButtonAnim.ToString();

            } else if (pressTauntDown && actualFrame.trigger.tauntAnim != null) {
                //Taunt Button
                eventNextAnim = actualFrame.trigger.tauntAnim.ToString();
            }

            // combination
            //DuJ
            if (defense_up_jump && actualFrame.combination.defenseUpJumpAnim != null) {
                eventNextAnim = actualFrame.combination.defenseUpJumpAnim.ToString();
                //DuW
            } else if (defense_up_weapon && actualFrame.combination.defenseUpWeaponAnim != null) {
                eventNextAnim = actualFrame.combination.defenseUpWeaponAnim.ToString();
                //DuA
            } else if (defense_up_attack && actualFrame.combination.defenseUpAttackAnim != null) {
                eventNextAnim = actualFrame.combination.defenseUpAttackAnim.ToString();
                //DdJ
            } else if (defense_down_jump && actualFrame.combination.defenseDownJumpAnim != null) {
                eventNextAnim = actualFrame.combination.defenseDownJumpAnim.ToString();
                //DdW
            } else if (defense_down_weapon && actualFrame.combination.defenseDownWeaponAnim != null) {
                eventNextAnim = actualFrame.combination.defenseDownWeaponAnim.ToString();
                //DdA
            } else if (defense_down_attack && actualFrame.combination.defenseDownAttackAnim != null) {
                eventNextAnim = actualFrame.combination.defenseDownAttackAnim.ToString();
                //DfJ
            } else if (defense_forward_jump && actualFrame.combination.defenseForwardJumpAnim != null) {
                eventNextAnim = actualFrame.combination.defenseForwardJumpAnim.ToString();
                //DfW
            } else if (defense_forward_weapon && actualFrame.combination.defenseForwardWeaponAnim != null) {
                eventNextAnim = actualFrame.combination.defenseForwardWeaponAnim.ToString();
                //DfA
            } else if (defense_forward_attack && actualFrame.combination.defenseForwardAttackAnim != null) {
                eventNextAnim = actualFrame.combination.defenseForwardAttackAnim.ToString();
                //DJA
            } else if (defense_jump_attack && actualFrame.combination.defenseJumpAttackAnim != null) {
                eventNextAnim = actualFrame.combination.defenseJumpAttackAnim.ToString();
            }

            //On Ground
            if (onGround && actualFrame.trigger.groundAnim != null) {
                eventNextAnim = actualFrame.trigger.groundAnim.ToString();
            }

            //On Fly
            if (!onGround && actualFrame.trigger.flyAnim != null) {
                eventNextAnim = actualFrame.trigger.flyAnim.ToString();
            }

            //On Wall
            if (onWall && actualFrame.trigger.wallAnim != null) {
                eventNextAnim = actualFrame.trigger.wallAnim.ToString();
            }

            //On Wall Debug
            if (onWallDebug && actualFrame.trigger.wallDebugAnim != null) {
                eventNextAnim = actualFrame.trigger.wallDebugAnim.ToString();
            }

            //Events with
            if (actualFrame.interactions.Length > 0) {
                foreach (Interaction interaction in actualFrame.interactions) {
                    if (isAttacking) {
                        hasAttacked = true;
                        lastAttackAnim = currentAnim;
                        if (interaction != null && interaction.nextAnimation != null) {
                            eventNextAnim = interaction.nextAnimation.ToString();
                        } else {
                            isAttacking = false;
                        }
                    }

                    if (hasAttacked) {
                        if (interaction != null && interaction.nextIfPressedAttackAnimation != null && pressAttackDown) {
                            eventNextAnim = interaction.nextIfPressedAttackAnimation.ToString();
                        }
                    }

                    //Next If Hit Enemy
                    if (interaction != null && interaction.nextAnimation != null) {
                        if (isAttacking && interaction.nextAnimation != null) {
                            eventNextAnim = interaction.nextAnimation.ToString();
                        }
                    }
                }
            }

            //Objeto filho escolhe animação do pai
            if (selfOpointAffectedFrame && ownerFrame != null) {
                eventNextAnim = ownerFrame.ToString();
                selfOpointAffectedFrame = false;
                ownerFrame = null;
            }

            if (enableInjured && isInjured && externalItr != null) {
                if (!sameExternalItr) {
                    eventNextAnim = SetupInjuredEvent();
                } else {
                    if (hurtboxManager.damageRestTimer <= 0) {
                        eventNextAnim = SetupInjuredEvent();
                        hurtboxManager.damageRestTimer = 0;

                    } else if (hurtboxManager.damageRestTimer.Equals(0) && externalItr.damageRestTU.Equals(0)) {
                        eventNextAnim = SetupInjuredEvent();

                    } else if (externalItr.ignoreDamageRestTU) {
                        eventNextAnim = SetupInjuredEvent();
                    }
                }
            }

            if (isDead && actualFrame.trigger.deathAnim != null) {
                eventNextAnim = actualFrame.trigger.deathAnim.ToString();
            }

            //Without Action
            if (eventNextAnim == null) {
                if (actualFrame.core.touchHurtBoxNextAnim != null && hasTouchedHurtBox) {
                    hasTouchedHurtBox = false;
                    eventNextAnim = actualFrame.core.touchHurtBoxNextAnim.ToString();

                } else if (actualFrame.core.nextAnim != null) {
                    if (actualFrame.core.nextAnim != null) {
                        eventNextAnim = actualFrame.core.nextAnim.ToString();
                    }
                }
            }

            if (eventNextAnim != null) {
                ChangeAnimation(eventNextAnim);
            }
        }

        private string SetupInjuredEvent() {
            hurtboxManager.damageRestTimer = externalItr.damageRestTU;
            string affectedAnimation = externalItr.affectedAnimation;

            string eventNextFrame = null;
            switch (affectedAnimation) {
                case "DefendingMovementDebug":
                    eventNextFrame = affectedAnimation.ToString();
                    break;
                case "JumpDefenseMovementDebug":
                    eventNextFrame = affectedAnimation.ToString();
                    break;
                default:
                    Debug.LogWarning("Frame de Dano " + affectedAnimation + " selecionado ainda não tem atribuição com alguma animação no objeto, caso precise de tratamento, favor revisar");
                    eventNextFrame = affectedAnimation.ToString();
                    break;
            }
            //ITR do atacante sendo transferida para o atacado
            if (!affectedAnimation.Equals("NONE")) {
                actualFrame.physic.externalForceX = externalItr.force.x;
                actualFrame.physic.externalForceY = externalItr.force.y;
                actualFrame.physic.externalForceZ = externalItr.force.z;
            }

            this.currentHP -= externalItr.injury;
            enableInjured = false;
            sameExternalItr = false;
            executeExternalForce = true;
            return eventNextFrame;
        }

        private void ToggleTriggersForState() {
            if (objectType.Equals(ObjectEnum.CHARACTER)) {
                if (currentAnim.Equals(defaultStandingAnim)) {
                    isWalkingEnabled = true;
                    isRunningEnabled = false;
                    isSideDashEnabled = false;
                }

                foreach (string anim in defaultMovementAnims) {
                    if (currentAnim.Equals(anim)) {
                        isWalkingEnabled = false;
                        isRunningEnabled = false;
                        isSideDashEnabled = false;
                    }
                }

                foreach (string anim in defaultInjuredAnims) {
                    if (currentAnim.Equals(anim)) {
                        isWalkingEnabled = false;
                        isRunningEnabled = false;
                        isSideDashEnabled = false;
                        isInjured = false;
                    }
                }

                if ((currentAnim.Equals(defaultDefenseAnim.ToString()) && currentAnim.Equals(defaultDefenseMovementDebugAnim))
                || (currentAnim.Equals(defaultJumpDefenseAnim) && currentAnim.Equals(defaultJumpDefenseMovementDebugAnim))) {
                    isWalkingEnabled = false;
                    isRunningEnabled = false;
                    isSideDashEnabled = false;
                    isInjured = false;
                }
            }
        }

        private void ChangeAnimation(string anim) {
            if (!actualFrame.core.resetAnimation) {
                animator.Play($"Base Layer.{anim}", 0);
            } else {
                animator.Play($"Base Layer.{anim}", 0, 0f);
            }
            execOpointOneTime = true;
            execRecoverManaOneTime = true;
            execUsageManaOneTime = true;
            SetupAudio();
            actualFrame.core.nextAnim = null;
        }

        private void ResetRunningInterval() {
            if (actualFrame.core.resetRunningInterval) {
                stepOneRunningLeftEnabled = false;
                runningCountTapLeft = 0f;
                stepOneRunningRightEnabled = false;
                runningCountTapRight = 0f;
            }
        }

        private void Walking() {
            if (objectType.Equals(ObjectEnum.CHARACTER)) {
                if (isWalkingEnabled) {
                    Flip(true);

                    //Running Triggger
                    if (runningCountTapRight >= 0f) {
                        runningCountTapRight -= Time.fixedDeltaTime;
                    }

                    if (runningCountTapLeft >= 0f) {
                        runningCountTapLeft -= Time.fixedDeltaTime;
                    }

                    if (moveHorizontalUp && isFacingRight) {
                        stepOneRunningRightEnabled = true;
                        runningCountTapRight = intervalDoubleTapRunning;

                        stepOneRunningLeftEnabled = false;
                        runningCountTapLeft = 0f;

                    } else if (moveHorizontalUp && !isFacingRight) {
                        stepOneRunningLeftEnabled = true;
                        runningCountTapLeft = intervalDoubleTapRunning;

                        stepOneRunningRightEnabled = false;
                        runningCountTapRight = 0f;
                    }
                    if (moveHorizontal > 0f && stepOneRunningRightEnabled && runningCountTapRight > 0) {
                        isRunningEnabled = true;
                        stepOneRunningRightEnabled = false;
                        stepOneRunningLeftEnabled = false;
                        runningCountTapRight = 0f;
                        runningCountTapLeft = 0f;
                        flipOneTimeForFrame = true;
                        ChangeAnimation(defaultRunningAnim.ToString());
                        return;

                    } else if (moveHorizontal < 0f && stepOneRunningLeftEnabled && runningCountTapLeft > 0) {
                        isRunningEnabled = true;
                        stepOneRunningRightEnabled = false;
                        stepOneRunningLeftEnabled = false;
                        runningCountTapRight = 0f;
                        runningCountTapLeft = 0f;
                        flipOneTimeForFrame = true;
                        ChangeAnimation(defaultRunningAnim.ToString());
                        return;
                    }


                    //SideDash Trigger
                    if (sideDashCountTapUp >= 0f) {
                        sideDashCountTapUp -= Time.fixedDeltaTime;
                    }

                    if (sideDashCountTapDown >= 0f) {
                        sideDashCountTapDown -= Time.fixedDeltaTime;
                    }
                    if (moveVerticalUp && lastMoveVerticalUpValue > 0f) {
                        stepOneSideDashUpEnabled = true;
                        sideDashCountTapUp = intervalDoubleTapSideDash;

                        stepOneSideDashDownEnabled = false;
                        sideDashCountTapDown = 0f;

                    } else if (moveVerticalUp && lastMoveVerticalUpValue < 0f) {
                        stepOneSideDashDownEnabled = true;
                        sideDashCountTapDown = intervalDoubleTapSideDash;

                        stepOneSideDashUpEnabled = false;
                        sideDashCountTapUp = 0f;
                    }

                    if (moveVertical > 0f && stepOneSideDashUpEnabled && sideDashCountTapUp > 0) {
                        isSideDashEnabled = true;
                        stepOneSideDashDownEnabled = false;
                        stepOneSideDashUpEnabled = false;
                        sideDashCountTapDown = 0f;
                        sideDashCountTapUp = 0f;
                        flipOneTimeForFrame = true;
                        ChangeAnimation(defaultSideDashAnim.ToString());
                        return;

                    } else if (moveVertical < 0f && stepOneSideDashDownEnabled && sideDashCountTapDown > 0) {
                        isSideDashEnabled = true;
                        stepOneSideDashDownEnabled = false;
                        stepOneSideDashUpEnabled = false;
                        sideDashCountTapDown = 0f;
                        sideDashCountTapUp = 0f;
                        flipOneTimeForFrame = true;
                        ChangeAnimation(defaultSideDashAnim.ToString());
                        return;
                    }

                    if (moveVertical != 0) {
                        lastMoveVerticalUpValue = moveVertical;
                    }

                    if (moveHorizontal == 0 && moveVertical == 0) {
                        //Standing anim
                        if (!currentAnim.Equals(defaultStandingAnim)) {
                            flipOneTimeForFrame = true;
                            ChangeAnimation(defaultStandingAnim.ToString());
                            return;
                        }
                    }
                } else if (currentAnim.Equals(defaultWalkingAnim)) {
                    //Standing anim
                    if (!currentAnim.Equals(defaultStandingAnim)) {
                        flipOneTimeForFrame = true;
                        ChangeAnimation(defaultStandingAnim.ToString());
                        return;
                    }
                }
            }
        }

        private void Running() {
            if (objectType.Equals(ObjectEnum.CHARACTER)) {
                if (isRunningEnabled) {
                    //Disable transition to walking
                    isWalkingEnabled = false;
                    isSideDashEnabled = false;

                    if (!currentAnim.Equals(defaultRunningAnim) && !currentAnim.Equals(defaultRunning2Anim)) {
                        flipOneTimeForFrame = true;
                        isRunningEnabled = true;
                        stepOneRunningRightEnabled = false;
                        stepOneRunningLeftEnabled = false;
                        ChangeAnimation(defaultRunningAnim.ToString());
                        return;
                    }

                    //Active Stop Running
                    if (!isFacingRight && moveHorizontal > 0) {
                        isRunningEnabled = false;
                    } else if (isFacingRight && moveHorizontal < 0) {
                        isRunningEnabled = false;
                    }
                } else {
                    //Stop Running anim
                    if (currentAnim.Equals(defaultRunningAnim) || currentAnim.Equals(defaultRunning2Anim)) {
                        flipOneTimeForFrame = true;
                        isRunningEnabled = true;
                        stepOneRunningRightEnabled = false;
                        stepOneRunningLeftEnabled = false;
                        ChangeAnimation(defaultStopRunningAnim.ToString());
                        return;
                    }
                }
            }
        }

        private void SideDash() {
            if (objectType.Equals(ObjectEnum.CHARACTER)) {
                if (isSideDashEnabled) {
                    //Disable transition to walking
                    isWalkingEnabled = false;
                    isRunningEnabled = false;
                }
            }
        }

        private void JumpingFrontBackDash() {
            if(!objectType.Equals(ObjectEnum.CHARACTER)){
                return;
            }

            if (currentAnim.Equals(defaultJumping3Anim) || currentAnim.Equals(defaultJumping4Anim)
            || currentAnim.Equals(defaultJumpingDash3Anim) || currentAnim.Equals(defaultJumpingDash4Anim)
            || currentAnim.Equals(defaultJumping3WithComboAnim) || currentAnim.Equals(defaultJumping4WithComboAnim)) {
                Flip(true);
                //Jumping Front Back Dash Triggger
                if (runningCountTapRight >= 0f) {
                    runningCountTapRight -= Time.fixedDeltaTime;
                }

                if (runningCountTapLeft >= 0f) {
                    runningCountTapLeft -= Time.fixedDeltaTime;
                }

                if (moveHorizontalUp && isFacingRight) {
                    stepOneRunningRightEnabled = true;
                    runningCountTapRight = intervalDoubleTapRunning;

                    stepOneRunningLeftEnabled = false;
                    runningCountTapLeft = 0f;

                } else if (moveHorizontalUp && !isFacingRight) {
                    stepOneRunningLeftEnabled = true;
                    runningCountTapLeft = intervalDoubleTapRunning;

                    stepOneRunningRightEnabled = false;
                    runningCountTapRight = 0f;
                }
                if (moveHorizontal > 0f && stepOneRunningRightEnabled && runningCountTapRight > 0) {
                    stepOneRunningRightEnabled = false;
                    stepOneRunningLeftEnabled = false;
                    runningCountTapRight = 0f;
                    runningCountTapLeft = 0f;
                    flipOneTimeForFrame = true;
                    ChangeAnimation(defaultJumpingFrontBackDashAnim.ToString());
                    return;

                } else if (moveHorizontal < 0f && stepOneRunningLeftEnabled && runningCountTapLeft > 0) {
                    stepOneRunningRightEnabled = false;
                    stepOneRunningLeftEnabled = false;
                    runningCountTapRight = 0f;
                    runningCountTapLeft = 0f;
                    flipOneTimeForFrame = true;
                    ChangeAnimation(defaultJumpingFrontBackDashAnim.ToString());
                    return;
                }
            }
        }

        private void SetupGravity() {
            if (!onGround && actualFrame.physic.dvy < 0 && actualFrame.physic.useConstantGravity) {
                if (constantGravity > -30f) {
                    constantGravity += actualFrame.physic.dvy;
                }
            }
            if (!actualFrame.physic.useConstantGravity) {
                constantGravity = 0f;
            }
        }

        private void ExecUsageManaOneTime(int usageMP) {
            if (execUsageManaOneTime) {
                if (usageMP > 0) {

                    currentMP -= usageMP;

                    if (currentMP < 0) {
                        currentMP = 0;
                    }

                    execUsageManaOneTime = false;
                }

                execUsageManaOneTime = true;
            }

            execUsageManaOneTime = true;
        }

        private void ForceFlip() {
            if (actualFrame.flip.executeFlip && flipOneTimeForFrame) {
                flipOneTimeForFrame = false;
                spriteRenderer.flipX = !spriteRenderer.flipX;
            }
        }

        private void SharePositionToOwner() {
            foreach (Spawn opoint in actualFrame.spawns) {
                if (opoint.useSharedOpointPosition) {
                    transform.position = opointPosition;
                }
            }
        }

        public void SetSameExternalItr(bool sameExternalItr) {
            this.sameExternalItr = sameExternalItr;
        }
    }
}


