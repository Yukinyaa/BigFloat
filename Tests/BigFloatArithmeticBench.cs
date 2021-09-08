using System;
using System.Diagnostics;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using BigFloatNumerics;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class BigFloatArithmeticBench
    {
        const int randomTestIteration = 500000;
        [Test]
        public void AddBenchComparedToInt()
        {
            void AddTestInt(int no)
            {
                BigFloat cast1 = no;
                Assert.AreEqual(cast1, (BigFloat)(int)cast1); //imply tolerance
            }
            Assert.AreEqual(5, (int)(BigFloat)5);
            var rng = new System.Random();

            int[] i_a = new int[randomTestIteration];
            int[] i_b = new int[randomTestIteration];
            BigFloat[] bf_a = new BigFloat[randomTestIteration];
            BigFloat[] bf_b = new BigFloat[randomTestIteration];

            for (int i = 0; i < randomTestIteration; i++)
            {
                int randomNo1 = rng.Next();
                int randomNo2 = rng.Next();
                i_a[i] = randomNo1;
                i_a[i] = randomNo2;
                bf_a[i] = (BigFloat)randomNo1;
                bf_b[i] = (BigFloat)randomNo2;
            }

            Stopwatch stopwatch = new Stopwatch();
            
            stopwatch.Start();
            int res = 0;
            for (int i = 0; i < randomTestIteration; i++)
            {
                res = i_a[i] + i_b[i];
            }
            stopwatch.Stop();

            TimeSpan intElaped = stopwatch.Elapsed;

            stopwatch.Restart();
            BigFloat resbf;
            for (int i = 0; i < randomTestIteration; i++)
            {
                resbf = bf_a[i] + bf_b[i];
            }
            stopwatch.Stop();

            TimeSpan bfElaped = stopwatch.Elapsed;
            UnityEngine.Debug.Log($"Int test time: {intElaped}");
            UnityEngine.Debug.Log($"BigFloat test time: {stopwatch.Elapsed}");

            UnityEngine.Debug.Log($"Delta: {stopwatch.Elapsed - intElaped}({ (double)stopwatch.Elapsed.Milliseconds / intElaped.Milliseconds} )");
        }


        [Test]
        public void AddBenchComparedToFloat()
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

            float[] i_a = new float[randomTestIteration];
            float[] i_b = new float[randomTestIteration];
            BigFloat[] bf_a = new BigFloat[randomTestIteration];
            BigFloat[] bf_b = new BigFloat[randomTestIteration];

            for (int i = 0; i < randomTestIteration; i++)
            {
                int randomNo1 = rng.Next();
                int randomNo2 = rng.Next();
                i_a[i] = randomNo1;
                i_a[i] = randomNo2;
                bf_a[i] = (BigFloat)randomNo1;
                bf_b[i] = (BigFloat)randomNo2;
            }

            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            float res = 0;
            for (int i = 0; i < randomTestIteration; i++)
            {
                res = i_a[i] + i_b[i];
            }
            stopwatch.Stop();

            TimeSpan floatElaped = stopwatch.Elapsed;

            stopwatch.Restart();
            BigFloat resbf;
            for (int i = 0; i < randomTestIteration; i++)
            {
                resbf = bf_a[i] + bf_b[i];
            }
            stopwatch.Stop();

            TimeSpan bfElaped = stopwatch.Elapsed;
            UnityEngine.Debug.Log($"Float test time: {floatElaped}");
            UnityEngine.Debug.Log($"BigFloat test time: {stopwatch.Elapsed}");

            UnityEngine.Debug.Log($"Delta: {stopwatch.Elapsed - floatElaped}({ (double)stopwatch.Elapsed.Milliseconds / floatElaped.Milliseconds} )");
        }

        [Test]
        public void CompareBigFloatToBiggerFloat()
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

            BiggerFloat[] i_a = new BiggerFloat[randomTestIteration];
            BiggerFloat[] i_b = new BiggerFloat[randomTestIteration];
            BigFloat[] bf_a = new BigFloat[randomTestIteration];
            BigFloat[] bf_b = new BigFloat[randomTestIteration];

            for (int i = 0; i < randomTestIteration; i++)
            {
                int randomNo1 = rng.Next();
                int randomNo2 = rng.Next();
                i_a[i] = randomNo1;
                i_a[i] = randomNo2;
                bf_a[i] = randomNo1;
                bf_b[i] = randomNo2;
            }

            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();
            BiggerFloat res = 0;
            for (int i = 0; i < randomTestIteration; i++)
            {
                res = i_a[i] + i_b[i];
            }
            stopwatch.Stop();

            TimeSpan floatElaped = stopwatch.Elapsed;

            stopwatch.Restart();
            BigFloat resbf;
            for (int i = 0; i < randomTestIteration; i++)
            {
                resbf = bf_a[i] + bf_b[i];
            }
            stopwatch.Stop();

            TimeSpan bfElaped = stopwatch.Elapsed;
            UnityEngine.Debug.Log($"BiggerFloat test time: {floatElaped}");
            UnityEngine.Debug.Log($"BigFloat test time: {stopwatch.Elapsed}");

            UnityEngine.Debug.Log($"Delta: {stopwatch.Elapsed - floatElaped}({ (double)stopwatch.Elapsed.Milliseconds / floatElaped.Milliseconds} )");
        }
    }
}
