using StoreApp.Library.Interfaces;

namespace StoreApp.Library.Model
{
    public class OrderService
    {
        private readonly IOrderRepository _orderRepo;

        private readonly ILocationRepository _LocationRepo;

        public OrderService(IOrderRepository orderRepo, ILocationRepository locationRepo)
        {
            _orderRepo = orderRepo;
            _LocationRepo = locationRepo;
        }

        public void PlaceOrder(ShoppingCart cart, Customer customer)
        {
            decimal totalCost = 0;
            foreach(var product in cart.Items.Keys)
            {
                cart.Location.Inventory[product] -= cart.Items[product];
                totalCost += product.Price * cart.Items[product];
            }

            var order = new Order(cart, customer, totalCost);

            _orderRepo.Create(order);
            _LocationRepo.Update(cart.Location);
        }
    }
}
