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
            byte[] relativeJumpAddressBytes = GetRelativeAddress(from, to);
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
        private byte[] GetRelativeAddress(IntPtr from, IntPtr to)
        {
            // Calculate the distance between the two memory addresses
            long offsetDifference = IntPtr.Size == 4 
                ? to.ToInt32() - from.ToInt32() 
                : to.ToInt64() - from.ToInt64();

            // If we jump back in memory then we have a negative jump.
            bool negativeJump = offsetDifference < 0;

            // Level it out so it's no longer a negative.
            offsetDifference = Math.Abs(offsetDifference);

            byte[] relativeAddressInBytes = new byte[4];
            if (negativeJump)
            {
                uint returnAddress = uint.MaxValue - (uint) offsetDifference;
                relativeAddressInBytes = BitConverter.GetBytes(returnAddress);
            }
            else
            {
                relativeAddressInBytes = BitConverter.GetBytes(offsetDifference);
            }
            return relativeAddressInBytes.Take(4).ToArray();
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
                _newMem = CreateTrampolineFunc(_from);
                CreateJumpToTrampoline(_from, _newMem);
                _isActive = true;
            }
        }

        private void CreateJumpToTrampoline(IntPtr from, IntPtr to)
        {
            // Remove the address bytes from the New memory func
            to -= 4;

            // Remove the jump byte if the new memory is allocated ahead of the address we're coming from.
            bool allocatedMemoryAhead = IntPtr.Size == 4 ? to.ToInt32() > from.ToInt32() : to.ToInt64() > from.ToInt64();
            if (allocatedMemoryAhead)
                to -= 1;

            from.Write(CreateJump(from, to));
        }

        private IntPtr CreateTrampolineFunc(IntPtr from)
        {
            int totalBytes = _originalOpCodes.Length + _newOpCodes.Length + 5;

            IntPtr newMem = Marshal.AllocHGlobal(totalBytes);

            List<byte> trampoline = new List<byte>();
            trampoline.AddRange(_newOpCodes);
            trampoline.AddRange(_originalOpCodes);

            // The old address minus the amount of extra bytes we added + the added bytes for the jump.
            IntPtr oldFunc = from - totalBytes + 5;

            bool allocatedMemoryAhead = IntPtr.Size == 4 ? newMem.ToInt32() > from.ToInt32() : newMem.ToInt64() > from.ToInt64();
            // When the new memory if further ahead then we need to add an additional byte.
            if (allocatedMemoryAhead)
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
