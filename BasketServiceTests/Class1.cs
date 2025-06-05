using BookFreelanceTask.Services;
using Xunit;

namespace BasketServiceTests
{
    public class Class1
    {
        private readonly BasketService _basketService = new BasketService();

        [Fact]
        public void AddItem_ShouldAddItemToBasket()
        {
            int userId = 1;
            int bookId = 101;

            _basketService.ConfirmReservation(userId);

            var result = _basketService.AddItem(userId, bookId, 2);

            var basket = _basketService.GetBasket(userId);

            Assert.True(result);
            Assert.Single(basket);
            Assert.Equal(bookId, basket[0].BookId);
            Assert.Equal(2, basket[0].Quantity);
        }

        [Fact]
        public void RemoveItem_ShouldDecreaseQuantityOrRemoveItem()
        {
            int userId = 2;
            int bookId = 102;

            _basketService.ConfirmReservation(userId);

            _basketService.AddItem(userId, bookId, 3);

            var result1 = _basketService.RemoveItem(userId, bookId, 1);
            Assert.True(result1);

            var basket = _basketService.GetBasket(userId);
            Assert.Single(basket);
            Assert.Equal(2, basket[0].Quantity);

            var result2 = _basketService.RemoveItem(userId, bookId, 2);
            Assert.True(result2);

            basket = _basketService.GetBasket(userId);
            Assert.Empty(basket);
        }


        [Fact]
        public void ConfirmReservation_ShouldClearBasket()
        {
            int userId = 3;
            int bookId = 103;

            _basketService.AddItem(userId, bookId, 1);
            _basketService.ConfirmReservation(userId);

            var basket = _basketService.GetBasket(userId);
            Assert.Empty(basket);
        }

        [Fact]
        public void AddItem_InvalidQuantity_ShouldReturnFalse()
        {
            var result = _basketService.AddItem(1, 1, -1);
            Assert.False(result);
        }
    }
}
