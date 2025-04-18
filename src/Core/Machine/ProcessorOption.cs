#region License
/* 
 * Copyright (C) 1999-2025 John Källén.
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2, or (at your option)
 * any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; see the file COPYING.  If not, write to
 * the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA.
 */
#endregion

using System;
using System.Collections.Generic;
using System.Text;

namespace Reko.Core.Machine
{
    /// <summary>
    /// Commonly used processor options.
    /// </summary>
    public static class ProcessorOption
    {
        public const string Model = "Model";
        public const string Endianness = "Endianness";
        public const string WordSize = "WordSize";
        public const string InstructionSet = "ISA";

        /// <summary>
        /// Parse a numeric option, JavaScript-style. That is, accept strings
        /// but convert them to ints.
        ///  //$TODO: support for hexadecimal string representations?
        /// </summary>
        public static bool TryParseNumericOption(object? oValue, out int value)
        {
            value = 0;
            if (oValue is null)
                return false;
            switch (oValue)
            {
            case string s:
                return int.TryParse(s, out value);
            case int i:
                value = i;
                return true;
            case long l:
                value = (int) l;
                return true;
            default:
                return false;
            }
        }
    }
}
