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
        #region Properties

        /// <summary>
        ///     Address in memory which we are going to patch.
        /// </summary>
        private IntPtr _addressToPatch { get; }

        /// <summary>
        ///     The original opcodes which belonged to this patch.
        /// </summary>
        private byte[] _originalBytes { get; set; }

        /// <summary>
        ///     State of our current patch.
        /// </summary>
        private bool _isActive { get; set; } = false;

        /// <summary>
        ///     New bytes which will be used to patch.
        /// </summary>
        private byte[] _newBytes { get; set; }

        #endregion Properties

        #region Constructor

        /// <summary>
        ///     Overloaded method of the default BytePatcher
        /// </summary>
        /// <param name="addressToPatch">The address of the byte which we want to patch</param>
        public Patch(IntPtr addressToPatch, byte[] newBytes)
        {
            _addressToPatch = new IntPtr(IntPtr.Size == 4 ? addressToPatch.ToInt32() : addressToPatch.ToInt64());
            _newBytes = newBytes;
            _originalBytes = _addressToPatch.Read<byte[]>(_newBytes.Length);
        }

        #endregion Constructor

        #region Methods

        /// <summary>
        ///     Disables the patch by restoring the original opcodes.
        /// </summary>
        public void Disable()
        {
            if (_isActive)
            {
                _addressToPatch.Write(_originalBytes);
                _isActive = false;
            }
        }

        /// <summary>
        ///     Enables the patch by using the provided new bytes
        /// </summary>
        public void Enable()
        {
            if (!_isActive)
            {
                _addressToPatch.Write(_newBytes);
                _isActive = true;
            }
        }

        #region Dispose (Implemented from IDisposable)

        /// <summary>
        ///     Disables the patch and disposes of the object
        /// </summary>
        public void Dispose()
        {
            Disable();
            GC.SuppressFinalize(this);
        }

        #endregion Dispose (Implemented from IDisposable)

        #endregion Methods
    }
}