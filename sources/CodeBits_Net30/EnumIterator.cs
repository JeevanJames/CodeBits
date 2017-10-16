#region --- License & Copyright Notice ---
/*
CodeBits Code Snippets
Copyright (c) 2012-2017 Jeevan James
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

/* Documentation: https://github.com/JeevanJames/CodeBits/wiki/EnumIterator */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CodeBits
{
    /// <summary>
    /// Provides iterators for enum types. Can be used in a LINQ expression.
    /// </summary>
    public static partial class EnumIterator
    {
        /// <summary>
        /// Generates an iterator for the enum type specified by the TEnum generic parameter.
        /// </summary>
        /// <typeparam name="TEnum">The enum type to generate the iterator for</typeparam>
        /// <returns>An generic iterator that can iterate over the values of TEnum</returns>
        public static IEnumerable<TEnum> For<TEnum>()
        {
            if (!typeof(TEnum).IsEnum)
                throw new ArgumentException("Generic parameter must be an enum");
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
        }

        /// <summary>
        /// Generates an iterator for the enum type specified by the TEnum generic parameter.
        /// </summary>
        /// <param name="enumType">The enum type to generate the iterator for</param>
        /// <returns>A non-generic iterator that can iterate over the values of the enum</returns>
        public static IEnumerable For(Type enumType)
        {
            if (enumType == null)
                throw new ArgumentNullException("enumType");
            if (!enumType.IsEnum)
                throw new ArgumentException("enumType must be an enum");
            return Enum.GetValues(enumType);
        }
    }
}