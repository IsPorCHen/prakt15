using ElectronicsShop.Models;

namespace ElectronicsShop.Services
{
    public class DBService
    {
        private ElectronicsShopDbContext context;
        public ElectronicsShopDbContext Context => context;

        private static DBService? instance;
        public static DBService Instance
        {
            get
            {
                if (instance == null)
                    instance = new DBService();
                return instance;
            }
        }

        private DBService()
        {
            context = new ElectronicsShopDbContext();
        }
    }
}