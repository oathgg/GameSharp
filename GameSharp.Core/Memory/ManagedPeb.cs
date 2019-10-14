using GameSharp.Core.Native.Enums;
using System;

namespace GameSharp.Core.Memory
{
    public class ManagedPeb
    {
        private readonly IMemoryAddress PebAddress;

        public ManagedPeb(IMemoryAddress pebAddress)
        {
            PebAddress = pebAddress;
        }

        public byte InheritedAddressSpace
        {
            get { return PebAddress.Read<byte>((int)PebStructure.InheritedAddressSpace); }
            set { PebAddress.Write(value, (int)PebStructure.InheritedAddressSpace); }
        }
        public byte ReadImageFileExecOptions
        {
            get { return PebAddress.Read<byte>((int)PebStructure.ReadImageFileExecOptions); }
            set { PebAddress.Write(value, (int)PebStructure.ReadImageFileExecOptions); }
        }
        public bool BeingDebugged
        {
            get { return PebAddress.Read<bool>((int)PebStructure.BeingDebugged); }
            set { PebAddress.Write(value, (int)PebStructure.BeingDebugged); }
        }
        public byte SpareBool
        {
            get { return PebAddress.Read<byte>((int)PebStructure.SpareBool); }
            set { PebAddress.Write(value, (int)PebStructure.SpareBool); }
        }
        public IntPtr Mutant
        {
            get { return PebAddress.Read<IntPtr>((int)PebStructure.Mutant); }
            set { PebAddress.Write(value, (int)PebStructure.Mutant); }
        }
        public IntPtr Ldr
        {
            get { return PebAddress.Read<IntPtr>((int)PebStructure.Ldr); }
            set { PebAddress.Write(value, (int)PebStructure.Ldr); }
        }
        public IntPtr ProcessParameters
        {
            get { return PebAddress.Read<IntPtr>((int)PebStructure.ProcessParameters); }
            set { PebAddress.Write(value, (int)PebStructure.ProcessParameters); }
        }
        public IntPtr SubSystemData
        {
            get { return PebAddress.Read<IntPtr>((int)PebStructure.SubSystemData); }
            set { PebAddress.Write(value, (int)PebStructure.SubSystemData); }
        }
        public IntPtr ProcessHeap
        {
            get { return PebAddress.Read<IntPtr>((int)PebStructure.ProcessHeap); }
            set { PebAddress.Write(value, (int)PebStructure.ProcessHeap); }
        }
        public IntPtr FastPebLock
        {
            get { return PebAddress.Read<IntPtr>((int)PebStructure.FastPebLock); }
            set { PebAddress.Write(value, (int)PebStructure.FastPebLock); }
        }
        public IntPtr FastPebLockRoutine
        {
            get { return PebAddress.Read<IntPtr>((int)PebStructure.FastPebLockRoutine); }
            set { PebAddress.Write(value, (int)PebStructure.FastPebLockRoutine); }
        }
        public IntPtr FastPebUnlockRoutine
        {
            get { return PebAddress.Read<IntPtr>((int)PebStructure.FastPebUnlockRoutine); }
            set { PebAddress.Write(value, (int)PebStructure.FastPebUnlockRoutine); }
        }
        public IntPtr EnvironmentUpdateCount
        {
            get { return PebAddress.Read<IntPtr>((int)PebStructure.EnvironmentUpdateCount); }
            set { PebAddress.Write(value, (int)PebStructure.EnvironmentUpdateCount); }
        }
        public IntPtr KernelCallbackTable
        {
            get { return PebAddress.Read<IntPtr>((int)PebStructure.KernelCallbackTable); }
            set { PebAddress.Write(value, (int)PebStructure.KernelCallbackTable); }
        }
        public int SystemReserved
        {
            get { return PebAddress.Read<int>((int)PebStructure.SystemReserved); }
            set { PebAddress.Write(value, (int)PebStructure.SystemReserved); }
        }
        public int AtlThunkSListPtr32
        {
            get { return PebAddress.Read<int>((int)PebStructure.AtlThunkSListPtr32); }
            set { PebAddress.Write(value, (int)PebStructure.AtlThunkSListPtr32); }
        }
        public IntPtr FreeList
        {
            get { return PebAddress.Read<IntPtr>((int)PebStructure.FreeList); }
            set { PebAddress.Write(value, (int)PebStructure.FreeList); }
        }
        public IntPtr TlsExpansionCounter
        {
            get { return PebAddress.Read<IntPtr>((int)PebStructure.TlsExpansionCounter); }
            set { PebAddress.Write(value, (int)PebStructure.TlsExpansionCounter); }
        }
        public IntPtr TlsBitmap
        {
            get { return PebAddress.Read<IntPtr>((int)PebStructure.TlsBitmap); }
            set { PebAddress.Write(value, (int)PebStructure.TlsBitmap); }
        }
        public long TlsBitmapBits
        {
            get { return PebAddress.Read<long>((int)PebStructure.TlsBitmapBits); }
            set { PebAddress.Write(value, (int)PebStructure.TlsBitmapBits); }
        }
        public IntPtr ReadOnlySharedMemoryBase
        {
            get { return PebAddress.Read<IntPtr>((int)PebStructure.ReadOnlySharedMemoryBase); }
            set { PebAddress.Write(value, (int)PebStructure.ReadOnlySharedMemoryBase); }
        }
        public IntPtr ReadOnlySharedMemoryHeap
        {
            get { return PebAddress.Read<IntPtr>((int)PebStructure.ReadOnlySharedMemoryHeap); }
            set { PebAddress.Write(value, (int)PebStructure.ReadOnlySharedMemoryHeap); }
        }
        public IntPtr ReadOnlyStaticServerData
        {
            get { return PebAddress.Read<IntPtr>((int)PebStructure.ReadOnlyStaticServerData); }
            set { PebAddress.Write(value, (int)PebStructure.ReadOnlyStaticServerData); }
        }
        public IntPtr AnsiCodePageData
        {
            get { return PebAddress.Read<IntPtr>((int)PebStructure.AnsiCodePageData); }
            set { PebAddress.Write(value, (int)PebStructure.AnsiCodePageData); }
        }
        public IntPtr OemCodePageData
        {
            get { return PebAddress.Read<IntPtr>((int)PebStructure.OemCodePageData); }
            set { PebAddress.Write(value, (int)PebStructure.OemCodePageData); }
        }
        public IntPtr UnicodeCaseTableData
        {
            get { return PebAddress.Read<IntPtr>((int)PebStructure.UnicodeCaseTableData); }
            set { PebAddress.Write(value, (int)PebStructure.UnicodeCaseTableData); }
        }
        public int NumberOfProcessors
        {
            get { return PebAddress.Read<int>((int)PebStructure.NumberOfProcessors); }
            set { PebAddress.Write(value, (int)PebStructure.NumberOfProcessors); }
        }
        public long NtGlobalFlag
        {
            get { return PebAddress.Read<long>((int)PebStructure.NtGlobalFlag); }
            set { PebAddress.Write(value, (int)PebStructure.NtGlobalFlag); }
        }
        public long CriticalSectionTimeout
        {
            get { return PebAddress.Read<long>((int)PebStructure.CriticalSectionTimeout); }
            set { PebAddress.Write(value, (int)PebStructure.CriticalSectionTimeout); }
        }
        public IntPtr HeapSegmentReserve
        {
            get { return PebAddress.Read<IntPtr>((int)PebStructure.HeapSegmentReserve); }
            set { PebAddress.Write(value, (int)PebStructure.HeapSegmentReserve); }
        }
        public IntPtr HeapSegmentCommit
        {
            get { return PebAddress.Read<IntPtr>((int)PebStructure.HeapSegmentCommit); }
            set { PebAddress.Write(value, (int)PebStructure.HeapSegmentCommit); }
        }
        public IntPtr HeapDeCommitTotalFreeThreshold
        {
            get { return PebAddress.Read<IntPtr>((int)PebStructure.HeapDeCommitTotalFreeThreshold); }
            set { PebAddress.Write(value, (int)PebStructure.HeapDeCommitTotalFreeThreshold); }
        }
        public IntPtr HeapDeCommitFreeBlockThreshold
        {
            get { return PebAddress.Read<IntPtr>((int)PebStructure.HeapDeCommitFreeBlockThreshold); }
            set { PebAddress.Write(value, (int)PebStructure.HeapDeCommitFreeBlockThreshold); }
        }
        public int NumberOfHeaps
        {
            get { return PebAddress.Read<int>((int)PebStructure.NumberOfHeaps); }
            set { PebAddress.Write(value, (int)PebStructure.NumberOfHeaps); }
        }
        public int MaximumNumberOfHeaps
        {
            get { return PebAddress.Read<int>((int)PebStructure.MaximumNumberOfHeaps); }
            set { PebAddress.Write(value, (int)PebStructure.MaximumNumberOfHeaps); }
        }
        public IntPtr ProcessHeaps
        {
            get { return PebAddress.Read<IntPtr>((int)PebStructure.ProcessHeaps); }
            set { PebAddress.Write(value, (int)PebStructure.ProcessHeaps); }
        }
        public IntPtr GdiSharedHandleTable
        {
            get { return PebAddress.Read<IntPtr>((int)PebStructure.GdiSharedHandleTable); }
            set { PebAddress.Write(value, (int)PebStructure.GdiSharedHandleTable); }
        }
        public IntPtr ProcessStarterHelper
        {
            get { return PebAddress.Read<IntPtr>((int)PebStructure.ProcessStarterHelper); }
            set { PebAddress.Write(value, (int)PebStructure.ProcessStarterHelper); }
        }
        public IntPtr GdiDcAttributeList
        {
            get { return PebAddress.Read<IntPtr>((int)PebStructure.GdiDcAttributeList); }
            set { PebAddress.Write(value, (int)PebStructure.GdiDcAttributeList); }
        }
        public IntPtr LoaderLock
        {
            get { return PebAddress.Read<IntPtr>((int)PebStructure.LoaderLock); }
            set { PebAddress.Write(value, (int)PebStructure.LoaderLock); }
        }
        public int OsMajorVersion
        {
            get { return PebAddress.Read<int>((int)PebStructure.OsMajorVersion); }
            set { PebAddress.Write(value, (int)PebStructure.OsMajorVersion); }
        }
        public int OsMinorVersion
        {
            get { return PebAddress.Read<int>((int)PebStructure.OsMinorVersion); }
            set { PebAddress.Write(value, (int)PebStructure.OsMinorVersion); }
        }
        public ushort OsBuildNumber
        {
            get { return PebAddress.Read<ushort>((int)PebStructure.OsBuildNumber); }
            set { PebAddress.Write(value, (int)PebStructure.OsBuildNumber); }
        }
        public ushort OsCsdVersion
        {
            get { return PebAddress.Read<ushort>((int)PebStructure.OsCsdVersion); }
            set { PebAddress.Write(value, (int)PebStructure.OsCsdVersion); }
        }
        public int OsPlatformId
        {
            get { return PebAddress.Read<int>((int)PebStructure.OsPlatformId); }
            set { PebAddress.Write(value, (int)PebStructure.OsPlatformId); }
        }
        public int ImageSubsystem
        {
            get { return PebAddress.Read<int>((int)PebStructure.ImageSubsystem); }
            set { PebAddress.Write(value, (int)PebStructure.ImageSubsystem); }
        }
        public int ImageSubsystemMajorVersion
        {
            get { return PebAddress.Read<int>((int)PebStructure.ImageSubsystemMajorVersion); }
            set { PebAddress.Write(value, (int)PebStructure.ImageSubsystemMajorVersion); }
        }
        public IntPtr ImageSubsystemMinorVersion
        {
            get { return PebAddress.Read<IntPtr>((int)PebStructure.ImageSubsystemMinorVersion); }
            set { PebAddress.Write(value, (int)PebStructure.ImageSubsystemMinorVersion); }
        }
        public IntPtr ImageProcessAffinityMask
        {
            get { return PebAddress.Read<IntPtr>((int)PebStructure.ImageProcessAffinityMask); }
            set { PebAddress.Write(value, (int)PebStructure.ImageProcessAffinityMask); }
        }
        //public IntPtr[] GdiHandleBuffer
        //{
        //    get { return PebAddress.Read<IntPtr>((int)PebStructure.GdiHandleBuffer, 0x22); }
        //    set { PebAddress.Write(value, (int)PebStructure.GdiHandleBuffer); }
        //}
        public IntPtr PostProcessInitRoutine
        {
            get { return PebAddress.Read<IntPtr>((int)PebStructure.PostProcessInitRoutine); }
            set { PebAddress.Write(value, (int)PebStructure.PostProcessInitRoutine); }
        }
        public IntPtr TlsExpansionBitmap
        {
            get { return PebAddress.Read<IntPtr>((int)PebStructure.TlsExpansionBitmap); }
            set { PebAddress.Write(value, (int)PebStructure.TlsExpansionBitmap); }
        }
        //public IntPtr[] TlsExpansionBitmapBits
        //{
        //    get { return PebAddress.Read<IntPtr>((int)PebStructure.TlsExpansionBitmapBits, 0x20); }
        //    set { PebAddress.Write(value, (int)PebStructure.TlsExpansionBitmapBits); }
        //}
        public IntPtr SessionId
        {
            get { return PebAddress.Read<IntPtr>((int)PebStructure.SessionId); }
            set { PebAddress.Write(value, (int)PebStructure.SessionId); }
        }
        public long AppCompatFlags
        {
            get { return PebAddress.Read<long>((int)PebStructure.AppCompatFlags); }
            set { PebAddress.Write(value, (int)PebStructure.AppCompatFlags); }
        }
        public long AppCompatFlagsUser
        {
            get { return PebAddress.Read<long>((int)PebStructure.AppCompatFlagsUser); }
            set { PebAddress.Write(value, (int)PebStructure.AppCompatFlagsUser); }
        }
        public IntPtr ShimData
        {
            get { return PebAddress.Read<IntPtr>((int)PebStructure.ShimData); }
            set { PebAddress.Write(value, (int)PebStructure.ShimData); }
        }
        public IntPtr AppCompatInfo
        {
            get { return PebAddress.Read<IntPtr>((int)PebStructure.AppCompatInfo); }
            set { PebAddress.Write(value, (int)PebStructure.AppCompatInfo); }
        }
        public long CsdVersion
        {
            get { return PebAddress.Read<long>((int)PebStructure.CsdVersion); }
            set { PebAddress.Write(value, (int)PebStructure.CsdVersion); }
        }
        public IntPtr ActivationContextData
        {
            get { return PebAddress.Read<IntPtr>((int)PebStructure.ActivationContextData); }
            set { PebAddress.Write(value, (int)PebStructure.ActivationContextData); }
        }
        public IntPtr ProcessAssemblyStorageMap
        {
            get { return PebAddress.Read<IntPtr>((int)PebStructure.ProcessAssemblyStorageMap); }
            set { PebAddress.Write(value, (int)PebStructure.ProcessAssemblyStorageMap); }
        }
        public IntPtr SystemDefaultActivationContextData
        {
            get { return PebAddress.Read<IntPtr>((int)PebStructure.SystemDefaultActivationContextData); }
            set { PebAddress.Write(value, (int)PebStructure.SystemDefaultActivationContextData); }
        }
        public IntPtr SystemAssemblyStorageMap
        {
            get { return PebAddress.Read<IntPtr>((int)PebStructure.SystemAssemblyStorageMap); }
            set { PebAddress.Write(value, (int)PebStructure.SystemAssemblyStorageMap); }
        }
        public IntPtr MinimumStackCommit
        {
            get { return PebAddress.Read<IntPtr>((int)PebStructure.MinimumStackCommit); }
            set { PebAddress.Write(value, (int)PebStructure.MinimumStackCommit); }
        }
    }
}
