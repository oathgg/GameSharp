using System;
using System.Collections.Generic;

namespace GameSharp.Core.Memory
{
    /// <summary>
    ///     Keeps track of all the bytes patched and keeps track of the original opcodes.
    ///     If we want to get rid of all the patches we can call the public static method DisposePatches.
    ///     Otherwise call a Dispose which will deactivate the patch by restoring the original opcodes and then disposes the object.
    /// </summary>
    public class MemoryPatch : IDisposable
    {
        public IMemoryAddress PatchAddress { get; }
        private byte[] OriginalBytes { get; set; }
        private bool IsActive { get; set; } = false;
        private byte[] NewBytes { get; set; }

        private static List<MemoryPatch> MemoryPatches = new List<MemoryPatch>();

        /// <summary>
        ///     A patch can be used to change byte(s) starting at the defined address.
        /// </summary>
        /// <param name="addressToPatch">The address of the byte where we want our patch to start.</param>
        public MemoryPatch(IMemoryAddress addressToPatch, byte[] newBytes)
        {
            PatchAddress = addressToPatch;
            NewBytes = newBytes;
            OriginalBytes = PatchAddress.Read(NewBytes.Length);

            MemoryPatches.Add(this);
        }

        public void Disable()
        {
            if (IsActive)
            {
                PatchAddress.Write(OriginalBytes);
                IsActive = false;
            }
        }

        public void Enable()
        {
            if (!IsActive)
            {
                PatchAddress.Write(NewBytes);
                IsActive = true;
            }
        }

        public void Dispose()
        {
            Disable();

            MemoryPatches.Remove(this);

            GC.SuppressFinalize(this);
        }

        public static void DisposeAllPatches()
        {
            foreach (var p in MemoryPatches)
            {
                p.Dispose();
            }
        }
    }
}