using System.Linq;
using System.Threading.Tasks;
using TweetSearchCSharp;
using Xunit;
using Xunit.Extensions;

namespace Tests
{
    public class CSharpSearcherTests
    {
        [Fact]
        public async Task When_getting_bearer_token_then_should_succeed()
        {
            var token = await Searcher.BearerTokenAsync();
            Assert.False(string.IsNullOrEmpty(token));
        }

        [Theory]
        [InlineData("6Wunderkinder")]
        [InlineData("wunderkinder")]
        public async Task When_searching_tweets_then_should_succeed(string searchTerm)
        {
            var token = await Searcher.BearerTokenAsync();
            var results = await new Searcher(token).SearchAsync(searchTerm);
            Assert.NotNull(results);
            Assert.True(results.Any());
        }
    }
}
