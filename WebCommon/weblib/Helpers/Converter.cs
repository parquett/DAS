// ------------------------------public --------------------------------------------------------------------------------------
// <copyright file="Converter.cs" company="SecurityCRM">
//   Copyright 2020
// </copyright>
// <summary>
//   Defines the Converter type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Weblib.Helpers
{
    using Lib.AdvancedProperties;
    using Lib.Tools.Controls;

    using Weblib.Models.Common.Enums;

    /// <summary>
    /// The converter.
    /// </summary>
    public class Converter
    {
        /// <summary>
        /// The get textbox type.
        /// </summary>
        /// <param name="type">
        /// The type.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string GetTextboxType(TextboxType type)
        {
            switch (type)
            {
                 case TextboxType.Text:
                    return "text";

                 case TextboxType.Number:
                 case TextboxType.Integer:
                    return "number";

                case TextboxType.Password:
                    return "password";

                case TextboxType.Email:
                    return "email";
            }

            return "text";
        }

    } 
}
