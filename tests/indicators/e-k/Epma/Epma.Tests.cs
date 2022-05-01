using Microsoft.VisualStudio.TestTools.UnitTesting;
using Skender.Stock.Indicators;

namespace Internal.Tests;

[TestClass]
public class Epma : TestBase
{
    [TestMethod]
    public void Standard()
    {
        List<EpmaResult> results = quotes.GetEpma(20).ToList();

        // assertions

        // proper quantities
        // should always be the same number of results as there is quotes
        Assert.AreEqual(502, results.Count);
        Assert.AreEqual(483, results.Where(x => x.Epma != null).Count());

        // sample values
        EpmaResult r1 = results[18];
        Assert.IsNull(r1.Epma);

        EpmaResult r2 = results[19];
        Assert.AreEqual(215.6189m, NullMath.Round(r2.Epma, 4));

        EpmaResult r3 = results[149];
        Assert.AreEqual(236.7060m, NullMath.Round(r3.Epma, 4));

        EpmaResult r4 = results[249];
        Assert.AreEqual(258.5179m, NullMath.Round(r4.Epma, 4));

        EpmaResult r5 = results[501];
        Assert.AreEqual(235.8131m, NullMath.Round(r5.Epma, 4));
    }

    [TestMethod]
    public void BadData()
    {
        IEnumerable<EpmaResult> r = Indicator.GetEpma(badQuotes, 15);
        Assert.AreEqual(502, r.Count());
    }

    [TestMethod]
    public void NoQuotes()
    {
        IEnumerable<EpmaResult> r0 = noquotes.GetEpma(5);
        Assert.AreEqual(0, r0.Count());

        IEnumerable<EpmaResult> r1 = onequote.GetEpma(5);
        Assert.AreEqual(1, r1.Count());
    }

    [TestMethod]
    public void Removed()
    {
        List<EpmaResult> results = quotes.GetEpma(20)
            .RemoveWarmupPeriods()
            .ToList();

        // assertions
        Assert.AreEqual(502 - 19, results.Count);

        EpmaResult last = results.LastOrDefault();
        Assert.AreEqual(235.8131m, NullMath.Round(last.Epma, 4));
    }

    [TestMethod]
    public void Exceptions()
    {
        // bad lookback period
        Assert.ThrowsException<ArgumentOutOfRangeException>(() =>
            Indicator.GetEpma(quotes, 0));
    }
}
