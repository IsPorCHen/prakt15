using ElectronicsShop.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectronicsShop.Services
{
    public class DBService
    {
        private static DBService? instance;
        public static DBService Instance => instance ??= new DBService();

        public ElectronicsShopDbContext Context { get; private set; }

        private DBService()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ElectronicsShopDbContext>();
            var options = optionsBuilder
                .UseSqlServer("Server=IPCH-NOTEBOOK\\IPCHSERVER;Database=ElectronicsShopDB;Trusted_Connection=True;TrustServerCertificate=True;")
                .Options;

            Context = new ElectronicsShopDbContext(options);
        }
    }
}