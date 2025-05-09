using ST10263027_PROG7311_POE.Models;
using ST10263027_PROG7311_POE.Repository;

namespace ST10263027_PROG7311_POE.Services
{
    public class FarmerService
    {
        private readonly FarmerRepository _farmerRepository;

        public FarmerService(FarmerRepository farmerRepository)
        {
            _farmerRepository = farmerRepository;
        }

       
        public Farmer FarmerLogin(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Username and password cannot be empty.");
            }

            Farmer farmer = _farmerRepository.GetFarmerByUsernameAndPassword(username, password);

            if (farmer == null)
            {
                throw new UnauthorizedAccessException("Invalid username or password.");
            }

            return farmer;
        }
        // Add to FarmerService class
        public void AddProduct(Product product)
        {
            // Ensure FarmerId exists
            if (!product.FarmerId.HasValue)
            {
                throw new ArgumentException("Product must be associated with a farmer");
            }

            _farmerRepository.AddProduct(product);
        }

        public List<Product> GetFarmerProducts(int farmerId)
        {
            return _farmerRepository.GetProductsByFarmerId(farmerId);
        }
    }
}
