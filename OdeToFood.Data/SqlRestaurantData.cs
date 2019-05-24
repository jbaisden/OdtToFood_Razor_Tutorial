using System.Collections.Generic;
using OdeToFood.Core;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace OdeToFood.Data
{
    public class SqlRestaurantData : IRestaurantData
    {
        private readonly OdeToFoodDbContext db;

        public SqlRestaurantData(OdeToFoodDbContext db)
        {
            this.db = db;
        }

        public Restaurant Add(Restaurant restaurant)
        {
            db.Add(restaurant);
            return restaurant;
        }

        public int Commit()
        {
            return db.SaveChanges();
        }

        public Restaurant Delete(int id)
        {
            var rest = GetById(id);
            if (rest != null)
                db.Restaurants.Remove(rest);

            return rest;
        }

        public Restaurant GetById(int restaurantId)
        {
            return db.Restaurants.Find(restaurantId);
        }

        public int GetCountOfRestaurants()
        {
            return db.Restaurants.Count();
        }

        public IEnumerable<Restaurant> GetRestaurantsByName(string name)
        {
            var qry = from r in db.Restaurants
                      where r.Name.StartsWith(name) || string.IsNullOrEmpty(name)
                      orderby r.Name
                      select r;
            return qry;
        }

        public Restaurant Save(Restaurant restaurant)
        {
            if (restaurant.Id > 0)
                return Update(restaurant);
            else
                return Add(restaurant);
        }

        public Restaurant Update(Restaurant updatedRestaurant)
        {
            var entity = db.Restaurants.Attach(updatedRestaurant);
            entity.State = EntityState.Modified;
            return updatedRestaurant;
        }
    }
}
