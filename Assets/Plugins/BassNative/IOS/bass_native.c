// Stub declarations for IL2CPP linker - Core BASS functions
extern int BASS_Init(int device, int freq, int flags, void* win, void* clsid);
extern int BASS_Free(void);
extern int BASS_PluginLoad(const char* file, int flags);
extern int BASS_PluginFree(int handle);
extern int BASS_ErrorGetCode(void);
extern int BASS_SetConfig(int option, int value);
extern int BASS_GetConfig(int option);
extern int BASS_GetVersion(void);
extern int BASS_GetDeviceInfo(int device, void* info);
extern int BASS_GetDevice(void);
extern int BASS_GetInfo(void* info);

// Recording device methods
extern int BASS_RecordGetDeviceInfo(int device, void* info);
extern int BASS_RecordGetDevice(void);
extern int BASS_RecordSetDevice(int device);
extern int BASS_RecordInit(int device);
extern int BASS_RecordFree(void);
extern int BASS_RecordStart(int freq, int chans, int flags, void* proc, void* user);

// Channel attribute methods
extern int BASS_ChannelGetAttribute(int handle, int attrib, float* value);
extern int BASS_ChannelSetAttribute(int handle, int attrib, float value);
extern int BASS_ChannelSlideAttribute(int handle, int attrib, float value, int time);

// Channel position methods
extern int BASS_ChannelSetPosition(int handle, long long pos, int mode);
extern long long BASS_ChannelGetPosition(int handle, int mode);
extern long long BASS_ChannelGetLength(int handle, int mode);
extern double BASS_ChannelBytes2Seconds(int handle, long long pos);
extern long long BASS_ChannelSeconds2Bytes(int handle, double pos);

// Channel playback control
extern int BASS_ChannelPlay(int handle, int restart);
extern int BASS_ChannelStop(int handle);
extern int BASS_ChannelPause(int handle);
extern int BASS_ChannelUpdate(int handle, int length);
extern int BASS_ChannelIsActive(int handle);

// Channel data methods
extern int BASS_ChannelGetData(int handle, void* buffer, int length);
extern int BASS_ChannelGetLevel(int handle);

// Channel FX/DSP methods
extern int BASS_ChannelSetFX(int handle, int type, int priority);
extern int BASS_ChannelRemoveFX(int handle, int fx);
extern int BASS_ChannelSetDSP(int handle, void* proc, void* user, int priority);

// Stream methods
extern int BASS_StreamCreate(int freq, int chans, int flags, void* proc, void* user);
extern int BASS_StreamCreateFile(int mem, void* file, long long offset, long long length, int flags);
extern int BASS_StreamCreateFileUser(int system, int flags, void* procs, void* user);
extern int BASS_StreamFree(int handle);
extern int BASS_StreamPutData(int handle, void* buffer, int length);

// Sample methods
extern int BASS_SampleLoad(int mem, void* file, long long offset, int length, int max, int flags);
extern int BASS_SampleGetChannel(int handle, int flags);
extern int BASS_SampleFree(int handle);

// FX methods
extern int BASS_FXSetParameters(int handle, void* params);

// BASS.FX functions
extern int BASS_FX_TempoCreate(int chan, int flags);

// BASS.FX version
extern int BASS_FX_GetVersion(void);
extern int BASS_FX_TempoGetSource(int handle);

// BASS.MIX functions
extern int BASS_Mixer_StreamCreate(int freq, int chans, int flags);
extern int BASS_Mixer_StreamAddChannel(int handle, int channel, int flags);
extern int BASS_Split_StreamCreate(int channel, int flags, int* chanmap);
extern int BASS_Mixer_GetVersion(void);
extern int BASS_Mixer_StreamCreate(int freq, int chans, int flags);
extern int BASS_Mixer_StreamAddChannel(int handle, int channel, int flags);
extern int BASS_Split_StreamCreate(int channel, int flags, int* chanmap);
extern int BASS_Split_StreamReset(int handle);
extern int BASS_Split_StreamResetEx(int handle, int offset);
extern long long BASS_Mixer_ChannelGetPosition(int handle, int mode);
extern int BASS_Mixer_ChannelSetPosition(int handle, long long pos, int mode);
extern int BASS_Mixer_ChannelSetSync(int handle, int type, long long param, void* proc, void* user);
extern int BASS_Mixer_ChannelSetMatrix(int handle, void* matrix);
extern int BASS_Mixer_ChannelSetMatrixEx(int handle, void* matrix, int length, float time);
