using Microsoft.AspNetCore.Mvc;
using BookFreelanceTask.Services;

namespace BookFreelanceTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly BasketService _basketService;

        public BasketController(BasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpGet("{userId}")]
        public IActionResult GetBasket([FromRoute] int userId)
        {
            if (userId <= 0) return BadRequest("Invalid userId");
            var basket = _basketService.GetBasket(userId);
            return Ok(basket);
        }
        [HttpGet("all")]
        public IActionResult GetAllBaskets()
        {
            var baskets = _basketService.GetAllBaskets();

            if (baskets == null || !baskets.Any())
                return NotFound("No baskets found");

            return Ok(baskets);
        }


        [HttpPost("add")]
        public IActionResult AddItem([FromQuery] int userId, [FromQuery] int bookId, [FromQuery] int quantity)
        {
            if (userId <= 0 || bookId <= 0 || quantity <= 0)
                return BadRequest("Invalid input");

            var result = _basketService.AddItem(userId, bookId, quantity);
            return result ? Ok("Item added") : BadRequest("Failed to add item");
        }

        [HttpPost("remove")]
        public IActionResult RemoveItem([FromQuery] int userId, [FromQuery] int bookId, [FromQuery] int quantity)
        {
            if (userId <= 0 || bookId <= 0 || quantity <= 0)
                return BadRequest("Invalid input");

            var result = _basketService.RemoveItem(userId, bookId, quantity);

            return result
                ? Ok("Item removed or quantity decreased")
                : NotFound("Item not found or already removed");
        }


        [HttpPost("confirm")]
        public IActionResult ConfirmReservation([FromQuery] int userId)
        {
            if (userId <= 0)
                return BadRequest("Invalid userId");

            var result = _basketService.ConfirmReservation(userId);
            return result ? Ok("Reservation confirmed and basket cleared") : NotFound("Basket not found");
        }

    }
}
