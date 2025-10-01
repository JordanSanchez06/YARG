// This class has been refactored to use static methods with the [MonoPInvokeCallback] attribute.
// This is a necessary workaround to resolve a NotSupportedException on iOS builds.
// Unity's IL2CPP compiler does not support marshaling instance method delegates (like the original
// lambda expressions) to native code, but it does support these attributed static methods.
using System;
using System.IO;
using ManagedBass;
using AOT; // <-- ADD THIS
using System.Runtime.InteropServices; // <-- ADD THIS

namespace YARG.Audio.BASS
{
    public class BassStreamProcedures : FileProcedures
    {
        private readonly Stream _stream;
        private readonly long _start;
        private readonly long _length;

        // The four callback methods, now marked as static
        [MonoPInvokeCallback(typeof(FileCloseProcedure))]
        private static void CloseCallback(IntPtr user)
        {
            UnityEngine.Debug.Log("Fortnite 1 Start");
            var handle = GCHandle.FromIntPtr(user);
            var procedures = (BassStreamProcedures) handle.Target;
            procedures._stream.Close();
            handle.Free();
            UnityEngine.Debug.Log("Fortnite 1 end");
        }

        [MonoPInvokeCallback(typeof(FileLengthProcedure))]
        private static long LengthCallback(IntPtr user)
        {
            UnityEngine.Debug.Log("Fortnite 2 Start");
            var handle = GCHandle.FromIntPtr(user);
            var procedures = (BassStreamProcedures) handle.Target;
            UnityEngine.Debug.Log("Fortnite 2 end");
            return procedures._length;
        }

        [MonoPInvokeCallback(typeof(FileReadProcedure))]
        private static int ReadCallback(IntPtr buffer, int length, IntPtr user)
        {
            try
            {
                UnityEngine.Debug.Log("Fortnite 3 start");
                var handle = GCHandle.FromIntPtr(user);
                var procedures = (BassStreamProcedures) handle.Target;
                unsafe
                {
                    UnityEngine.Debug.Log("Fortnite 3 end");
                    return procedures._stream.Read(new Span<byte>((byte*) buffer, length));
                }
            }
            catch
            {
                UnityEngine.Debug.Log("Fortnite 3 catch");
                return 0;
            }
        }

        [MonoPInvokeCallback(typeof(FileSeekProcedure))]
        private static bool SeekCallback(long offset, IntPtr user)
        {
            try
            {
                UnityEngine.Debug.Log("Fortnite 4 start");
                var handle = GCHandle.FromIntPtr(user);
                var procedures = (BassStreamProcedures) handle.Target;
                procedures._stream.Seek(offset + procedures._start, SeekOrigin.Begin);
                UnityEngine.Debug.Log("Fortnite 4 end");
                return true;
            }
            catch
            {
                UnityEngine.Debug.Log("Fortnite 4 catch");
                return false;
            }
        }

        public BassStreamProcedures(Stream stream)
        {
            UnityEngine.Debug.Log("Fortnite 5 start");
            _stream = stream;
            _start = stream.Position;
            _length = stream.Length - _start;

            // Assign the static methods to the delegates
            Close = CloseCallback;
            Length = LengthCallback;
            Read = ReadCallback;
            Seek = SeekCallback;
            UnityEngine.Debug.Log("Fortnite 5 end");
        }
    }
}