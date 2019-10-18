using GameSharp.Core.Native.Enums;
using System;

namespace GameSharp.Core.Memory
{
    public class MemoryPeb32 : IMemoryPeb
    {
        private readonly IMemoryPointer PebBasePointer;

        public MemoryPeb32(IMemoryPointer pebAddress)
        {
            PebBasePointer = pebAddress;
        }

        public byte InheritedAddressSpace
        {
            get => PebBasePointer.Read<byte>((int)PEB32.InheritedAddressSpace);
            set => PebBasePointer.Write(value, (int)PEB32.InheritedAddressSpace);
        }
        public byte ReadImageFileExecOptions
        {
            get => PebBasePointer.Read<byte>((int)PEB32.ReadImageFileExecOptions);
            set => PebBasePointer.Write(value, (int)PEB32.ReadImageFileExecOptions);
        }
        public bool BeingDebugged
        {
            get => PebBasePointer.Read<bool>((int)PEB32.BeingDebugged);
            set => PebBasePointer.Write(value, (int)PEB32.BeingDebugged);
        }
        public byte SpareBool
        {
            get => PebBasePointer.Read<byte>((int)PEB32.SpareBool);
            set => PebBasePointer.Write(value, (int)PEB32.SpareBool);
        }
        public IntPtr Mutant
        {
            get => PebBasePointer.Read<IntPtr>((int)PEB32.Mutant);
            set => PebBasePointer.Write(value, (int)PEB32.Mutant);
        }
        public IntPtr Ldr
        {
            get => PebBasePointer.Read<IntPtr>((int)PEB32.Ldr);
            set => PebBasePointer.Write(value, (int)PEB32.Ldr);
        }
        public IntPtr ProcessParameters
        {
            get => PebBasePointer.Read<IntPtr>((int)PEB32.ProcessParameters);
            set => PebBasePointer.Write(value, (int)PEB32.ProcessParameters);
        }
        public IntPtr SubSystemData
        {
            get => PebBasePointer.Read<IntPtr>((int)PEB32.SubSystemData);
            set => PebBasePointer.Write(value, (int)PEB32.SubSystemData);
        }
        public IntPtr ProcessHeap
        {
            get => PebBasePointer.Read<IntPtr>((int)PEB32.ProcessHeap);
            set => PebBasePointer.Write(value, (int)PEB32.ProcessHeap);
        }
        public IntPtr FastPebLock
        {
            get => PebBasePointer.Read<IntPtr>((int)PEB32.FastPebLock);
            set => PebBasePointer.Write(value, (int)PEB32.FastPebLock);
        }
        public IntPtr FastPebLockRoutine
        {
            get => PebBasePointer.Read<IntPtr>((int)PEB32.FastPebLockRoutine);
            set => PebBasePointer.Write(value, (int)PEB32.FastPebLockRoutine);
        }
        public IntPtr FastPebUnlockRoutine
        {
            get => PebBasePointer.Read<IntPtr>((int)PEB32.FastPebUnlockRoutine);
            set => PebBasePointer.Write(value, (int)PEB32.FastPebUnlockRoutine);
        }
        public IntPtr EnvironmentUpdateCount
        {
            get => PebBasePointer.Read<IntPtr>((int)PEB32.EnvironmentUpdateCount);
            set => PebBasePointer.Write(value, (int)PEB32.EnvironmentUpdateCount);
        }
        public IntPtr KernelCallbackTable
        {
            get => PebBasePointer.Read<IntPtr>((int)PEB32.KernelCallbackTable);
            set => PebBasePointer.Write(value, (int)PEB32.KernelCallbackTable);
        }
        public int SystemReserved
        {
            get => PebBasePointer.Read<int>((int)PEB32.SystemReserved);
            set => PebBasePointer.Write(value, (int)PEB32.SystemReserved);
        }
        public int AtlThunkSListPtr32
        {
            get => PebBasePointer.Read<int>((int)PEB32.AtlThunkSListPtr32);
            set => PebBasePointer.Write(value, (int)PEB32.AtlThunkSListPtr32);
        }
        public IntPtr FreeList
        {
            get => PebBasePointer.Read<IntPtr>((int)PEB32.FreeList);
            set => PebBasePointer.Write(value, (int)PEB32.FreeList);
        }
        public IntPtr TlsExpansionCounter
        {
            get => PebBasePointer.Read<IntPtr>((int)PEB32.TlsExpansionCounter);
            set => PebBasePointer.Write(value, (int)PEB32.TlsExpansionCounter);
        }
        public IntPtr TlsBitmap
        {
            get => PebBasePointer.Read<IntPtr>((int)PEB32.TlsBitmap);
            set => PebBasePointer.Write(value, (int)PEB32.TlsBitmap);
        }
        public long TlsBitmapBits
        {
            get => PebBasePointer.Read<long>((int)PEB32.TlsBitmapBits);
            set => PebBasePointer.Write(value, (int)PEB32.TlsBitmapBits);
        }
        public IntPtr ReadOnlySharedMemoryBase
        {
            get => PebBasePointer.Read<IntPtr>((int)PEB32.ReadOnlySharedMemoryBase);
            set => PebBasePointer.Write(value, (int)PEB32.ReadOnlySharedMemoryBase);
        }
        public IntPtr ReadOnlySharedMemoryHeap
        {
            get => PebBasePointer.Read<IntPtr>((int)PEB32.ReadOnlySharedMemoryHeap);
            set => PebBasePointer.Write(value, (int)PEB32.ReadOnlySharedMemoryHeap);
        }
        public IntPtr ReadOnlyStaticServerData
        {
            get => PebBasePointer.Read<IntPtr>((int)PEB32.ReadOnlyStaticServerData);
            set => PebBasePointer.Write(value, (int)PEB32.ReadOnlyStaticServerData);
        }
        public IntPtr AnsiCodePageData
        {
            get => PebBasePointer.Read<IntPtr>((int)PEB32.AnsiCodePageData);
            set => PebBasePointer.Write(value, (int)PEB32.AnsiCodePageData);
        }
        public IntPtr OemCodePageData
        {
            get => PebBasePointer.Read<IntPtr>((int)PEB32.OemCodePageData);
            set => PebBasePointer.Write(value, (int)PEB32.OemCodePageData);
        }
        public IntPtr UnicodeCaseTableData
        {
            get => PebBasePointer.Read<IntPtr>((int)PEB32.UnicodeCaseTableData);
            set => PebBasePointer.Write(value, (int)PEB32.UnicodeCaseTableData);
        }
        public int NumberOfProcessors
        {
            get => PebBasePointer.Read<int>((int)PEB32.NumberOfProcessors);
            set => PebBasePointer.Write(value, (int)PEB32.NumberOfProcessors);
        }
        public long NtGlobalFlag
        {
            get => PebBasePointer.Read<long>((int)PEB32.NtGlobalFlag);
            set => PebBasePointer.Write(value, (int)PEB32.NtGlobalFlag);
        }
        public long CriticalSectionTimeout
        {
            get => PebBasePointer.Read<long>((int)PEB32.CriticalSectionTimeout);
            set => PebBasePointer.Write(value, (int)PEB32.CriticalSectionTimeout);
        }
        public IntPtr HeapSegmentReserve
        {
            get => PebBasePointer.Read<IntPtr>((int)PEB32.HeapSegmentReserve);
            set => PebBasePointer.Write(value, (int)PEB32.HeapSegmentReserve);
        }
        public IntPtr HeapSegmentCommit
        {
            get => PebBasePointer.Read<IntPtr>((int)PEB32.HeapSegmentCommit);
            set => PebBasePointer.Write(value, (int)PEB32.HeapSegmentCommit);
        }
        public IntPtr HeapDeCommitTotalFreeThreshold
        {
            get => PebBasePointer.Read<IntPtr>((int)PEB32.HeapDeCommitTotalFreeThreshold);
            set => PebBasePointer.Write(value, (int)PEB32.HeapDeCommitTotalFreeThreshold);
        }
        public IntPtr HeapDeCommitFreeBlockThreshold
        {
            get => PebBasePointer.Read<IntPtr>((int)PEB32.HeapDeCommitFreeBlockThreshold);
            set => PebBasePointer.Write(value, (int)PEB32.HeapDeCommitFreeBlockThreshold);
        }
        public int NumberOfHeaps
        {
            get => PebBasePointer.Read<int>((int)PEB32.NumberOfHeaps);
            set => PebBasePointer.Write(value, (int)PEB32.NumberOfHeaps);
        }
        public int MaximumNumberOfHeaps
        {
            get => PebBasePointer.Read<int>((int)PEB32.MaximumNumberOfHeaps);
            set => PebBasePointer.Write(value, (int)PEB32.MaximumNumberOfHeaps);
        }
        public IntPtr ProcessHeaps
        {
            get => PebBasePointer.Read<IntPtr>((int)PEB32.ProcessHeaps);
            set => PebBasePointer.Write(value, (int)PEB32.ProcessHeaps);
        }
        public IntPtr GdiSharedHandleTable
        {
            get => PebBasePointer.Read<IntPtr>((int)PEB32.GdiSharedHandleTable);
            set => PebBasePointer.Write(value, (int)PEB32.GdiSharedHandleTable);
        }
        public IntPtr ProcessStarterHelper
        {
            get => PebBasePointer.Read<IntPtr>((int)PEB32.ProcessStarterHelper);
            set => PebBasePointer.Write(value, (int)PEB32.ProcessStarterHelper);
        }
        public IntPtr GdiDcAttributeList
        {
            get => PebBasePointer.Read<IntPtr>((int)PEB32.GdiDcAttributeList);
            set => PebBasePointer.Write(value, (int)PEB32.GdiDcAttributeList);
        }
        public IntPtr LoaderLock
        {
            get => PebBasePointer.Read<IntPtr>((int)PEB32.LoaderLock);
            set => PebBasePointer.Write(value, (int)PEB32.LoaderLock);
        }
        public int OsMajorVersion
        {
            get => PebBasePointer.Read<int>((int)PEB32.OsMajorVersion);
            set => PebBasePointer.Write(value, (int)PEB32.OsMajorVersion);
        }
        public int OsMinorVersion
        {
            get => PebBasePointer.Read<int>((int)PEB32.OsMinorVersion);
            set => PebBasePointer.Write(value, (int)PEB32.OsMinorVersion);
        }
        public ushort OsBuildNumber
        {
            get => PebBasePointer.Read<ushort>((int)PEB32.OsBuildNumber);
            set => PebBasePointer.Write(value, (int)PEB32.OsBuildNumber);
        }
        public ushort OsCsdVersion
        {
            get => PebBasePointer.Read<ushort>((int)PEB32.OsCsdVersion);
            set => PebBasePointer.Write(value, (int)PEB32.OsCsdVersion);
        }
        public int OsPlatformId
        {
            get => PebBasePointer.Read<int>((int)PEB32.OsPlatformId);
            set => PebBasePointer.Write(value, (int)PEB32.OsPlatformId);
        }
        public int ImageSubsystem
        {
            get => PebBasePointer.Read<int>((int)PEB32.ImageSubsystem);
            set => PebBasePointer.Write(value, (int)PEB32.ImageSubsystem);
        }
        public int ImageSubsystemMajorVersion
        {
            get => PebBasePointer.Read<int>((int)PEB32.ImageSubsystemMajorVersion);
            set => PebBasePointer.Write(value, (int)PEB32.ImageSubsystemMajorVersion);
        }
        public IntPtr ImageSubsystemMinorVersion
        {
            get => PebBasePointer.Read<IntPtr>((int)PEB32.ImageSubsystemMinorVersion);
            set => PebBasePointer.Write(value, (int)PEB32.ImageSubsystemMinorVersion);
        }
        public IntPtr ImageProcessAffinityMask
        {
            get => PebBasePointer.Read<IntPtr>((int)PEB32.ImageProcessAffinityMask);
            set => PebBasePointer.Write(value, (int)PEB32.ImageProcessAffinityMask);
        }
        //public IntPtr[] GdiHandleBuffer
        //{
        //    get { return PebAddress.Read<IntPtr>((int)PebStructure.GdiHandleBuffer, 0x22); }
        //    set { PebAddress.Write(value, (int)PebStructure.GdiHandleBuffer); }
        //}
        public IntPtr PostProcessInitRoutine
        {
            get => PebBasePointer.Read<IntPtr>((int)PEB32.PostProcessInitRoutine);
            set => PebBasePointer.Write(value, (int)PEB32.PostProcessInitRoutine);
        }
        public IntPtr TlsExpansionBitmap
        {
            get => PebBasePointer.Read<IntPtr>((int)PEB32.TlsExpansionBitmap);
            set => PebBasePointer.Write(value, (int)PEB32.TlsExpansionBitmap);
        }
        //public IntPtr[] TlsExpansionBitmapBits
        //{
        //    get { return PebAddress.Read<IntPtr>((int)PebStructure.TlsExpansionBitmapBits, 0x20); }
        //    set { PebAddress.Write(value, (int)PebStructure.TlsExpansionBitmapBits); }
        //}
        public IntPtr SessionId
        {
            get => PebBasePointer.Read<IntPtr>((int)PEB32.SessionId);
            set => PebBasePointer.Write(value, (int)PEB32.SessionId);
        }
        public long AppCompatFlags
        {
            get => PebBasePointer.Read<long>((int)PEB32.AppCompatFlags);
            set => PebBasePointer.Write(value, (int)PEB32.AppCompatFlags);
        }
        public long AppCompatFlagsUser
        {
            get => PebBasePointer.Read<long>((int)PEB32.AppCompatFlagsUser);
            set => PebBasePointer.Write(value, (int)PEB32.AppCompatFlagsUser);
        }
        public IntPtr ShimData
        {
            get => PebBasePointer.Read<IntPtr>((int)PEB32.ShimData);
            set => PebBasePointer.Write(value, (int)PEB32.ShimData);
        }
        public IntPtr AppCompatInfo
        {
            get => PebBasePointer.Read<IntPtr>((int)PEB32.AppCompatInfo);
            set => PebBasePointer.Write(value, (int)PEB32.AppCompatInfo);
        }
        public long CsdVersion
        {
            get => PebBasePointer.Read<long>((int)PEB32.CsdVersion);
            set => PebBasePointer.Write(value, (int)PEB32.CsdVersion);
        }
        public IntPtr ActivationContextData
        {
            get => PebBasePointer.Read<IntPtr>((int)PEB32.ActivationContextData);
            set => PebBasePointer.Write(value, (int)PEB32.ActivationContextData);
        }
        public IntPtr ProcessAssemblyStorageMap
        {
            get => PebBasePointer.Read<IntPtr>((int)PEB32.ProcessAssemblyStorageMap);
            set => PebBasePointer.Write(value, (int)PEB32.ProcessAssemblyStorageMap);
        }
        public IntPtr SystemDefaultActivationContextData
        {
            get => PebBasePointer.Read<IntPtr>((int)PEB32.SystemDefaultActivationContextData);
            set => PebBasePointer.Write(value, (int)PEB32.SystemDefaultActivationContextData);
        }
        public IntPtr SystemAssemblyStorageMap
        {
            get => PebBasePointer.Read<IntPtr>((int)PEB32.SystemAssemblyStorageMap);
            set => PebBasePointer.Write(value, (int)PEB32.SystemAssemblyStorageMap);
        }
        public IntPtr MinimumStackCommit
        {
            get => PebBasePointer.Read<IntPtr>((int)PEB32.MinimumStackCommit);
            set => PebBasePointer.Write(value, (int)PEB32.MinimumStackCommit);
        }
    }
}
