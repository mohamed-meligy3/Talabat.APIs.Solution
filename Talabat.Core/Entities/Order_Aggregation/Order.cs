using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Aggregation
{
    public class Order : BaseEntity
    {
        public Order()
        {
            
        }
        public Order(string buyerEmail, Address shippingAddress,string IntentId ,DeliveryMethod deliveryMethod, ICollection<OrderItem> items, decimal subTotal)
        {
            BuyerEmail = buyerEmail;
            ShipToAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            PaymentIntentId = IntentId;
            Items = items;
            SubTotal = subTotal;
        }

        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;

        public OrderState Status { get; set; } = OrderState.Pending;
        public Address ShipToAddress { get; set; }

        public DeliveryMethod DeliveryMethod { get; set; }//Navigtional Property One

        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();  //Navigtional Property Many

        public decimal SubTotal { get; set; } 
        //Quantity * Price
        //[NotMapped]
        //public decimal Total => SubTotal + DeliveryMethod.Cost;
        public decimal GetTotal()
            => SubTotal + DeliveryMethod.Cost;

        public string PaymentIntentId { get; set; }
    }
}
