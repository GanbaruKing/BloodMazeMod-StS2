using System;
using System.IO;
using System.Text.Json;
using Godot;
using MegaCrit.Sts2.Core.Saves;

namespace BloodMaze.BloodMazeCode.Mp;

public static class MpSaveData
{
    private const string FileName = "bloodmaze_mp.json";

    public static event Action? MpChanged;

    private static int _currentMp = 0;
    private static int _maxMp = 0;
    private static int? _combatStartMp = null;
    private static int? _restSiteEnteredMp = null;
    private static bool _restSiteHealed = false;

    public static int CurrentMp
    {
        get => _currentMp;
        set
        {
            _currentMp = value;
            MpChanged?.Invoke();
        }
    }

    public static int MaxMp
    {
        get => _maxMp;
        set
        {
            _maxMp = value;
            MpChanged?.Invoke();
        }
    }

    public static int? CombatStartMp => _combatStartMp;
    public static int? RestSiteEnteredMp => _restSiteEnteredMp;
    public static bool RestSiteHealed => _restSiteHealed;

    private static string SavePath =>
        ProjectSettings.GlobalizePath(
            SaveManager.Instance.GetProfileScopedPath(FileName)
        );

    public static void Save()
    {
        try
        {
            var payload = new MpSavePayload
            {
                CurrentMp = CurrentMp,
                MaxMp = MaxMp,
                CombatStartMp = _combatStartMp,
                RestSiteEnteredMp = _restSiteEnteredMp,
                RestSiteHealed = _restSiteHealed
            };
            File.WriteAllText(SavePath, JsonSerializer.Serialize(payload));
        }
        catch (Exception e)
        {
            GD.PrintErr($"[BloodMaze] MpSaveData.Save failed: {e}");
        }
    }

    public static void Load()
    {
        GD.Print($"[BloodMaze] Load() called from:\n{System.Environment.StackTrace}");
        try
        {
            if (!File.Exists(SavePath)) return;

            var payload = JsonSerializer.Deserialize<MpSavePayload>(File.ReadAllText(SavePath));
            if (payload == null) return;

            _combatStartMp = payload.CombatStartMp;
            _restSiteEnteredMp = payload.RestSiteEnteredMp;
            _restSiteHealed = payload.RestSiteHealed;
            MaxMp = payload.MaxMp;
            CurrentMp = _combatStartMp ?? payload.CurrentMp;
        }
        catch (Exception e)
        {
            GD.PrintErr($"[BloodMaze] MpSaveData.Load failed: {e}");
        }
    }

    public static void Delete()
    {
        try
        {
            if (File.Exists(SavePath))
                File.Delete(SavePath);

            _combatStartMp = null;
            _restSiteEnteredMp = null;
            _restSiteHealed = false;
            _currentMp = 0;
            _maxMp = 0;
            MpChanged?.Invoke();
        }
        catch (Exception e)
        {
            GD.PrintErr($"[BloodMaze] MpSaveData.Delete failed: {e}");
        }
    }

    public static bool TryConsume(int amount)
    {
        if (CurrentMp < amount) return false;
        CurrentMp -= amount;
        Save();
        return true;
    }

    public static void Restore(int amount)
    {
        CurrentMp = Math.Min(CurrentMp + amount, MaxMp);
        Save();
    }

    public static void Initialize(int maxMp)
    {
        _combatStartMp = null;
        _restSiteEnteredMp = null;
        _restSiteHealed = false;
        MaxMp = maxMp;
        CurrentMp = maxMp;
        Save();
    }

    public static void SaveCombatStart()
    {
        _combatStartMp = CurrentMp;
        Save();
    }

    public static void ClearCombatStart()
    {
        _combatStartMp = null;
        Save();
    }

    public static void SaveRestSiteEntered()
    {
        _restSiteEnteredMp = CurrentMp;
        _restSiteHealed = false;
        Save();
    }

    public static void SetRestSiteHealed()
    {
        _restSiteHealed = true;
        Save();
    }

    public static void ClearRestSiteEntered()
    {
        _restSiteEnteredMp = null;
        _restSiteHealed = false;
        Save();
    }

    private class MpSavePayload
    {
        public int CurrentMp { get; set; }
        public int MaxMp { get; set; }
        public int? CombatStartMp { get; set; }
        public int? RestSiteEnteredMp { get; set; }
        public bool RestSiteHealed { get; set; }
    }
}