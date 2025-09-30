using System;
using System.Runtime.InteropServices;
using ManagedBass;

namespace YARG.Audio.BASS
{
#if (UNITY_IOS || UNITY_STANDALONE_OSX) && !UNITY_EDITOR
    internal static class Bass
    {
        // ===== Native P/Invoke declarations =====
        [DllImport("__Internal")]
        private static extern bool BASS_Init(int device, int freq, int flags, IntPtr win, IntPtr clsid);

        [DllImport("__Internal")]
        private static extern bool BASS_Free();

        [DllImport("__Internal")]
        private static extern int BASS_PluginLoad([MarshalAs(UnmanagedType.LPStr)] string file, int flags);

        [DllImport("__Internal")]
        private static extern bool BASS_PluginFree(int handle);

        [DllImport("__Internal")]
        private static extern int BASS_ErrorGetCode();

        [DllImport("__Internal")]
        private static extern bool BASS_SetConfig(int option, int value);

        [DllImport("__Internal")]
        private static extern int BASS_GetConfig(int option);

        [DllImport("__Internal")]
        private static extern int BASS_GetVersion();

        [DllImport("__Internal")]
        private static extern bool BASS_GetDeviceInfo(int device, out DeviceInfo info);

        [DllImport("__Internal")]
        private static extern int BASS_GetDevice();

        [DllImport("__Internal")]
        private static extern bool BASS_GetInfo(out BassInfo info);

        [DllImport("__Internal")]
        private static extern bool BASS_RecordGetDeviceInfo(int device, out DeviceInfo info);

        [DllImport("__Internal")]
        private static extern int BASS_RecordGetDevice();

        [DllImport("__Internal")]
        private static extern bool BASS_RecordSetDevice(int device);

        [DllImport("__Internal")]
        private static extern bool BASS_RecordInit(int device);

        [DllImport("__Internal")]
        private static extern bool BASS_RecordFree();

        [DllImport("__Internal")]
        private static extern int BASS_RecordStart(int freq, int chans, int flags, IntPtr proc, IntPtr user);

        [DllImport("__Internal")]
        private static extern bool BASS_ChannelGetAttribute(int handle, int attrib, out float value);

        [DllImport("__Internal")]
        private static extern bool BASS_ChannelSetAttribute(int handle, int attrib, float value);

        [DllImport("__Internal")]
        private static extern bool BASS_ChannelSlideAttribute(int handle, int attrib, float value, int time);

        [DllImport("__Internal")]
        private static extern bool BASS_ChannelSetPosition(int handle, long pos, int mode);

        [DllImport("__Internal")]
        private static extern long BASS_ChannelGetPosition(int handle, int mode);

        [DllImport("__Internal")]
        private static extern long BASS_ChannelGetLength(int handle, int mode);

        [DllImport("__Internal")]
        private static extern double BASS_ChannelBytes2Seconds(int handle, long pos);

        [DllImport("__Internal")]
        private static extern long BASS_ChannelSeconds2Bytes(int handle, double pos);

        [DllImport("__Internal")]
        private static extern bool BASS_ChannelPlay(int handle, bool restart);

        [DllImport("__Internal")]
        private static extern bool BASS_ChannelStop(int handle);

        [DllImport("__Internal")]
        private static extern bool BASS_ChannelPause(int handle);

        [DllImport("__Internal")]
        private static extern bool BASS_ChannelUpdate(int handle, int length);

        [DllImport("__Internal")]
        private static extern int BASS_ChannelIsActive(int handle);

        [DllImport("__Internal")]
        private static extern int BASS_ChannelGetData(int handle, IntPtr buffer, int length);

        [DllImport("__Internal")]
        private static extern int BASS_ChannelGetData(int handle, [In, Out] float[] buffer, int length);

        [DllImport("__Internal")]
        private static extern int BASS_ChannelGetLevel(int handle);

        [DllImport("__Internal")]
        private static extern bool BASS_ChannelGetLevel(int handle, [In, Out] float[] levels, float length, int flags);

        [DllImport("__Internal")]
        private static extern int BASS_ChannelSetFX(int handle, int type, int priority);

        [DllImport("__Internal")]
        private static extern bool BASS_ChannelRemoveFX(int handle, int fx);

        [DllImport("__Internal")]
        private static extern int BASS_ChannelSetDSP(int handle, IntPtr proc, IntPtr user, int priority);

        [DllImport("__Internal")]
        private static extern int BASS_StreamCreate(int freq, int chans, int flags, IntPtr proc, IntPtr user);

        [DllImport("__Internal")]
        private static extern int BASS_StreamCreateFile(bool mem, IntPtr file, long offset, long length, int flags);

        [DllImport("__Internal")]
        private static extern int BASS_StreamCreateFile(bool mem, [MarshalAs(UnmanagedType.LPStr)] string file, long offset, long length, int flags);

        [DllImport("__Internal")]
        private static extern int BASS_StreamCreateFileUser(int system, int flags, [In] FileProcedures procs, IntPtr user);

        [DllImport("__Internal")]
        private static extern bool BASS_StreamFree(int handle);

