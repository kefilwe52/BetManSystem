using BetManSystem.Application.Services;
using BetManSystem.Common.Entities;
using BetManSystem.DataAccess.IRepositories;
using Microsoft.Extensions.Logging;
using Moq;

namespace BetManSystem.Test
{
    public class MessageLogServiceTests
    {
        private readonly Mock<IMessageLogRepository> _repoMock;
        private readonly Mock<ILogger<MessageLogService>> _loggerMock;
        private readonly MessageLogService _service;

        public MessageLogServiceTests()
        {
            _repoMock = new Mock<IMessageLogRepository>();
            _loggerMock = new Mock<ILogger<MessageLogService>>();
            _service = new MessageLogService(_repoMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task LogAsync_SetsTimestamp_WhenDefault()
        {
            // arrange
            var entry = new MessageTransmissionLog();
            MessageTransmissionLog captured = null;
            _repoMock
                .Setup(r => r.LogAsync(It.IsAny<MessageTransmissionLog>()))
                .Callback<MessageTransmissionLog>(e => captured = e)
                .Returns(Task.CompletedTask);

            // act
            await _service.LogAsync(entry);

            // assert
            Assert.NotNull(captured);
            Assert.NotEqual(default, captured!.Timestamp);
            _repoMock.Verify(r => r.LogAsync(captured!), Times.Once);
        }

        [Fact]
        public async Task LogAsync_DoesNotThrowAndLogsError_WhenRepoThrows()
        {
            // arrange
            var entry = new MessageTransmissionLog { Timestamp = DateTime.Now };
            var repoEx = new Exception("Database failure");
            _repoMock
                .Setup(r => r.LogAsync(It.IsAny<MessageTransmissionLog>()))
                .ThrowsAsync(repoEx);

            // act & assert
            var ex = await Record.ExceptionAsync(() => _service.LogAsync(entry));
            Assert.Null(ex);

            // verify repo was called once
            _repoMock.Verify(r => r.LogAsync(entry), Times.Once);

            // verify that the error is loged
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Failed to add MessageTransmissionLog")),
                    repoEx,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}
