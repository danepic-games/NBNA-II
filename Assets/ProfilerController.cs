using System.Text;
using Unity.Profiling;
using UnityEngine;

public class ProfilerController : MonoBehaviour {
    string statsText;
    ProfilerRecorder totalReservedMemoryRecorder;
    ProfilerRecorder gcReservedMemoryRecorder;
    ProfilerRecorder systemUsedMemoryRecorder;
    ProfilerRecorder setpassTimeRecorder;
    ProfilerRecorder renderThread;
    ProfilerRecorder mainthread;
    ProfilerRecorder vertsRecorder;

    void OnEnable() {
        totalReservedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "Total Reserved Memory");
        systemUsedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "System Used Memory");
        gcReservedMemoryRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Memory, "GC Reserved Memory");
        setpassTimeRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "SetPass Calls Count", 15);
        renderThread = ProfilerRecorder.StartNew(ProfilerCategory.Internal, "Render Thread", 32);
        mainthread = ProfilerRecorder.StartNew(ProfilerCategory.Internal, "Main Thread", 32);
        vertsRecorder = ProfilerRecorder.StartNew(ProfilerCategory.Render, "Vertices Count", 16);
    }

    void OnDisable() {
        totalReservedMemoryRecorder.Dispose();
        gcReservedMemoryRecorder.Dispose();
        systemUsedMemoryRecorder.Dispose();
        setpassTimeRecorder.Dispose();
        renderThread.Dispose();
        vertsRecorder.Dispose();
        mainthread.Dispose();
    }

    void Update() {
        var sb = new StringBuilder(500);
        if (totalReservedMemoryRecorder.Valid)
            sb.AppendLine($"Total Reserved Memory: {totalReservedMemoryRecorder.LastValue / (1024 * 1024)} MB");
        if (gcReservedMemoryRecorder.Valid)
            sb.AppendLine($"GC Reserved Memory: {gcReservedMemoryRecorder.LastValue / (1024 * 1024)} MB");
        if (systemUsedMemoryRecorder.Valid)
            sb.AppendLine($"System Used Memory: {systemUsedMemoryRecorder.LastValue / (1024 * 1024)} MB");
        if (setpassTimeRecorder.Valid)
            sb.AppendLine($"SetPass Calls Count: {setpassTimeRecorder.LastValue}");
        if (renderThread.Valid)
            sb.AppendLine($"Render Thread: {renderThread.LastValue * (1e-6d):F2} ms");
        if (mainthread.Valid)
            sb.AppendLine($"Main Thread: {mainthread.LastValue * (1e-6d):F2} ms");
        if (vertsRecorder.Valid)
            sb.AppendLine($"Vertices count: {vertsRecorder.LastValue}");

        statsText = sb.ToString();
    }

    void OnGUI() {
        GUI.TextArea(new Rect(10, 30, 250, 125), statsText);
    }
}