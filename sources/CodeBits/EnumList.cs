#region --- License & Copyright Notice ---
/*
Copyright (c) 2005-2011 Jeevan James
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CodeBits
{
    /// <summary>
    /// Provides an enumerator for an enum type
    /// </summary>
    /// <typeparam name="T">The type of enum to enumerate over</typeparam>
    public sealed class EnumList<T> : IEnumerable<T>
        where T : struct
    {
        private IEnumerator<T> _enumerator;

        public EnumList()
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("Generic parameter must be an enum");
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            if (_enumerator == null)
                _enumerator = Enum.GetValues(typeof(T)).Cast<T>().GetEnumerator();
            return _enumerator;
        }
    }
}