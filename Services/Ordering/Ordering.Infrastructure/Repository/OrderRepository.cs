using Microsoft.EntityFrameworkCore;
using Ordering.Application.Contracts.Persisitence;
using Ordering.Domain.Entites;
using Ordering.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Repository
{
    public class OrderRepository : BaseRepository<Order>, IOrderRepository
    {
        public OrderRepository(OrderContext dbContext) : base(dbContext)
        {

        }

        public async Task<IEnumerable<Order>> GetOrdersByUsername(string username)
        {
            return await _orderContext.Orders.Where(o => o.Username == username).ToListAsync();
        }
    }
}
