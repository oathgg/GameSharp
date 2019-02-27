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
        bool _executeOriginal { get; set; }
        
        /// <summary>
        ///     Creates a trampoline which we can place any where in the code to run some of our injected Assembly.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="opCodes"></param>
        public Trampoline(IntPtr from, byte[] opCodes, bool executeOriginal = true)
        {
            _from = from;
            _originalOpCodes = from.Read<byte[]>(5);
            _newOpCodes = opCodes;
            _executeOriginal = executeOriginal;
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

        private IntPtr CreateTrampolineFunc(IntPtr from)
        {
            // Total amount of bytes the trampoline requires, originalcode, the new code + the 5 bytes for the jmp back.
            int totalBytes = _originalOpCodes.Length + _newOpCodes.Length + 5;

            // Allocate space in the process for our trampoline
            IntPtr codeCavePtr = Marshal.AllocHGlobal(totalBytes);

            // Create our trampoline func in the newly assigned codecave
            List<byte> codeCaveBytes = new List<byte>();
            codeCaveBytes.AddRange(_newOpCodes);

            // Add the original code to the detour.
            if (_executeOriginal)
                codeCaveBytes.AddRange(_originalOpCodes);

            // The old address minus the amount of extra bytes we added + the added bytes for the jump.
            IntPtr oldFunc = from - totalBytes + 5;

            bool allocatedMemoryAhead = IntPtr.Size == 4 ? codeCavePtr.ToInt32() > from.ToInt32() : codeCavePtr.ToInt64() > from.ToInt64();
            // When the new memory is further ahead then we need to add an additional byte.
            if (allocatedMemoryAhead)
                oldFunc += 1;

            // Creates a relative jump and adds it to the array.
            codeCaveBytes.AddRange(CreateJump(codeCavePtr, oldFunc));

            // Write the array to the memory.
            codeCavePtr.Write(codeCaveBytes.ToArray());

            return codeCavePtr;
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

        /// <summary>
        ///     Creates a CALL to a defined address based on the architecture of the program.
        /// </summary>
        /// <param name="to"></param>
        /// <returns></returns>
        private byte[] CreateJump(IntPtr from, IntPtr to)
        {
            return IntPtr.Size == 4 ? CreateJump_x86(from, to) : CreateJump_x64v1(from, to);
        }

        /// <summary>
        ///     JMP 0xCCCCCCCC          ; Jump to the relative address of the current EIP.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private byte[] CreateJump_x86(IntPtr from, IntPtr to)
        {
            List<byte> jump = new List<byte> { 0xE9 };
            byte[] relativeJumpAddressBytes = from.GetRelativeAddress(to);
            jump.AddRange(relativeJumpAddressBytes);

            return jump.ToArray();
        }

        /// <summary>
        ///     Creates a trampoline function by using a call PTR
        ///     
        ///     0xFF25DEADBEEF JMP [DEADBEF4] ([RIP+DEADBEEF]) -- 6 bytes total
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private byte[] CreateJump_x64v1(IntPtr from, IntPtr to)
        {
            // TODO: Check if it stays withing 2GB of the from address, if so we can do 0xFF, 0x25 call.
            if (IsAbsJump(from, to))
                return CreateJump_x64v2(from, to);

            List<byte> trampoline = new List<byte> { 0xFF, 0x25 };
            byte[] relativeJumpAddressBytes = BitConverter.GetBytes(to.ToInt64());
            trampoline.AddRange(relativeJumpAddressBytes);

            return trampoline.ToArray();
        }

        private bool IsAbsJump(IntPtr from, IntPtr to)
        {
            return Math.Abs(from.ToInt64() - to.ToInt64()) >= long.MaxValue;
        }

        /// <summary>
        ///     push rax                            ; Save current value of RAX register
        ///     movabs rax, 0xCCCCCCCCCCCCCCCC      ; Move memory address of our Trampoline func into RAX register
        ///     xchg qword ptr ss:[rsp], rax        ; Switch the stack with the RAX value
        ///     ret                                 ; Return to the address which is in the top of the stack.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        private byte[] CreateJump_x64v2(IntPtr from, IntPtr to)
        {
            List<byte> trampoline = new List<byte> { 0x50, 0x48, 0xB8 };
            byte[] relativeJumpAddressBytes = BitConverter.GetBytes(to.ToInt64() + 4);
            trampoline.AddRange(relativeJumpAddressBytes);
            trampoline.AddRange(new byte[] { 0x48, 0x87, 0x04, 0x24, 0xC3 });

            return trampoline.ToArray();
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
