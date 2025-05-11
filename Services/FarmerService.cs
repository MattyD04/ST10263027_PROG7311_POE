using ST10263027_PROG7311_POE.Models;
using ST10263027_PROG7311_POE.Repository;

namespace ST10263027_PROG7311_POE.Services
{
    //This file contains the business logic for the Farmer controller and acting as an intermediary between the controller and repository
    public class FarmerService
    {
        //Variable for accessing the repository
        private readonly FarmerRepository _farmerRepository;

        //Initializes an instance of the FarmerService class
        public FarmerService(FarmerRepository farmerRepository)
        {
            _farmerRepository = farmerRepository;
        }
        //***************************************************************************************//
        //This method handles the business logic for when a farmer logs in
        public Farmer FarmerLogin(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Username and password cannot be empty.");//if a farmer attempts to login without filling in the username and password, this messages displays
            }

            Farmer farmer = _farmerRepository.GetFarmerByUsernameAndPassword(username, password);

            if (farmer == null)
            {
                throw new UnauthorizedAccessException("Invalid username or password.");//if the username or password do not match what is in the database, this message is displayed
            }

            return farmer;
        }
        //***************************************************************************************//
        //This method handles the business logic for when a farmer adds a product to their profile
        public void AddProduct(Product product)
        {
            //Ensures FarmerId exists
            if (!product.FarmerId.HasValue)
            {
                throw new ArgumentException("Product must be associated with a farmer"); //if the farmerId is not associated with the product, this message is displayed
            }

            _farmerRepository.AddProduct(product);
        }
        //***************************************************************************************//
        //this method handles the business logic for displaying all the products that a farmer has added to their profile
        public List<Product> GetFarmerProducts(int farmerId)
        {
            return _farmerRepository.GetProductsByFarmerId(farmerId);
        }
    }
}
//***********************************************End of file*****************************************//