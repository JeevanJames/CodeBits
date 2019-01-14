#region --- License & Copyright Notice ---
/*
Useful code blocks that can included in your C# projects through NuGet
Copyright (c) 2012-2019 Jeevan James
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
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CodeBits
{
    public static class UniqueItemGenerator
    {
        public static string GetNextUniqueItem(IEnumerable<string> items, string itemName)
        {
            return GetNextUniqueItem(items, itemName, "{0} - {1}", StringComparison.OrdinalIgnoreCase);
        }

        public static string GetNextUniqueItem(IEnumerable<string> items, string itemName,
            string duplicateItemFormat, StringComparison comparison)
        {
            if (items == null)
                throw new ArgumentNullException("items");
            if (itemName == null)
                throw new ArgumentNullException("itemName");
            if (duplicateItemFormat == null)
                throw new ArgumentNullException("duplicateItemFormat");

            //If the items collection is empty, return itemName
            if (!items.Any())
                return itemName;

            IEnumerable<string> eligibleItems = items.Where(item => item.StartsWith(itemName, comparison));
            if (!eligibleItems.Any(item => item.Equals(itemName, comparison)))
                return itemName;

            for (int i = 1; i < int.MaxValue; i++)
            {
                string newItemName = string.Format(duplicateItemFormat, itemName, i);
                if (!eligibleItems.Any(item => item.Equals(newItemName, comparison)))
                    return newItemName;
            }
            return null;
        }

        public static string GetUniqueFileName(string directory, string fileName)
        {
            return GetUniqueFileName(directory, fileName, "{0} - {1}");
        }

        public static string GetUniqueFileName(string directory, string fileName, string fileNameFormat)
        {
            if (directory == null)
                throw new ArgumentNullException("directory");
            if (fileNameFormat == null)
                throw new ArgumentNullException("fileNameFormat");
            if (!Directory.Exists(directory))
                throw new DirectoryNotFoundException(string.Format("The directory '{0}' does not exist", directory));

            IEnumerable<string> files =
                Directory.EnumerateFiles(directory, "*" + fileName + "*", SearchOption.TopDirectoryOnly);
            return GetNextUniqueItem(files, fileName, fileNameFormat, StringComparison.OrdinalIgnoreCase);
        }

        public static string GetUniqueFileName(DirectoryInfo directory, string fileName)
        {
            return GetUniqueFileName(directory.FullName, fileName, "{0} - {1}");
        }

        public static string GetUniqueFileName(DirectoryInfo directory, string fileName, string fileNameFormat)
        {
            return GetUniqueFileName(directory.FullName, fileName, fileNameFormat);
        }
    }
}
