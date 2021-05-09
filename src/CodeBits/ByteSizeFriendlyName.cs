#region --- License & Copyright Notice ---
/*
Useful code blocks that can included in your C# projects through NuGet
Copyright (c) 2012-2021 Jeevan James
All rights reserved.

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.
*/
#endregion

/* Documentation: https://github.com/JeevanJames/CodeBits/wiki/ByteSizeFriendlyName */

using System;
using System.Collections.Generic;

namespace CodeBits
{
    /// <summary>
    ///     Builds a friendly string representation of a specified byte size value, after converting
    ///     it to the best matching unit (bytes, KB, MB, GB, etc.).
    /// </summary>
    public static partial class ByteSizeFriendlyName
    {
        public static string Build(long bytes)
        {
            return Build(bytes, null);
        }

        public static string Build(long bytes, FriendlyNameOptions options)
        {
            if (bytes < 0)
                throw new ArgumentOutOfRangeException("bytes", "bytes cannot be a negative value");

            options = options ?? FriendlyNameOptions.Default;

            string units;
            double friendlySize = GetFriendlySize(bytes, out units);

            string sizeFormat = (options.GroupDigits ? "#,0." : "0.") + new string('#', options.DecimalPlaces);
            string size = friendlySize.ToString(sizeFormat);

            if (options.UnitDisplayMode == UnitDisplayMode.AlwaysDisplay)
                return string.Format("{0} {1}", size, units);
            if (options.UnitDisplayMode == UnitDisplayMode.AlwaysHide)
                return size;
            return bytes < 1024 ? size : string.Format("{0} {1}", size, units);
        }

        private static double GetFriendlySize(long bytes, out string units)
        {
            foreach (KeyValuePair<double, string> byteMapping in ByteMappings)
            {
                if (bytes >= byteMapping.Key)
                {
                    units = byteMapping.Value;
                    return bytes / byteMapping.Key;
                }
            }
            units = bytes == 1 ? "byte" : "bytes";
            return bytes;
        }

        private static readonly Dictionary<double, string> ByteMappings = new Dictionary<double, string> {
            { Math.Pow(1024, 5), "PB" },
            { Math.Pow(1024, 4), "TB" },
            { Math.Pow(1024, 3), "GB" },
            { Math.Pow(1024, 2), "MB" },
            { Math.Pow(1024, 1), "KB" },
        };
    }

    /// <summary>
    ///     Options for generating the friendly name of a byte size.
    /// </summary>
    public sealed class FriendlyNameOptions
    {
        /// <summary>
        ///     Creates an instance of the FriendlyNameOptions class.
        /// </summary>
        public FriendlyNameOptions()
        {
            DecimalPlaces = 2;
            UnitDisplayMode = UnitDisplayMode.AlwaysDisplay;
        }

        /// <summary>
        ///     The number of decimal places to calculate for the friendly name size value.
        /// </summary>
        public int DecimalPlaces { get; set; }

        /// <summary>
        ///     Specifies whether to group digits in the friendly name size value.
        /// </summary>
        public bool GroupDigits { get; set; }

        /// <summary>
        ///     Specifies how the size unit is displayed in the friendly name.
        /// </summary>
        public UnitDisplayMode UnitDisplayMode { get; set; }

        /// <summary>
        ///     Represents the default options value for generating a friendly name.
        /// </summary>
        public static readonly FriendlyNameOptions Default = new FriendlyNameOptions();
    }

    /// <summary>
    ///     Specifies how the size unit (KB, MB, etc) is displayed in the friendly name.
    /// </summary>
    public enum UnitDisplayMode
    {
        /// <summary>
        ///     Always display the size unit (the default).
        /// </summary>
        AlwaysDisplay,

        /// <summary>
        ///     Always hide the size unit.
        /// </summary>
        AlwaysHide,

        /// <summary>
        ///     Only display the size unit if the value is 1 KB or more. Never display for sizes less
        ///     than that.
        /// </summary>
        HideOnlyForBytes
    }
}
