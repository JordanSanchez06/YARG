using YARG.Core;
using YARG.Core.Input;

namespace YARG.Input
{
    public partial class BindingCollection
    {
        public bool SetDefaultBindings(VirtualStrumButtons touchpad)
        {
            // This method splits the logic: one for gameplay, one for menus.
            return IsMenu ? SetDefaultMenuBindings(touchpad) : SetDefaultGameplayBindings(touchpad);
        }

        private bool SetDefaultGameplayBindings(VirtualStrumButtons touchpad)
        {
            // Call the correct binding method based on the current instrument.
            // For now, we'll just handle guitar-like instruments.
            return Mode switch
            {
                GameMode.FiveFretGuitar => SetDefaultGuitarBindings_VirtualTouchpad(touchpad),
                GameMode.SixFretGuitar => SetDefaultGuitarBindings_VirtualTouchpad(touchpad),
                _ => false
            };
        }

        private bool SetDefaultGuitarBindings_VirtualTouchpad(VirtualStrumButtons touchpad)
        {
            if (Mode != GameMode.FiveFretGuitar && Mode != GameMode.SixFretGuitar)
                return false;

            // Left side acts as an alt-strum (strum down on tap, strum up on release)
            AddBinding(GuitarAction.StrumDown, touchpad.LeftTap);
            AddBinding(GuitarAction.StrumUp, touchpad.LeftRelease);

            // Right side acts as a single-strum (strum down on tap only)
            AddBinding(GuitarAction.StrumDown, touchpad.RightTap);

            // Note: RightRelease is left unbound by default, but the player can map it
            // to something else (like Star Power) in the binding menu.

            return true;
        }

        private bool SetDefaultMenuBindings(VirtualStrumButtons touchpad)
        {
            if (!IsMenu)
                return false;

            // Provide some sensible defaults for navigating menus with the touch zones.
            //AddBinding(MenuAction.Up, touchpad.LeftRelease);

            AddBinding(MenuAction.Down, touchpad.LeftTap);
            AddBinding(MenuAction.Up, touchpad.RightTap);

            //AddBinding(MenuAction.Green, touchpad.RightTap);
            //AddBinding(MenuAction.Red, touchpad.RightRelease);

            return true;
        }
    }
}
