//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/InputSystem/PlayerInputActions.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerInputActions : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputActions"",
    ""maps"": [
        {
            ""name"": ""Player1"",
            ""id"": ""2a7ab401-1b0a-4cc1-b3da-cebf5b3df78e"",
            ""actions"": [
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""1176edcc-7a22-42d8-9cd6-7e7b53c8eccd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MoveX"",
                    ""type"": ""Value"",
                    ""id"": ""f31a9f40-67fe-4956-815a-6a4379ba9b89"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""MoveZ"",
                    ""type"": ""Value"",
                    ""id"": ""f5c223a1-2f11-4d7c-bfd3-8dc874793352"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""290019a9-4b8a-4669-b5a5-1b4134c8d669"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Defense"",
                    ""type"": ""Button"",
                    ""id"": ""b25874eb-ce43-4570-b45b-7d5c6f41af9f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Taunt"",
                    ""type"": ""Button"",
                    ""id"": ""d776cdef-c4db-400b-b327-ed17d8422485"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""7544e728-10e2-4e40-979d-4e581b4f0359"",
                    ""path"": ""<Keyboard>/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""09aa8380-45f3-4b3a-bb86-4a565cee4c06"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveX"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""left"",
                    ""id"": ""51a78908-a66a-4e6f-999d-9c404805fb6a"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveX"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""d596f42e-2b4f-4fdd-8f8a-03e0ebfd63a7"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveX"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""fe120294-c61c-41cd-a54e-7b100081e551"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveZ"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""bea8ccf0-2b22-4a89-ad4f-d66f131705ff"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveZ"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""943bb46a-99e9-4275-b0aa-de7129f13be5"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveZ"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""ff5896b6-915a-4289-9e95-2b547159a158"",
                    ""path"": ""<Keyboard>/z"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3d9409a6-f9ae-4f09-89d0-5056b0537683"",
                    ""path"": ""<Keyboard>/leftCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Taunt"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""779e8d85-9946-482f-bd95-439dd5969007"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Defense"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Player2"",
            ""id"": ""a4534447-7b9c-4a19-ba98-83890414ed89"",
            ""actions"": [
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""60fdd6f3-bb1d-40d0-9fe7-bca9d0793849"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MoveX"",
                    ""type"": ""Value"",
                    ""id"": ""882ad1d5-af26-43f9-a1b7-980b5eb87b42"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""MoveZ"",
                    ""type"": ""Value"",
                    ""id"": ""b86a6214-30c8-48a3-a469-1f37ebde226c"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""17acb3bd-c67c-4ed2-bc16-c75baaa83451"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Taunt"",
                    ""type"": ""Button"",
                    ""id"": ""e91d6364-4811-4155-b93a-d8fcc6b9231c"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Defense"",
                    ""type"": ""Button"",
                    ""id"": ""b3b2928d-e13b-4f35-910d-6c07310ce6ce"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""4841a2be-a24a-45f1-ba71-5bfcfd2ce15b"",
                    ""path"": ""<Keyboard>/l"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""650770c8-f189-44f9-84c5-42837cae294b"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveX"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""left"",
                    ""id"": ""a2287768-4cd5-4d96-9f60-abac10605fae"",
                    ""path"": ""<Keyboard>/numpad4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveX"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""6afc866c-5c31-414b-8595-2139d1a21c66"",
                    ""path"": ""<Keyboard>/numpad6"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveX"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""54ad7e17-033c-4009-8c44-46ba70229dcb"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveZ"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""813fdc3c-72da-4a7a-8c1f-e33e839f0e02"",
                    ""path"": ""<Keyboard>/numpad8"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveZ"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""bf9701ec-ccad-42e0-a4f4-1662fd39f5be"",
                    ""path"": ""<Keyboard>/numpad5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveZ"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""60269b6c-aecd-45fb-a681-ba2541aeab85"",
                    ""path"": ""<Keyboard>/k"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a0717cc1-243a-4973-8889-0d52d22426b4"",
                    ""path"": ""<Keyboard>/rightCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Taunt"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b0e8e5ff-016e-4706-9c44-91fd4ee1fef8"",
                    ""path"": ""<Keyboard>/i"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Defense"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Debug"",
            ""id"": ""24f72231-d5f3-4cb8-b113-808b06b7c19c"",
            ""actions"": [
                {
                    ""name"": ""Reload Current Scene"",
                    ""type"": ""Button"",
                    ""id"": ""a13aa5ae-1c3c-48fb-b73c-87226d092583"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""6e7cc0ab-ce36-4995-af9e-230281f65013"",
                    ""path"": ""<Keyboard>/backspace"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Reload Current Scene"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player1
        m_Player1 = asset.FindActionMap("Player1", throwIfNotFound: true);
        m_Player1_Jump = m_Player1.FindAction("Jump", throwIfNotFound: true);
        m_Player1_MoveX = m_Player1.FindAction("MoveX", throwIfNotFound: true);
        m_Player1_MoveZ = m_Player1.FindAction("MoveZ", throwIfNotFound: true);
        m_Player1_Attack = m_Player1.FindAction("Attack", throwIfNotFound: true);
        m_Player1_Defense = m_Player1.FindAction("Defense", throwIfNotFound: true);
        m_Player1_Taunt = m_Player1.FindAction("Taunt", throwIfNotFound: true);
        // Player2
        m_Player2 = asset.FindActionMap("Player2", throwIfNotFound: true);
        m_Player2_Jump = m_Player2.FindAction("Jump", throwIfNotFound: true);
        m_Player2_MoveX = m_Player2.FindAction("MoveX", throwIfNotFound: true);
        m_Player2_MoveZ = m_Player2.FindAction("MoveZ", throwIfNotFound: true);
        m_Player2_Attack = m_Player2.FindAction("Attack", throwIfNotFound: true);
        m_Player2_Taunt = m_Player2.FindAction("Taunt", throwIfNotFound: true);
        m_Player2_Defense = m_Player2.FindAction("Defense", throwIfNotFound: true);
        // Debug
        m_Debug = asset.FindActionMap("Debug", throwIfNotFound: true);
        m_Debug_ReloadCurrentScene = m_Debug.FindAction("Reload Current Scene", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Player1
    private readonly InputActionMap m_Player1;
    private IPlayer1Actions m_Player1ActionsCallbackInterface;
    private readonly InputAction m_Player1_Jump;
    private readonly InputAction m_Player1_MoveX;
    private readonly InputAction m_Player1_MoveZ;
    private readonly InputAction m_Player1_Attack;
    private readonly InputAction m_Player1_Defense;
    private readonly InputAction m_Player1_Taunt;
    public struct Player1Actions
    {
        private @PlayerInputActions m_Wrapper;
        public Player1Actions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Jump => m_Wrapper.m_Player1_Jump;
        public InputAction @MoveX => m_Wrapper.m_Player1_MoveX;
        public InputAction @MoveZ => m_Wrapper.m_Player1_MoveZ;
        public InputAction @Attack => m_Wrapper.m_Player1_Attack;
        public InputAction @Defense => m_Wrapper.m_Player1_Defense;
        public InputAction @Taunt => m_Wrapper.m_Player1_Taunt;
        public InputActionMap Get() { return m_Wrapper.m_Player1; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(Player1Actions set) { return set.Get(); }
        public void SetCallbacks(IPlayer1Actions instance)
        {
            if (m_Wrapper.m_Player1ActionsCallbackInterface != null)
            {
                @Jump.started -= m_Wrapper.m_Player1ActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_Player1ActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_Player1ActionsCallbackInterface.OnJump;
                @MoveX.started -= m_Wrapper.m_Player1ActionsCallbackInterface.OnMoveX;
                @MoveX.performed -= m_Wrapper.m_Player1ActionsCallbackInterface.OnMoveX;
                @MoveX.canceled -= m_Wrapper.m_Player1ActionsCallbackInterface.OnMoveX;
                @MoveZ.started -= m_Wrapper.m_Player1ActionsCallbackInterface.OnMoveZ;
                @MoveZ.performed -= m_Wrapper.m_Player1ActionsCallbackInterface.OnMoveZ;
                @MoveZ.canceled -= m_Wrapper.m_Player1ActionsCallbackInterface.OnMoveZ;
                @Attack.started -= m_Wrapper.m_Player1ActionsCallbackInterface.OnAttack;
                @Attack.performed -= m_Wrapper.m_Player1ActionsCallbackInterface.OnAttack;
                @Attack.canceled -= m_Wrapper.m_Player1ActionsCallbackInterface.OnAttack;
                @Defense.started -= m_Wrapper.m_Player1ActionsCallbackInterface.OnDefense;
                @Defense.performed -= m_Wrapper.m_Player1ActionsCallbackInterface.OnDefense;
                @Defense.canceled -= m_Wrapper.m_Player1ActionsCallbackInterface.OnDefense;
                @Taunt.started -= m_Wrapper.m_Player1ActionsCallbackInterface.OnTaunt;
                @Taunt.performed -= m_Wrapper.m_Player1ActionsCallbackInterface.OnTaunt;
                @Taunt.canceled -= m_Wrapper.m_Player1ActionsCallbackInterface.OnTaunt;
            }
            m_Wrapper.m_Player1ActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @MoveX.started += instance.OnMoveX;
                @MoveX.performed += instance.OnMoveX;
                @MoveX.canceled += instance.OnMoveX;
                @MoveZ.started += instance.OnMoveZ;
                @MoveZ.performed += instance.OnMoveZ;
                @MoveZ.canceled += instance.OnMoveZ;
                @Attack.started += instance.OnAttack;
                @Attack.performed += instance.OnAttack;
                @Attack.canceled += instance.OnAttack;
                @Defense.started += instance.OnDefense;
                @Defense.performed += instance.OnDefense;
                @Defense.canceled += instance.OnDefense;
                @Taunt.started += instance.OnTaunt;
                @Taunt.performed += instance.OnTaunt;
                @Taunt.canceled += instance.OnTaunt;
            }
        }
    }
    public Player1Actions @Player1 => new Player1Actions(this);

    // Player2
    private readonly InputActionMap m_Player2;
    private IPlayer2Actions m_Player2ActionsCallbackInterface;
    private readonly InputAction m_Player2_Jump;
    private readonly InputAction m_Player2_MoveX;
    private readonly InputAction m_Player2_MoveZ;
    private readonly InputAction m_Player2_Attack;
    private readonly InputAction m_Player2_Taunt;
    private readonly InputAction m_Player2_Defense;
    public struct Player2Actions
    {
        private @PlayerInputActions m_Wrapper;
        public Player2Actions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Jump => m_Wrapper.m_Player2_Jump;
        public InputAction @MoveX => m_Wrapper.m_Player2_MoveX;
        public InputAction @MoveZ => m_Wrapper.m_Player2_MoveZ;
        public InputAction @Attack => m_Wrapper.m_Player2_Attack;
        public InputAction @Taunt => m_Wrapper.m_Player2_Taunt;
        public InputAction @Defense => m_Wrapper.m_Player2_Defense;
        public InputActionMap Get() { return m_Wrapper.m_Player2; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(Player2Actions set) { return set.Get(); }
        public void SetCallbacks(IPlayer2Actions instance)
        {
            if (m_Wrapper.m_Player2ActionsCallbackInterface != null)
            {
                @Jump.started -= m_Wrapper.m_Player2ActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_Player2ActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_Player2ActionsCallbackInterface.OnJump;
                @MoveX.started -= m_Wrapper.m_Player2ActionsCallbackInterface.OnMoveX;
                @MoveX.performed -= m_Wrapper.m_Player2ActionsCallbackInterface.OnMoveX;
                @MoveX.canceled -= m_Wrapper.m_Player2ActionsCallbackInterface.OnMoveX;
                @MoveZ.started -= m_Wrapper.m_Player2ActionsCallbackInterface.OnMoveZ;
                @MoveZ.performed -= m_Wrapper.m_Player2ActionsCallbackInterface.OnMoveZ;
                @MoveZ.canceled -= m_Wrapper.m_Player2ActionsCallbackInterface.OnMoveZ;
                @Attack.started -= m_Wrapper.m_Player2ActionsCallbackInterface.OnAttack;
                @Attack.performed -= m_Wrapper.m_Player2ActionsCallbackInterface.OnAttack;
                @Attack.canceled -= m_Wrapper.m_Player2ActionsCallbackInterface.OnAttack;
                @Taunt.started -= m_Wrapper.m_Player2ActionsCallbackInterface.OnTaunt;
                @Taunt.performed -= m_Wrapper.m_Player2ActionsCallbackInterface.OnTaunt;
                @Taunt.canceled -= m_Wrapper.m_Player2ActionsCallbackInterface.OnTaunt;
                @Defense.started -= m_Wrapper.m_Player2ActionsCallbackInterface.OnDefense;
                @Defense.performed -= m_Wrapper.m_Player2ActionsCallbackInterface.OnDefense;
                @Defense.canceled -= m_Wrapper.m_Player2ActionsCallbackInterface.OnDefense;
            }
            m_Wrapper.m_Player2ActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @MoveX.started += instance.OnMoveX;
                @MoveX.performed += instance.OnMoveX;
                @MoveX.canceled += instance.OnMoveX;
                @MoveZ.started += instance.OnMoveZ;
                @MoveZ.performed += instance.OnMoveZ;
                @MoveZ.canceled += instance.OnMoveZ;
                @Attack.started += instance.OnAttack;
                @Attack.performed += instance.OnAttack;
                @Attack.canceled += instance.OnAttack;
                @Taunt.started += instance.OnTaunt;
                @Taunt.performed += instance.OnTaunt;
                @Taunt.canceled += instance.OnTaunt;
                @Defense.started += instance.OnDefense;
                @Defense.performed += instance.OnDefense;
                @Defense.canceled += instance.OnDefense;
            }
        }
    }
    public Player2Actions @Player2 => new Player2Actions(this);

    // Debug
    private readonly InputActionMap m_Debug;
    private IDebugActions m_DebugActionsCallbackInterface;
    private readonly InputAction m_Debug_ReloadCurrentScene;
    public struct DebugActions
    {
        private @PlayerInputActions m_Wrapper;
        public DebugActions(@PlayerInputActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @ReloadCurrentScene => m_Wrapper.m_Debug_ReloadCurrentScene;
        public InputActionMap Get() { return m_Wrapper.m_Debug; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DebugActions set) { return set.Get(); }
        public void SetCallbacks(IDebugActions instance)
        {
            if (m_Wrapper.m_DebugActionsCallbackInterface != null)
            {
                @ReloadCurrentScene.started -= m_Wrapper.m_DebugActionsCallbackInterface.OnReloadCurrentScene;
                @ReloadCurrentScene.performed -= m_Wrapper.m_DebugActionsCallbackInterface.OnReloadCurrentScene;
                @ReloadCurrentScene.canceled -= m_Wrapper.m_DebugActionsCallbackInterface.OnReloadCurrentScene;
            }
            m_Wrapper.m_DebugActionsCallbackInterface = instance;
            if (instance != null)
            {
                @ReloadCurrentScene.started += instance.OnReloadCurrentScene;
                @ReloadCurrentScene.performed += instance.OnReloadCurrentScene;
                @ReloadCurrentScene.canceled += instance.OnReloadCurrentScene;
            }
        }
    }
    public DebugActions @Debug => new DebugActions(this);
    public interface IPlayer1Actions
    {
        void OnJump(InputAction.CallbackContext context);
        void OnMoveX(InputAction.CallbackContext context);
        void OnMoveZ(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
        void OnDefense(InputAction.CallbackContext context);
        void OnTaunt(InputAction.CallbackContext context);
    }
    public interface IPlayer2Actions
    {
        void OnJump(InputAction.CallbackContext context);
        void OnMoveX(InputAction.CallbackContext context);
        void OnMoveZ(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
        void OnTaunt(InputAction.CallbackContext context);
        void OnDefense(InputAction.CallbackContext context);
    }
    public interface IDebugActions
    {
        void OnReloadCurrentScene(InputAction.CallbackContext context);
    }
}
