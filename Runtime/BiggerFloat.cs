using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using UnityEngine;

namespace BigFloatNumerics
{
    [Serializable]
    public struct BiggerFloat : IComparable, IComparable<BiggerFloat>, IEquatable<BiggerFloat>
    ///number is m × 10^n
    {
        public float m { get; private set; } // you could set this to `double` and there should be minimal problem. Decimal is better.
        public BigInteger n { get; private set; }
        public static readonly BiggerFloat Zero = new BiggerFloat() { m = 0, n = 0 };
        public static readonly BiggerFloat One = new BiggerFloat() { m = 01, n = 0 };

        public static readonly BiggerFloat IntMax = (BiggerFloat)int.MaxValue;

        const float CompTolerance = 1e-6f;
        const int CompTolerancei = 6;


        public BiggerFloat Arrange() // Sets Numerator to be at range of `[1,10)`
        {
            float absm = Mathf.Abs(m);
            if (absm < float.Epsilon)
            {
                n = 0;
                return this;
            }
            int log = (int)Mathf.Floor(Mathf.Log10(absm));
            n += log;
            m /= Mathf.Pow(10, log);
            return this;
        }

        #region constructors
        public BiggerFloat(string value)
        {
            BiggerFloat bf = Parse(value);
            this.m = bf.m;
            this.n = bf.n;
        }
        public BiggerFloat(float m, BigInteger n)
        {
            this.m = m;
            this.n = n;
            Arrange();
        }
        public BiggerFloat(BigInteger value)
        {
            int log = (int)Math.Floor(BigInteger.Log10(BigInteger.Abs(value)));
            if (log < 8)
            {
                m = (float)value;
                Arrange();
                return;
            }
            n = log;
            int valueDividedByLog = (int)(value / BigInteger.Pow(10, log - 8));
            m = valueDividedByLog / 1e8f; // Int.Max =~ 2e9. so divide to int range then divide again to double range.
        }
        public BiggerFloat(BiggerFloat value)
        {
            this.m = value.m;
            this.n = value.n;
        }
        public BiggerFloat(ulong value)
        {
            m = (float)value;
            n = BigInteger.Zero;
            Arrange();
        }
        public BiggerFloat(long value)
        {
            m = (float)value;
            n = BigInteger.Zero;
            Arrange();
        }
        public BiggerFloat(uint value)
        {
            m = (float)value;
            n = BigInteger.Zero;
            Arrange();
        }
        public BiggerFloat(int value)
        {
            m = (float)value;
            n = BigInteger.Zero;
            Arrange();
        }
        public BiggerFloat(float value)
        {
            m = (float)value;
            n = BigInteger.Zero;
            Arrange();
        }
        public BiggerFloat(double value) : this(value.ToString("e9"))// converts to "123e+5678", hence "+5678" can be parsed correctly, is very lazy approach
        {
        }
        public BiggerFloat(decimal value)
        {
            m = (float)value;
            Arrange();
        }
        #endregion

        #region basic arithmetic
        public BiggerFloat Add(BiggerFloat other)
        {
            if (this.n > other.n)
            {
                if (this.n - other.n > 20) return this;
                m += other.m / Mathf.Pow(10, (float)(this.n - other.n));
                this.Arrange();
            }
            else
            {
                if (other.n - this.n > 20) return other;
                m *= Mathf.Pow(10, (float)(this.n - other.n));
                m += other.m;
                this.n = other.n;
                this.Arrange();
            }

            return this;
        }
        public BiggerFloat Subtract(BiggerFloat other)
        {
            return Add(other.Negate());
        }


        public BiggerFloat Multiply(BiggerFloat other)
        {
            this.m *= other.m;
            this.n += other.n;
            this.Arrange();
            return this;
        }
        public BiggerFloat Divide(BiggerFloat other)
        {
            if (other.m == 0)
                throw new System.DivideByZeroException("other");

            this.m /= other.m;
            this.n -= other.n;
            this.Arrange();
            return this;
        }
        public BiggerFloat Remainder(BiggerFloat other)
        {

            throw new NotImplementedException("Does not mattered to me lol");
        }
        public BiggerFloat Pow(int exponent) // there is no smart way to do float lol
        {
            if (m == 0)
            {
                // Nothing to do
            }
            if (exponent > 30)
                return Pow(Pow(this, exponent / 30), 30) + Pow(exponent % 30);

            this.n += exponent;
            this.m = Mathf.Pow(m, exponent);
            Arrange();

            return this;
        }

        public BiggerFloat Pow(BigInteger exponent) // there is no smart way to do float lol
        {
            if (m == 0)
            {
                // Nothing to do
            }
            if (exponent > 30)
                return Pow(Pow(this, exponent / 30), 30) + Pow(exponent % 30);

            this.n += exponent;
            this.m = Mathf.Pow(m, (float)exponent);
            Arrange();

            return this;
        }
        public BiggerFloat Abs()
        {
            m = Mathf.Abs(m);
            return this;
        }
        public BiggerFloat Negate()
        {
            m = -m;
            return this;
        }
        public BigInteger Log10i()
        {
            return n;
        }
        public BiggerFloat Log10()
        {
            return Mathf.Log10(m) + (BiggerFloat)n;
        }
        public BiggerFloat Log(double baseValue)
        {
            return Log10() * Math.Log(baseValue, 10);
        }

        public int CompareTo(BiggerFloat other)
        {
            var diff = this - other;

            if (diff.n == 0 && diff.m == 0)
                return 0;
            if (this.n - diff.n > CompTolerancei)
                return 0;

            else return diff.m.CompareTo(0);
        }
        public int CompareTo(object other)
        {
            if (other == null)
                throw new ArgumentNullException("other");

            if (!(other is BiggerFloat))
                throw new System.ArgumentException("other is not a BiggerFloat");

            return CompareTo((BiggerFloat)other);
        }
        public bool Equals(BiggerFloat other)
        {
            int comResult = CompareTo(other);
            return comResult == 0;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion

        #region Derived static Arithmetic methods
        public static int Compare(BiggerFloat left, BiggerFloat right)
        {
            return (new BiggerFloat(left)).CompareTo(right);
        }

        public static BiggerFloat Negate(BiggerFloat value)
        {
            return (new BiggerFloat(value)).Negate();
        }
        public static BiggerFloat Abs(BiggerFloat value)
        {
            return (new BiggerFloat(value)).Abs();
        }
        public static BiggerFloat Add(BiggerFloat left, BiggerFloat right)
        {
            return (new BiggerFloat(left)).Add(right);
        }
        public static BiggerFloat Subtract(BiggerFloat left, BiggerFloat right)
        {
            return (new BiggerFloat(left)).Subtract(right);
        }
        public static BiggerFloat Multiply(BiggerFloat left, BiggerFloat right)
        {
            return (new BiggerFloat(left)).Multiply(right);
        }
        public static BiggerFloat Divide(BiggerFloat left, BiggerFloat right)
        {
            return (new BiggerFloat(left)).Divide(right);
        }
        public static BiggerFloat Pow(BiggerFloat value, int exponent)
        {
            return (new BiggerFloat(value)).Pow(exponent);
        }
        public static BiggerFloat Pow(BiggerFloat value, BigInteger exponent)
        {
            return (new BiggerFloat(value)).Pow(exponent);
        }
        public static BiggerFloat Remainder(BiggerFloat left, BiggerFloat right)
        {
            return (new BiggerFloat(left)).Remainder(right);
        }
        public static BiggerFloat Log10(BiggerFloat value)
        {
            return (new BiggerFloat(value)).Log10();
        }
        public static BiggerFloat Log(BiggerFloat value, double baseValue)
        {
            return (new BiggerFloat(value)).Log(baseValue);
        }
        #endregion

        #region ToString and Parse
        public override string ToString()
        {
            return $"{m:F10}e{n}";
        }
        public string ToHumanFriendlyString(int? maxLength = null)
        {
            if(maxLength == null)
                return $"{m:F10}e{n}";
            if (BigInteger.Abs(n) < maxLength)
            {
                return $"{(double)m:F99}".Substring(0, (int)maxLength);
            }
            else
            {
                int len = (int)(float)this.Log(10);
                if (len > maxLength)
                {
                    return $"{m:F0}e{n}";
                }
                else
                {
                    return $"{m.ToString("F" + (maxLength - len - 3))}e{n}";
                }
            }
        }

        public static BiggerFloat Parse(string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            value.Trim();
            value = value.Replace(",", "");
            int pos = value.IndexOf('e');
            value = value.Replace("e", "");

            if (pos < 0)
            {
                if (value.IndexOf('.') >= 0)
                {
                    //just floating point 
                    double Signifacand = double.Parse(value);
                    return (new BiggerFloat(Signifacand)).Arrange();
                }
                else
                {
                    //just big integer
                    BigInteger nu = BigInteger.Parse(value);
                    return (new BiggerFloat(nu)).Arrange();
                }
            }
            else
            {
                //decimal point (length - pos - 1)
                float Signifacand = float.Parse(value.Substring(0, pos));

                BigInteger denominator = BigInteger.Parse(value.Substring(pos));

                return (new BiggerFloat(Signifacand, denominator)).Arrange();
            }
        }
        public static bool TryParse(string value, out BiggerFloat result)
        {
            try
            {
                result = BiggerFloat.Parse(value);
                return true;
            }
            catch (ArgumentNullException)
            {
                result = Zero;
                return false;
            }
            catch (FormatException)
            {
                result = Zero;
                return false;
            }
        }

        #endregion

        #region Derived Functions and Operators
        public static BiggerFloat operator -(BiggerFloat value)
        {
            return (new BiggerFloat(value)).Negate();
        }
        public static BiggerFloat operator -(BiggerFloat left, BiggerFloat right)
        {
            return (new BiggerFloat(left)).Subtract(right);
        }
        public static BiggerFloat operator --(BiggerFloat value)
        {
            return value.Subtract(1);
        }
        public static BiggerFloat operator +(BiggerFloat left, BiggerFloat right)
        {
            return (new BiggerFloat(left)).Add(right);
        }
        public static BiggerFloat operator +(BiggerFloat value)
        {
            return (new BiggerFloat(value)).Abs();
        }
        public static BiggerFloat operator ++(BiggerFloat value)
        {
            return value.Add(1);
        }
        public static BiggerFloat operator %(BiggerFloat left, BiggerFloat right)
        {
            return (new BiggerFloat(left)).Remainder(right);
        }
        public static BiggerFloat operator *(BiggerFloat left, BiggerFloat right)
        {
            return (new BiggerFloat(left)).Multiply(right);
        }
        public static BiggerFloat operator /(BiggerFloat left, BiggerFloat right)
        {
            return (new BiggerFloat(left)).Divide(right);
        }
        public static BiggerFloat operator ^(BiggerFloat left, int right)
        {
            return (new BiggerFloat(left)).Pow(right);
        }

        public static bool operator !=(BiggerFloat left, BiggerFloat right)
        {
            return Compare(left, right) != 0;
        }
        public static bool operator ==(BiggerFloat left, BiggerFloat right)
        {
            return Compare(left, right) == 0;
        }
        public static bool operator <(BiggerFloat left, BiggerFloat right)
        {
            return Compare(left, right) < 0;
        }
        public static bool operator <=(BiggerFloat left, BiggerFloat right)
        {
            return Compare(left, right) <= 0;
        }
        public static bool operator >(BiggerFloat left, BiggerFloat right)
        {
            return Compare(left, right) > 0;
        }
        public static bool operator >=(BiggerFloat left, BiggerFloat right)
        {
            return Compare(left, right) >= 0;
        }
        #endregion

        #region Casts
        public static explicit operator decimal(BiggerFloat value)
        {
            if (decimal.MinValue > value) throw new System.OverflowException("value is less than System.decimal.MinValue.");
            if (decimal.MaxValue < value) throw new System.OverflowException("value is greater than System.decimal.MaxValue.");

            return (decimal)(value.m * Math.Pow(10, (double)value.n));
        }

        public static explicit operator BigInteger(BiggerFloat v)
        {
            if ((v.n - 5) > int.MaxValue) throw new OverflowException("value is too large");
            if ((v > int.MaxValue || v < int.MinValue))
            {
                return BigInteger.Multiply((BigInteger)(v.m * 1e5f), BigInteger.Pow(10, (int)(v.n - 5)));
            }
            else return (int)v;
        }

        public static explicit operator double(BiggerFloat value)
        {
            if (double.MinValue > value) throw new System.OverflowException("value is less than System.double.MinValue.");
            if (double.MaxValue < value) throw new System.OverflowException("value is greater than System.double.MaxValue.");

            return (double)value.m * Math.Pow(10, (double)value.n);
        }
        public static explicit operator float(BiggerFloat value)
        {
            if (float.MinValue > value) throw new System.OverflowException("value is less than System.float.MinValue.");
            if (float.MaxValue < value) throw new System.OverflowException("value is greater than System.float.MaxValue.");

            return (float)(value.m * Mathf.Pow(10, (float)value.n));
        }

        public static explicit operator int(BiggerFloat value)
        {
            if (int.MinValue > value) throw new System.OverflowException("value is less than System.float.MinValue.");
            if (int.MaxValue < value) throw new System.OverflowException("value is greater than System.float.MaxValue.");

            return (int)(value.m * Mathf.Pow(10, (float)value.n));
        }


        public static implicit operator BiggerFloat(byte value)
        {
            return new BiggerFloat((uint)value);
        }
        public static implicit operator BiggerFloat(sbyte value)
        {
            return new BiggerFloat((int)value);
        }
        public static implicit operator BiggerFloat(short value)
        {
            return new BiggerFloat((int)value);
        }
        public static implicit operator BiggerFloat(ushort value)
        {
            return new BiggerFloat((uint)value);
        }
        public static implicit operator BiggerFloat(int value)
        {
            return new BiggerFloat(value);
        }
        public static implicit operator BiggerFloat(long value)
        {
            return new BiggerFloat(value);
        }
        public static implicit operator BiggerFloat(uint value)
        {
            return new BiggerFloat(value);
        }
        public static implicit operator BiggerFloat(ulong value)
        {
            return new BiggerFloat(value);
        }
        public static implicit operator BiggerFloat(decimal value)
        {
            return new BiggerFloat(value);
        }
        public static implicit operator BiggerFloat(double value)
        {
            return new BiggerFloat(value);
        }
        public static implicit operator BiggerFloat(float value)
        {
            return new BiggerFloat(value);
        }
        public static implicit operator BiggerFloat(BigInteger value)
        {
            return new BiggerFloat(value);
        }
        public static explicit operator BiggerFloat(string value)
        {
            return new BiggerFloat(value);
        }
        #endregion
    }
}
