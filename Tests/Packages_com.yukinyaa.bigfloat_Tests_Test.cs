using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using BigFloatNumerics;
using System.Numerics;
using System;

namespace BigFloatNumerics
{
    public class BigFloatBasicParsingTest
    {
        [Test]
        public void EqualTest()
        {
            Assert.AreEqual(new BigFloat(0, 0), new BigFloat(0, 0));
            Assert.AreEqual(new BigFloat(9, 9), new BigFloat(9, 9));
            Assert.AreEqual(new BigFloat(8, 0), new BigFloat(8, 0));
        }
        [Test]
        public void BasicConstructorArrangeTest()
        {
            Assert.AreEqual(BigFloat.Zero, new BigFloat(0, 0));
            Assert.AreEqual(BigFloat.One, new BigFloat(1, 0));
            Assert.AreEqual(new BigFloat(5.12f, 2), new BigFloat(512, 0));
            Assert.AreEqual(new BigFloat(9, 9), new BigFloat(9, 9));
            Assert.AreEqual(new BigFloat(0, 9), new BigFloat(0, 0));
        }
        [Test]
        public void IntCastTest()
        {
            Assert.AreEqual(new BigFloat(5, 0), new BigFloat(5));
            Assert.AreEqual(new BigFloat(512, 0), new BigFloat(512));
            Assert.AreEqual(5, (int)(BigFloat)5);

            BigFloat veryLargeNumber = new BigFloat(int.MaxValue) * 62;
            Assert.IsTrue(veryLargeNumber > int.MaxValue);

            void Func() { int tmp = (int)veryLargeNumber; }

            Assert.That(Func, Throws.TypeOf<OverflowException>());
        }
        [Test]
        public void CastFromBigIntTest()
        {
            Assert.AreEqual(new BigFloat(5, 0), new BigFloat((BigInteger)5));
            Assert.AreEqual(new BigFloat(512), new BigFloat((BigInteger)512));

            BigInteger randomBigNumber = BigInteger.Parse("591000000000000");
            Assert.AreEqual(new BigFloat(5.91f, 14), new BigFloat(randomBigNumber));


            BigInteger veryBigNumber = BigInteger.Parse("59000000000000000000000");
            Assert.AreEqual(new BigFloat(5.9f, 22), new BigFloat(veryBigNumber));
        }
        [Test]
        public void CastFromFloatTest()
        {
            Assert.AreEqual(new BigFloat(5, 0), new BigFloat(5f));
            Assert.AreEqual(new BigFloat(512, 0), new BigFloat(512f));

            Assert.AreEqual(new BigFloat(5, -5), new BigFloat(0.00005f));
            Assert.AreEqual(new BigFloat(512, -3), new BigFloat(0.512f));
        }

        [Test]
        public void CastFromUlongTest()
        {
            Assert.AreEqual(new BigFloat(5, 0), new BigFloat(5f));
            Assert.AreEqual(new BigFloat(512, 0), new BigFloat(512f));

            Assert.AreEqual(new BigFloat(5, -5), new BigFloat(0.00005f));
            Assert.AreEqual(new BigFloat(512, -3), new BigFloat(0.512f));
        }
    }

    public class BigFloatArithmeticTest
    {
        [Test]
        public void AddTest()
        {
            Assert.AreEqual(new BigFloat(1.2e1), new BigFloat(5) + new BigFloat(7));
            Assert.AreEqual(new BigFloat(2e10), new BigFloat(1e10) + new BigFloat(1e10));
            Assert.AreEqual(new BigFloat(1e50), new BigFloat(1e50) + new BigFloat(1e10));
        }
        [Test]
        public void NegateTest()
        {
            Assert.AreEqual(new BigFloat(-2), new BigFloat(2).Negate());
            Assert.AreEqual(new BigFloat(0), new BigFloat(0).Negate());

            Assert.AreEqual(new BigFloat(-4e70d), new BigFloat(4e70d).Negate());
        }

        [Test]
        public void SubTest()
        {
            Assert.AreEqual(new BigFloat(-2), new BigFloat(5) - new BigFloat(7));
            Assert.AreEqual(new BigFloat(0), new BigFloat(1e10) - new BigFloat(1e10));

            Assert.AreEqual(new BigFloat(1e8), new BigFloat(1e10 + 1e8) - new BigFloat(1e10));
        }

        [Test]
        public void DivideTest()
        {
            Assert.AreEqual(new BigFloat(2), new BigFloat(4) / new BigFloat(2));
            Assert.AreEqual(new BigFloat(512), new BigFloat(1024) / new BigFloat(2));

            Assert.AreEqual(new BigFloat(256), new BigFloat(2048) / new BigFloat(8));

            Assert.AreEqual(new BigFloat(5, 7), new BigFloat(1, 10) / new BigFloat(2, 2));
        }

        [Test]
        public void CompareTest()
        {
            Assert.AreEqual(true, BigFloat.One > BigFloat.Zero);
            Assert.AreEqual(false, BigFloat.One < BigFloat.Zero);
            Assert.AreEqual(false, BigFloat.One == BigFloat.Zero);

            Assert.AreEqual(true, new BigFloat(512) > new BigFloat(500) );
            Assert.AreEqual(true, BigFloat.One >= BigFloat.One);

            Assert.AreEqual(true, new BigFloat(9, 10) <= new BigFloat(9, 10));
            Assert.AreEqual(true, new BigFloat(-1, 10) < new BigFloat(1, 10));
            Assert.AreEqual(true, new BigFloat(-1, 10) <= new BigFloat(1, 10));


            Assert.AreEqual(true, new BigFloat(1, 0) > new BigFloat(1, -1));
        }
        [Test]
        public void LargeCompareTest()
        {

            Assert.AreEqual(true, new BigFloat(9, 123959) > new BigFloat(-9, 1284423));
            Assert.AreEqual(true, new BigFloat(-1, 10) < new BigFloat(1, 10));
            Assert.AreEqual(true, new BigFloat(-1, 10) <= new BigFloat(1, 10));
        }


        [Test]
        public void CompareByHash()
        {
            Assert.AreEqual(new BigFloat(0, 0).GetHashCode(), new BigFloat(0, 0).GetHashCode());
            Assert.AreEqual(new BigFloat(9, 9).GetHashCode(), new BigFloat(9, 9).GetHashCode());
            Assert.AreEqual(new BigFloat(8, 0).GetHashCode(), new BigFloat(8, 0).GetHashCode());
        }

    }
}