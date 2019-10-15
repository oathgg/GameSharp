using GameSharp.Core.Native.Enums;
using System;

namespace GameSharp.Core.Memory
{
    public class MemoryPeb
    {
        private readonly IMemoryPointer PebBasePointer;

        public MemoryPeb(IMemoryPointer pebAddress)
        {
            PebBasePointer = pebAddress;
        }

        public byte InheritedAddressSpace
        {
            get => PebBasePointer.Read<byte>((int)PebStructure.InheritedAddressSpace);
            set => PebBasePointer.Write(value, (int)PebStructure.InheritedAddressSpace);
        }
        public byte ReadImageFileExecOptions
        {
            get => PebBasePointer.Read<byte>((int)PebStructure.ReadImageFileExecOptions);
            set => PebBasePointer.Write(value, (int)PebStructure.ReadImageFileExecOptions);
        }
        public bool BeingDebugged
        {
            get => PebBasePointer.Read<bool>((int)PebStructure.BeingDebugged);
            set => PebBasePointer.Write(value, (int)PebStructure.BeingDebugged);
        }
        public byte SpareBool
        {
            get => PebBasePointer.Read<byte>((int)PebStructure.SpareBool);
            set => PebBasePointer.Write(value, (int)PebStructure.SpareBool);
        }
        public IntPtr Mutant
        {
            get => PebBasePointer.Read<IntPtr>((int)PebStructure.Mutant);
            set => PebBasePointer.Write(value, (int)PebStructure.Mutant);
        }
        public IntPtr Ldr
        {
            get => PebBasePointer.Read<IntPtr>((int)PebStructure.Ldr);
            set => PebBasePointer.Write(value, (int)PebStructure.Ldr);
        }
        public IntPtr ProcessParameters
        {
            get => PebBasePointer.Read<IntPtr>((int)PebStructure.ProcessParameters);
            set => PebBasePointer.Write(value, (int)PebStructure.ProcessParameters);
        }
        public IntPtr SubSystemData
        {
            get => PebBasePointer.Read<IntPtr>((int)PebStructure.SubSystemData);
            set => PebBasePointer.Write(value, (int)PebStructure.SubSystemData);
        }
        public IntPtr ProcessHeap
        {
            get => PebBasePointer.Read<IntPtr>((int)PebStructure.ProcessHeap);
            set => PebBasePointer.Write(value, (int)PebStructure.ProcessHeap);
        }
        public IntPtr FastPebLock
        {
            get => PebBasePointer.Read<IntPtr>((int)PebStructure.FastPebLock);
            set => PebBasePointer.Write(value, (int)PebStructure.FastPebLock);
        }
        public IntPtr FastPebLockRoutine
        {
            get => PebBasePointer.Read<IntPtr>((int)PebStructure.FastPebLockRoutine);
            set => PebBasePointer.Write(value, (int)PebStructure.FastPebLockRoutine);
        }
        public IntPtr FastPebUnlockRoutine
        {
            get => PebBasePointer.Read<IntPtr>((int)PebStructure.FastPebUnlockRoutine);
            set => PebBasePointer.Write(value, (int)PebStructure.FastPebUnlockRoutine);
        }
        public IntPtr EnvironmentUpdateCount
        {
            get => PebBasePointer.Read<IntPtr>((int)PebStructure.EnvironmentUpdateCount);
            set => PebBasePointer.Write(value, (int)PebStructure.EnvironmentUpdateCount);
        }
        public IntPtr KernelCallbackTable
        {
            get => PebBasePointer.Read<IntPtr>((int)PebStructure.KernelCallbackTable);
            set => PebBasePointer.Write(value, (int)PebStructure.KernelCallbackTable);
        }
        public int SystemReserved
        {
            get => PebBasePointer.Read<int>((int)PebStructure.SystemReserved);
            set => PebBasePointer.Write(value, (int)PebStructure.SystemReserved);
        }
        public int AtlThunkSListPtr32
        {
            get => PebBasePointer.Read<int>((int)PebStructure.AtlThunkSListPtr32);
            set => PebBasePointer.Write(value, (int)PebStructure.AtlThunkSListPtr32);
        }
        public IntPtr FreeList
        {
            get => PebBasePointer.Read<IntPtr>((int)PebStructure.FreeList);
            set => PebBasePointer.Write(value, (int)PebStructure.FreeList);
        }
        public IntPtr TlsExpansionCounter
        {
            get => PebBasePointer.Read<IntPtr>((int)PebStructure.TlsExpansionCounter);
            set => PebBasePointer.Write(value, (int)PebStructure.TlsExpansionCounter);
        }
        public IntPtr TlsBitmap
        {
            get => PebBasePointer.Read<IntPtr>((int)PebStructure.TlsBitmap);
            set => PebBasePointer.Write(value, (int)PebStructure.TlsBitmap);
        }
        public long TlsBitmapBits
        {
            get => PebBasePointer.Read<long>((int)PebStructure.TlsBitmapBits);
            set => PebBasePointer.Write(value, (int)PebStructure.TlsBitmapBits);
        }
        public IntPtr ReadOnlySharedMemoryBase
        {
            get => PebBasePointer.Read<IntPtr>((int)PebStructure.ReadOnlySharedMemoryBase);
            set => PebBasePointer.Write(value, (int)PebStructure.ReadOnlySharedMemoryBase);
        }
        public IntPtr ReadOnlySharedMemoryHeap
        {
            get => PebBasePointer.Read<IntPtr>((int)PebStructure.ReadOnlySharedMemoryHeap);
            set => PebBasePointer.Write(value, (int)PebStructure.ReadOnlySharedMemoryHeap);
        }
        public IntPtr ReadOnlyStaticServerData
        {
            get => PebBasePointer.Read<IntPtr>((int)PebStructure.ReadOnlyStaticServerData);
            set => PebBasePointer.Write(value, (int)PebStructure.ReadOnlyStaticServerData);
        }
        public IntPtr AnsiCodePageData
        {
            get => PebBasePointer.Read<IntPtr>((int)PebStructure.AnsiCodePageData);
            set => PebBasePointer.Write(value, (int)PebStructure.AnsiCodePageData);
        }
        public IntPtr OemCodePageData
        {
            get => PebBasePointer.Read<IntPtr>((int)PebStructure.OemCodePageData);
            set => PebBasePointer.Write(value, (int)PebStructure.OemCodePageData);
        }
        public IntPtr UnicodeCaseTableData
        {
            get => PebBasePointer.Read<IntPtr>((int)PebStructure.UnicodeCaseTableData);
            set => PebBasePointer.Write(value, (int)PebStructure.UnicodeCaseTableData);
        }
        public int NumberOfProcessors
        {
            get => PebBasePointer.Read<int>((int)PebStructure.NumberOfProcessors);
            set => PebBasePointer.Write(value, (int)PebStructure.NumberOfProcessors);
        }
        public long NtGlobalFlag
        {
            get => PebBasePointer.Read<long>((int)PebStructure.NtGlobalFlag);
            set => PebBasePointer.Write(value, (int)PebStructure.NtGlobalFlag);
        }
        public long CriticalSectionTimeout
        {
            get => PebBasePointer.Read<long>((int)PebStructure.CriticalSectionTimeout);
            set => PebBasePointer.Write(value, (int)PebStructure.CriticalSectionTimeout);
        }
        public IntPtr HeapSegmentReserve
        {
            get => PebBasePointer.Read<IntPtr>((int)PebStructure.HeapSegmentReserve);
            set => PebBasePointer.Write(value, (int)PebStructure.HeapSegmentReserve);
        }
        public IntPtr HeapSegmentCommit
        {
            get => PebBasePointer.Read<IntPtr>((int)PebStructure.HeapSegmentCommit);
            set => PebBasePointer.Write(value, (int)PebStructure.HeapSegmentCommit);
        }
        public IntPtr HeapDeCommitTotalFreeThreshold
        {
            get => PebBasePointer.Read<IntPtr>((int)PebStructure.HeapDeCommitTotalFreeThreshold);
            set => PebBasePointer.Write(value, (int)PebStructure.HeapDeCommitTotalFreeThreshold);
        }
        public IntPtr HeapDeCommitFreeBlockThreshold
        {
            get => PebBasePointer.Read<IntPtr>((int)PebStructure.HeapDeCommitFreeBlockThreshold);
            set => PebBasePointer.Write(value, (int)PebStructure.HeapDeCommitFreeBlockThreshold);
        }
        public int NumberOfHeaps
        {
            get => PebBasePointer.Read<int>((int)PebStructure.NumberOfHeaps);
            set => PebBasePointer.Write(value, (int)PebStructure.NumberOfHeaps);
        }
        public int MaximumNumberOfHeaps
        {
            get => PebBasePointer.Read<int>((int)PebStructure.MaximumNumberOfHeaps);
            set => PebBasePointer.Write(value, (int)PebStructure.MaximumNumberOfHeaps);
        }
        public IntPtr ProcessHeaps
        {
            get => PebBasePointer.Read<IntPtr>((int)PebStructure.ProcessHeaps);
            set => PebBasePointer.Write(value, (int)PebStructure.ProcessHeaps);
        }
        public IntPtr GdiSharedHandleTable
        {
            get => PebBasePointer.Read<IntPtr>((int)PebStructure.GdiSharedHandleTable);
            set => PebBasePointer.Write(value, (int)PebStructure.GdiSharedHandleTable);
        }
        public IntPtr ProcessStarterHelper
        {
            get => PebBasePointer.Read<IntPtr>((int)PebStructure.ProcessStarterHelper);
            set => PebBasePointer.Write(value, (int)PebStructure.ProcessStarterHelper);
        }
        public IntPtr GdiDcAttributeList
        {
            get => PebBasePointer.Read<IntPtr>((int)PebStructure.GdiDcAttributeList);
            set => PebBasePointer.Write(value, (int)PebStructure.GdiDcAttributeList);
        }
        public IntPtr LoaderLock
        {
            get => PebBasePointer.Read<IntPtr>((int)PebStructure.LoaderLock);
            set => PebBasePointer.Write(value, (int)PebStructure.LoaderLock);
        }
        public int OsMajorVersion
        {
            get => PebBasePointer.Read<int>((int)PebStructure.OsMajorVersion);
            set => PebBasePointer.Write(value, (int)PebStructure.OsMajorVersion);
        }
        public int OsMinorVersion
        {
            get => PebBasePointer.Read<int>((int)PebStructure.OsMinorVersion);
            set => PebBasePointer.Write(value, (int)PebStructure.OsMinorVersion);
        }
        public ushort OsBuildNumber
        {
            get => PebBasePointer.Read<ushort>((int)PebStructure.OsBuildNumber);
            set => PebBasePointer.Write(value, (int)PebStructure.OsBuildNumber);
        }
        public ushort OsCsdVersion
        {
            get => PebBasePointer.Read<ushort>((int)PebStructure.OsCsdVersion);
            set => PebBasePointer.Write(value, (int)PebStructure.OsCsdVersion);
        }
        public int OsPlatformId
        {
            get => PebBasePointer.Read<int>((int)PebStructure.OsPlatformId);
            set => PebBasePointer.Write(value, (int)PebStructure.OsPlatformId);
        }
        public int ImageSubsystem
        {
            get => PebBasePointer.Read<int>((int)PebStructure.ImageSubsystem);
            set => PebBasePointer.Write(value, (int)PebStructure.ImageSubsystem);
        }
        public int ImageSubsystemMajorVersion
        {
            get => PebBasePointer.Read<int>((int)PebStructure.ImageSubsystemMajorVersion);
            set => PebBasePointer.Write(value, (int)PebStructure.ImageSubsystemMajorVersion);
        }
        public IntPtr ImageSubsystemMinorVersion
        {
            get => PebBasePointer.Read<IntPtr>((int)PebStructure.ImageSubsystemMinorVersion);
            set => PebBasePointer.Write(value, (int)PebStructure.ImageSubsystemMinorVersion);
        }
        public IntPtr ImageProcessAffinityMask
        {
            get => PebBasePointer.Read<IntPtr>((int)PebStructure.ImageProcessAffinityMask);
            set => PebBasePointer.Write(value, (int)PebStructure.ImageProcessAffinityMask);
        }
        //public IntPtr[] GdiHandleBuffer
        //{
        //    get { return PebAddress.Read<IntPtr>((int)PebStructure.GdiHandleBuffer, 0x22); }
        //    set { PebAddress.Write(value, (int)PebStructure.GdiHandleBuffer); }
        //}
        public IntPtr PostProcessInitRoutine
        {
            get => PebBasePointer.Read<IntPtr>((int)PebStructure.PostProcessInitRoutine);
            set => PebBasePointer.Write(value, (int)PebStructure.PostProcessInitRoutine);
        }
        public IntPtr TlsExpansionBitmap
        {
            get => PebBasePointer.Read<IntPtr>((int)PebStructure.TlsExpansionBitmap);
            set => PebBasePointer.Write(value, (int)PebStructure.TlsExpansionBitmap);
        }
        //public IntPtr[] TlsExpansionBitmapBits
        //{
        //    get { return PebAddress.Read<IntPtr>((int)PebStructure.TlsExpansionBitmapBits, 0x20); }
        //    set { PebAddress.Write(value, (int)PebStructure.TlsExpansionBitmapBits); }
        //}
        public IntPtr SessionId
        {
            get => PebBasePointer.Read<IntPtr>((int)PebStructure.SessionId);
            set => PebBasePointer.Write(value, (int)PebStructure.SessionId);
        }
        public long AppCompatFlags
        {
            get => PebBasePointer.Read<long>((int)PebStructure.AppCompatFlags);
            set => PebBasePointer.Write(value, (int)PebStructure.AppCompatFlags);
        }
        public long AppCompatFlagsUser
        {
            get => PebBasePointer.Read<long>((int)PebStructure.AppCompatFlagsUser);
            set => PebBasePointer.Write(value, (int)PebStructure.AppCompatFlagsUser);
        }
        public IntPtr ShimData
        {
            get => PebBasePointer.Read<IntPtr>((int)PebStructure.ShimData);
            set => PebBasePointer.Write(value, (int)PebStructure.ShimData);
        }
        public IntPtr AppCompatInfo
        {
            get => PebBasePointer.Read<IntPtr>((int)PebStructure.AppCompatInfo);
            set => PebBasePointer.Write(value, (int)PebStructure.AppCompatInfo);
        }
        public long CsdVersion
        {
            get => PebBasePointer.Read<long>((int)PebStructure.CsdVersion);
            set => PebBasePointer.Write(value, (int)PebStructure.CsdVersion);
        }
        public IntPtr ActivationContextData
        {
            get => PebBasePointer.Read<IntPtr>((int)PebStructure.ActivationContextData);
            set => PebBasePointer.Write(value, (int)PebStructure.ActivationContextData);
        }
        public IntPtr ProcessAssemblyStorageMap
        {
            get => PebBasePointer.Read<IntPtr>((int)PebStructure.ProcessAssemblyStorageMap);
            set => PebBasePointer.Write(value, (int)PebStructure.ProcessAssemblyStorageMap);
        }
        public IntPtr SystemDefaultActivationContextData
        {
            get => PebBasePointer.Read<IntPtr>((int)PebStructure.SystemDefaultActivationContextData);
            set => PebBasePointer.Write(value, (int)PebStructure.SystemDefaultActivationContextData);
        }
        public IntPtr SystemAssemblyStorageMap
        {
            get => PebBasePointer.Read<IntPtr>((int)PebStructure.SystemAssemblyStorageMap);
            set => PebBasePointer.Write(value, (int)PebStructure.SystemAssemblyStorageMap);
        }
        public IntPtr MinimumStackCommit
        {
            get => PebBasePointer.Read<IntPtr>((int)PebStructure.MinimumStackCommit);
            set => PebBasePointer.Write(value, (int)PebStructure.MinimumStackCommit);
        }
    }
}
