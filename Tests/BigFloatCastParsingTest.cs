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
    public class BigFloatCastParsingTest
    {
        const int randomTestIteration = 99;
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
        public void CastFromIntTest()
        {
            Assert.AreEqual(new BigFloat(5, 0), new BigFloat(5));
            Assert.AreEqual(new BigFloat(5.12f, 2), new BigFloat(512));
        }
        [Test]
        public void CastToIntTest()
        {
            void TestNumber(int no)
            {
                BigFloat cast1 = no;
                Assert.AreEqual(cast1, (BigFloat)(int)cast1); //imply tolerance
            }
            Assert.AreEqual(5, (int)(BigFloat)5);
            TestNumber(int.MaxValue);
            TestNumber(int.MinValue);
            var rng = new System.Random();
            
            for (int i = 0; i < randomTestIteration; i++)
            {
                int randomNo = rng.Next();
                TestNumber(randomNo);
            }
            

            BigFloat veryLargeNumber = new BigFloat(int.MaxValue) * 62;

            void LargFunc() { int tmp = (int)veryLargeNumber; }

            Assert.That(LargFunc, Throws.TypeOf<OverflowException>());
            
            BigFloat verySmallNumber = new BigFloat(int.MinValue) * 62;

            void SmolFunc() { int tmp = (int)verySmallNumber; }

            Assert.That(SmolFunc, Throws.TypeOf<OverflowException>());
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
        public void CastToBigIntTest()
        {
            void TestNumber(BigInteger no)
            {
                BigFloat cast1 = no;
                Assert.AreEqual(cast1, (BigFloat)(BigInteger)cast1); //imply tolerance
            }
            TestNumber(5);

            var rng = new System.Random();

            for (int i = 0; i < randomTestIteration; i++)
            {
                int randomNo = rng.Next();
                TestNumber(randomNo);
            }



            BigFloat superLargeNumber = new BigFloat(1, (long)int.MaxValue + 1);

            void LargFunc() { int tmp = (int)superLargeNumber; }

            Assert.That(LargFunc, Throws.TypeOf<OverflowException>());
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
            Assert.AreEqual(new BigFloat(5, 0), new BigFloat((ulong)5));
            Assert.AreEqual(new BigFloat(512, 0), new BigFloat((ulong)512));

            Assert.AreEqual(new BigFloat(1.2f, 7), new BigFloat((ulong)12000000));
            Assert.AreEqual(new BigFloat(5, 1), new BigFloat((ulong)50));
        }
        [Test]
        public void CastFromlongTest()
        {
            Assert.AreEqual(new BigFloat(5, 0), new BigFloat((long)5));
            Assert.AreEqual(new BigFloat(512, 0), new BigFloat((long)512));

            Assert.AreEqual(new BigFloat(-1.2f, 7), new BigFloat((long)-12000000));
            Assert.AreEqual(new BigFloat(-5, 1), new BigFloat((long)-50));
        }
        [Test]
        public void CastFromUintongTest()
        {
            Assert.AreEqual(new BigFloat(5, 0), new BigFloat((uint)5));
            Assert.AreEqual(new BigFloat(512, 0), new BigFloat((uint)512));

            Assert.AreEqual(new BigFloat(1.2f, 7), new BigFloat((uint)12000000));
            Assert.AreEqual(new BigFloat(5, 1), new BigFloat((uint)50));
        }
        [Test]
        public void CastFromDoubleTest()
        {
            Assert.AreEqual(new BigFloat(5, 0), new BigFloat(5));
            Assert.AreEqual(new BigFloat(512, 0), new BigFloat(512));

            Assert.AreEqual(new BigFloat(5, -5), new BigFloat(0.00005));
            Assert.AreEqual(new BigFloat(512, -3), new BigFloat(0.512));

            Assert.AreEqual(new BigFloat(5, -10), new BigFloat  (0.0000000005));
            Assert.AreEqual(new BigFloat(512, -10), new BigFloat(0.0000000512));
        }
        [Test]
        public void CastFromDecimalTest()
        {
            Assert.AreEqual(new BigFloat(5, 0), new BigFloat(5M));
            Assert.AreEqual(new BigFloat(512, 0), new BigFloat(512M));

            Assert.AreEqual(new BigFloat(5, -5), new BigFloat(0.00005M));
            Assert.AreEqual(new BigFloat(512, -3), new BigFloat(0.512M));

            Assert.AreEqual(new BigFloat(5  , -10), new BigFloat(0.0000000005M));
            Assert.AreEqual(new BigFloat(512, -10), new BigFloat(0.0000000512M));
        }

        [Test]
        public void CastFromStringTest()
        {
            Assert.AreEqual(new BigFloat(5.12f , 2   ), new BigFloat("512"));
            Assert.AreEqual(new BigFloat(1.209f, 1928), BigFloat.Parse("1.209e+1928"));
        }
    }
}