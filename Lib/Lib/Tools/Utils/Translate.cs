using lib;
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Translate.cs" company="SecurityCRM">
//   Copyright ©  2020
// </copyright>
// <summary>
//   Defines the Translate type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Lib.Tools.Utils
{
    using System.Collections.Generic;
    using System;
    using System.Data.SqlClient;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Resources;
    using Lib.Tools.BO;
    using Lib.Tools.Localization;
    using System.Text.Encodings.Web;
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;
    using Lib.BusinessObjects.Translations;

    /// <summary>
    /// The translate.
    /// </summary>
    public class Translate
    {
        /// <summary>
        /// The words.
        /// </summary>
        private static Dictionary<string, string> words;



        /// <summary>
        /// The get translated value.
        /// </summary>
        /// <param name="alias">
        /// The alias.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetTranslatedValue(string alias, string ResourceFile)
        {
            return GetTranslatedValue(alias, ResourceFile, alias);
        }

        public static Dictionary<string, ResourceData> Resources = new Dictionary<string, ResourceData>();

        public static List<StaticTranslation> Translations = null;
        /// <summary>
        /// The get translated value.
        /// </summary>
        /// <param name="alias">
        /// The alias.
        /// </param>
        /// <param name="defaultvalue">
        /// The default value.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetTranslatedValue(string alias, string ResourceFile, string defaultvalue)
        {
            string translated = string.Empty;

            try
            {
                return alias;
                //var cachedTranslations = CacheManager.GetCachedIISItem("StaticTranslation");
                
                Language currentLanguage = Language.Current();
                if (currentLanguage == null)
                {
                    currentLanguage = Language.Romanian;
                    currentLanguage.ShortName = "ro";
                }

                if (Translations == null)
                {
                    Translations = StaticTranslation.Populate();
                }

                if (Translations != null && Translations.Any(x => x.Key == alias))
                {
                    var Translation = new StaticTranslation();
                    if (Translations.Any(x => x.Key == alias && x.StaticTranslationAssembly.Key == ResourceFile))
                    {
                        Translation = Translations.First(x => x.Key == alias && x.StaticTranslationAssembly.Key == ResourceFile);
                    }
                    else
                    {
                        Translation = Translations.First(x => x.Key == alias);
                    }
                    translated = Translation != null && Translation.Id > 0 && Translation.Values != null && Translation.Values.Values.Any(x => ((StaticTranslationValue)x).Language == currentLanguage) ?
                        ((StaticTranslationValue)Translation.Values.Values.First(x => ((StaticTranslationValue)x).Language == currentLanguage)).Value : "";
                }
                
            }
            catch (Exception ex)
            {
                //if (HttpContextHelper.Current != null)
                //{
                    //CacheManager.RemoveCachedIISItem("StaticTranslation");
                //}
                Debug.Write(ex.ToString());
            }

            return !string.IsNullOrEmpty(translated) ? translated : defaultvalue;
        }


        /// <summary>
        /// The transliteral.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="count">
        /// The count.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string Translit(string value, int count)
        {
            if (words == null)
            {
                GenerateWords();
            }

            if (value.Length > count)
            {
                value = value.Substring(0, count);
            }

            value = words.Aggregate(value, (current, pair) => current.Replace(pair.Key, pair.Value));

            var result = value.Where((t, i) => char.IsLetter(value, i) || IsWord(t)).Aggregate(string.Empty, (current, t) => current + t);

            return UrlEncoder.Default.Encode(result);
        }



        /// <summary>
        /// The is word.
        /// </summary>
        /// <param name="character">
        /// The character.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public static bool IsWord(char character)
        {
            return words.Values.Any(val => val == character.ToString());
        }

        /// <summary>
        /// The generate words.
        /// </summary>
        public static void GenerateWords()
        {
            words = new Dictionary<string, string>
                        {
                            { "à", "a" },
                            { "á", "b" },
                            { "â", "v" },
                            { "ã", "g" },
                            { "ä", "d" },
                            { "å", "e" },
                            { "¸", "yo" },
                            { "æ", "zh" },
                            { "ç", "z" },
                            { "è", "i" },
                            { "é", "j" },
                            { "ê", "k" },
                            { "ë", "l" },
                            { "ì", "m" },
                            { "í", "n" },
                            { "î", "o" },
                            { "ï", "p" },
                            { "ð", "r" },
                            { "ñ", "s" },
                            { "ò", "t" },
                            { "ó", "u" },
                            { "ô", "f" },
                            { "õ", "h" },
                            { "ö", "c" },
                            { "÷", "ch" },
                            { "ø", "sh" },
                            { "ù", "sch" },
                            { "ú", "j" },
                            { "û", "i" },
                            { "ü", "j" },
                            { "ý", "e" },
                            { "þ", "yu" },
                            { "ÿ", "ya" },
                            { "À", "A" },
                            { "Á", "B" },
                            { "Â", "V" },
                            { "Ã", "G" },
                            { "Ä", "D" },
                            { "Å", "E" },
                            { "¨", "Yo" },
                            { "Æ", "Zh" },
                            { "Ç", "Z" },
                            { "È", "I" },
                            { "É", "J" },
                            { "Ê", "K" },
                            { "Ë", "L" },
                            { "Ì", "M" },
                            { "Í", "N" },
                            { "Î", "O" },
                            { "Ï", "P" },
                            { "Ð", "R" },
                            { "Ñ", "S" },
                            { "Ò", "T" },
                            { "Ó", "U" },
                            { "Ô", "F" },
                            { "Õ", "H" },
                            { "Ö", "C" },
                            { "×", "Ch" },
                            { "Ø", "Sh" },
                            { "Ù", "Sch" },
                            { "Ú", "J" },
                            { "Û", "I" },
                            { "Ü", "J" },
                            { "Ý", "E" },
                            { "Þ", "Yu" },
                            { "ß", "Ya" },
                            { " ", "_" },
                            { "-", "_" },
                            { "/", "_" },
                            { ".", "_" },
                            { ",", "_" }
                        };
        }
    }
}