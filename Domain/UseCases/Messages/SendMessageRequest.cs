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
        private readonly IBlockedService blockedService;

        public SendMessageRequestHandler(IMessageService messageService, IProfileService profileService,
            IBlockedService blockedService)
        {
            this.messageService = messageService;
            this.profileService = profileService;
            this.blockedService = blockedService;
        }

        public async Task<MessageSentModelResponse> Handle(SendMessageRequest request, CancellationToken cancellationToken)
        {
            ValidateRequest(request);
            var message = request.MessageSent;
            message.Sent = true;
            message.Status = MessageStatus.Sent;
            var messageSentId = await messageService.Insert(message);

            var profileSender = await profileService.Get(request.MessageSent.MyNick);

            if (profileSender is null || string.IsNullOrEmpty(profileSender.Id))
                throw new ArgumentException("Perfil do remetente não localizado");

            var profileReceiver = await profileService.Get(request.MessageSent.FriendNick);

            if (profileReceiver is null || string.IsNullOrEmpty(profileReceiver.Id))
                throw new ArgumentException("Perfil do destinatario não localizado");

            var blockeds = await blockedService.GetBlockeds(request.MessageSent.FriendNick);

            if (blockeds.BlockedList is not null && blockeds.BlockedList.Any(x => x.Id == request.MessageSent.MyNick))
                throw new ArgumentException("Perfil do remetente bloqueado pelo destinatario");

            await messageService.Insert(BuildFriendMessage(message));
            await messageService.UpdateStatus(messageSentId, (int)MessageStatus.Deliveried);

            return new MessageSentModelResponse { Sucess = true };
        }

        private MessageEntity BuildFriendMessage(MessageEntity messageEntity)
        {
            return new MessageEntity
            {
                MyNick = messageEntity.FriendNick,
                FriendNick = messageEntity.MyNick,
                Message = messageEntity.Message,
                Date = messageEntity.Date,
                Sent = !messageEntity.Sent,
                Status = MessageStatus.Deliveried
            };
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

            if (request.MessageSent.MyNick == request.MessageSent.FriendNick)
                throw new ArgumentException("Não é possível enviar mensagem pra você mesmo");
        }
    }
}
