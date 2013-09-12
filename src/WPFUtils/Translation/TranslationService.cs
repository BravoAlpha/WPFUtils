using System.Globalization;
using System.Threading;
using System.Reflection;

namespace WPFUtils.Translation
{
    public class TranslationService
    {
        private static TranslationService m_translationService;

        public ITranslationProvider TranslationProvider { get; set; }

        public CultureInfo CurrentLanguage
        {
            get { return Thread.CurrentThread.CurrentUICulture; }
            set
            {
                if (Equals(value, Thread.CurrentThread.CurrentUICulture))
                    return;

                Thread.CurrentThread.CurrentUICulture = value;
            }
        }

        public static TranslationService Instance
        {
            get { return m_translationService ?? (m_translationService = new TranslationService()); }
        }

        private TranslationService()
        {
            if (Assembly.GetEntryAssembly() == null) // Is in design mode
            {
                Assembly resourcesAssembly = Assembly.Load("Translation");
                TranslationProvider = new ResxTranslationProvider("Translation.Resources", resourcesAssembly);
            }
        }

        public string Translate(string key)
        {
            if (TranslationProvider == null)
                return string.Format("!{0}!", key);

            string translatedValue = TranslationProvider.Translate(key);
            return translatedValue ?? string.Format("!{0}!", key);
        }
    }
}
