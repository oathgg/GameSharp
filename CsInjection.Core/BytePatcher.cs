using CsInjection.Core.Models;
using System;
using System.Collections.Generic;

namespace CsInjection.Core
{
    /// <summary>
    /// Keeps track of all the bytes patched and keeps track of the original opcodes.
    /// If we want to get rid of all the patches we can call the public static method DisposePatches.
    /// Otherwise call a Dispose which will deactivate the patch by restoring the original opcodes and then disposes the object.
    /// </summary>
    public class BytePatcher : IDisposable
    {
        #region Properties
        /// <summary>
        /// List of all our current patches, either active or inactive.
        /// </summary>
        private static List<BytePatcher> _patchList = new List<BytePatcher>();
        /// <summary>
        /// Address in memory which we are going to patch.
        /// </summary>
        private MemoryAddress _memoryAddress { get; }
        /// <summary>
        /// The original opcodes which belonged to this patch.
        /// </summary>
        private byte[] _originalBytes { get; set; }
        /// <summary>
        /// State of our current patch.
        /// </summary>
        private bool _isActive { get; set; } = false;
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="patchKey">Unique key for this patch, will be used to deactivate a patch</param>
        /// <param name="addressToPatch"></param>
        public BytePatcher(IntPtr addressToPatch)
        {
            _memoryAddress = new MemoryAddress(addressToPatch);
            _patchList.Add(this);
        }
        #endregion

        #region Methods

        #region Patch
        /// <summary>
        /// Patch by using new bytes in a list
        /// </summary>
        /// <param name="newBytes">A list of bytes</param>
        public void Patch(List<byte> newBytes)
        {
            Patch(newBytes.ToArray());
        }

        /// <summary>
        /// Patch by using new bytes in an array.
        /// </summary>
        /// <param name="newBytes">An array of bytes</param>
        public void Patch(byte[] newBytes)
        {
            if (!_isActive)
            {
                _originalBytes = _memoryAddress.Read<byte[]>(newBytes.Length);
                _memoryAddress.Write(newBytes);
                _isActive = true;
            }
            else
            {
                Console.WriteLine($"There is already a patch active at address {_memoryAddress}");
            }
        }
        #endregion

        #region Deactivate
        /// <summary>
        /// Deactivates the patch by restoring the original opcodes.
        /// </summary>
        public void Deactivate()
        {
            if (_isActive)
            {
                _memoryAddress.Write(_originalBytes);
                _isActive = false;
            }
        }
        #endregion

        #region Dispose (Implemented from IDisposable)
        /// <summary>
        /// Deactivates the patch and disposes of the object
        /// </summary>
        public void Dispose()
        {
            Deactivate();
        }
        #endregion

        #region DisposePatches
        /// <summary>
        /// Disposes all the patches
        /// </summary>
        public static void DisposePatches()
        {
            foreach (var patch in _patchList)
            {
                patch.Dispose();
            }
            _patchList.Clear();
        }
        #endregion

        #endregion
    }
}
