//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/ZombieControl.inputactions
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

public partial class @ZombieControl: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @ZombieControl()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""ZombieControl"",
    ""maps"": [
        {
            ""name"": ""ZombieMovement"",
            ""id"": ""1682bbd8-5862-4400-9a6f-adf77fb529a2"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""85163f74-da4d-48c5-95a9-6c21c7c52ae1"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Run"",
                    ""type"": ""Button"",
                    ""id"": ""6031183d-c1b1-4f1d-a1b5-b99af7df618a"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""8379dc7c-2d35-4e2e-ac5c-c963bac22207"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Scream"",
                    ""type"": ""Button"",
                    ""id"": ""93322ed3-1476-460d-a8c6-2fe812c8dbf6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""296ffc22-21c4-4524-b593-fa1d107913cd"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""af18cbe2-2e37-4441-9081-df095f55224a"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cdb55c5d-35bc-405f-810c-828d529f4c61"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": ""Hold(pressPoint=0.1)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d4a64c9d-80bd-42a0-b402-72c67108705f"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Scream"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""ZombieMovement1"",
            ""id"": ""d0760275-1840-40ad-aee1-a9a626fe6008"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""85e55a38-a6e4-49fe-9fd6-6984744d482e"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Run"",
                    ""type"": ""Button"",
                    ""id"": ""623fb878-a1f4-439a-b1da-1bf2d84a509e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""f9e64a3d-5a1a-456b-807c-14cb1f8ce14d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Scream"",
                    ""type"": ""Button"",
                    ""id"": ""8ab069e3-ec38-4369-88dc-482ac4293ef7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""fe34f7f6-2d98-4ecd-9751-c1eee627817d"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""91338ad3-90a4-4043-bcdc-a9d8f3d5fb69"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fadebc31-4d0c-44c9-8ba4-1eed1d8c4f95"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": ""Hold(pressPoint=0.1)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2233720c-30c6-4971-8e27-2e09547740b4"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Scream"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""ZombieMovement2"",
            ""id"": ""45f94e9f-3ef6-48a0-ba5c-204ad6c9473b"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""aeccbed4-29cd-406a-9aab-6ae7ab68719f"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Run"",
                    ""type"": ""Button"",
                    ""id"": ""5c63db6b-3500-4b4c-be5f-09726c4ee945"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""11608282-be11-47eb-9c22-346a38a7f413"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Scream"",
                    ""type"": ""Button"",
                    ""id"": ""aa650b60-b8a2-43c2-a6ea-02159c62d480"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""3ed8dfd5-4bc5-4edc-b999-57f67eb5cf56"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""00f36619-329d-4b7f-bfec-e6e66c6d1119"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""43e8ef61-6291-4369-bded-a7e07f6cf68b"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": ""Hold(pressPoint=0.1)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f47929fd-370f-4545-b7e9-31d97faf2f24"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Scream"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""ZombieMovement3"",
            ""id"": ""664df108-0961-4a9e-93b2-85b583959985"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""PassThrough"",
                    ""id"": ""6aeea4db-51d8-432e-9c22-7b13a5b263ab"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Run"",
                    ""type"": ""Button"",
                    ""id"": ""16e358ed-c336-48fb-9432-29ec4557e4c1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""2e1107ef-cfb5-4d3f-b623-b4720eaa9cb8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Scream"",
                    ""type"": ""Button"",
                    ""id"": ""da6c5382-c4fb-46db-84ec-f26b870622a5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""9cb84591-c91e-40e7-b233-cb3b2c7bd2d1"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7020803a-dbd1-47fd-b956-6cce32926fc4"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ef7f9d9d-fae0-4ce1-a3b6-09788e3d18c6"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": ""Hold(pressPoint=0.1)"",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a7a9564c-8ee2-462a-9e89-ef1560f7badb"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Scream"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // ZombieMovement
        m_ZombieMovement = asset.FindActionMap("ZombieMovement", throwIfNotFound: true);
        m_ZombieMovement_Movement = m_ZombieMovement.FindAction("Movement", throwIfNotFound: true);
        m_ZombieMovement_Run = m_ZombieMovement.FindAction("Run", throwIfNotFound: true);
        m_ZombieMovement_Attack = m_ZombieMovement.FindAction("Attack", throwIfNotFound: true);
        m_ZombieMovement_Scream = m_ZombieMovement.FindAction("Scream", throwIfNotFound: true);
        // ZombieMovement1
        m_ZombieMovement1 = asset.FindActionMap("ZombieMovement1", throwIfNotFound: true);
        m_ZombieMovement1_Movement = m_ZombieMovement1.FindAction("Movement", throwIfNotFound: true);
        m_ZombieMovement1_Run = m_ZombieMovement1.FindAction("Run", throwIfNotFound: true);
        m_ZombieMovement1_Attack = m_ZombieMovement1.FindAction("Attack", throwIfNotFound: true);
        m_ZombieMovement1_Scream = m_ZombieMovement1.FindAction("Scream", throwIfNotFound: true);
        // ZombieMovement2
        m_ZombieMovement2 = asset.FindActionMap("ZombieMovement2", throwIfNotFound: true);
        m_ZombieMovement2_Movement = m_ZombieMovement2.FindAction("Movement", throwIfNotFound: true);
        m_ZombieMovement2_Run = m_ZombieMovement2.FindAction("Run", throwIfNotFound: true);
        m_ZombieMovement2_Attack = m_ZombieMovement2.FindAction("Attack", throwIfNotFound: true);
        m_ZombieMovement2_Scream = m_ZombieMovement2.FindAction("Scream", throwIfNotFound: true);
        // ZombieMovement3
        m_ZombieMovement3 = asset.FindActionMap("ZombieMovement3", throwIfNotFound: true);
        m_ZombieMovement3_Movement = m_ZombieMovement3.FindAction("Movement", throwIfNotFound: true);
        m_ZombieMovement3_Run = m_ZombieMovement3.FindAction("Run", throwIfNotFound: true);
        m_ZombieMovement3_Attack = m_ZombieMovement3.FindAction("Attack", throwIfNotFound: true);
        m_ZombieMovement3_Scream = m_ZombieMovement3.FindAction("Scream", throwIfNotFound: true);
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

    // ZombieMovement
    private readonly InputActionMap m_ZombieMovement;
    private List<IZombieMovementActions> m_ZombieMovementActionsCallbackInterfaces = new List<IZombieMovementActions>();
    private readonly InputAction m_ZombieMovement_Movement;
    private readonly InputAction m_ZombieMovement_Run;
    private readonly InputAction m_ZombieMovement_Attack;
    private readonly InputAction m_ZombieMovement_Scream;
    public struct ZombieMovementActions
    {
        private @ZombieControl m_Wrapper;
        public ZombieMovementActions(@ZombieControl wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_ZombieMovement_Movement;
        public InputAction @Run => m_Wrapper.m_ZombieMovement_Run;
        public InputAction @Attack => m_Wrapper.m_ZombieMovement_Attack;
        public InputAction @Scream => m_Wrapper.m_ZombieMovement_Scream;
        public InputActionMap Get() { return m_Wrapper.m_ZombieMovement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ZombieMovementActions set) { return set.Get(); }
        public void AddCallbacks(IZombieMovementActions instance)
        {
            if (instance == null || m_Wrapper.m_ZombieMovementActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_ZombieMovementActionsCallbackInterfaces.Add(instance);
            @Movement.started += instance.OnMovement;
            @Movement.performed += instance.OnMovement;
            @Movement.canceled += instance.OnMovement;
            @Run.started += instance.OnRun;
            @Run.performed += instance.OnRun;
            @Run.canceled += instance.OnRun;
            @Attack.started += instance.OnAttack;
            @Attack.performed += instance.OnAttack;
            @Attack.canceled += instance.OnAttack;
            @Scream.started += instance.OnScream;
            @Scream.performed += instance.OnScream;
            @Scream.canceled += instance.OnScream;
        }

        private void UnregisterCallbacks(IZombieMovementActions instance)
        {
            @Movement.started -= instance.OnMovement;
            @Movement.performed -= instance.OnMovement;
            @Movement.canceled -= instance.OnMovement;
            @Run.started -= instance.OnRun;
            @Run.performed -= instance.OnRun;
            @Run.canceled -= instance.OnRun;
            @Attack.started -= instance.OnAttack;
            @Attack.performed -= instance.OnAttack;
            @Attack.canceled -= instance.OnAttack;
            @Scream.started -= instance.OnScream;
            @Scream.performed -= instance.OnScream;
            @Scream.canceled -= instance.OnScream;
        }

        public void RemoveCallbacks(IZombieMovementActions instance)
        {
            if (m_Wrapper.m_ZombieMovementActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IZombieMovementActions instance)
        {
            foreach (var item in m_Wrapper.m_ZombieMovementActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_ZombieMovementActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public ZombieMovementActions @ZombieMovement => new ZombieMovementActions(this);

    // ZombieMovement1
    private readonly InputActionMap m_ZombieMovement1;
    private List<IZombieMovement1Actions> m_ZombieMovement1ActionsCallbackInterfaces = new List<IZombieMovement1Actions>();
    private readonly InputAction m_ZombieMovement1_Movement;
    private readonly InputAction m_ZombieMovement1_Run;
    private readonly InputAction m_ZombieMovement1_Attack;
    private readonly InputAction m_ZombieMovement1_Scream;
    public struct ZombieMovement1Actions
    {
        private @ZombieControl m_Wrapper;
        public ZombieMovement1Actions(@ZombieControl wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_ZombieMovement1_Movement;
        public InputAction @Run => m_Wrapper.m_ZombieMovement1_Run;
        public InputAction @Attack => m_Wrapper.m_ZombieMovement1_Attack;
        public InputAction @Scream => m_Wrapper.m_ZombieMovement1_Scream;
        public InputActionMap Get() { return m_Wrapper.m_ZombieMovement1; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ZombieMovement1Actions set) { return set.Get(); }
        public void AddCallbacks(IZombieMovement1Actions instance)
        {
            if (instance == null || m_Wrapper.m_ZombieMovement1ActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_ZombieMovement1ActionsCallbackInterfaces.Add(instance);
            @Movement.started += instance.OnMovement;
            @Movement.performed += instance.OnMovement;
            @Movement.canceled += instance.OnMovement;
            @Run.started += instance.OnRun;
            @Run.performed += instance.OnRun;
            @Run.canceled += instance.OnRun;
            @Attack.started += instance.OnAttack;
            @Attack.performed += instance.OnAttack;
            @Attack.canceled += instance.OnAttack;
            @Scream.started += instance.OnScream;
            @Scream.performed += instance.OnScream;
            @Scream.canceled += instance.OnScream;
        }

        private void UnregisterCallbacks(IZombieMovement1Actions instance)
        {
            @Movement.started -= instance.OnMovement;
            @Movement.performed -= instance.OnMovement;
            @Movement.canceled -= instance.OnMovement;
            @Run.started -= instance.OnRun;
            @Run.performed -= instance.OnRun;
            @Run.canceled -= instance.OnRun;
            @Attack.started -= instance.OnAttack;
            @Attack.performed -= instance.OnAttack;
            @Attack.canceled -= instance.OnAttack;
            @Scream.started -= instance.OnScream;
            @Scream.performed -= instance.OnScream;
            @Scream.canceled -= instance.OnScream;
        }

        public void RemoveCallbacks(IZombieMovement1Actions instance)
        {
            if (m_Wrapper.m_ZombieMovement1ActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IZombieMovement1Actions instance)
        {
            foreach (var item in m_Wrapper.m_ZombieMovement1ActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_ZombieMovement1ActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public ZombieMovement1Actions @ZombieMovement1 => new ZombieMovement1Actions(this);

    // ZombieMovement2
    private readonly InputActionMap m_ZombieMovement2;
    private List<IZombieMovement2Actions> m_ZombieMovement2ActionsCallbackInterfaces = new List<IZombieMovement2Actions>();
    private readonly InputAction m_ZombieMovement2_Movement;
    private readonly InputAction m_ZombieMovement2_Run;
    private readonly InputAction m_ZombieMovement2_Attack;
    private readonly InputAction m_ZombieMovement2_Scream;
    public struct ZombieMovement2Actions
    {
        private @ZombieControl m_Wrapper;
        public ZombieMovement2Actions(@ZombieControl wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_ZombieMovement2_Movement;
        public InputAction @Run => m_Wrapper.m_ZombieMovement2_Run;
        public InputAction @Attack => m_Wrapper.m_ZombieMovement2_Attack;
        public InputAction @Scream => m_Wrapper.m_ZombieMovement2_Scream;
        public InputActionMap Get() { return m_Wrapper.m_ZombieMovement2; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ZombieMovement2Actions set) { return set.Get(); }
        public void AddCallbacks(IZombieMovement2Actions instance)
        {
            if (instance == null || m_Wrapper.m_ZombieMovement2ActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_ZombieMovement2ActionsCallbackInterfaces.Add(instance);
            @Movement.started += instance.OnMovement;
            @Movement.performed += instance.OnMovement;
            @Movement.canceled += instance.OnMovement;
            @Run.started += instance.OnRun;
            @Run.performed += instance.OnRun;
            @Run.canceled += instance.OnRun;
            @Attack.started += instance.OnAttack;
            @Attack.performed += instance.OnAttack;
            @Attack.canceled += instance.OnAttack;
            @Scream.started += instance.OnScream;
            @Scream.performed += instance.OnScream;
            @Scream.canceled += instance.OnScream;
        }

        private void UnregisterCallbacks(IZombieMovement2Actions instance)
        {
            @Movement.started -= instance.OnMovement;
            @Movement.performed -= instance.OnMovement;
            @Movement.canceled -= instance.OnMovement;
            @Run.started -= instance.OnRun;
            @Run.performed -= instance.OnRun;
            @Run.canceled -= instance.OnRun;
            @Attack.started -= instance.OnAttack;
            @Attack.performed -= instance.OnAttack;
            @Attack.canceled -= instance.OnAttack;
            @Scream.started -= instance.OnScream;
            @Scream.performed -= instance.OnScream;
            @Scream.canceled -= instance.OnScream;
        }

        public void RemoveCallbacks(IZombieMovement2Actions instance)
        {
            if (m_Wrapper.m_ZombieMovement2ActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IZombieMovement2Actions instance)
        {
            foreach (var item in m_Wrapper.m_ZombieMovement2ActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_ZombieMovement2ActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public ZombieMovement2Actions @ZombieMovement2 => new ZombieMovement2Actions(this);

    // ZombieMovement3
    private readonly InputActionMap m_ZombieMovement3;
    private List<IZombieMovement3Actions> m_ZombieMovement3ActionsCallbackInterfaces = new List<IZombieMovement3Actions>();
    private readonly InputAction m_ZombieMovement3_Movement;
    private readonly InputAction m_ZombieMovement3_Run;
    private readonly InputAction m_ZombieMovement3_Attack;
    private readonly InputAction m_ZombieMovement3_Scream;
    public struct ZombieMovement3Actions
    {
        private @ZombieControl m_Wrapper;
        public ZombieMovement3Actions(@ZombieControl wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_ZombieMovement3_Movement;
        public InputAction @Run => m_Wrapper.m_ZombieMovement3_Run;
        public InputAction @Attack => m_Wrapper.m_ZombieMovement3_Attack;
        public InputAction @Scream => m_Wrapper.m_ZombieMovement3_Scream;
        public InputActionMap Get() { return m_Wrapper.m_ZombieMovement3; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ZombieMovement3Actions set) { return set.Get(); }
        public void AddCallbacks(IZombieMovement3Actions instance)
        {
            if (instance == null || m_Wrapper.m_ZombieMovement3ActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_ZombieMovement3ActionsCallbackInterfaces.Add(instance);
            @Movement.started += instance.OnMovement;
            @Movement.performed += instance.OnMovement;
            @Movement.canceled += instance.OnMovement;
            @Run.started += instance.OnRun;
            @Run.performed += instance.OnRun;
            @Run.canceled += instance.OnRun;
            @Attack.started += instance.OnAttack;
            @Attack.performed += instance.OnAttack;
            @Attack.canceled += instance.OnAttack;
            @Scream.started += instance.OnScream;
            @Scream.performed += instance.OnScream;
            @Scream.canceled += instance.OnScream;
        }

        private void UnregisterCallbacks(IZombieMovement3Actions instance)
        {
            @Movement.started -= instance.OnMovement;
            @Movement.performed -= instance.OnMovement;
            @Movement.canceled -= instance.OnMovement;
            @Run.started -= instance.OnRun;
            @Run.performed -= instance.OnRun;
            @Run.canceled -= instance.OnRun;
            @Attack.started -= instance.OnAttack;
            @Attack.performed -= instance.OnAttack;
            @Attack.canceled -= instance.OnAttack;
            @Scream.started -= instance.OnScream;
            @Scream.performed -= instance.OnScream;
            @Scream.canceled -= instance.OnScream;
        }

        public void RemoveCallbacks(IZombieMovement3Actions instance)
        {
            if (m_Wrapper.m_ZombieMovement3ActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IZombieMovement3Actions instance)
        {
            foreach (var item in m_Wrapper.m_ZombieMovement3ActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_ZombieMovement3ActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public ZombieMovement3Actions @ZombieMovement3 => new ZombieMovement3Actions(this);
    public interface IZombieMovementActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnRun(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
        void OnScream(InputAction.CallbackContext context);
    }
    public interface IZombieMovement1Actions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnRun(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
        void OnScream(InputAction.CallbackContext context);
    }
    public interface IZombieMovement2Actions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnRun(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
        void OnScream(InputAction.CallbackContext context);
    }
    public interface IZombieMovement3Actions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnRun(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
        void OnScream(InputAction.CallbackContext context);
    }
}
