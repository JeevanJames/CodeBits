#region --- License & Copyright Notice ---
/*
CodeBits Code Snippets
Copyright (c) 2012 Jeevan James
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

/* Documentation: http://codebits.codeplex.com/wikipage?title=IniFile */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace CodeBits
{
    public sealed class IniFile : Collection<IniFileSection>
    {
        public IniFile(string iniFilePath)
        {
            if (iniFilePath == null)
                throw new ArgumentNullException("iniFilePath");
            if (!File.Exists(iniFilePath))
                throw new ArgumentException(string.Format("INI file '{0}' does not exist", iniFilePath), "iniFilePath");

            using (StreamReader reader = File.OpenText(iniFilePath))
                ParseIniFile(reader);
        }

        public IniFile(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (!stream.CanRead)
                throw new ArgumentException("Cannot read from specified stream", "stream");

            using (var reader = new StreamReader(stream, true))
                ParseIniFile(reader);
        }

        public IniFile(TextReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");
            
            ParseIniFile(reader);
        }

        public static IniFile Load(string content)
        {
            using (var reader = new StringReader(content))
                return new IniFile(reader);
        }

        private void ParseIniFile(TextReader reader)
        {
            IniFileSection currentSection = null;
            for (string line = reader.ReadLine(); line != null; line = reader.ReadLine())
            {
                //Blank line
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                line = line.Trim();

                //Comment
                if (line.StartsWith(";"))
                    continue;

                //Section
                Match sectionMatch = SectionPattern.Match(line);
                if (sectionMatch.Success)
                {
                    currentSection = new IniFileSection(sectionMatch.Groups[1].Value);
                    Add(currentSection);
                    continue;
                }

                //Property
                Match propertyMatch = PropertyPattern.Match(line);
                if (propertyMatch.Success)
                {
                    string propertyName = propertyMatch.Groups[1].Value;
                    string propertyValue = propertyMatch.Groups[2].Value;

                    if (currentSection == null)
                        throw new NotSupportedException(string.Format("Property '{0}' is not part of any section", propertyName));
                    currentSection.Add(propertyName, propertyValue);

                    continue;
                }

                throw new NotSupportedException(string.Format("Unrecognized line '{0}'", line));
            }
        }

        public IEnumerable<string> GetSections()
        {
            return this.Select(section => section.Name);
        }

        public IReadOnlyDictionary<string, string> GetSection(string sectionName)
        {
            IniFileSection matchingSection = this.FirstOrDefault(section => section.Name == sectionName);
            return matchingSection != null ? new ReadOnlyDictionary<string, string>(matchingSection) : null;
        }

        private static readonly Regex SectionPattern = new Regex(@"^\[\s*(\w[\w\s]*)\s*\]$");
        private static readonly Regex PropertyPattern = new Regex(@"^(\w[\w\s]+\w)\s*=(.*)$");
    }

    public sealed class IniFileSection : Dictionary<string, string>
    {
        private readonly string _name;

        internal IniFileSection(string name)
        {
            _name = name;
        }

        public string Name
        {
            get { return _name; }
        }
    }

    public sealed class IniFileSettings
    {
        public IniFileSettings()
        {
            Encoding = Encoding.UTF8;
        }

        public Encoding Encoding { get; set; }
        public bool DetectEncoding { get; set; }

        public bool CaseSensitive { get; set; }

    }
}