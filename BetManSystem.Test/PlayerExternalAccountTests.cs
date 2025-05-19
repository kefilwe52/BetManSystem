using BetManSystem.Application.Services;
using BetManSystem.Common.Entities;
using BetManSystem.Common.Enums;
using BetManSystem.DataAccess.IRepositories;
using Microsoft.Extensions.Logging;
using Moq;

namespace BetManSystem.Test
{
    public class PlayerExternalAccountServiceTests
    {
        private readonly Mock<IPlayerExternalAccountRepository> _repoMock;
        private readonly Mock<ILogger<PlayerExternalAccountService>> _loggerMock;
        private readonly PlayerExternalAccountService _service;

        public PlayerExternalAccountServiceTests()
        {
            _repoMock = new Mock<IPlayerExternalAccountRepository>();
            _loggerMock = new Mock<ILogger<PlayerExternalAccountService>>();
            _service = new PlayerExternalAccountService(_repoMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetAsync_WithBetGamesAndKefilwe_ReturnsAccount()
        {
            // arrange
            var provider = ProviderType.BetGames;
            var externalId = "kefilwe";
            var expected = new PlayerExternalAccount
            {
                Provider = provider,
                ExternalPlayerId = externalId,
                PlayerId = Guid.NewGuid()
            };
            _repoMock
                .Setup(r => r.GetByProviderAndExternalIdAsync(provider, externalId))
                .ReturnsAsync(expected);

            // act
            var actual = await _service.GetAsync(provider, externalId);

            // assert
            Assert.Same(expected, actual);
            _repoMock.Verify(r => r.GetByProviderAndExternalIdAsync(provider, externalId), Times.Once);
        }

        [Fact]
        public async Task GetAsync_WithBetWayAndKefilwe_ThrowsException()
        {
            // arrange
            var provider = ProviderType.BetWay;
            var externalId = "kefilwe";
            var repoEx = new InvalidOperationException("repo failure");
            _repoMock
                .Setup(r => r.GetByProviderAndExternalIdAsync(provider, externalId))
                .ThrowsAsync(repoEx);

            // act & assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.GetAsync(provider, externalId));
            Assert.Equal(repoEx, ex);
        }

        [Fact]
        public async Task CreateAsync_WithBetGamesAndKefilwe_SetsIdsAndCallsRepo()
        {
            // arrange
            var account = new PlayerExternalAccount
            {
                Provider = ProviderType.BetGames,
                ExternalPlayerId = "kefilwe",
                PlayerId = Guid.NewGuid()
            };
            PlayerExternalAccount? captured = null;
            _repoMock
                .Setup(r => r.AddAsync(It.IsAny<PlayerExternalAccount>()))
                .Callback<PlayerExternalAccount>(a => captured = a)
                .Returns(Task.CompletedTask);

            // act
            var result = await _service.CreateAsync(account);

            // assert
            Assert.Same(account, result);
            Assert.NotEqual(default, captured!.Id);
            Assert.NotEqual(default, captured.CreatedAt);
            Assert.NotEqual(default, captured.LastLoginAt);
            _repoMock.Verify(r => r.AddAsync(captured!), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WithBetWayAndKefilwe_ThrowsException()
        {
            // arrange
            var account = new PlayerExternalAccount
            {
                Provider = ProviderType.BetWay,
                ExternalPlayerId = "kefilwe",
                PlayerId = Guid.NewGuid()
            };
            var repoEx = new Exception("add failure");
            _repoMock
                .Setup(r => r.AddAsync(It.IsAny<PlayerExternalAccount>()))
                .ThrowsAsync(repoEx);

            // act & assert
            var ex = await Assert.ThrowsAsync<Exception>(() =>
                _service.CreateAsync(account));
            Assert.Equal(repoEx, ex);
        }

        [Fact]
        public async Task UpdateAsync_WithBetGamesAndKefilwe_SetsLastLoginAndCallsRepo()
        {
            // arrange
            var account = new PlayerExternalAccount
            {
                Id = Guid.NewGuid(),
                Provider = ProviderType.BetGames,
                ExternalPlayerId = "kefilwe",
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                LastLoginAt = default
            };
            PlayerExternalAccount captured = null;
            _repoMock
                .Setup(r => r.UpdateAsync(It.IsAny<PlayerExternalAccount>()))
                .Callback<PlayerExternalAccount>(a => captured = a)
                .Returns(Task.CompletedTask);

            // act
            var result = await _service.UpdateAsync(account);

            // assert
            Assert.Same(account, result);
            Assert.NotEqual(default, captured!.LastLoginAt);
            _repoMock.Verify(r => r.UpdateAsync(captured!), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WithBetWayAndKefilwe_ThrowsException()
        {
            // arrange
            var account = new PlayerExternalAccount
            {
                Id = Guid.NewGuid(),
                Provider = ProviderType.BetWay,
                ExternalPlayerId = "kefilwe",
                CreatedAt = DateTime.UtcNow,
                LastLoginAt = DateTime.UtcNow
            };
            var repoEx = new Exception("update failure");
            _repoMock
                .Setup(r => r.UpdateAsync(It.IsAny<PlayerExternalAccount>()))
                .ThrowsAsync(repoEx);

            // act & assert
            var ex = await Assert.ThrowsAsync<Exception>(() =>
                _service.UpdateAsync(account));
            Assert.Equal(repoEx, ex);
        }
    }
}
