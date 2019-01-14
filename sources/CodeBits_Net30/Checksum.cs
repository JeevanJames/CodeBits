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

/* Documentation: https://github.com/JeevanJames/CodeBits/wiki/Checksum */

using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace CodeBits
{
    public static class Checksum
    {
        private static IChecksumImplementation _md5;
        private static IChecksumImplementation _sha1;
        private static IChecksumImplementation _sha256;
        private static IChecksumImplementation _sha384;
        private static IChecksumImplementation _sha512;

        /// <summary>
        ///     Calculates checksums using the MD5 algorithm
        /// </summary>
        public static IChecksumImplementation Md5
        {
            get { return _md5 ?? (_md5 = new ChecksumImplementation<MD5>(MD5.Create)); }
        }

        /// <summary>
        ///     Calculates checksums using the SHA1 algorithm
        /// </summary>
        public static IChecksumImplementation Sha1
        {
            get { return _sha1 ?? (_sha1 = new ChecksumImplementation<SHA1>(SHA1.Create)); }
        }

        /// <summary>
        ///     Calculates checksums using the SHA256 algorithm
        /// </summary>
        public static IChecksumImplementation Sha256
        {
            get { return _sha256 ?? (_sha256 = new ChecksumImplementation<SHA256>(SHA256.Create)); }
        }

        /// <summary>
        ///     Calculates checksums using the SHA384 algorithm
        /// </summary>
        public static IChecksumImplementation Sha384
        {
            get { return _sha384 ?? (_sha384 = new ChecksumImplementation<SHA384>(SHA384.Create)); }
        }

        /// <summary>
        ///     Calculates checksums using the SHA512 algorithm
        /// </summary>
        public static IChecksumImplementation Sha512
        {
            get { return _sha512 ?? (_sha512 = new ChecksumImplementation<SHA512>(SHA512.Create)); }
        }
    }

    public interface IChecksumImplementation
    {
        /// <summary>
        ///     Calculates the checksum of a byte array
        /// </summary>
        /// <param name="bytes">The byte array</param>
        /// <returns>Checksum of the byte array</returns>
        /// <exception cref="ArgumentNullException">The byte array is null</exception>
        string FromBytes(byte[] bytes);

        /// <summary>
        ///     Calculates the checksum of a file
        /// </summary>
        /// <param name="fileName">Path to the file</param>
        /// <returns>Checksum of the file</returns>
        /// <exception cref="ArgumentNullException">The file name is null</exception>
        /// <exception cref="FileNotFoundException">The file does not exist</exception>
        string FromFile(string fileName);

        /// <summary>
        ///     Calculates the checksum of a file
        /// </summary>
        /// <param name="fileName">Path to the file</param>
        /// <param name="bufferSize">Byte size of the buffer used to read the file</param>
        /// <returns>Checksum of the file</returns>
        /// <exception cref="ArgumentNullException">The file name is null</exception>
        /// <exception cref="FileNotFoundException">The file does not exist</exception>
        string FromFile(string fileName, int bufferSize);

        /// <summary>
        ///     Calculates the checksum of a stream
        /// </summary>
        /// <param name="stream">The stream</param>
        /// <returns>Checksum of the stream</returns>
        /// <exception cref="ArgumentNullException">The stream is null</exception>
        string FromStream(Stream stream);

        /// <summary>
        ///     Calculates the checksum of a string
        /// </summary>
        /// <param name="str">The string</param>
        /// <returns>Checksum of the string</returns>
        /// <exception cref="ArgumentNullException">The string is null</exception>
        string FromString(string str);
    }

    internal sealed class ChecksumImplementation<TAlgo> : IChecksumImplementation
        where TAlgo : HashAlgorithm
    {
        private readonly TAlgo _algorithm;

        internal ChecksumImplementation(Func<TAlgo> algoFactory)
        {
            _algorithm = algoFactory();
        }

        string IChecksumImplementation.FromBytes(byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException("bytes");

            byte[] hashBytes = _algorithm.ComputeHash(bytes);
            string checksum = HashBytesToHexString(hashBytes);
            return checksum;
        }

        string IChecksumImplementation.FromFile(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");
            if (!File.Exists(fileName))
                throw new FileNotFoundException("File does not exist", fileName);

            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                return ((IChecksumImplementation)this).FromStream(fs);
        }

        string IChecksumImplementation.FromFile(string fileName, int bufferSize)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");
            if (!File.Exists(fileName))
                throw new FileNotFoundException("File does not exist", fileName);
            if (bufferSize < 4096)
                bufferSize = 4096;

            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var bufferedStream = new BufferedStream(fs, bufferSize))
                return ((IChecksumImplementation)this).FromStream(bufferedStream);
        }

        string IChecksumImplementation.FromStream(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            byte[] hashBytes = _algorithm.ComputeHash(stream);
            string checksum = HashBytesToHexString(hashBytes);
            return checksum;
        }

        string IChecksumImplementation.FromString(string str)
        {
            if (str == null)
                throw new ArgumentNullException("str");

            byte[] bytes = Encoding.ASCII.GetBytes(str);
            return ((IChecksumImplementation)this).FromBytes(bytes);
        }

        private static string HashBytesToHexString(byte[] hashBytes)
        {
            return hashBytes.Aggregate(new StringBuilder(hashBytes.Length * 2),
                (builder, b) => builder.Append(b.ToString("x2"))).ToString();
        }
    }
}
