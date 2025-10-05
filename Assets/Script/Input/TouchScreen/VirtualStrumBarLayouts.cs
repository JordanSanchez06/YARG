using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.LowLevel;

public struct VirtualTouchpadState : IInputStateTypeInfo
{
    public FourCC format => new FourCC('V', 'T', 'P', 'D');

    // Bit positions for each control
    public const int LEFT_TAP_BIT = 0;
    public const int LEFT_RELEASE_BIT = 1;
    public const int RIGHT_TAP_BIT = 2;
    public const int RIGHT_RELEASE_BIT = 3;

    // Convenience masks
    public const int LEFT_TAP_MASK = 1 << LEFT_TAP_BIT;
    public const int LEFT_RELEASE_MASK = 1 << LEFT_RELEASE_BIT;
    public const int RIGHT_TAP_MASK = 1 << RIGHT_TAP_BIT;
    public const int RIGHT_RELEASE_MASK = 1 << RIGHT_RELEASE_BIT;

    [InputControl(name = "leftTap", displayName = "Left Tap", layout = "Button", bit = LEFT_TAP_BIT)]
    [InputControl(name = "leftRelease", displayName = "Left Release", layout = "Button", bit = LEFT_RELEASE_BIT)]
    [InputControl(name = "rightTap", displayName = "Right Tap", layout = "Button", bit = RIGHT_TAP_BIT)]
    [InputControl(name = "rightRelease", displayName = "Right Release", layout = "Button", bit = RIGHT_RELEASE_BIT)]
    public int buttons;
}

[InputControlLayout(stateType = typeof(VirtualTouchpadState), displayName = "On-Screen Buttons")]
public class VirtualTouchpadDevice : InputDevice
{
    public ButtonControl LeftTap { get; private set; }
    public ButtonControl LeftRelease { get; private set; }
    public ButtonControl RightTap { get; private set; }
    public ButtonControl RightRelease { get; private set; }

    protected override void FinishSetup()
    {
        base.FinishSetup();
        LeftTap = GetChildControl<ButtonControl>("leftTap");
        LeftRelease = GetChildControl<ButtonControl>("leftRelease");
        RightTap = GetChildControl<ButtonControl>("rightTap");
        RightRelease = GetChildControl<ButtonControl>("rightRelease");
    }
}
