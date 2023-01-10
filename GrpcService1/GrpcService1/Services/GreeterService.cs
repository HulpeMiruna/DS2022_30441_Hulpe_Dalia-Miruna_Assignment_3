using Grpc.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcService1
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<MessageReply> SendMessage(MessageRequest request, ServerCallContext context)
        {
            return Task.FromResult(new MessageReply
            {
                Id = request.Id,
                Message = request.Message
            });
        }
    }
}
