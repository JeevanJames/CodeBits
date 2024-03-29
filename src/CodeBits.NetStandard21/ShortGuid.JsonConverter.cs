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

#nullable enable

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CodeBits
{
    [JsonConverter(typeof(ShortGuidJsonConverter))]
    public readonly partial struct ShortGuid
    {
        private sealed class ShortGuidJsonConverter : JsonConverter<ShortGuid>
        {
            public override ShortGuid Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                string? str = reader.GetString();
                return str is not null ? Parse(str) : default;
            }

            public override void Write(Utf8JsonWriter writer, ShortGuid value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToString());
            }
        }
    }
}
