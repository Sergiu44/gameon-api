using Infrastructure.Context;
using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Infrastructure
{
    public class UnitOfWork
    {
        private readonly wbookdbContext Context;

        public UnitOfWork(wbookdbContext context)
        {
            Context = context;
        }

        private IRepository<User> users;
        public IRepository<User> Users => users ?? (users = new BaseRepository<User>(Context));

        public IRepository<Game> games;
        public IRepository<Game> Games => games ?? (games = new BaseRepository<Game>(Context));
        
        public IRepository<GameVariant> gameVariants;
        public IRepository<GameVariant> GameVariants => gameVariants ?? (gameVariants = new BaseRepository<GameVariant>(Context));
        
        public IRepository<Bundle> bundles;
        public IRepository<Bundle> Bundles => bundles ?? (bundles = new BaseRepository<Bundle>(Context));

        public IRepository<BasketItem> basketItems;
        public IRepository<BasketItem> BasketItems => basketItems ?? (basketItems = new BaseRepository<BasketItem>(Context));

        public IRepository<WishlistItem> wishlistItems;
        public IRepository<WishlistItem> WishlistItems => wishlistItems ?? (wishlistItems = new BaseRepository<WishlistItem>(Context));

        public IRepository<BundleGameMapping> bundleGameMapping;
        public IRepository<BundleGameMapping> BundleGameMapping => bundleGameMapping ?? (bundleGameMapping = new BaseRepository<BundleGameMapping>(Context));
        public IRepository<Category> categories;
        public IRepository<Category> Categories => categories ?? (categories = new BaseRepository<Category>(Context));
        public void SaveChanges()
        {
            Context.SaveChanges();
        }
    }
}
