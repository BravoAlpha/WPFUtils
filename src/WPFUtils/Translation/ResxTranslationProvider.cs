using System.Resources;
using System.Reflection;

namespace WPFUtils.Translation
{
    public class ResxTranslationProvider : ITranslationProvider
    {
        private readonly ResourceManager m_resourceManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResxTranslationProvider"/> class.
        /// </summary>
        /// <param name="baseName">Name of the base.</param>
        /// <param name="assembly">The assembly.</param>
        public ResxTranslationProvider(string baseName, Assembly assembly)
        {
            m_resourceManager = new ResourceManager(baseName, assembly);
        }

        public string Translate(string key)
        {
            return m_resourceManager.GetString(key);
        }
    }
}
