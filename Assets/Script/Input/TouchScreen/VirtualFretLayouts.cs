using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.LowLevel;

public struct VirtualFretState : IInputStateTypeInfo
{
    public FourCC format => new FourCC('V', 'F', 'R', 'T');

    // Bit positions for each fret button
    public const int FRET1_BIT = 0;
    public const int FRET2_BIT = 1;
    public const int FRET3_BIT = 2;
    public const int FRET4_BIT = 3;
    public const int FRET5_BIT = 4;

    // Convenience masks
    public const int FRET1_MASK = 1 << FRET1_BIT;
    public const int FRET2_MASK = 1 << FRET2_BIT;
    public const int FRET3_MASK = 1 << FRET3_BIT;
    public const int FRET4_MASK = 1 << FRET4_BIT;
    public const int FRET5_MASK = 1 << FRET5_BIT;

    [InputControl(name = "Fret1", displayName = "Fret 1", layout = "Button", bit = FRET1_BIT)]
    [InputControl(name = "Fret2", displayName = "Fret 2", layout = "Button", bit = FRET2_BIT)]
    [InputControl(name = "Fret3", displayName = "Fret 3", layout = "Button", bit = FRET3_BIT)]
    [InputControl(name = "Fret4", displayName = "Fret 4", layout = "Button", bit = FRET4_BIT)]
    [InputControl(name = "Fret5", displayName = "Fret 5", layout = "Button", bit = FRET5_BIT)]
    public int buttons;
}

[InputControlLayout(stateType = typeof(VirtualFretState), displayName = "On-Screen Fret Buttons")]
public class VirtualFretButtons : InputDevice
{
    public ButtonControl Fret1 { get; private set; }
    public ButtonControl Fret2 { get; private set; }
    public ButtonControl Fret3 { get; private set; }
    public ButtonControl Fret4 { get; private set; }
    public ButtonControl Fret5 { get; private set; }

    protected override void FinishSetup()
    {
        base.FinishSetup();
        Fret1 = GetChildControl<ButtonControl>("Fret1");
        Fret2 = GetChildControl<ButtonControl>("Fret2");
        Fret3 = GetChildControl<ButtonControl>("Fret3");
        Fret4 = GetChildControl<ButtonControl>("Fret4");
        Fret5 = GetChildControl<ButtonControl>("Fret5");
    }
}
