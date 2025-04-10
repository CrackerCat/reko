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

using NUnit.Framework;
using Reko.Core;
using Reko.Core.Expressions;
using Reko.Core.Types;
using Reko.Typing;
using Reko.UnitTests.Mocks;
using System.ComponentModel.Design;

namespace Reko.UnitTests.Decompiler.Typing
{
    [TestFixture]
	public class ComplexExpressionBuilderTests
	{
        private Program program;
		private TypeStore store;
		private TypeFactory factory;
		private Pointer ptrPoint;
		private Pointer ptrUnion;
        private Pointer ptrInt;
        private Pointer ptrWord;
        private Pointer ptrDouble;
        private Identifier globals;
        private ExpressionEmitter m;

        [SetUp]
		public void Setup()
		{
            program = new Program();
            program.Architecture = new FakeArchitecture(new ServiceContainer());
            program.Platform = new DefaultPlatform(program.Architecture.Services, program.Architecture);
            store = program.TypeStore;
			factory = program.TypeFactory;
            globals = program.Globals;
            StructureType point = new StructureType("Point", 0)
            {
                Fields = {
                    { 0, PrimitiveType.Word32, null },
			        { 4, PrimitiveType.Word32, null }
                }
            };
			TypeVariable tvPoint = store.CreateTypeVariable(factory);
            EquivalenceClass eq = new EquivalenceClass(tvPoint)
            {
                DataType = point
            };
			tvPoint.DataType = eq;
			ptrPoint = new Pointer(eq, 32);

            UnionType u = new UnionType("RealInt", null)
            {
                Alternatives = {
                    new UnionAlternative("w", PrimitiveType.Word32, 0),
			        new UnionAlternative("r", PrimitiveType.Real32, 1),
                }
            };
			TypeVariable tvUnion = store.CreateTypeVariable(factory);
            eq = new EquivalenceClass(tvUnion)
            {
                DataType = u
            };
			tvUnion.DataType = eq;
			ptrUnion = new Pointer(eq, 32);

            ptrInt = new Pointer(PrimitiveType.Int32, 32);
            ptrWord = new Pointer(PrimitiveType.Word32, 32);
            ptrDouble = new Pointer(PrimitiveType.Real64, 32);
            m = new ExpressionEmitter();
		}

        private Pointer Ptr32(DataType dataType)
        {
            return new Pointer(dataType, 32);
        }

        private Pointer Ptr16(DataType dataType)
        {
            return new Pointer(dataType, 16);
        }

        private MemberPointer MemPtr(DataType baseType, DataType fieldType)
        {
            return new MemberPointer(
                new Pointer(baseType, 16),
                fieldType, 2);
        }

        private StructureType Struct(params StructureField[] fields)
        {
            StructureType str = new StructureType();
            foreach (StructureField f in fields)
            {
                str.Fields.Add(f);
            }
            return str;
        }

        private StructureType Segment(params StructureField[] fields)
        {
            StructureType str = new StructureType();
            str.IsSegment = true;
            foreach (StructureField f in fields)
            {
                str.Fields.Add(f);
            }
            return str;
        }

        private StructureField Fld(int offset, DataType dt)
        {
            return new StructureField(offset, dt);
        }

        private ComplexExpressionBuilder CreateBuilder(
            Expression basePtr,
            Expression complex,
            Expression index = null,
            int offset = 0)
        {
            return new ComplexExpressionBuilder(program, store, basePtr, complex, index, offset);
        }

        private TypeVariable CreateTv(Expression e, DataType dt, DataType dtOrig)
        {
            TypeVariable tv = store.EnsureExpressionTypeVariable(factory, null, e);
            tv.DataType = dt;
            tv.OriginalDataType = dtOrig;
            if (dt.IsComplex)
            {
                tv.Class = new EquivalenceClass(tv);
                tv.Class.DataType = dt;
            }
            return tv;
        }

		[Test]
        public void CEB_BuildPrimitive()
		{
			var id = new Identifier("id", PrimitiveType.Word32, null);
            var ceb = new ComplexExpressionBuilder(program, store, null, id, null, 0);
			Assert.AreEqual("id", ceb.BuildComplex(null).ToString());
		}

