using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregation;
using Talabat.Core.Repositories;
using Talabat.Core.Services;
using Talabat.Core.Specifications.Order_Specification;

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository basketRepository;
        private readonly IUnitOfWork unitOfWork;

        public OrderService(IBasketRepository basketRepository,IUnitOfWork unitOfWork)
        {
            this.basketRepository = basketRepository;
            this.unitOfWork = unitOfWork;
        }
        public async Task<Order?> CreateOrderAsync(string bayerEmail, string basketId, Address shippingAddress, int DeliveryMethodId)
        {
            

            var basket = await basketRepository.GetBasketAsync(basketId);

          
            var orderItems = new List<OrderItem>();

            if(basket?.Items?.Count > 0)
            {
                foreach(var item in basket.Items)
                {
                    var product = await unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    var productItemOrdered = new ProductOrderItem(product.Id, product.Name, product.PictureUrl);
                    var orderItem = new OrderItem(productItemOrdered, product.Price, item.Quantity);

                    orderItems.Add(orderItem);  
                }
            }

           
            var subTotal = orderItems.Sum(item => item.Price * item.Quantity);

            var deliveryMethod = await unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(DeliveryMethodId);

            var spec = new OrderWithPaymentSpecifications(basket.PaymentIntentId);
            var exsistOrder = await unitOfWork.Repository<Order>().GetByIdWithSpecAsync(spec);

            if(exsistOrder is not null)
            {
                unitOfWork.Repository<Order>().Delete(exsistOrder);
            }

            var Order = new Order(bayerEmail, shippingAddress,basket.PaymentIntentId ,deliveryMethod, orderItems, subTotal);

            await unitOfWork.Repository<Order>().Add(Order);//Local

            var result  = await unitOfWork.Complete();
            if (result <= 0) return null; 
            return Order;
        }


        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new OrderSpecifications(buyerEmail);
            var Orders = await unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);

            return Orders;
        }


        public async Task<Order> GetOrderByIdForUserAsync(int orderId, string buyerEmail)
        {
            var spec = new OrderSpecifications(orderId, buyerEmail);
            var order = await unitOfWork.Repository<Order>().GetByIdWithSpecAsync(spec);

            return order;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            var deliveryMethods = await unitOfWork.Repository<DeliveryMethod>().GetAllAsync();

            return deliveryMethods;
        }
    }
}
