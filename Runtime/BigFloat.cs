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
    public struct BigFloat : IComparable, IComparable<BigFloat>, IEquatable<BigFloat>
    ///number is m × 10^n
    {
        public float m { get; private set; } // you could set this to `double` and there should be minimal problem. Decimal is better.
        public BigInteger n { get; private set; }
        public static readonly BigFloat Zero = new BigFloat() { m = 0, n = 0 };
        public static readonly BigFloat One = new BigFloat() { m = 01, n = 0 };

        public static readonly BigFloat IntMax = (BigFloat)int.MaxValue;

        const float CompTolerance = 1e-6f;
        const int CompTolerancei = 6;


        public BigFloat Arrange() // Sets Numerator to be at range of `[1,10)`
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
        public BigFloat(string value)
        {
            BigFloat bf = Parse(value);
            this.m = bf.m;
            this.n = bf.n;
        }
        public BigFloat(float m, BigInteger n)
        {
            this.m = m;
            this.n = n;
            Arrange();
        }
        public BigFloat(BigInteger value)
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
        public BigFloat(BigFloat value)
        {
            this.m = value.m;
            this.n = value.n;
        }
        public BigFloat(ulong value)
        {
            m = (float)value;
            n = BigInteger.Zero;
            Arrange();
        }
        public BigFloat(long value)
        {
            m = (float)value;
            n = BigInteger.Zero;
            Arrange();
        }
        public BigFloat(uint value)
        {
            m = (float)value;
            n = BigInteger.Zero;
            Arrange();
        }
        public BigFloat(int value)
        {
            m = (float)value;
            n = BigInteger.Zero;
            Arrange();
        }
        public BigFloat(float value)
        {
            m = (float)value;
            n = BigInteger.Zero;
            Arrange();
        }
        public BigFloat(double value) : this(value.ToString("e9"))// converts to "123e+5678", hence "+5678" can be parsed correctly, is very lazy approach
        {
        }
        public BigFloat(decimal value)
        {
            m = (float)value;
            Arrange();
        }
        #endregion

        #region basic arithmetic
        public BigFloat Add(BigFloat other)
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
        public BigFloat Subtract(BigFloat other)
        {
            return Add(other.Negate());
        }


        public BigFloat Multiply(BigFloat other)
        {
            this.m *= other.m;
            this.n += other.n;
            this.Arrange();
            return this;
        }
        public BigFloat Divide(BigFloat other)
        {
            if (other.m == 0)
                throw new System.DivideByZeroException("other");

            this.m /= other.m;
            this.n -= other.n;
            this.Arrange();
            return this;
        }
        public BigFloat Remainder(BigFloat other)
        {

            throw new NotImplementedException("Does not mattered to me lol");
        }
        public BigFloat Pow(int exponent) // there is no smart way to do float lol
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

        public BigFloat Pow(BigInteger exponent) // there is no smart way to do float lol
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
        public BigFloat Abs()
        {
            m = Mathf.Abs(m);
            return this;
        }
        public BigFloat Negate()
        {
            m = -m;
            return this;
        }
        public BigInteger Log10i()
        {
            return n;
        }
        public BigFloat Log10()
        {
            return Mathf.Log10(m) + (BigFloat)n;
        }
        public BigFloat Log(double baseValue)
        {
            return Log10() * Math.Log(baseValue, 10);
        }

        public int CompareTo(BigFloat other)
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

            if (!(other is BigFloat))
                throw new System.ArgumentException("other is not a BigFloat");

            return CompareTo((BigFloat)other);
        }
        public bool Equals(BigFloat other)
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
        public static int Compare(BigFloat left, BigFloat right)
        {
            return (new BigFloat(left)).CompareTo(right);
        }

        public static BigFloat Negate(BigFloat value)
        {
            return (new BigFloat(value)).Negate();
        }
        public static BigFloat Abs(BigFloat value)
        {
            return (new BigFloat(value)).Abs();
        }
        public static BigFloat Add(BigFloat left, BigFloat right)
        {
            return (new BigFloat(left)).Add(right);
        }
        public static BigFloat Subtract(BigFloat left, BigFloat right)
        {
            return (new BigFloat(left)).Subtract(right);
        }
        public static BigFloat Multiply(BigFloat left, BigFloat right)
        {
            return (new BigFloat(left)).Multiply(right);
        }
        public static BigFloat Divide(BigFloat left, BigFloat right)
        {
            return (new BigFloat(left)).Divide(right);
        }
        public static BigFloat Pow(BigFloat value, int exponent)
        {
            return (new BigFloat(value)).Pow(exponent);
        }
        public static BigFloat Pow(BigFloat value, BigInteger exponent)
        {
            return (new BigFloat(value)).Pow(exponent);
        }
        public static BigFloat Remainder(BigFloat left, BigFloat right)
        {
            return (new BigFloat(left)).Remainder(right);
        }
        public static BigFloat Log10(BigFloat value)
        {
            return (new BigFloat(value)).Log10();
        }
        public static BigFloat Log(BigFloat value, double baseValue)
        {
            return (new BigFloat(value)).Log(baseValue);
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

        public static BigFloat Parse(string value)
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
                    return (new BigFloat(Signifacand)).Arrange();
                }
                else
                {
                    //just big integer
                    BigInteger nu = BigInteger.Parse(value);
                    return (new BigFloat(nu)).Arrange();
                }
            }
            else
            {
                //decimal point (length - pos - 1)
                float Signifacand = float.Parse(value.Substring(0, pos));

                BigInteger denominator = BigInteger.Parse(value.Substring(pos));

                return (new BigFloat(Signifacand, denominator)).Arrange();
            }
        }
        public static bool TryParse(string value, out BigFloat result)
        {
            try
            {
                result = BigFloat.Parse(value);
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
        public static BigFloat operator -(BigFloat value)
        {
            return (new BigFloat(value)).Negate();
        }
        public static BigFloat operator -(BigFloat left, BigFloat right)
        {
            return (new BigFloat(left)).Subtract(right);
        }
        public static BigFloat operator --(BigFloat value)
        {
            return value.Subtract(1);
        }
        public static BigFloat operator +(BigFloat left, BigFloat right)
        {
            return (new BigFloat(left)).Add(right);
        }
        public static BigFloat operator +(BigFloat value)
        {
            return (new BigFloat(value)).Abs();
        }
        public static BigFloat operator ++(BigFloat value)
        {
            return value.Add(1);
        }
        public static BigFloat operator %(BigFloat left, BigFloat right)
        {
            return (new BigFloat(left)).Remainder(right);
        }
        public static BigFloat operator *(BigFloat left, BigFloat right)
        {
            return (new BigFloat(left)).Multiply(right);
        }
        public static BigFloat operator /(BigFloat left, BigFloat right)
        {
            return (new BigFloat(left)).Divide(right);
        }
        public static BigFloat operator ^(BigFloat left, int right)
        {
            return (new BigFloat(left)).Pow(right);
        }

        public static bool operator !=(BigFloat left, BigFloat right)
        {
            return Compare(left, right) != 0;
        }
        public static bool operator ==(BigFloat left, BigFloat right)
        {
            return Compare(left, right) == 0;
        }
        public static bool operator <(BigFloat left, BigFloat right)
        {
            return Compare(left, right) < 0;
        }
        public static bool operator <=(BigFloat left, BigFloat right)
        {
            return Compare(left, right) <= 0;
        }
        public static bool operator >(BigFloat left, BigFloat right)
        {
            return Compare(left, right) > 0;
        }
        public static bool operator >=(BigFloat left, BigFloat right)
        {
            return Compare(left, right) >= 0;
        }
        #endregion

        #region Casts
        public static explicit operator decimal(BigFloat value)
        {
            if (decimal.MinValue > value) throw new System.OverflowException("value is less than System.decimal.MinValue.");
            if (decimal.MaxValue < value) throw new System.OverflowException("value is greater than System.decimal.MaxValue.");

            return (decimal)(value.m * Math.Pow(10, (double)value.n));
        }

        public static explicit operator BigInteger(BigFloat v)
        {
            if ((v.n - 5) > int.MaxValue) throw new OverflowException("value is too large");
            if ((v > int.MaxValue || v < int.MinValue))
            {
                return BigInteger.Multiply((BigInteger)(v.m * 1e5f), BigInteger.Pow(10, (int)(v.n - 5)));
            }
            else return (int)v;
        }

        public static explicit operator double(BigFloat value)
        {
            if (double.MinValue > value) throw new System.OverflowException("value is less than System.double.MinValue.");
            if (double.MaxValue < value) throw new System.OverflowException("value is greater than System.double.MaxValue.");

            return (double)value.m * Math.Pow(10, (double)value.n);
        }
        public static explicit operator float(BigFloat value)
        {
            if (float.MinValue > value) throw new System.OverflowException("value is less than System.float.MinValue.");
            if (float.MaxValue < value) throw new System.OverflowException("value is greater than System.float.MaxValue.");

            return (float)(value.m * Mathf.Pow(10, (float)value.n));
        }

        public static explicit operator int(BigFloat value)
        {
            if (int.MinValue > value) throw new System.OverflowException("value is less than System.float.MinValue.");
            if (int.MaxValue < value) throw new System.OverflowException("value is greater than System.float.MaxValue.");

            return (int)(value.m * Mathf.Pow(10, (float)value.n));
        }


        public static implicit operator BigFloat(byte value)
        {
            return new BigFloat((uint)value);
        }
        public static implicit operator BigFloat(sbyte value)
        {
            return new BigFloat((int)value);
        }
        public static implicit operator BigFloat(short value)
        {
            return new BigFloat((int)value);
        }
        public static implicit operator BigFloat(ushort value)
        {
            return new BigFloat((uint)value);
        }
        public static implicit operator BigFloat(int value)
        {
            return new BigFloat(value);
        }
        public static implicit operator BigFloat(long value)
        {
            return new BigFloat(value);
        }
        public static implicit operator BigFloat(uint value)
        {
            return new BigFloat(value);
        }
        public static implicit operator BigFloat(ulong value)
        {
            return new BigFloat(value);
        }
        public static implicit operator BigFloat(decimal value)
        {
            return new BigFloat(value);
        }
        public static implicit operator BigFloat(double value)
        {
            return new BigFloat(value);
        }
        public static implicit operator BigFloat(float value)
        {
            return new BigFloat(value);
        }
        public static implicit operator BigFloat(BigInteger value)
        {
            return new BigFloat(value);
        }
        public static explicit operator BigFloat(string value)
        {
            return new BigFloat(value);
        }
        #endregion
    }
}
