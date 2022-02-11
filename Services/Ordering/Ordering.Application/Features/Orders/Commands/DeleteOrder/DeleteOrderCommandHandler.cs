using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persisitence;
using Ordering.Application.Exceptions;
using Ordering.Domain.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Features.Orders.Commands.DeleteOrder
{
    public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<DeleteOrderCommand> _logger;

        public DeleteOrderCommandHandler(IOrderRepository orderRepository, ILogger<DeleteOrderCommand> logger)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var toBeDeletedOrder = await _orderRepository.GetByIdAsync(request.Id);

                if (toBeDeletedOrder is null)
                {
                    _logger.LogError($"order with id:{request.Id} does not exist.");
                    throw new NotFoundException(nameof(Order), request.Id);
                }

                await _orderRepository.DeleteAsync(toBeDeletedOrder);

                _logger.LogInformation($"order with id {request.Id} successfully deleted.");

            }
            catch (Exception ex)
            {
                _logger.LogInformation($"there was an exception while deleting Order with id {request.Id} read more {ex}.");
            }
            return true;
        }
    }
}
