using BetManSystem.Application.Services;
using BetManSystem.Common.Entities;
using BetManSystem.DataAccess.IRepositories;
using Microsoft.Extensions.Logging;
using Moq;

namespace BetManSystem.Test
{
    public class PlayerServiceTests
    {
        private readonly Mock<IPlayerRepository> _repoMock;
        private readonly PlayerService _service;

        public PlayerServiceTests()
        {
            _repoMock = new Mock<IPlayerRepository>();
            var logger = new Mock<ILogger<PlayerService>>().Object;

            _service = new PlayerService(_repoMock.Object, logger);
        }

        [Fact]
        public async Task GetByIdAsync_ValidId_ReturnsSamePlayer()
        {
            // arrange
            var id = Guid.NewGuid();
            var player = new Player { Id = id };
            _repoMock.Setup(r => r.GetByIdAsync(id))
                     .ReturnsAsync(player);

            // act
            var result = await _service.GetByIdAsync(id);

            // assert
            Assert.Same(player, result);
            _repoMock.Verify(r => r.GetByIdAsync(id), Times.Once);
        }

        [Fact]
        public async Task GetByEmailAsync_ValidEmail_ReturnsSamePlayer()
        {
            // arrange
            const string email = "kefilwe@gmail.com";
            var player = new Player { Email = email };
            _repoMock.Setup(r => r.GetByEmailAsync(email))
                     .ReturnsAsync(player);

            // act
            var result = await _service.GetByEmailAsync(email);

            // assert
            Assert.Same(player, result);
            _repoMock.Verify(r => r.GetByEmailAsync(email), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_NewPlayer_CallsAddAndReturnsPlayer()
        {
            // arrange
            var player = new Player { Id = Guid.NewGuid(), Email = "kay@gmail.com" };

            // act
            var result = await _service.CreateAsync(player);

            // assert
            Assert.Same(player, result);
            _repoMock.Verify(r => r.AddAsync(player), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ExistingPlayer_CallsUpdateAndReturnsPlayer()
        {
            // arrange
            var player = new Player { Id = Guid.NewGuid() };

            // act
            var result = await _service.UpdateAsync(player);

            // assert
            Assert.Same(player, result);
            _repoMock.Verify(r => r.UpdateAsync(player), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ValidId_CallsDelete()
        {
            // arrange
            var id = Guid.NewGuid();

            // act
            await _service.DeleteAsync(id);

            // assert
            _repoMock.Verify(r => r.DeleteAsync(id), Times.Once);
        }
    }
}
