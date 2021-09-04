using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using BigFloatNumerics;

namespace BigFloatNumerics
{
    public class BigFloatTest
    {
        [Test]
        public void CastParseTest()
        {
            //Assert.AreEqual();
        }
        [Test]
        public void AddTest()
        {
            Assert.AreEqual(new BigFloat(1e10) + new BigFloat(1e10), new BigFloat(2e10));
            Assert.AreEqual(new BigFloat(5   ) + new BigFloat(7   ), new BigFloat(1.2e1));
            // Use the Assert class to test conditions
        }
    }
}