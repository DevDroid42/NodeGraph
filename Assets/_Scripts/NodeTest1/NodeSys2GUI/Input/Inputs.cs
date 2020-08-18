// GENERATED AUTOMATICALLY FROM 'Assets/_Scripts/NodeTest1/NodeSys2GUI/Input/Inputs.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @Inputs : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @Inputs()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Inputs"",
    ""maps"": [
        {
            ""name"": ""Main"",
            ""id"": ""454f63ef-e183-45b4-a53f-0787633ffcae"",
            ""actions"": [
                {
                    ""name"": ""zoom"",
                    ""type"": ""Value"",
                    ""id"": ""1ca91a50-1d5e-4086-82a2-54e69ccbbb35"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pan"",
                    ""type"": ""Value"",
                    ""id"": ""0337515e-1b33-400d-a29e-da9692545f8f"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""OpenNodeMenu"",
                    ""type"": ""Button"",
                    ""id"": ""0e3b8a8b-60ae-4360-b672-fa6c05688f52"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Select"",
                    ""type"": ""Value"",
                    ""id"": ""556b71a9-9f75-4219-b640-4c045cc265f3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""1eef73b8-1bd6-4862-b1ee-0fc48350b4bc"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Drag"",
                    ""id"": ""24d7bf56-7908-443b-b85f-de78296f5307"",
                    ""path"": ""ButtonWithTwoModifiers"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pan"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""modifier1"",
                    ""id"": ""792b706b-38a7-4cb8-b524-e1f8ee9ba4c4"",
                    ""path"": ""<Mouse>/delta/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Pan"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""modifier2"",
                    ""id"": ""3326690c-803b-438e-9c3b-f05475c58b94"",
                    ""path"": ""<Mouse>/delta/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Pan"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""button"",
                    ""id"": ""16fc7bf2-de62-444e-a577-eee9e1fea587"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Pan"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""232405e3-8a1b-4f91-8ec3-ee8118592ecd"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""OpenNodeMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c4146eb7-5937-45a0-87ce-d30037c87ddf"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyboardMouse"",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""KeyboardMouse"",
            ""bindingGroup"": ""KeyboardMouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Touch"",
            ""bindingGroup"": ""Touch"",
            ""devices"": [
                {
                    ""devicePath"": ""<Touchscreen>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Main
        m_Main = asset.FindActionMap("Main", throwIfNotFound: true);
        m_Main_zoom = m_Main.FindAction("zoom", throwIfNotFound: true);
        m_Main_Pan = m_Main.FindAction("Pan", throwIfNotFound: true);
        m_Main_OpenNodeMenu = m_Main.FindAction("OpenNodeMenu", throwIfNotFound: true);
        m_Main_Select = m_Main.FindAction("Select", throwIfNotFound: true);
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

    // Main
    private readonly InputActionMap m_Main;
    private IMainActions m_MainActionsCallbackInterface;
    private readonly InputAction m_Main_zoom;
    private readonly InputAction m_Main_Pan;
    private readonly InputAction m_Main_OpenNodeMenu;
    private readonly InputAction m_Main_Select;
    public struct MainActions
    {
        private @Inputs m_Wrapper;
        public MainActions(@Inputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @zoom => m_Wrapper.m_Main_zoom;
        public InputAction @Pan => m_Wrapper.m_Main_Pan;
        public InputAction @OpenNodeMenu => m_Wrapper.m_Main_OpenNodeMenu;
        public InputAction @Select => m_Wrapper.m_Main_Select;
        public InputActionMap Get() { return m_Wrapper.m_Main; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MainActions set) { return set.Get(); }
        public void SetCallbacks(IMainActions instance)
        {
            if (m_Wrapper.m_MainActionsCallbackInterface != null)
            {
                @zoom.started -= m_Wrapper.m_MainActionsCallbackInterface.OnZoom;
                @zoom.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnZoom;
                @zoom.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnZoom;
                @Pan.started -= m_Wrapper.m_MainActionsCallbackInterface.OnPan;
                @Pan.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnPan;
                @Pan.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnPan;
                @OpenNodeMenu.started -= m_Wrapper.m_MainActionsCallbackInterface.OnOpenNodeMenu;
                @OpenNodeMenu.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnOpenNodeMenu;
                @OpenNodeMenu.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnOpenNodeMenu;
                @Select.started -= m_Wrapper.m_MainActionsCallbackInterface.OnSelect;
                @Select.performed -= m_Wrapper.m_MainActionsCallbackInterface.OnSelect;
                @Select.canceled -= m_Wrapper.m_MainActionsCallbackInterface.OnSelect;
            }
            m_Wrapper.m_MainActionsCallbackInterface = instance;
            if (instance != null)
            {
                @zoom.started += instance.OnZoom;
                @zoom.performed += instance.OnZoom;
                @zoom.canceled += instance.OnZoom;
                @Pan.started += instance.OnPan;
                @Pan.performed += instance.OnPan;
                @Pan.canceled += instance.OnPan;
                @OpenNodeMenu.started += instance.OnOpenNodeMenu;
                @OpenNodeMenu.performed += instance.OnOpenNodeMenu;
                @OpenNodeMenu.canceled += instance.OnOpenNodeMenu;
                @Select.started += instance.OnSelect;
                @Select.performed += instance.OnSelect;
                @Select.canceled += instance.OnSelect;
            }
        }
    }
    public MainActions @Main => new MainActions(this);
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("KeyboardMouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    private int m_TouchSchemeIndex = -1;
    public InputControlScheme TouchScheme
    {
        get
        {
            if (m_TouchSchemeIndex == -1) m_TouchSchemeIndex = asset.FindControlSchemeIndex("Touch");
            return asset.controlSchemes[m_TouchSchemeIndex];
        }
    }
    public interface IMainActions
    {
        void OnZoom(InputAction.CallbackContext context);
        void OnPan(InputAction.CallbackContext context);
        void OnOpenNodeMenu(InputAction.CallbackContext context);
        void OnSelect(InputAction.CallbackContext context);
    }
}
