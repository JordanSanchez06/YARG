using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using System.Linq; // Required for the .OfType<T>() method

public class VirtualStrumBarDeviceManager : MonoBehaviour
{
    public static VirtualStrumBarDeviceManager Instance { get; private set; }
    public VirtualStrumButtons Touchpad { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeVirtualDevice();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeVirtualDevice()
    {
        EnhancedTouchSupport.Enable();
        InputSystem.RegisterLayout<VirtualStrumButtons>("VirtualTouchpad");

        // CHANGED: This section now prevents duplicate devices.
        // First, check if a device of this type already exists in the input system.
        // This can happen when re-entering Play Mode in the editor.
        Touchpad = InputSystem.devices.OfType<VirtualStrumButtons>().FirstOrDefault();

        // If no device was found, then it's safe to create a new one.
        if (Touchpad == null)
        {
            Touchpad = InputSystem.AddDevice<VirtualStrumButtons>("OnScreenTouch");
        }
    }
}

