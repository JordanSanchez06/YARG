using System;
using System.Runtime.InteropServices;
using ManagedBass;
using ManagedBass.Mix;

namespace YARG.Audio.BASS
{
#if (UNITY_IOS || UNITY_STANDALONE_OSX) && !UNITY_EDITOR
    internal static class BassMix
    {
        // Native declarations
        [DllImport("__Internal")]
        private static extern int BASS_Mixer_GetVersion();

        [DllImport("__Internal")]
        private static extern int BASS_Mixer_StreamCreate(int freq, int chans, int flags);

        [DllImport("__Internal")]
        private static extern bool BASS_Mixer_StreamAddChannel(int handle, int channel, int flags);

        [DllImport("__Internal")]
        private static extern int BASS_Split_StreamCreate(int channel, int flags, int[] chanmap);

        [DllImport("__Internal")]
        private static extern bool BASS_Split_StreamReset(int handle);

        [DllImport("__Internal")]
        private static extern bool BASS_Split_StreamResetEx(int handle, int offset);

        [DllImport("__Internal")]
        private static extern long BASS_Mixer_ChannelGetPosition(int handle, int mode);

        [DllImport("__Internal")]
        private static extern bool BASS_Mixer_ChannelSetPosition(int handle, long pos, int mode);

        [DllImport("__Internal")]
        private static extern int BASS_Mixer_ChannelSetSync(int handle, int type, long param, IntPtr proc, IntPtr user);

        [DllImport("__Internal")]
        private static extern bool BASS_Mixer_ChannelSetMatrix(int handle, IntPtr matrix);

        [DllImport("__Internal")]
        private static extern bool BASS_Mixer_ChannelSetMatrixEx(int handle, float[,] matrix, int length, float time);

        // Public API
        public static Version Version => Extensions.GetVersion(BASS_Mixer_GetVersion());

        public static int CreateMixerStream(int freq, int chans, BassFlags flags)
            => BASS_Mixer_StreamCreate(freq, chans, (int)flags);

        public static bool MixerAddChannel(int handle, int channel, BassFlags flags)
            => BASS_Mixer_StreamAddChannel(handle, channel, (int)flags);

        public static int CreateSplitStream(int channel, BassFlags flags, int[] channelMap)
            => BASS_Split_StreamCreate(channel, (int)flags, channelMap);

        public static bool SplitStreamReset(int handle)
            => BASS_Split_StreamReset(handle);

        public static bool SplitStreamReset(int handle, int offset)
            => BASS_Split_StreamResetEx(handle, offset);

        public static long ChannelGetPosition(int handle, PositionFlags mode = PositionFlags.Bytes)
            => BASS_Mixer_ChannelGetPosition(handle, (int)mode);

        public static bool ChannelSetPosition(int handle, long pos, PositionFlags mode = PositionFlags.Bytes)
            => BASS_Mixer_ChannelSetPosition(handle, pos, (int)mode);

        public static int ChannelSetSync(int handle, SyncFlags type, long param, SyncProcedure proc, IntPtr user = default)
        {
            var procPtr = proc != null ? Marshal.GetFunctionPointerForDelegate(proc) : IntPtr.Zero;
            return BASS_Mixer_ChannelSetSync(handle, (int)type, param, procPtr, user);
        }

        public static bool ChannelSetMatrix(int handle, IntPtr matrix)
            => BASS_Mixer_ChannelSetMatrix(handle, matrix);

        public static bool ChannelSetMatrix(int handle, float[,] matrix)
        {
            if (matrix == null)
                return BASS_Mixer_ChannelSetMatrix(handle, IntPtr.Zero);

            int length = matrix.GetLength(0) * matrix.GetLength(1);
            return BASS_Mixer_ChannelSetMatrixEx(handle, matrix, length, 0);
        }
    }
#endif
}
