namespace AspNetCoreLocalizer.Abstraction
{
    public interface ILocalizerService
    {
        void SetEntry(string key, string value, string culture);

        string GetLocalizedValue(string key, string culture);
        string GetLocalizedValue(string key);

        void ClearAll();
    }
}
