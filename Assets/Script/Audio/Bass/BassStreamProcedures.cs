using System;
using System.IO;
using ManagedBass;
#if  UNITY_ANDROID
    using System.Runtime.InteropServices;
    using AOT;
    using UnityEngine;
#endif

namespace YARG.Audio.BASS
{
    public class BassStreamProcedures : FileProcedures
    {
        private readonly Stream _stream;
        private readonly long _start;
        private readonly long _length;

        #if UNITY_ANDROID
            public GCHandle GCHandle;
        #endif

        public BassStreamProcedures(Stream stream)
        {
            _stream = stream;
            _start = stream.Position;
            _length = stream.Length - _start;

            #if UNITY_ANDROID
                Close = StaticClose;
                Length = StaticLength;
                Read = StaticRead;
                Seek = StaticSeek;
            #else

            Close = (IntPtr) => _stream.Close();
            Length = (IntPtr) => _length;
            Read = (IntPtr Buffer, int Length, IntPtr User) =>
            {
                try
                {
                    unsafe
                    {
                        return _stream.Read(new Span<byte>((byte*) Buffer, Length));
                    }
                }
                catch
                {
                    return 0;
                }
            };

            Seek = (long Offset, IntPtr User) =>
            {
                try
                {
                    _stream.Seek(Offset + _start, SeekOrigin.Begin);
                    return true;
                }
                catch
                {
                    return false;
                }
            };

            #endif
        }

        #if UNITY_ANDROID
            [MonoPInvokeCallback(typeof(FileCloseProcedure))]
            private static void StaticClose(IntPtr user)
            {
                try
                {
                    var handle = GCHandle.FromIntPtr(user);
                    var procs = (BassStreamProcedures)handle.Target;
            
                    procs._stream.Close();
            
                    // Free the handle
                    handle.Free();
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogError($"Error in StaticClose: {e.Message}");
                }
            }

            [MonoPInvokeCallback(typeof(FileLengthProcedure))]
            private static long StaticLength(IntPtr user)
            {
                try
                {
                    var handle = GCHandle.FromIntPtr(user);
                    var procs = (BassStreamProcedures)handle.Target;
                    return procs._length;
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogError($"Error in StaticLength: {e.Message}");
                    return 0;
                }
            }

            [MonoPInvokeCallback(typeof(FileReadProcedure))]
            private static int StaticRead(IntPtr buffer, int length, IntPtr user)
            {
                try
                {
                    var handle = GCHandle.FromIntPtr(user);
                    var procs = (BassStreamProcedures)handle.Target;
            
                    unsafe
                    {
                        return procs._stream.Read(new Span<byte>((byte*)buffer, length));
                    }
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogError($"Error in StaticRead: {e.Message}");
                    return 0;
                }
            }

            [MonoPInvokeCallback(typeof(FileSeekProcedure))]
            private static bool StaticSeek(long offset, IntPtr user)
            {
                try
                {
                    var handle = GCHandle.FromIntPtr(user);
                    var procs = (BassStreamProcedures)handle.Target;
            
                    procs._stream.Seek(offset + procs._start, SeekOrigin.Begin);
                    return true;
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.LogError($"Error in StaticSeek: {e.Message}");
                    return false;
                }
            }

        #endif //UNITY_ANDROID
    }
}
