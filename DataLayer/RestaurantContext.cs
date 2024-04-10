using BusinessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    internal class RestaurantContext : IDb<Restaurant, string>
    {

        private readonly ReservationsDbContext _dbContext;
        public RestaurantContext(ReservationsDbContext context)
        {
            _dbContext = context;
        }
        public void Create(Restaurant entity, bool useNavigationalProperties)
        {
            if (useNavigationalProperties)
            {
                ReservationContext reservationContext = new ReservationContext(_dbContext);
                Reservation current;

                for (int i = 0; i < entity.Reservations.Count; i++)
                {
                     current = _dbContext.Reservations.Find(entity.Reservations[i].Id);
                    if (current is not null)
                    {
                        entity.Reservations[i] = current;
                    }
                    else
                    {                   
                        reservationContext.Create(entity.Reservations[i], true);
                    }
                }
            }

            _dbContext.Restaurants.Add(entity);
            _dbContext.SaveChanges();
        }

        public void Delete(string key)
        {
            Restaurant restaurant = _dbContext.Restaurants.Find(key);

            if (restaurant is null)
            {
                throw new NullReferenceException("Object doesn't exist");
            }

            _dbContext.Restaurants.Remove(restaurant);
            _dbContext.SaveChanges();
        }

        public Restaurant Read(string key, bool useNavigationalProperties, bool isReadOnlyTrue)
        {
            IQueryable<Restaurant> restaurants = _dbContext.Restaurants;

            if (useNavigationalProperties)
            {
                restaurants = restaurants.Include(c => c.Reservations);
            }
            if (isReadOnlyTrue)
            {
                restaurants.AsNoTrackingWithIdentityResolution();
            }

            return restaurants.FirstOrDefault(r => r.Name == key);
        }

        public List<Restaurant> ReadAll(bool useNavigationalProperties, bool isReadOnlyTrue)
        {
            IQueryable<Restaurant> restaurants = _dbContext.Restaurants;

            if (useNavigationalProperties)
            {
                restaurants = restaurants.Include(c => c.Reservations);
            }
            if (isReadOnlyTrue)
            {
                restaurants.AsNoTrackingWithIdentityResolution();
            }

            return restaurants.ToList();
        }

        public void Update(string key, Restaurant entity, bool useNavigationalProperties)
        {
            Restaurant restaurant = _dbContext.Restaurants.Find(key);

            if (restaurant is null)
            {
                throw new NullReferenceException("Object doesn't exist");
            }

            _dbContext.Restaurants.Entry(restaurant).CurrentValues.SetValues(entity);

            if (useNavigationalProperties)
            {
                ReservationContext reservationContext = new ReservationContext(_dbContext);
                Reservation current;

                for (int i = 0; i < entity.Reservations.Count; i++)
                {
                    current = _dbContext.Reservations.Find(entity.Reservations[i].Id);
                    if (current is not null)
                    {
                        entity.Reservations[i] = current;
                    }
                    else
                    {
                        reservationContext.Create(entity.Reservations[i], true);
                    }
                }
                restaurant.Reservations = entity.Reservations;
            }

            _dbContext.SaveChanges();
        }
    }
}
