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

using Reko.Core;
using Reko.Core.Machine;
using Reko.Core.Types;
using System;

namespace Reko.Arch.Telink
{
    public class MemoryOperand : AbstractMachineOperand
    {
        public MemoryOperand(DataType width) : base(width)
        {
        }

        public RegisterStorage Base { get; private set; } = default!;
        public int Offset { get; private set; }

        public static MemoryOperand Create(PrimitiveType dt, RegisterStorage baseRegister, int offset)
        {
            var m = new MemoryOperand(dt)
            {
                Base = baseRegister,
                Offset = offset
            };
            return m;
        }

        protected override void DoRender(MachineInstructionRenderer renderer, MachineInstructionRendererOptions options)
        {
            renderer.WriteChar('[');
            renderer.WriteString(Base.Name);
            if (Offset > 0)
            {
                renderer.WriteFormat("+{0}", Offset);
            }
            else if (Offset < 0)
            {
                renderer.WriteFormat("-{0}", -Offset);
            }
            renderer.WriteChar(']');
        }
    }
}
