using CustomerMicroServices.OrdersApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace CustomerMicroServices.OrdersApi.Persistence
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrdersContext _context;

        public OrderRepository(OrdersContext context)
        {
            _context = context;
        }

        public async Task<Order> GetOrderAsync(Guid id)
        {
            return await _context.Orders
                .Include("OrderDetails")
                .FirstOrDefaultAsync(o => o.OrderId == id);
        }

        public async Task RegisterOrder(Order order)
        {
            _context.Add(order);
            await _context.SaveChangesAsync();
        }
    }
}