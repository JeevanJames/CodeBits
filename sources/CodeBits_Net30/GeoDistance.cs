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

using System;
using System.Collections.Generic;

namespace CodeBits
{
    /// <summary>
    ///     Calculates the distance between two geographics locations
    /// </summary>
    /// <remarks>
    ///     If you are using .NET 4.0+ or Windows Phone, then this functionality is available through the
    ///     GeoCoordinate.GetDistanceTo method.
    /// </remarks>
    public static class GeoDistance
    {
        public static double Between(double latitude1, double longitude1, double latitude2, double longitude2,
            DistanceUnits units)
        {
            double a1 = latitude1 * RadianConversionFactor;
            double b1 = longitude1 * RadianConversionFactor;
            double a2 = latitude2 * RadianConversionFactor;
            double b2 = longitude2 * RadianConversionFactor;

            double distance = Math.Acos(Math.Cos(a1) * Math.Cos(b1) * Math.Cos(a2) * Math.Cos(b2) +
                    Math.Cos(a1) * Math.Sin(b1) * Math.Cos(a2) * Math.Sin(b2) + Math.Sin(a1) * Math.Sin(a2)) *
                DistanceConstants[units];

            return distance;
        }

        private const double RadianConversionFactor = Math.PI / 180;

        private static readonly Dictionary<DistanceUnits, double> DistanceConstants =
            new Dictionary<DistanceUnits, double>(3) {
                { DistanceUnits.Miles, 3963.1 },
                { DistanceUnits.NauticalMiles, 3443.9 },
                { DistanceUnits.Kilometers, 6378 }
            };
    }

    public enum DistanceUnits
    {
        Miles,
        NauticalMiles,
        Kilometers
    }
}
