using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using System.Collections.Generic;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using System.Linq;
using UnityEngine.SceneManagement;
using YARG.Player;

public class TouchInputDriver : MonoBehaviour
{
    [Header("UI Zones")]
    [SerializeField]
    private RectTransform leftZone;
    [SerializeField]
    private RectTransform rightZone;

    private enum TouchStartLocation { None, Left, Right }
    private readonly Dictionary<int, TouchStartLocation> activeTouches = new Dictionary<int, TouchStartLocation>();

    private GameObject touchStrumCanvas;

    private VirtualTouchpadDevice touchpadDevice;

    private void Awake()
    {
        EnhancedTouchSupport.Enable();
        this.touchStrumCanvas = this.gameObject;
        UpdateButtonVisibility();
    }

    private void OnEnable()
    {
        touchpadDevice = VirtualDeviceManager.Instance?.Touchpad;
    }


    private void Update()
    {
        if (touchpadDevice == null)
        {
            // Keep trying to get the device if it wasn't ready during OnEnable
            touchpadDevice = VirtualDeviceManager.Instance?.Touchpad;
            if (touchpadDevice == null) return;
        }

        foreach (var touch in Touch.activeTouches)
        {
            if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
            {
                HandleTouchBegan(touch);
            }
            else if (touch.phase == UnityEngine.InputSystem.TouchPhase.Ended)
            {
                HandleTouchEnded(touch);
            }
        }
    }

    private void UpdateButtonVisibility()
    {
        bool showControls = false;

        foreach (var p in PlayerContainer.Players)
        {
            if (p.Bindings.InputDevices.Any(device => device is VirtualTouchpadDevice))
            {
                showControls = true;
                break;
            }
        }

        if (touchStrumCanvas != null)
        {
            touchStrumCanvas.SetActive(showControls);
        }
    }

    private void HandleTouchBegan(Touch touch)
    {
        var state = new VirtualTouchpadState();

        // For Screen Space - Overlay, the camera parameter is null.
        if (RectTransformUtility.RectangleContainsScreenPoint(leftZone, touch.screenPosition, null))
        {
            activeTouches[touch.touchId] = TouchStartLocation.Left;
            state.buttons |= 1 << 0;
            InputSystem.QueueStateEvent(touchpadDevice, state);
        }
        else if (RectTransformUtility.RectangleContainsScreenPoint(rightZone, touch.screenPosition, null))
        {
            activeTouches[touch.touchId] = TouchStartLocation.Right;
            state.buttons |= 1 << 2;
            InputSystem.QueueStateEvent(touchpadDevice, state);
        }
    }

    private void HandleTouchEnded(Touch touch)
    {
        if (activeTouches.TryGetValue(touch.touchId, out var startLocation))
        {
            var state = new VirtualTouchpadState();

            if (startLocation == TouchStartLocation.Left && RectTransformUtility.RectangleContainsScreenPoint(leftZone, touch.screenPosition, null))
            {
                state.buttons |= 1 << 1;
                InputSystem.QueueStateEvent(touchpadDevice, state);
            }
            else if (startLocation == TouchStartLocation.Right && RectTransformUtility.RectangleContainsScreenPoint(rightZone, touch.screenPosition, null))
            {
                state.buttons |= 1 << 3;
                InputSystem.QueueStateEvent(touchpadDevice, state);
            }

            activeTouches.Remove(touch.touchId);
        }
    }
}