        [DllImport("__Internal")]
        private static extern int BASS_StreamPutData(int handle, IntPtr buffer, int length);

        [DllImport("__Internal")]
        private static extern int BASS_SampleLoad(bool mem, IntPtr file, long offset, int length, int max, int flags);

        [DllImport("__Internal")]
        private static extern int BASS_SampleLoad(bool mem, [MarshalAs(UnmanagedType.LPStr)] string file, long offset, int length, int max, int flags);

        [DllImport("__Internal")]
        private static extern int BASS_SampleGetChannel(int handle, int flags);

        [DllImport("__Internal")]
        private static extern bool BASS_SampleFree(int handle);

        [DllImport("__Internal")]
        private static extern bool BASS_FXSetParameters(int handle, IntPtr par);

        // ===== Public API =====
        public static bool Init(int device, int freq, DeviceInitFlags flags, IntPtr win = default)
        {
            return BASS_Init(device, freq, (int)flags, win, IntPtr.Zero);
        }

        public static bool Free() => BASS_Free();

        public static int PluginLoad(string file) => BASS_PluginLoad(file, 0);

        public static bool PluginFree(int handle) => BASS_PluginFree(handle);

        public static Errors LastError => (Errors)BASS_ErrorGetCode();

        public static bool Configure(Configuration option, bool value) => BASS_SetConfig((int)option, value ? 1 : 0);

        public static bool Configure(Configuration option, int value) => BASS_SetConfig((int)option, value);

        public static int GetConfig(Configuration option) => BASS_GetConfig((int)option);

        public static Version Version => Extensions.GetVersion(BASS_GetVersion());

        public static bool GetDeviceInfo(int device, out DeviceInfo info) => BASS_GetDeviceInfo(device, out info);

        public static DeviceInfo GetDeviceInfo(int device)
        {
            BASS_GetDeviceInfo(device, out var info);
            return info;
        }

        public static int CurrentDevice => BASS_GetDevice();

        public static BassInfo Info
        {
            get
            {
                BASS_GetInfo(out var info);
                return info;
            }
        }

        public static bool RecordGetDeviceInfo(int device, out DeviceInfo info) => BASS_RecordGetDeviceInfo(device, out info);

        public static int CurrentRecordingDevice
        {
            get => BASS_RecordGetDevice();
            set => BASS_RecordSetDevice(value);
        }

        public static bool RecordInit(int device) => BASS_RecordInit(device);

        public static bool RecordFree() => BASS_RecordFree();

        public static int RecordStart(int freq, int chans, BassFlags flags, RecordProcedure proc, IntPtr user = default)
        {
            var procPtr = proc != null ? Marshal.GetFunctionPointerForDelegate(proc) : IntPtr.Zero;
            return BASS_RecordStart(freq, chans, (int)flags, procPtr, user);
        }

        public static int RecordStart(int freq, int chans, BassFlags flags, int period, RecordProcedure proc, IntPtr user = default)
        {
            // BASS doesn't have a direct period parameter in RecordStart, this maps to the standard call
            var procPtr = proc != null ? Marshal.GetFunctionPointerForDelegate(proc) : IntPtr.Zero;
            return BASS_RecordStart(freq, chans, (int)flags, procPtr, user);
        }

        public static bool ChannelGetAttribute(int handle, ChannelAttribute attrib, out float value)
            => BASS_ChannelGetAttribute(handle, (int)attrib, out value);

        public static bool ChannelSetAttribute(int handle, ChannelAttribute attrib, float value)
            => BASS_ChannelSetAttribute(handle, (int)attrib, value);

        public static bool ChannelSetAttribute(int handle, ChannelAttribute attrib, double value)
            => BASS_ChannelSetAttribute(handle, (int)attrib, (float)value);

        public static bool ChannelSlideAttribute(int handle, ChannelAttribute attrib, float value, int time)
            => BASS_ChannelSlideAttribute(handle, (int)attrib, value, time);

        public static bool ChannelSetPosition(int handle, long pos, PositionFlags mode = PositionFlags.Bytes)
            => BASS_ChannelSetPosition(handle, pos, (int)mode);

        public static long ChannelGetLength(int handle, PositionFlags mode = PositionFlags.Bytes)
            => BASS_ChannelGetLength(handle, (int)mode);

        public static double ChannelBytes2Seconds(int handle, long pos)
            => BASS_ChannelBytes2Seconds(handle, pos);

        public static long ChannelSeconds2Bytes(int handle, double pos)
            => BASS_ChannelSeconds2Bytes(handle, pos);

        public static bool ChannelPlay(int handle, bool restart = false)
            => BASS_ChannelPlay(handle, restart);

        public static bool ChannelPause(int handle)
            => BASS_ChannelPause(handle);

        public static bool ChannelStop(int handle)
            => BASS_ChannelStop(handle);

        public static bool ChannelUpdate(int handle, int length)
            => BASS_ChannelUpdate(handle, length);

        public static PlaybackState ChannelIsActive(int handle)
            => (PlaybackState)BASS_ChannelIsActive(handle);

