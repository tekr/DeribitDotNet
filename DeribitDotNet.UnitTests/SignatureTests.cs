using Xunit;

namespace DeribitDotNet.UnitTests
{
    public class SignatureTests
    {
        [Fact]
        public void CompareToPythonExampleCode()
        {
            const string signatureString = "_=1452237485895&_ackey=2YZn85siaUf5A&_acsec=BTMSIAJ8IYQTAV4MLN88UAHLIUNYZ3HN&" +
                                           "instrument=BTC-15JAN16&price=500&quantity=1";

            Assert.Equal("LQctRklxPiJDHJj9ZYp78Epilx7N78crGghzr1pvNlI=", SignatureFactory.Get(signatureString));
        }

        [Fact]
        public void CrossPlatCompare()
        {
            const string signatureString = "_=63671578716339&_ackey=4ByxzB8MwCqxP&_acsec=LG5HW5B3W4SG7RYT3AQ44AQUW5LOBXWU&" + 
                                           "_action=/api/v1/private/subscribe&event=\norder_book\ntrade\n&instrument=\nfutures\n";

            Assert.Equal("U6mG426T4E5009HzHf5VwkRLMg/WTqedkleuJElae4k=", SignatureFactory.Get(signatureString));
        }
    }
}
