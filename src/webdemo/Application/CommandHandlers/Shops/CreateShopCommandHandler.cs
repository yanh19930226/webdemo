using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using webdemo.Application.Commands.Shops;

namespace webdemo.Application.CommandHandlers.Shops
{
    public class CreateShopCommandHandler : INotificationHandler<CreateShopCommand>
    {
        public Task Handle(CreateShopCommand notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
