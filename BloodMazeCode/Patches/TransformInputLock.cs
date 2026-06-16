namespace BloodMaze.BloodMazeCode.Patches;

public static class TransformInputLock
{
    private static int _lockCount;

    public static bool IsLocked => _lockCount > 0;

    public static void Lock()
    {
        _lockCount++;
        MainFile.Logger.Info($"[TransformInputLock] Lock count={_lockCount}");
    }

    public static void Unlock()
    {
        _lockCount--;
        if (_lockCount < 0)
            _lockCount = 0;

        MainFile.Logger.Info($"[TransformInputLock] Unlock count={_lockCount}");
    }
}