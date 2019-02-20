using GameSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace GameSharp.Utilities
{
    public class Trampoline : IDisposable
    {
        IntPtr _addr { get; set; }
        IntPtr _newMem { get; set; }
        byte[] _originalOpCodes { get; set; }
        byte[] _newOpCodes { get; set; }
        bool _isActive { get; set; }
        
        /// <summary>
        ///     Creates a trampoline which we can place any where in the code to run some of our injected Assembly.
        /// </summary>
        /// <param name="addr"></param>
        /// <param name="opCodes"></param>
        public Trampoline(IntPtr addr, byte[] opCodes)
        {
            _addr = addr;
            _originalOpCodes = addr.Read<byte[]>(5);
            _newOpCodes = opCodes;
        }

        /// <summary>
        ///     Create a generic jump to a defined address
        /// </summary>
        /// <param name="addressToJumpTo"></param>
        /// <returns></returns>
        public byte[] CreateJump(IntPtr comingFromAddress, IntPtr addressToJumpTo)
        {
            List<byte> jump = new List<byte>();

            // JUMP opcode
            jump.Add(0xE9);

            // Address to jump to
            byte[] jumpAddressToBytes = BitConverter.GetBytes(addressToJumpTo.ToInt32() - comingFromAddress.ToInt32());

            jump.AddRange(jumpAddressToBytes);

            return jump.ToArray();
        }

        /// <summary>
        ///     Allocates a new memory region where we can write the trampoline to.
        ///     Adds the new opcodes the user wishes to apply.
        ///     Adds the previous opcodes the original code had.
        ///     Jumps back to the original code but with an offset based on architecture so we don't go back to our jump.
        /// </summary>
        public void Enable()
        {
            if (!_isActive)
            {
                _newMem = Marshal.AllocHGlobal(_originalOpCodes.Length + _newOpCodes.Length + 5);

                List<byte> trampoline = new List<byte>();
                trampoline.AddRange(_newOpCodes);
                trampoline.AddRange(_originalOpCodes);
                trampoline.AddRange(CreateJump(_newMem, _addr - 0x10));
                _newMem.Write(trampoline.ToArray());

                List<byte> trampJump = new List<byte>();
                trampJump.AddRange(CreateJump(_addr, _newMem));
                _addr.Write(trampJump.ToArray());

                _isActive = true;
            }
        }

        /// <summary>
        ///     Restore the previous bytes and releases the allocated memory
        /// </summary>
        public void Disable()
        {
            if (_isActive)
            {
                _addr.Write(_originalOpCodes);
                Marshal.FreeHGlobal(_newMem);
                _isActive = false;
            }
        }

        /// <summary>
        ///     Calls Disable to release the allocated memory
        /// </summary>
        public void Dispose()
        {
            Disable();
        }
    }
}