		[Test]
        public void CEB_BuildPointer()
		{
			var ptr = new Identifier("ptr", PrimitiveType.Word32, null);
            CreateTv(ptr, ptrPoint, Ptr32(PrimitiveType.Word32));
			var ceb = new ComplexExpressionBuilder(program, store,null, ptr, null, 0);
			Assert.AreEqual("&ptr->dw0000", ceb.BuildComplex(null).ToString());
		}

		[Test]
        public void CEB_BuildPointerFetch()
		{
			var ptr = new Identifier("ptr", PrimitiveType.Word32, null);
            CreateTv(ptr, ptrPoint, Ptr32(PrimitiveType.Word32));
            var ceb = new ComplexExpressionBuilder(program, store,null, ptr, null, 0);
            var e = ceb.BuildComplex(PrimitiveType.Word32);
			Assert.AreEqual("ptr->dw0000", e.ToString());
		}

        [Test]
        public void CEB_BuildAddrOfPointer()
        {
            var id = new Identifier("id", PrimitiveType.Word32, null);
            var addrOf = m.AddrOf(PrimitiveType.Ptr32, id);
            CreateTv(addrOf, Ptr32(Ptr32(PrimitiveType.Int32)), Ptr32(PrimitiveType.Word32));
            var ceb = CreateBuilder(null, addrOf);
            Assert.AreEqual("&id", ceb.BuildComplex(null).ToString());
        }

        [Test]
        public void CEB_BuildAddrOfPointerFetch()
        {
            var id = new Identifier("id", PrimitiveType.Word32, null);
            var addrOf = m.AddrOf(PrimitiveType.Ptr32, id);
            CreateTv(addrOf, Ptr32(Ptr32(PrimitiveType.Int32)), Ptr32(PrimitiveType.Word32));
            var ceb = CreateBuilder(null, addrOf);
            var e = ceb.BuildComplex(PrimitiveType.Word32);
            Assert.AreEqual("id", e.ToString());
        }

        [Test]
        public void CEB_BuildPointerToVoid()
        {
            var id = new Identifier("id", PrimitiveType.Word32, null);
            CreateTv(id, Ptr32(VoidType.Instance), PrimitiveType.Word32);
            var ceb = CreateBuilder(null, id, null, 4);
            var e = ceb.BuildComplex(null);
            Assert.AreEqual("(char *) id + 4<i32>", e.ToString());
        }

        [Test]
        public void CEB_BuildPointerToUnknown()
        {
            var id = new Identifier("id", PrimitiveType.Word32, null);
            var index = new Identifier("index", PrimitiveType.Word32, null);
            CreateTv(id, Ptr32(new UnknownType()), PrimitiveType.Word32);
            var ceb = CreateBuilder(null, id, index, 0);
            var e = ceb.BuildComplex(null);
            Assert.AreEqual("(char *) id + index", e.ToString());
        }

        [Test]
        public void CEB_BuildPointerToCode()
        {
            var id = new Identifier("id", PrimitiveType.Word32, null);
            var indexId = new Identifier("index", PrimitiveType.Word32, null);
            var index = m.IMul(indexId, 16);
            CreateTv(id, Ptr32(new CodeType()), PrimitiveType.Word32);
            var ceb = CreateBuilder(null, id, index, -4);
            var e = ceb.BuildComplex(null);
            Assert.AreEqual(
                "(char *) id + (index * 0x10<32> - 4<i32>)",
                e.ToString());
        }

        [Test]
        public void CEB_BuildPointerToPointerToInteger()
        {
            var id = new Identifier("id", PrimitiveType.Word32, null);
            var index = new Identifier("index", PrimitiveType.Word32, null);
            CreateTv(id, Ptr32(Ptr32(PrimitiveType.Int32)), PrimitiveType.Word32);
            var ceb = CreateBuilder(null, id, index, -8);
            var e = ceb.BuildComplex(null);
            Assert.AreEqual("(char *) id + (index - 8<i32>)", e.ToString());
        }

        [Test]
        public void CEB_BuildPointerToStruct_MiddleOfTheField()
        {
            var id = new Identifier("id", PrimitiveType.Word32, null);
            var str = Struct(Fld(4, PrimitiveType.Int32));
            CreateTv(id, Ptr32(str), PrimitiveType.Word32);
            var ceb = CreateBuilder(null, id, null, 6);
            var e = ceb.BuildComplex(null);
            Assert.AreEqual("(char *) &id->dw0004 + 2<i32>", e.ToString());
        }

