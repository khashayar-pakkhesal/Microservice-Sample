using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.infrastructure;
using Ordering.Application.Contracts.Persisitence;
using Ordering.Application.Models;
using Ordering.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.CheckoutOrder
{
    public class CheckoutOrderCommandHandler : IRequestHandler<CheckoutOrderCommand, int>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IEmailSender _emailSender;
        private readonly ILogger<CheckoutOrderCommand> _logger;
        private readonly IMapper _mapper;

        public CheckoutOrderCommandHandler(IMapper mapper, ILogger<CheckoutOrderCommand> logger, IEmailSender emailSender, IOrderRepository orderRepository)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        }

        public async Task<int> Handle(CheckoutOrderCommand request, CancellationToken cancellationToken)
        {
            var newOrder = await _orderRepository.AddAsync(_mapper.Map<Order>(request));

            _logger.LogInformation($"Order {newOrder.Id} is Successfully created.");

            await SendMail(newOrder);

            return newOrder.Id;
        }

        private async Task SendMail(Order order)
        {
            var email = new Email { To = "kh.p1378@gmail.com", Body = $"Order creation {order.Id}", Subject = $"order { order.Username}" };
            try
            {
                await _emailSender.SendMail(email);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Could not send email {ex}");
            }
        }
    }
}
