using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.LowLevel;

public struct VirtualTouchpadState : IInputStateTypeInfo
{
    public FourCC format => new FourCC('V', 'T', 'P', 'D');

    [InputControl(name = "leftTap", displayName = "Left Tap", layout = "Button", bit = 0)]
    [InputControl(name = "leftRelease", displayName = "Left Release", layout = "Button", bit = 1)]
    [InputControl(name = "rightTap", displayName = "Right Tap", layout = "Button", bit = 2)]
    [InputControl(name = "rightRelease", displayName = "Right Release", layout = "Button", bit = 3)]
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