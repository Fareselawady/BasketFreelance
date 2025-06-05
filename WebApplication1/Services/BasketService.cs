using WebApplication1.Models;
using BookFreelanceTask.Services;
using System.Collections.Concurrent;

namespace BookFreelanceTask.Services
{
    public class BasketService
    {
        private readonly ConcurrentDictionary<int, List<BasketItem>> _baskets = new();

        public BasketService()
        {
            _baskets[1] = new List<BasketItem>
        {
            new BasketItem { BookId = 101, Quantity = 2 },
            new BasketItem { BookId = 102, Quantity = 1 }
        };

            _baskets[2] = new List<BasketItem>
        {
            new BasketItem { BookId = 103, Quantity = 3 }
        };
        }


        public Dictionary<int, List<BasketItem>> GetAllBaskets()
        {
            return _baskets.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToList());
        }


        public List<BasketItem> GetBasket(int userId)
        {
            _baskets.TryGetValue(userId, out var basket);
            return basket ?? new List<BasketItem>();
        }

        public bool AddItem(int userId, int bookId, int quantity)
        {
            if (quantity <= 0) return false;

            var basket = _baskets.GetOrAdd(userId, _ => new List<BasketItem>());
            var item = basket.FirstOrDefault(x => x.BookId == bookId);

            if (item != null)
            {
                item.Quantity += quantity;
            }
            else
            {
                basket.Add(new BasketItem { BookId = bookId, Quantity = quantity });
            }

            return true;
        }

        public bool RemoveItem(int userId, int bookId, int quantity)
        {
            if (!_baskets.TryGetValue(userId, out var basket)) return false;

            var item = basket.FirstOrDefault(x => x.BookId == bookId);
            if (item == null) return false;

            item.Quantity -= quantity;
            if (item.Quantity <= 0)
            {
                basket.Remove(item);  
            }

            return true;
        }




        public bool ConfirmReservation(int userId)
        {
            return _baskets.TryRemove(userId, out _);
        }
    }
}
