using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Services;

namespace Talabat.APIs.Controllers
{
 
    public class PaymentsController : ApiBaseController
    {
        private readonly IPaymentService paymentService;
        private readonly IMapper mapper;
        private readonly ILogger<PaymentsController> logger;
        private const string _whSercert = "whsec_63b41ddf4a80d5147de59849e26e11fb9a304b3925271482aff600b539ffe7c4";


        public PaymentsController(IPaymentService paymentService,IMapper mapper,ILogger<PaymentsController> logger)
        {
            this.paymentService = paymentService;
            this.mapper = mapper;
            this.logger = logger;
        }

        [HttpPost("{basketId}")]//api/Payments
        public async Task<ActionResult<CustomerBasketDto>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await paymentService.CreateOrUpdatePaymentIntent(basketId);
            if (basket is null) return BadRequest(new ApiErrorResponse(400, "A Problem With Your Basket"));

            var mappedbasket = mapper.Map<CustomerBasket, CustomerBasketDto>(basket);

            return Ok(mappedbasket);


        }
        [HttpPost("webhook")]//api/payments/Webhook
        public async Task<IActionResult> StripeWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], _whSercert);

                var paymentIntent = (PaymentIntent)stripeEvent.Data.Object;

            switch (stripeEvent.Type)
            {
                case Events.PaymentIntentSucceeded:
                    await paymentService.UpdatePaymentIntentToSucceedOrFaild(paymentIntent.Id, true);
                    logger.LogInformation("Payment is Successed ya Hamada",paymentIntent.Id);
                    break;
                case Events.PaymentIntentPaymentFailed:
                    await paymentService.UpdatePaymentIntentToSucceedOrFaild(paymentIntent.Id, false);
                    logger.LogInformation("Payment is Failed ya Hamada :(", paymentIntent.Id);
                    break;


            }
                return Ok();
            
          
        }
    }
}
