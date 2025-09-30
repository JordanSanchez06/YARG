using System;
using System.Runtime.InteropServices;
using ManagedBass;
using ManagedBass.Fx;

namespace YARG.Audio.BASS
{
#if (UNITY_IOS || UNITY_STANDALONE_OSX) && !UNITY_EDITOR
    internal static class BassFx
    {
        // Native declarations
        [DllImport("__Internal")]
        private static extern int BASS_FX_GetVersion();

        [DllImport("__Internal")]
        private static extern int BASS_FX_TempoCreate(int chan, int flags);

        [DllImport("__Internal")]
        private static extern bool BASS_FX_TempoGetSource(int handle);

        // Public API
        public static Version Version => Extensions.GetVersion(BASS_FX_GetVersion());

        public static int TempoCreate(int channel, BassFlags flags)
            => BASS_FX_TempoCreate(channel, (int)flags);

        public static int TempoGetSource(int handle)
            => BASS_FX_TempoGetSource(handle) ? 1 : 0;
    }
#endif
}
