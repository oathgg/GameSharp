using CsInjection.Library.Models;
using System;
using System.Collections.Generic;

namespace CsInjection.Library.Helpers
{
    /// <summary>
    /// Keeps track of all the bytes patched and keeps track of the original opcodes.
    /// If we want to get rid of all the patches we can call the public static method DisposePatches.
    /// Otherwise call a Dispose which will deactivate the patch by restoring the original opcodes and then disposes the object.
    /// </summary>
    public class BytePatch : IDisposable
    {
        #region Properties
        /// <summary>
        /// List of all our current patches, either active or inactive.
        /// </summary>
        private static List<BytePatch> _patchList = new List<BytePatch>();
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
        private bool IsActive { get; set; } = false;
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="patchKey">Unique key for this patch, will be used to deactivate a patch</param>
        /// <param name="addressToPatch"></param>
        public BytePatch(IntPtr addressToPatch)
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
            if (!IsActive)
            {
                _originalBytes = new byte[newBytes.Length];
                // Save the original opcodes (same length) so we can restore it to it's default value.
                for (int i = 0; i < newBytes.Length; i++)
                {
                    _originalBytes[i] = _memoryAddress.Read<byte>(i, newBytes.Length);
                }
                _memoryAddress.Write(newBytes);
                IsActive = true;
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
            if (IsActive)
            {
                _memoryAddress.Write(_originalBytes);
                IsActive = false;
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
