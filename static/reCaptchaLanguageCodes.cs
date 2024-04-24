﻿////////////////////////////////////////////////
// © https://github.com/badhitman - @fakegov 
////////////////////////////////////////////////

namespace reCaptcha;

/// <summary>
/// Языковые коды reCaptcha
/// </summary>
public static class ReCaptchaLanguageCodes
{
    /// <summary>
    /// Доступные языковые коды
    /// </summary>
    public static Dictionary<string, string> AvailableCodes => new()
    {
        {"ar", "Arabic"}, {"af", "Afrikaans"}, {"am", "Amharic"}, {"az", "Azerbaijani"}, {"hy", "Armenian"},
        {"eu", "Basque"}, { "bn", "Bengali"}, {"bg", "Bulgarian"}, {"ca", "Catalan"}, {"zh-HK", "Chinese (Hong Kong)"},
        {"zh-CN", "Chinese (Simplified)"}, { "zh-TW", "Chinese (Traditional)"}, {"hr", "Croatian"}, {"cs", "Czech"},
        {"da", "Danish"}, {"nl", "Dutch"}, {"en-GB", "English (UK)"}, {"en", "English (US)"}, {"et", "Estonian"},
        {"fil", "Filipino"}, {"fi", "Finnish"}, {"fr", "French"}, {"fr-CA", "French (Canadian)"}, { "gl", "Galician"},
        {"ka", "Georgian"}, {"de", "German"}, {"de-AT", "German (Austria)"}, {"de-CH", "German (Switzerland)"},
        {"el", "Greek"}, {"gu", "Gujarati"}, {"iw", "Hebrew"}, {"hi", "Hindi"}, {"hu", "Hungarain"}, {"is", "Icelandic"},
        {"id", "Indonesian"}, { "it", "Italian"}, {"ja", "Japanese"}, {"kn", "Kannada"}, {"ko", "Korean"}, {"lo", "Laothian"},
        {"lv", "Latvian"}, {"lt", "Lithuanian"}, { "ms", "Malay"}, {"ml", "Malayalam"}, {"mr", "Marathi"}, {"mn", "Mongolian"},
        {"no", "Norwegian"}, {"fa", "Persian"}, {"pl", "Polish"}, { "pt", "Portuguese"}, {"pt-BR", "Portuguese (Brazil)"},
        {"pt-PT", "Portuguese (Portugal)"}, {"ro", "Romanian"}, {"ru", "Russian"}, { "sr", "Serbian"}, {"si", "Sinhalese"},
        {"sk", "Slovak"}, { "sl", "Slovenian"}, {"es", "Spanish"}, {"es-419", "Spanish (Latin America)"}, { "sw", "Swahili"},
        {"sv", "Swedish"}, { "ta", "Tamil"}, {"te", "Telugu"}, {"th", "Thai"}, {"tr", "Turkish"}, {"uk", "Ukrainian"}, {"ur", "Urdu"},
        {"vi", "Vietnamese"}, {"zu", "Zulu"}
    };
}