using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using System.Linq; // Required for the .OfType<T>() method

public class VirtualFretDeviceManager : MonoBehaviour
{
    public static VirtualFretDeviceManager Instance { get; private set; }
    public VirtualFretButtons FretDevice { get; private set; }

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
        InputSystem.RegisterLayout<VirtualFretButtons>("VirtualFret");

        // Prevent duplicate devices
        FretDevice = InputSystem.devices.OfType<VirtualFretButtons>().FirstOrDefault();

        // If no device exists, create a new one
        if (FretDevice == null)
        {
            FretDevice = InputSystem.AddDevice<VirtualFretButtons>("OnScreenFret");
        }
    }
}
