using GameSharp.Memory;
using System;

namespace GameSharp.Interoperability
{
    /// <summary>
    ///     Keeps track of all the bytes patched and keeps track of the original opcodes.
    ///     If we want to get rid of all the patches we can call the public static method DisposePatches.
    ///     Otherwise call a Dispose which will deactivate the patch by restoring the original opcodes and then disposes the object.
    /// </summary>
    public class Patch : IDisposable
    {
        public MemoryAddress PatchAddress { get; }
        private byte[] OriginalBytes { get; set; }
        private bool IsActive { get; set; } = false;
        private byte[] NewBytes { get; set; }

        /// <summary>
        ///     A patch can be used to change byte(s) starting at the defined address.
        /// </summary>
        /// <param name="addressToPatch">The address of the byte where we want our patch to start.</param>
        public Patch(MemoryAddress addressToPatch, byte[] newBytes)
        {
            PatchAddress = addressToPatch;
            NewBytes = newBytes;
            OriginalBytes = PatchAddress.Read<byte[]>(NewBytes.Length);
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
            GC.SuppressFinalize(this);
        }
    }
}