        public static int ChannelGetData(int handle, IntPtr buffer, int length)
            => BASS_ChannelGetData(handle, buffer, length);

        public static int ChannelGetData(int handle, float[] buffer, int length)
            => BASS_ChannelGetData(handle, buffer, length);

        public static double ChannelGetLevel(int handle)
        {
            var level = BASS_ChannelGetLevel(handle);
            if (level == -1) return -1;
            return level / 32768.0;
        }

        public static bool ChannelGetLevel(int handle, float[] levels, float length, LevelRetrievalFlags flags)
            => BASS_ChannelGetLevel(handle, levels, length, (int)flags);

        public static int ChannelSetFX(int handle, EffectType type, int priority)
            => BASS_ChannelSetFX(handle, (int)type, priority);

        public static bool ChannelRemoveFX(int handle, int fx)
            => BASS_ChannelRemoveFX(handle, fx);

        public static int ChannelSetDSP(int handle, DSPProcedure proc, IntPtr user = default, int priority = 0)
        {
            var procPtr = proc != null ? Marshal.GetFunctionPointerForDelegate(proc) : IntPtr.Zero;
            return BASS_ChannelSetDSP(handle, procPtr, user, priority);
        }

        public static int CreateStream(int freq, int chans, BassFlags flags, StreamProcedure proc, IntPtr user = default)
        {
            var procPtr = proc != null ? Marshal.GetFunctionPointerForDelegate(proc) : IntPtr.Zero;
            return BASS_StreamCreate(freq, chans, (int)flags, procPtr, user);
        }

        public static int CreateStream(int freq, int chans, BassFlags flags, StreamProcedureType procType)
        {
            return BASS_StreamCreate(freq, chans, (int)flags | (int)procType, IntPtr.Zero, IntPtr.Zero);
        }

        public static int CreateStream(StreamSystem system, BassFlags flags, FileProcedures procs, IntPtr user = default)
        {
            return BASS_StreamCreateFileUser((int)system, (int)flags, procs, user);
        }

        public static bool StreamFree(int handle)
            => BASS_StreamFree(handle);

        public static int StreamPutData(int handle, IntPtr buffer, int length)
            => BASS_StreamPutData(handle, buffer, length);

        public static int SampleLoad(string file, long offset, int length, int max, BassFlags flags)
            => BASS_SampleLoad(false, file, offset, length, max, (int)flags);

        public static int SampleGetChannel(int handle, BassFlags flags = BassFlags.Default)
            => BASS_SampleGetChannel(handle, (int)flags);

        public static bool SampleFree(int handle)
            => BASS_SampleFree(handle);

        public static bool FXSetParameters(int handle, dynamic par)
        {
            // Marshal the parameter struct to IntPtr
            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(par));
            try
            {
                Marshal.StructureToPtr(par, ptr, false);
                return BASS_FXSetParameters(handle, ptr);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        // Property wrappers for config values
        public static int UpdatePeriod
        {
            get => GetConfig(Configuration.UpdatePeriod);
            set => Configure(Configuration.UpdatePeriod, value);
        }

        public static bool DeviceNonStop
        {
            get => GetConfig(Configuration.DevNonStop) != 0;
            set => Configure(Configuration.DevNonStop, value);
        }
        public static int AsyncFileBufferLength
        {
            get => GetConfig(Configuration.AsyncFileBufferLength);
            set => Configure(Configuration.AsyncFileBufferLength, value);
        }

        public static int DeviceBufferLength
        {
            get => GetConfig(Configuration.DeviceBufferLength);
            set => Configure(Configuration.DeviceBufferLength, value);
        }

        public static bool UnicodeDeviceInformation
        {
            get => GetConfig(Configuration.UnicodeDeviceInformation) != 0;
            set => Configure(Configuration.UnicodeDeviceInformation, value);
        }

        public static bool FloatingPointDSP
        {
            get => GetConfig(Configuration.FloatDSP) != 0;
            set => Configure(Configuration.FloatDSP, value);
        }

        public static bool VistaTruePlayPosition
        {
            get => GetConfig(Configuration.TruePlayPosition) != 0;
            set => Configure(Configuration.TruePlayPosition, value);
        }

        public static int UpdateThreads
        {
            get => GetConfig(Configuration.UpdateThreads);
            set => Configure(Configuration.UpdateThreads, value);
        }

        public static int DeviceCount
        {
            get
            {
                int count = 0;
                DeviceInfo info;
                while (BASS_GetDeviceInfo(count, out info))
                    count++;
                return count;
            }
        }

        public static int PlaybackBufferLength
        {
            get => GetConfig(Configuration.PlaybackBufferLength);
            set => Configure(Configuration.PlaybackBufferLength, value);
        }

        public static int GlobalStreamVolume
        {
            get => GetConfig(Configuration.GlobalStreamVolume);
            set => Configure(Configuration.GlobalStreamVolume, value);
        }

        public static int GlobalSampleVolume
        {
            get => GetConfig(Configuration.GlobalSampleVolume);
            set => Configure(Configuration.GlobalSampleVolume, value);
        }
    }
#endif
}
