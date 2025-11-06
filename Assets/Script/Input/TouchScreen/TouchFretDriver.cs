using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using System.Collections.Generic;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using System.Linq;
using YARG.Player;

public class TouchFretDriver : MonoBehaviour
{
    [Header("UI Zones")]
    [SerializeField]
    private RectTransform fret1;
    [SerializeField]
    private RectTransform fret2;
    [SerializeField]
    private RectTransform fret3;
    [SerializeField]
    private RectTransform fret4;
    [SerializeField]
    private RectTransform fret5;

    private enum TouchFret { None, Fret1, Fret2, Fret3, Fret4, Fret5 }
    private readonly Dictionary<int, TouchFret> activeTouches = new Dictionary<int, TouchFret>();

    private GameObject touchFretCanvas;
    private VirtualFretButtons fretDevice;
    private VirtualFretState currentFretState;

    private void Awake()
    {
        EnhancedTouchSupport.Enable();
        this.touchFretCanvas = this.gameObject;
        UpdateButtonVisibility();
    }

    private void OnEnable()
    {
        fretDevice = VirtualFretDeviceManager.Instance?.FretDevice;
    }

    private void OnDisable()
    {
        // Ensure all frets are released when this object is disabled
        if (fretDevice != null && activeTouches.Count > 0)
        {
            activeTouches.Clear();
            RecalculateAndSendState();
        }
    }

    private void Update()
    {
        if (fretDevice == null)
        {
            fretDevice = VirtualFretDeviceManager.Instance?.FretDevice;
            if (fretDevice == null) return;
        }

        // Before processing new inputs, clean up any touches that may have ended abruptly
        CleanupStaleTouches();

        foreach (var touch in Touch.activeTouches)
        {
            switch (touch.phase)
            {
                case UnityEngine.InputSystem.TouchPhase.Began:
                    HandleTouchBegan(touch);
                    break;

                case UnityEngine.InputSystem.TouchPhase.Moved:
                    HandleTouchMoved(touch);
                    break;

                case UnityEngine.InputSystem.TouchPhase.Ended:
                case UnityEngine.InputSystem.TouchPhase.Canceled:
                    HandleTouchEnded(touch);
                    break;
            }
        }
    }

    private void UpdateButtonVisibility()
    {
        bool showControls = PlayerContainer.Players.Any(p =>
            p.Bindings.InputDevices.Any(d => d is VirtualFretButtons));

        if (touchFretCanvas != null)
            touchFretCanvas.SetActive(showControls);
    }

    private void HandleTouchBegan(Touch touch)
    {
        TouchFret fret = GetFretFromPosition(touch.screenPosition);
        activeTouches[touch.touchId] = fret;
        RecalculateAndSendState();
    }

    private void HandleTouchMoved(Touch touch)
    {
        TouchFret fret = GetFretFromPosition(touch.screenPosition);
        // Only update if the finger has slid to a new fret
        if (activeTouches.TryGetValue(touch.touchId, out var existingFret) && existingFret != fret)
        {
            activeTouches[touch.touchId] = fret;
            RecalculateAndSendState();
        }
    }

    private void HandleTouchEnded(Touch touch)
    {
        if (activeTouches.Remove(touch.touchId))
        {
            RecalculateAndSendState();
        }
    }

    // --- THIS IS THE CORRECTED METHOD ---
    private void CleanupStaleTouches()
    {
        // Get all touch IDs currently active in the input system
        var validTouchIds = new HashSet<int>();
        foreach (var touch in Touch.activeTouches)
        {
            validTouchIds.Add(touch.touchId);
        }

        // Find which of our tracked touches are no longer valid
        var touchesToRemove = new List<int>();
        foreach (var trackedTouchId in activeTouches.Keys)
        {
            if (!validTouchIds.Contains(trackedTouchId))
            {
                touchesToRemove.Add(trackedTouchId);
            }
        }

        // If we found any stale touches, remove them and update the state
        if (touchesToRemove.Count > 0)
        {
            foreach (var touchId in touchesToRemove)
            {
                activeTouches.Remove(touchId);
            }
            RecalculateAndSendState();
        }
    }

    private void RecalculateAndSendState()
    {
        // Start with all buttons released
        currentFretState.buttons = 0;

        // Combine the state of all currently held frets
        foreach (var fret in activeTouches.Values)
        {
            switch (fret)
            {
                case TouchFret.Fret1: currentFretState.buttons |= VirtualFretState.FRET1_MASK; break;
                case TouchFret.Fret2: currentFretState.buttons |= VirtualFretState.FRET2_MASK; break;
                case TouchFret.Fret3: currentFretState.buttons |= VirtualFretState.FRET3_MASK; break;
                case TouchFret.Fret4: currentFretState.buttons |= VirtualFretState.FRET4_MASK; break;
                case TouchFret.Fret5: currentFretState.buttons |= VirtualFretState.FRET5_MASK; break;
            }
        }

        if (fretDevice != null && fretDevice.added)
        {
            InputSystem.QueueStateEvent(fretDevice, currentFretState);
        }
    }

    private TouchFret GetFretFromPosition(Vector2 screenPosition)
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(fret1, screenPosition, null)) return TouchFret.Fret1;
        if (RectTransformUtility.RectangleContainsScreenPoint(fret2, screenPosition, null)) return TouchFret.Fret2;
        if (RectTransformUtility.RectangleContainsScreenPoint(fret3, screenPosition, null)) return TouchFret.Fret3;
        if (RectTransformUtility.RectangleContainsScreenPoint(fret4, screenPosition, null)) return TouchFret.Fret4;
        if (RectTransformUtility.RectangleContainsScreenPoint(fret5, screenPosition, null)) return TouchFret.Fret5;
        return TouchFret.None;
    }
}