        [Test]
        public void CEB_BuildPointerToStruct_EndOfTheField()
        {
            var id = new Identifier("id", PrimitiveType.Word32, null);
            var str = Struct(Fld(4, PrimitiveType.Int32));
            CreateTv(id, Ptr32(str), PrimitiveType.Word32);
            var ceb = CreateBuilder(null, id, null, 8);
            var e = ceb.BuildComplex(null);
            Assert.AreEqual("&id->dw0004 + 1<i32>", e.ToString());
        }

        [Test]
        public void CEB_BuildPointerToNestedStruct()
        {
            var id = new Identifier("id", PrimitiveType.Word32, null);
            var nestedStr = Struct(
                Fld(0, PrimitiveType.Int32),
                Fld(4, PrimitiveType.Real32));
            var str = Struct(Fld(4, nestedStr));
            CreateTv(id, Ptr32(str), PrimitiveType.Word32);
            var ceb = CreateBuilder(null, id, null, 8);
            var e = ceb.BuildComplex(PrimitiveType.Real32);
            Assert.AreEqual("id->t0004.r0004", e.ToString());
        }

        [Test]
        public void CEB_BuildPointerToStruct_StructArrayField()
        {
            var id = new Identifier("id", PrimitiveType.Word32, null);
            var innerStr = Struct(Fld(0, PrimitiveType.Byte));
            innerStr.Size = 32;
            var arrayOfStructs = new ArrayType(innerStr, 0);
            var str = Struct(Fld(4, arrayOfStructs));
            CreateTv(id, Ptr32(str), PrimitiveType.Word32);
            var ceb = CreateBuilder(null, id, null, 4);
            var e = ceb.BuildComplex(null);
            Assert.AreEqual("id->a0004", e.ToString());
        }

        [Test]
        public void CEB_BuildPointerToStruct_StructArrayFieldFetch()
        {
            var id = new Identifier("id", PrimitiveType.Word32, null);
            var innerStr = Struct(Fld(0, PrimitiveType.Byte));
            innerStr.Size = 32;
            var arrayOfStructs = new ArrayType(innerStr, 0);
            var str = Struct(Fld(4, arrayOfStructs));
            CreateTv(id, Ptr32(str), PrimitiveType.Word32);
            var ceb = CreateBuilder(null, id, null, 4);
            var e = ceb.BuildComplex(PrimitiveType.Byte);
            Assert.AreEqual("id->a0004[0<i32>].b0000", e.ToString());
        }

        [Test]
        public void CEB_BuildPointerToInteger()
        {
            var id = new Identifier("id", PrimitiveType.Word32, null);
            CreateTv(id, Ptr32(PrimitiveType.Int32), PrimitiveType.Word32);
            var ceb = CreateBuilder(null, id, null, 6);
            var e = ceb.BuildComplex(null);
            Assert.AreEqual("(char *) id + 6<i32>", e.ToString());
        }

        [Test]
        public void CEB_BuildUnionWithOffset()
        {
            var id = new Identifier("id", PrimitiveType.Word32, null);
            CreateTv(id, ptrUnion.Pointee, PrimitiveType.Word32);
            var ceb = CreateBuilder(null, id, null, 2);
            var e = ceb.BuildComplex(null);
            Assert.AreEqual("(word32) id.w + 2<i32>", e.ToString());
        }

        [Test]
        public void CEB_BuildUnionWithNegativeOffset()
        {
            var id = new Identifier("id", PrimitiveType.Word32, null);
            CreateTv(id, ptrUnion.Pointee, PrimitiveType.Word32);
            var ceb = CreateBuilder(null, id, null, -2);
            var e = ceb.BuildComplex(null);
            Assert.AreEqual("(word32) id.w - 2<i32>", e.ToString());
        }

        [Test]
        public void CEB_BuildPointerToStruct_NoFieldAtGivenOffset()
        {
            var id = new Identifier("id", PrimitiveType.Word32, null);
            var str = Struct(Fld(8, PrimitiveType.Int32));
            CreateTv(id, Ptr32(str), PrimitiveType.Word32);
            var ceb = CreateBuilder(null, id, null, 4);
            var e = ceb.BuildComplex(null);
            Assert.AreEqual("(char *) id + 4<i32>", e.ToString());
        }

