using CsInjection.Core.Models;
using System;
using System.Collections.Generic;

namespace CsInjection.Core.Utilities
{
    /// <summary>
    ///     Keeps track of all the bytes patched and keeps track of the original opcodes.
    ///     If we want to get rid of all the patches we can call the public static method DisposePatches.
    ///     Otherwise call a Dispose which will deactivate the patch by restoring the original opcodes and then disposes the object.
    /// </summary>
    public class BytePatcher : IDisposable
    {
        #region Properties

        /// <summary>
        ///     List of all our current patches, either active or inactive.
        /// </summary>
        public static List<BytePatcher> PatchList = new List<BytePatcher>();

        /// <summary>
        ///     Address in memory which we are going to patch.
        /// </summary>
        private MemoryAddress _memoryAddress { get; }

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
        public BytePatcher(IntPtr addressToPatch)
        {
            _memoryAddress = new MemoryAddress(addressToPatch);
            PatchList.Add(this);
        }

        #endregion Constructor

        #region Methods

        #region Patch

        /// <summary>
        ///     Patches the bytes at the <see cref="MemoryAddress"/> provided in the constructor
        ///     and activates the patch.
        /// </summary>
        /// <param name="newBytes"></param>
        public void PatchAndEnable(byte[] newBytes)
        {
            Patch(newBytes);
            Enable();
        }

        /// <summary>
        ///     Patch by using new bytes in an array.
        /// </summary>
        /// <param name="newBytes">An array of bytes</param>
        public void Patch(byte[] newBytes)
        {
            _originalBytes = _memoryAddress.Read<byte[]>(newBytes.Length);
            _newBytes = newBytes;
        }

        #endregion Patch

        /// <summary>
        ///     Disables the patch by restoring the original opcodes.
        /// </summary>
        public void Disable()
        {
            if (_isActive)
            {
                _memoryAddress.Write(_originalBytes);
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
                _memoryAddress.Write(_newBytes);
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

        #region DisposePatches

        /// <summary>
        ///     Disposes all the patches
        /// </summary>
        public static void DisposePatches()
        {
            foreach (var patch in PatchList)
            {
                patch.Dispose();
            }
            PatchList.Clear();
        }

        #endregion DisposePatches

        #endregion Methods
    }
}