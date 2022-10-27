using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.UseCases.Messages
{
    public class SendMessageRequest : IRequest<MessageSentModelResponse>
    {
        public Message MessageSent { get; set; }
    }

    public class MessageSentModelResponse
    {
        public bool Sucess { get; set; }
    }

    public class SendMessageRequestHandler : IRequestHandler<SendMessageRequest, MessageSentModelResponse>
    {
        public SendMessageRequestHandler()
        {

        }

        public Task<MessageSentModelResponse> Handle(SendMessageRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
