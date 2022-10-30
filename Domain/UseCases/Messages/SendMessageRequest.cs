using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
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
        public MessageEntity MessageSent { get; set; }
    }

    public class MessageSentModelResponse
    {
        public bool Sucess { get; set; }
    }

    public class SendMessageRequestHandler : IRequestHandler<SendMessageRequest, MessageSentModelResponse>
    {
        private readonly IMessageService messageService;
        private readonly IProfileService profileService;

        public SendMessageRequestHandler(IMessageService messageService, IProfileService profileService)
        {
            this.messageService = messageService;
            this.profileService = profileService;
        }

        public async Task<MessageSentModelResponse> Handle(SendMessageRequest request, CancellationToken cancellationToken)
        {
            ValidateRequest(request);
            var message = request.MessageSent;
            message.Sent = true;
            message.Status = MessageStatus.Sent;
            await messageService.Insert(message);

            var profileSender = await profileService.Get(request.MessageSent.MyNick);
            
            if (profileSender is null || string.IsNullOrEmpty(profileSender.Id))
                throw new ArgumentException("Perfil do enviador não localizado");

            var profileReceiver = await profileService.Get(request.MessageSent.FriendNick);

            if (profileReceiver is null || string.IsNullOrEmpty(profileReceiver.Id))
                throw new ArgumentException("Perfil do receptor não localizado");

            return new MessageSentModelResponse { Sucess = true };
        }

        private void ValidateRequest(SendMessageRequest request)
        {
            if (request.MessageSent is null)
                throw new ArgumentException("Mensagem não localizada");

            if (string.IsNullOrEmpty(request.MessageSent.MyNick))
                throw new ArgumentException("MyNick precisa ser diferente de vazio e de nulo");

            if (string.IsNullOrEmpty(request.MessageSent.FriendNick))
                throw new ArgumentException("FriendNick precisa ser diferente de vazio e de nulo");

            if (string.IsNullOrEmpty(request.MessageSent.Message))
                throw new ArgumentException("MessageText precisa ser diferente de vazio e de nulo");
        }
    }
}
