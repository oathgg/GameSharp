using GameSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace GameSharp.Utilities
{
    public class Trampoline : IDisposable
    {
        IntPtr _from { get; set; }
        byte[] _originalOpCodes { get; set; }
        byte[] _newOpCodes { get; set; }
        IntPtr _newMem { get; set; }
        int _totalBytes { get; set; }
        bool _isActive { get; set; }
        
        /// <summary>
        ///     Creates a trampoline which we can place any where in the code to run some of our injected Assembly.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="opCodes"></param>
        public Trampoline(IntPtr from, byte[] opCodes)
        {
            _from = from;
            _originalOpCodes = from.Read<byte[]>(5);
            _newOpCodes = opCodes;
            _totalBytes = _originalOpCodes.Length + _newOpCodes.Length + 5;
        }

        /// <summary>
        ///     Creates a CALL to a defined address
        /// </summary>
        /// <param name="to"></param>
        /// <returns></returns>
        private byte[] CreateJump(IntPtr from, IntPtr to)
        {
            List<byte> jump = new List<byte>();

            // JMP https://c9x.me/x86/html/file_module_x86_id_147.html
            jump.Add(0xE9);

            // Address offset.
            byte[] relativeJumpAddressBytes = GetRelativeAddress(from.ToInt32(), to.ToInt32());
            jump.AddRange(relativeJumpAddressBytes);

            return jump.ToArray();
        }

        /// <summary>
        ///     We always think it will be a Jump Near instruction (E9).
        ///     From > To then the jump back should substract the difference from the max val of an IntPtr (0xFFFFFFFF).
        ///     To > From then we return the difference right away.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private byte[] GetRelativeAddress(int from, int to)
        {
            byte[] relativeAddressInBytes = new byte[4];

            // Level it out so it's no longer a negative.
            int difference = Math.Abs(from - to);

            if (from > to)
            {
                // Jump back
                uint returnAddress = uint.MaxValue - (uint) difference;
                relativeAddressInBytes = BitConverter.GetBytes(returnAddress);
            }
            else
            {
                // Jump forward
                relativeAddressInBytes = BitConverter.GetBytes(difference);
            }

            return relativeAddressInBytes;
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
                _newMem = CreateTrampoline();
                CreateJumpToTrampoline(_newMem);
                _isActive = true;
            }
        }

        private void CreateJumpToTrampoline(IntPtr newMemFunc)
        {
            // Remove the address bytes from the New memory func
            newMemFunc -= 4;

            // Remove the jump byte if the new memory is allocated ahead of the address we're coming from.
            if (newMemFunc.ToInt32() > _from.ToInt32())
                newMemFunc -= 1;

            _from.Write(CreateJump(_from, newMemFunc));
        }

        private IntPtr CreateTrampoline()
        {
            IntPtr newMem = Marshal.AllocHGlobal(_totalBytes);

            List<byte> trampoline = new List<byte>();
            trampoline.AddRange(_newOpCodes);
            trampoline.AddRange(_originalOpCodes);

            // The old address minus the amount of extra bytes we added + the added bytes for the jump.
            IntPtr oldFunc = _from - _totalBytes + 5;

            // When the new memory if further ahead then we need to add an additional byte.
            if (newMem.ToInt32() > _from.ToInt32())
                oldFunc += 1;

            // Creates a relative jump and adds it to the array.
            trampoline.AddRange(CreateJump(newMem, oldFunc));

            // Write the array to the memory.
            newMem.Write(trampoline.ToArray());

            return newMem;
        }

        /// <summary>
        ///     Restore the previous bytes and releases the allocated memory
        /// </summary>
        public void Disable()
        {
            if (_isActive)
            {
                _from.Write(_originalOpCodes);
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
