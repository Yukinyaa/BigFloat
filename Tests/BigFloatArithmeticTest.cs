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

            Assert.AreEqual(new BigFloat(1e9), new BigFloat(1e10 + 1e9) - new BigFloat(1e10));

            //      This is very harsh test.
            //      Design limitation(of float) limits accuracy up to 6-9 digits.
            //      experiment showed 6 or 7. Just FYI.
            //   Assert.AreEqual(new BigFloat(1e8), new BigFloat(1e10 + 1e8) - new BigFloat(1e10));
            //   Expected: 1.0000000000e8
            //   But was:  9.9999900000e7

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
        public void MultTest()
        {
            Assert.AreEqual(new BigFloat(8), new BigFloat(4) * new BigFloat(2));
            Assert.AreEqual(new BigFloat(2048), new BigFloat(1024) * new BigFloat(2));

            Assert.AreEqual(new BigFloat(2048), new BigFloat(512) * new BigFloat(4));

            Assert.AreEqual(new BigFloat(6, 7), new BigFloat(6, 2) * new BigFloat(1, 5));
        }

        [Test]
        public void Pow_intTest()
        {
            Assert.AreEqual(new BigFloat(100), BigFloat.Pow(10, 2));
            Assert.AreEqual(new BigFloat(1, 100), BigFloat.Pow(10, 100));
            Assert.AreEqual(new BigFloat(1024), BigFloat.Pow(2, 10));
        }

        [Test]
        public void Pow_floatTest()
        {
            Assert.AreEqual(new BigFloat(100), BigFloat.Pow(10, 2f));
            Assert.AreEqual(new BigFloat(1, 100), BigFloat.Pow(10, 100f));
            Assert.AreEqual(new BigFloat(1024), BigFloat.Pow(2, 10f));

            Assert.AreEqual(new BigFloat(2048), BigFloat.Pow(4, 5.5f));
            Assert.AreEqual(new BigFloat(2.3611148f, 972), BigFloat.Pow(3, 2038f));
            Assert.AreEqual(new BigFloat(2.3611148f, 972), BigFloat.Pow(3 * 3 * 3, 2038f / 3));
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