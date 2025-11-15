using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AegisTasks.UI.Language;

namespace AegisTasks.UnitTests.UI
{
    [TestClass]
    public class LanguageTexts_Test
    {
        // Valores esperados para cada idioma
        private const string ExpectedTestText_ES = "Texto de prueba";
        private const string ExpectedTestText_EN = "Test text";
        private const string ExpectedTestText_GL = "Texto de proba";

        [TestMethod]
        public void TestText_ShouldReturnExpectedValue_InSpanish()
        {
            // Cambiamos la cultura a español
            Texts.Culture = new CultureInfo("es-ES");

            string actualText = Texts.TestText;

            Assert.AreEqual(ExpectedTestText_ES, actualText);
        }

        [TestMethod]
        public void TestText_ShouldReturnExpectedValue_InEnglish()
        {
            // Cambiamos la cultura a inglés
            Texts.Culture = new CultureInfo("en-US");

            string actualText = Texts.TestText;

            Assert.AreEqual(ExpectedTestText_EN, actualText);
        }

        [TestMethod]
        public void TestText_ShouldReturnExpectedValue_InGalician()
        {
            // Cambiamos la cultura a gallego
            Texts.Culture = new CultureInfo("gl-ES");

            string actualText = Texts.TestText;

            Assert.AreEqual(ExpectedTestText_GL, actualText);
        }
    }
}
