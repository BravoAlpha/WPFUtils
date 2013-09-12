using System;
using System.Windows.Markup;

namespace WPFUtils.Translation
{
    /// <summary>
    /// The Translate Markup extension returns a binding to a TranslationData
    /// that provides a translated resource of the specified key
    /// </summary>
    public class TranslateExtension : MarkupExtension
    {
        private string m_key;

        public TranslateExtension () {}

        /// <summary>
        /// Initializes a new instance of the <see cref="TranslateExtension"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        public TranslateExtension(string key)
        {
            m_key = key;
        }

        [ConstructorArgument("key")]
        public string Key
        {
            get { return m_key; }
            set { m_key = value;}
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return TranslationService.Instance.Translate(m_key);
        }
    }
}
