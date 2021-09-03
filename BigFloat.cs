using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using UnityEngine;

[Serializable]
struct BigFloat : IComparable, IComparable<BigFloat>, IEquatable<BigFloat>
///number is m × 10^n
{
    public float m { get; private set; } // you could set this to `double` and there should be minimal problem
    public BigInteger n { get; private set; }
    public static BigFloat Zero = new BigFloat() { m = 0, n = 0 };
    public static BigFloat One = new BigFloat() { m = 1, n = 0 };
    public static BigFloat Ten = new BigFloat() { m = 1, n = 1 };

    public BigFloat Arrange() // Sets Numerator to be at range of `[1,10)`
    {
        if (m == 0)
        {
            n = 0;
            return this;
        }
        int log = (int)Math.Floor(Math.Log10(m));
        n += log;
        m /= Mathf.Pow(log, 10);
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
        if (n == 0)
            throw new ArgumentException("denominator equals 0");
        this.n = BigInteger.Abs(n);
    }
    public BigFloat(BigInteger value)
    {
        int log = (int)Math.Floor(BigInteger.Log10(value));
        n = log;
        int valueDividedByLog = (int)(value / BigInteger.Pow(log - 8, 10));
        m = valueDividedByLog / 1e8f; // Int.Max =~ 2e9. so divide to int range then divide again to double range.
    }
    public BigFloat(BigFloat value)
    {
        if (BigFloat.Equals(value, null))
        {
            this.m = 0;
            this.n = BigInteger.One;
        }
        else
        {

            this.m = value.m;
            this.n = value.n;
        }
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
    public BigFloat(double value) : this(value.ToString("e9"))// converts to "123e+5678", hence "+5678" can be parsed correctly
    {
    }
    public BigFloat(decimal value) : this(value.ToString("e9"))
    {
    }
    #endregion

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
            m *= Mathf.Pow(10, this.m - other.m);
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
        return this;
    }
    public BigFloat Remainder(BigFloat other)
    {
        if (BigInteger.Equals(other, null))
            throw new ArgumentNullException("other");

        throw new NotImplementedException("Does not mattered to me lol");
    }
    public BigFloat Pow(int exponent) // there is no smart way to do float lol
    {
        if (m == 0)
        {
            // Nothing to do
        }
        if(exponent > 30)
            return Pow(Pow(this ,exponent / 30), 30) + Pow(exponent % 30);

        this.n += exponent;
        this.m = Mathf.Pow(m, exponent);
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
    public BigFloat Log10()
    {
        return Mathf.Log10(m) + (BigFloat)n;
    }
    public BigFloat Log(double baseValue)
    {
        return Log10() * Math.Log(baseValue, 10);
    }
    public override string ToString()
    {
        //default precision = 10
        return ToString(10);
    }
    public string ToString(int? maxLength = null)
    {
        return $"{m}e{n}";
    }

    public int CompareTo(BigFloat other)
    {
        int mComp = BigInteger.Compare(this.n, other.n);
        if (mComp != 0)
            return mComp;
        return this.m.CompareTo(other.m);
    }
    public int CompareTo(object other)
    {
        if (other == null)
            throw new ArgumentNullException("other");

        if (!(other is BigFloat))
            throw new System.ArgumentException("other is not a BigFloat");

        return CompareTo((BigFloat)other);
    }
    public override bool Equals(object other)
    {
        if (other == null || GetType() != other.GetType())
        {
            return false;
        }

        return this.m == ((BigFloat)other).m && this.n == ((BigFloat)other).n;
    }
    public bool Equals(BigFloat other)
    {
        return (other.m == this.m && other.n == this.n);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    //static methods
    public static new bool Equals(object left, object right)
    {
        return (((BigInteger)left).Equals((BigInteger)right));
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
    public static BigFloat Remainder(BigFloat left, BigFloat right)
    {
        return (new BigFloat(left)).Remainder(right);
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
            float Signifacand = float.Parse(value);
            BigInteger denominator = BigInteger.Pow(10, value.Length - pos);

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
    public static int Compare(BigFloat left, BigFloat right)
    {
        if (BigFloat.Equals(left, null))
            throw new ArgumentNullException("left");
        if (BigFloat.Equals(right, null))
            throw new ArgumentNullException("right");

        return (new BigFloat(left)).CompareTo(right);
    }
    public static BigFloat Log10(BigFloat value)
    {
        return (new BigFloat(value)).Log10();
    }
    public static BigFloat Log(BigFloat value, double baseValue)
    {
        return (new BigFloat(value)).Log(baseValue);
    }

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

    public static bool operator true(BigFloat value)
    {
        return value != 0;
    }
    public static bool operator false(BigFloat value)
    {
        return value == 0;
    }

    public static explicit operator decimal(BigFloat value)
    {
        if (decimal.MinValue > value) throw new System.OverflowException("value is less than System.decimal.MinValue.");
        if (decimal.MaxValue < value) throw new System.OverflowException("value is greater than System.decimal.MaxValue.");

        return (decimal)value.m / (decimal)value.n;
    }
    public static explicit operator double(BigFloat value)
    {
        if (double.MinValue > value) throw new System.OverflowException("value is less than System.double.MinValue.");
        if (double.MaxValue < value) throw new System.OverflowException("value is greater than System.double.MaxValue.");

        return (double)value.m / (double)value.n;
    }
    public static explicit operator float(BigFloat value)
    {
        if (float.MinValue > value) throw new System.OverflowException("value is less than System.float.MinValue.");
        if (float.MaxValue < value) throw new System.OverflowException("value is greater than System.float.MaxValue.");

        return (float)value.m / (float)value.n;
    }

    //byte, sbyte, 
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
}