        [Test]
        public void CEB_BuildPointerToUnion_NotFoundAlternative()
        {
            var id = new Identifier("id", PrimitiveType.Word32, null);
            CreateTv(id, ptrUnion, PrimitiveType.Real64);
            var ceb = CreateBuilder(null, id, null, 2);
            var e = ceb.BuildComplex(null);
            Assert.AreEqual("(char *) id + 2<i32>", e.ToString());
        }

        [Test]
        public void CEB_BuildUnionFetch()
        {
            var ptr = new Identifier("ptr", PrimitiveType.Word32, null);
            CreateTv(ptr, ptrUnion, Ptr32(PrimitiveType.Real32));
            var ceb = new ComplexExpressionBuilder(program, store,null, ptr, null, 0);
            var e = ceb.BuildComplex(PrimitiveType.Real32);
            Assert.AreEqual("ptr->r", e.ToString());
        }

        [Test]
		public void CEB_BuildByteArrayFetch()
		{
            var i = new Identifier("i", PrimitiveType.Word32, null);
            DataType arrayOfBytes = new ArrayType(PrimitiveType.Byte, 0);
            StructureType str = Struct(
                Fld(0x800, arrayOfBytes));
            str.Size = 0x2000;
            CreateTv(globals, Ptr32(str), Ptr32(PrimitiveType.Byte));
            CreateTv(i, PrimitiveType.Int32, PrimitiveType.Word32);
            var ceb = CreateBuilder(null, globals, i, 0x800);
            var e = ceb.BuildComplex(PrimitiveType.Byte);
            Assert.AreEqual("g_a0800[i]", e.ToString());
		}

        [Test]
        public void CEB_BuildByteArrayFetch_Nondereferenced()
        {
            var i = new Identifier("i", PrimitiveType.Word32, null);
            DataType arrayOfBytes = new ArrayType(PrimitiveType.Byte, 0);
            StructureType str = Struct(
                Fld(0x01000, arrayOfBytes));
            str.Size = 0x2000;
            CreateTv(globals, Ptr32(str), Ptr32(PrimitiveType.Byte));
            CreateTv(i, PrimitiveType.Int32, PrimitiveType.Word32);
            var ceb = CreateBuilder(null, globals, i, 0x1000);
            Assert.AreEqual("g_a1000 + i", ceb.BuildComplex(null).ToString());
        }

        [Test]
        public void CEB_SegmentedArray()
        {
            var m = new Reko.UnitTests.Mocks.ProcedureBuilder("CEB_SegmentedArray");
            var aw = new ArrayType(PrimitiveType.Int16, 0);
            var ds = m.Temp(PrimitiveType.SegmentSelector, "ds");
            var bx = m.Temp(PrimitiveType.Word16, "bx");
            var acc =
                m.Array(
                    PrimitiveType.Word16,
                    m.Seq(
                        ds,
                        m.Word16(0x5388)),
                    m.IMul(bx, 2));

            var seg = Segment();
            CreateTv(globals, Ptr32(factory.CreateStructureType()), Ptr32(factory.CreateStructureType()));
            CreateTv(ds, Ptr16(seg), Ptr16(factory.CreateStructureType()));
            var ceb = CreateBuilder(ds, bx);
        }

        [Test]
        public void CEB_BuildMemberAccessFetch()
        {
            var ds = new Identifier("ds", PrimitiveType.SegmentSelector, null);
            var bx = new Identifier("bx", PrimitiveType.Word16, null);
            CreateTv(ds, Ptr16(Segment()), ds.DataType);
            CreateTv(bx, MemPtr(Segment(), PrimitiveType.Word16), MemPtr(new TypeVariable(43), PrimitiveType.Word16));
            var ceb = CreateBuilder(ds, bx);
            var e = ceb.BuildComplex(PrimitiveType.Word16);
            Assert.AreEqual("ds->*bx", e.ToString());
        }

        [Test]
        public void CEB_MemberOffset()
        {
            var dtPseg = Ptr16(Segment());
            var ds = new Identifier("ds", dtPseg, null);
            var bx = new Identifier("bx", PrimitiveType.Word16, null);
            CreateTv(ds, dtPseg, ds.DataType);
            CreateTv(bx, MemPtr(dtPseg, PrimitiveType.Real32), MemPtr(new TypeVariable(43), PrimitiveType.Real32));
            var ceb = CreateBuilder(ds, bx);
            Assert.AreEqual("&(ds->*bx)", ceb.BuildComplex(null).ToString());
        }

