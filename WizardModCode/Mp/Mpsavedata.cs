using System.IO;
using System.Text.Json;
using Godot;
using MegaCrit.Sts2.Core.Saves;

namespace WizardMod.WizardModCode.Mp;

/// <summary>
/// WizardModのMPをランをまたいで保持するための静的セーブデータクラス。
/// SaveManager.Instance.GetProfileScopedPath()を利用してゲームと同じプロファイルフォルダに保存する。
/// </summary>
public static class MpSaveData
{
    private const string FileName = "wizardmod_mp.json";

    public static int CurrentMp { get; set; } = 0;
    public static int MaxMp { get; set; } = 0;

    private static string SavePath => SaveManager.Instance.GetProfileScopedPath(FileName);

    /// <summary>
    /// MPデータをJSONファイルに保存する。
    /// MP変動時に呼ぶ。
    /// </summary>
    public static void Save()
    {
        try
        {
            string json = JsonSerializer.Serialize(new MpSavePayload
            {
                CurrentMp = CurrentMp,
                MaxMp = MaxMp
            });
            File.WriteAllText(SavePath, json);
        }
        catch (Exception e)
        {
            GD.PrintErr($"[WizardMod] MpSaveData.Save failed: {e}");
        }
    }

    /// <summary>
    /// JSONファイルからMPデータを読み込む。
    /// ファイルが存在しない場合は初期値のまま（新規ラン扱い）。
    /// ラン開始時またはゲーム起動時に呼ぶ。
    /// </summary>
    public static void Load()
    {
        try
        {
            if (!File.Exists(SavePath)) return;

            string json = File.ReadAllText(SavePath);
            var payload = JsonSerializer.Deserialize<MpSavePayload>(json);
            if (payload == null) return;

            CurrentMp = payload.CurrentMp;
            MaxMp = payload.MaxMp;
        }
        catch (Exception e)
        {
            GD.PrintErr($"[WizardMod] MpSaveData.Load failed: {e}");
        }
    }

    /// <summary>
    /// ランが終了したときにセーブファイルを削除する。
    /// ラン終了（勝利・敗北）時に呼ぶ。
    /// </summary>
    public static void Delete()
    {
        try
        {
            if (File.Exists(SavePath))
                File.Delete(SavePath);

            CurrentMp = 0;
            MaxMp = 0;
        }
        catch (Exception e)
        {
            GD.PrintErr($"[WizardMod] MpSaveData.Delete failed: {e}");
        }
    }

    /// <summary>
    /// MPを消費する。0未満にはならない。
    /// 消費できた場合はtrueを返す。
    /// </summary>
    public static bool TryConsume(int amount)
    {
        if (CurrentMp < amount) return false;
        CurrentMp -= amount;
        Save();
        return true;
    }

    /// <summary>
    /// MPを回復する。MaxMpを超えない。
    /// </summary>
    public static void Restore(int amount)
    {
        CurrentMp = Math.Min(CurrentMp + amount, MaxMp);
        Save();
    }

    /// <summary>
    /// MPを初期化する（MaxMpをセットして現在値もMaxMpにする）。
    /// 新規ランの開始時に呼ぶ。
    /// </summary>
    public static void Initialize(int maxMp)
    {
        MaxMp = maxMp;
        CurrentMp = maxMp;
        Save();
    }

    private class MpSavePayload
    {
        public int CurrentMp { get; set; }
        public int MaxMp { get; set; }
    }
}