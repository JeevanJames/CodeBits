﻿#region --- License & Copyright Notice ---
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

<auto-generated>
    This code is downloaded from a CodeBits NuGet package.
</auto-generated>
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
    ///     Provides iterators for enum types. Can be used in a LINQ expression.
    /// </summary>
    public static class EnumIterator
    {
        /// <summary>
        ///     Generates an iterator for the enum type specified by the TEnum generic parameter.
        /// </summary>
        /// <typeparam name="TEnum">The enum type to generate the iterator for</typeparam>
        /// <returns>An generic iterator that can iterate over the values of TEnum</returns>
        /// <exception cref="ArgumentException">Thrown when the generic parameter is not an enum</exception>
        public static IEnumerable<TEnum> For<TEnum>()
        {
            if (!typeof(TEnum).IsEnum)
                throw new ArgumentException("Generic parameter must be an enum");
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
        }

        /// <summary>
        ///     Generates an iterator for the enum type specified by the TEnum generic parameter.
        /// </summary>
        /// <param name="enumType">The enum type to generate the iterator for</param>
        /// <returns>A non-generic iterator that can iterate over the values of the enum</returns>
        /// <exception cref="ArgumentNullException">Thrown when the specified type is null</exception>
        /// <exception cref="ArgumentException">Thrown when the specified type is not an enum</exception>
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
