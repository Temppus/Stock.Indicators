using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skender.Stock.Indicators;

namespace Internal.Tests;

[TestClass]
public class Hurst : TestBase
{
    [TestMethod]
    public void StandardLong()
    {
        List<HurstResult> results = longestQuotes
            .GetHurst(longestQuotes.Count() - 1)
            .ToList();

        // assertions

        // proper quantities
        Assert.AreEqual(15821, results.Count);
        Assert.AreEqual(1, results.Count(x => x.HurstExponent != null));

        // sample value
        HurstResult r15820 = results[15820];
        Assert.AreEqual(0.483563, NullMath.Round(r15820.HurstExponent, 6));
    }

    [TestMethod]
    public void ToQuotes()
    {
        List<Quote> newQuotes = longestQuotes
            .GetHurst(longestQuotes.Count() - 1)
            .ToQuotes()
            .ToList();

        Assert.AreEqual(1, newQuotes.Count);

        Quote q = newQuotes.LastOrDefault();
        Assert.AreEqual(0.483563m, NullMath.Round(q.Open, 6));
        Assert.AreEqual(0.483563m, NullMath.Round(q.High, 6));
        Assert.AreEqual(0.483563m, NullMath.Round(q.Low, 6));
        Assert.AreEqual(0.483563m, NullMath.Round(q.Close, 6));
    }

    [TestMethod]
    public void BadData()
    {
        IEnumerable<HurstResult> r = Indicator.GetHurst(badQuotes, 150);
        Assert.AreEqual(502, r.Count());
    }

    [TestMethod]
    public void NoQuotes()
    {
        IEnumerable<HurstResult> r0 = noquotes.GetHurst();
        Assert.AreEqual(0, r0.Count());

        IEnumerable<HurstResult> r1 = onequote.GetHurst();
        Assert.AreEqual(1, r1.Count());
    }

    [TestMethod]
    public void Removed()
    {
        List<HurstResult> results = longestQuotes.GetHurst(longestQuotes.Count() - 1)
            .RemoveWarmupPeriods()
            .ToList();

        // assertions
        Assert.AreEqual(1, results.Count);

        HurstResult last = results.LastOrDefault();
        Assert.AreEqual(0.483563, NullMath.Round(last.HurstExponent, 6));
    }

    [TestMethod]
    public void Exceptions()
    {
        // bad lookback period
        Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            Indicator.GetHurst(quotes, 99));
    }
}
