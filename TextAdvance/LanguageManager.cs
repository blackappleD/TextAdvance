namespace TextAdvance;

/// <summary>
/// Selects the UI language. Translations are stored in Language{Name}.ini files next to the plugin assembly
/// and are looked up through ECommons <see cref="Localization"/> with <c>"text".Loc()</c>.
/// </summary>
internal static class LanguageManager
{
    internal const string English = "English";

    private static readonly Dictionary<string, string> DalamudLanguageMap = new()
    {
        ["zh"] = "ChineseSimplified",
        ["tw"] = "ChineseTraditional",
        ["ja"] = "Japanese",
        ["de"] = "German",
        ["fr"] = "French",
        ["ko"] = "Korean",
    };

    private static readonly Dictionary<string, string> DisplayNames = new()
    {
        [English] = "English",
        ["ChineseSimplified"] = "简体中文",
        ["ChineseTraditional"] = "繁體中文",
        ["Japanese"] = "日本語",
        ["German"] = "Deutsch",
        ["French"] = "Français",
        ["Korean"] = "한국어",
    };

    internal static void Init()
    {
        Svc.PluginInterface.LanguageChanged += OnLanguageChanged;
        Apply();
    }

    internal static void Dispose()
    {
        Svc.PluginInterface.LanguageChanged -= OnLanguageChanged;
    }

    private static void OnLanguageChanged(string langCode)
    {
        if (string.IsNullOrEmpty(C.Language)) Apply();
    }

    /// <summary>
    /// Returns configured language, or language matching Dalamud's UI language when set to auto.
    /// </summary>
    internal static string Resolve()
    {
        if (!string.IsNullOrEmpty(C.Language)) return C.Language;
        if (DalamudLanguageMap.TryGetValue(Svc.PluginInterface.UiLanguage ?? "", out var lang) && Localization.GetAvaliableLanguages().Contains(lang))
        {
            return lang;
        }
        return English;
    }

    internal static void Apply()
    {
        var lang = Resolve();
        Localization.Init(lang == English ? null : lang);
        P.RegisterCommand();
    }

    internal static string GetDisplayName(string lang)
    {
        return DisplayNames.TryGetValue(lang, out var name) ? name : lang;
    }
}