        [Test]
        public void CEB_ArrayOfStructs()
        {
            var array = new ArrayType(new StructureType(8)
            {
                Fields =
                {
                    new StructureField(0, PrimitiveType.Word32),
                    new StructureField(4, PrimitiveType.Real32),
                }
            }, 0);
            var a = new Identifier("a", Ptr32(array), null);
            var i = new Identifier("i", PrimitiveType.Int32, null);
            CreateTv(a, array, array);
            var ceb = CreateBuilder(null, a, m.SMul(i, 8));
            var e = ceb.BuildComplex(PrimitiveType.Word32);
            Assert.AreEqual("a[i].dw0000", e.ToString());
        }

        [Test]
        public void CEB_ArrayOfStructs_Shl()
        {
            var array = new ArrayType(new StructureType(8)
            {
                Fields =
                {
                    new StructureField(0, PrimitiveType.Word32),
                    new StructureField(4, PrimitiveType.Real32),
                }
            }, 0);
            var a = new Identifier("a", Ptr32(array), null);
            var i = new Identifier("i", PrimitiveType.Int32, null);
            CreateTv(a, array, array);
            var ceb = CreateBuilder(null, a, m.Shl(i, 3));
            var e = ceb.BuildComplex(PrimitiveType.Word32);
            Assert.AreEqual("a[i].dw0000", e.ToString());
        }

        [Test]
        public void CEB_ArrayOfPtrsToStruct()
        {
            var array = new ArrayType(ptrPoint, 0);
            var a = new Identifier("a", Ptr32(array), null);
            CreateTv(a, Ptr32(array), Ptr32(array));
            var ceb = CreateBuilder(null, a, null, 8);
            var e = ceb.BuildComplex(PrimitiveType.Ptr32);
            Assert.AreEqual("a[2<i32>]", e.ToString());
        }

        [Test]
        public void CEB_ChooseUnionAlternative_Real64()
        {
            var u = new UnionType("PointR64", null)
            {
                Alternatives = {
                    new UnionAlternative("point", ptrPoint, 0),
                    new UnionAlternative("r64", ptrDouble, 1),
                }
            };
            var a = new Identifier("a", PrimitiveType.Ptr32, null);
            CreateTv(a, u, PrimitiveType.Ptr32);
            var ceb = CreateBuilder(null, a, null, 0);
            var e = ceb.BuildComplex(PrimitiveType.Real64);
            Assert.AreEqual("*a.r64", e.ToString());
        }

        [Test]
        public void CEB_ChooseUnionAlternative_dw0000()
        {
            var u = new UnionType("PointR64", null)
            {
                Alternatives = {
                    new UnionAlternative("point", ptrPoint, 0),
                    new UnionAlternative("r64", ptrDouble, 1),
                }
            };
            var a = new Identifier("a", PrimitiveType.Ptr32, null);
            CreateTv(a, u, PrimitiveType.Ptr32);
            var ceb = CreateBuilder(null, a, null, 0);
            var e = ceb.BuildComplex(PrimitiveType.Int32);
            Assert.AreEqual("a.point->dw0000", e.ToString());
        }

        [Test]
        public void CEB_ChooseUnionAlternative_dw0004()
        {
            var u = new UnionType("PointR64", null)
            {
                Alternatives = {
                    new UnionAlternative("point", ptrPoint, 0),
                    new UnionAlternative("r64", ptrDouble, 1),
                }
            };
            var a = new Identifier("a", PrimitiveType.Ptr32, null);
            CreateTv(a, u, PrimitiveType.Ptr32);
            var ceb = CreateBuilder(null, a, null, 4);
            var e = ceb.BuildComplex(PrimitiveType.Int32);
            Assert.AreEqual("a.point->dw0004", e.ToString());
        }

        [Test]
        public void CEB_PointerToStruct_ArrayField()
        {
            StructureType str = Struct(
                Fld(0, new ArrayType(PrimitiveType.Real64, 4)));
            var id = new Identifier("id", PrimitiveType.Ptr32, null);
            var index = new Identifier("index", PrimitiveType.Ptr32, null);
            CreateTv(id, Ptr32(str), PrimitiveType.Ptr32);
            var ceb = CreateBuilder(null, id, m.IMul(index, 8));
            var e = ceb.BuildComplex(PrimitiveType.Real64);
            Assert.AreEqual("id->a0000[index]", e.ToString());
        }
    }
}
