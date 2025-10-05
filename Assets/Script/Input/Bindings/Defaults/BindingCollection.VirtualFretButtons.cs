using YARG.Core;
using YARG.Core.Input;

namespace YARG.Input
{
    public partial class BindingCollection
    {
        public bool SetDefaultBindings(VirtualFretButtons fretboard)
        {
            // This method splits the logic: one for gameplay, one for menus.
            return IsMenu ? SetDefaultMenuBindings(fretboard) : SetDefaultGameplayBindings(fretboard);
        }

        private bool SetDefaultGameplayBindings(VirtualFretButtons fretboard)
        {
            // Call the correct binding method based on the current instrument.
            return Mode switch
            {
                GameMode.FiveFretGuitar => SetDefaultGuitarBindings_VirtualFretboard(fretboard),
                GameMode.SixFretGuitar => SetDefaultGuitarBindings_VirtualFretboard(fretboard),
                _ => false
            };
        }

        private bool SetDefaultGuitarBindings_VirtualFretboard(VirtualFretButtons fretboard)
        {
            if (Mode != GameMode.FiveFretGuitar && Mode != GameMode.SixFretGuitar)
                return false;

            // Map frets to GuitarAction
            AddBinding(GuitarAction.GreenFret, fretboard.Fret1);
            AddBinding(GuitarAction.RedFret, fretboard.Fret2);
            AddBinding(GuitarAction.YellowFret, fretboard.Fret3);
            AddBinding(GuitarAction.BlueFret, fretboard.Fret4);
            AddBinding(GuitarAction.OrangeFret, fretboard.Fret5);

            return true;
        }

        private bool SetDefaultMenuBindings(VirtualFretButtons fretboard)
        {
            if (!IsMenu)
                return false;

            // Map frets to menu actions
            //AddBinding(MenuAction.Start, fretboard.Fret1);
            //AddBinding(MenuAction.Select, fretboard.Fret2);
            //AddBinding(MenuAction.Up, fretboard.Fret3);
            //AddBinding(MenuAction.Down, fretboard.Fret4);
            //AddBinding(MenuAction.Right, fretboard.Fret5);

            return true;
        }
    }
}