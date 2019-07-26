using GameSharp.Extensions;
using System;

namespace GameSharp.Utilities
{
    /// <summary>
    ///     Keeps track of all the bytes patched and keeps track of the original opcodes.
    ///     If we want to get rid of all the patches we can call the public static method DisposePatches.
    ///     Otherwise call a Dispose which will deactivate the patch by restoring the original opcodes and then disposes the object.
    /// </summary>
    public class Patch : IDisposable
    {
        private IntPtr _addressToPatch { get; }
        private byte[] _originalBytes { get; set; }
        private bool _isActive { get; set; } = false;
        private byte[] _newBytes { get; set; }

        /// <summary>
        ///     A patch can be used to change byte(s) starting at the defined address.
        /// </summary>
        /// <param name="addressToPatch">The address of the byte where we want our patch to start.</param>
        public Patch(IntPtr addressToPatch, byte[] newBytes)
        {
            _addressToPatch = IntPtr.Size == 4 ? new IntPtr(addressToPatch.ToInt32()) : new IntPtr(addressToPatch.ToInt64());
            _newBytes = newBytes;
            _originalBytes = _addressToPatch.Read<byte[]>(_newBytes.Length);
        }

        public void Disable()
        {
            if (_isActive)
            {
                _addressToPatch.Write(_originalBytes);
                _isActive = false;
            }
        }

        public void Enable()
        {
            if (!_isActive)
            {
                _addressToPatch.Write(_newBytes);
                _isActive = true;
            }
        }

        public void Dispose()
        {
            Disable();
            GC.SuppressFinalize(this);
        }
    }
}