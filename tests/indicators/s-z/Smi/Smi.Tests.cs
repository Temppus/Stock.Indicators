using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skender.Stock.Indicators;

namespace Internal.Tests;

[TestClass]
public class Smi : TestBase
{
    [TestMethod]
    public void Standard()
    {
        List<SmiResult> results = quotes.GetSmi(14, 20, 5, 3)
            .ToList();

        // assertions

        // proper quantities
        // should always be the same number of results as there is quotes
        Assert.AreEqual(502, results.Count);
        Assert.AreEqual(489, results.Where(x => x.Smi != null).Count());
        Assert.AreEqual(489, results.Where(x => x.Signal != null).Count());

        // sample values
        SmiResult r12 = results[12];
        Assert.IsNull(r12.Smi);
        Assert.IsNull(r12.Signal);

        SmiResult r13 = results[13];
        Assert.AreEqual(17.2603m, NullMath.Round(r13.Smi, 4));
        Assert.AreEqual(17.2603m, NullMath.Round(r13.Signal, 4));

        SmiResult r14 = results[14];
        Assert.AreEqual(18.6086m, NullMath.Round(r14.Smi, 4));
        Assert.AreEqual(17.9344m, NullMath.Round(r14.Signal, 4));

        SmiResult r28 = results[28];
        Assert.AreEqual(51.0417m, NullMath.Round(r28.Smi, 4));
        Assert.AreEqual(47.1207m, NullMath.Round(r28.Signal, 4));

        SmiResult r150 = results[150];
        Assert.AreEqual(65.6692m, NullMath.Round(r150.Smi, 4));
        Assert.AreEqual(66.3292m, NullMath.Round(r150.Signal, 4));

        SmiResult r250 = results[250];  // also testing aliases here
        Assert.AreEqual(67.2534m, NullMath.Round(r250.Smi, 4));
        Assert.AreEqual(67.6261m, NullMath.Round(r250.Signal, 4));

        SmiResult r501 = results[501];
        Assert.AreEqual(-52.6560m, NullMath.Round(r501.Smi, 4));
        Assert.AreEqual(-54.1903m, NullMath.Round(r501.Signal, 4));
    }

    [TestMethod]
    public void NoSignal()
    {
        List<SmiResult> results = quotes.GetSmi(5, 20, 20, 1)
            .ToList();

        // signal equals oscillator
        SmiResult r1 = results[487];
        Assert.AreEqual(r1.Smi, r1.Signal);

        SmiResult r2 = results[501];
        Assert.AreEqual(r2.Smi, r2.Signal);
    }

    [TestMethod]
    public void SmallPeriods()
    {
        List<SmiResult> results = quotes.GetSmi(1, 1, 1, 5)
            .ToList();

        // sample values
        SmiResult r51 = results[51];
        Assert.AreEqual(-100m, NullMath.Round(r51.Smi, 4));
        Assert.AreEqual(-20.8709m, NullMath.Round(r51.Signal, 4));

        SmiResult r81 = results[81];
        Assert.AreEqual(0m, NullMath.Round(r81.Smi, 4));
        Assert.AreEqual(-14.7101m, NullMath.Round(r81.Signal, 4));

        SmiResult r88 = results[88];
        Assert.AreEqual(100m, NullMath.Round(r88.Smi, 4));
        Assert.AreEqual(47.2291m, NullMath.Round(r88.Signal, 4));
    }

    [TestMethod]
    public void BadData()
    {
        IEnumerable<SmiResult> r = badQuotes.GetSmi(5, 5, 1, 5);
        Assert.AreEqual(502, r.Count());
    }

    [TestMethod]
    public void NoQuotes()
    {
        IEnumerable<SmiResult> r0 = noquotes.GetSmi(5, 5, 2);
        Assert.AreEqual(0, r0.Count());

        IEnumerable<SmiResult> r1 = onequote.GetSmi(5, 3, 3);
        Assert.AreEqual(1, r1.Count());
    }

    [TestMethod]
    public void Removed()
    {
        List<SmiResult> results = quotes.GetSmi(14, 20, 5, 3)
                .RemoveWarmupPeriods()
                .ToList();

        // assertions
        Assert.AreEqual(501 - (14 + 100), results.Count);

        SmiResult last = results.LastOrDefault();
        Assert.AreEqual(-52.6560m, NullMath.Round(last.Smi, 4));
        Assert.AreEqual(-54.1903m, NullMath.Round(last.Signal, 4));
    }

    [TestMethod]
    public void Exceptions()
    {
        // bad lookback period
        Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            Indicator.GetSmi(quotes, 0, 5, 5, 5));

        // bad first smooth period
        Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            Indicator.GetSmi(quotes, 14, 0, 5, 5));

        // bad second smooth period
        Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            Indicator.GetSmi(quotes, 14, 3, 0, 5));

        // bad signal
        Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            quotes.GetSmi(9, 3, 1, 0));
    }
}
