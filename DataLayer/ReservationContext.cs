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
    public class ReservationContext : IDb<Reservation, int>
    {
        private readonly ReservationsDbContext _dbContext;
        public ReservationContext(ReservationsDbContext context)
        {
            _dbContext = context;   
        }
        public void Create(Reservation entity, bool useNavigationalProperties)
        {
            if (useNavigationalProperties)
            {
                Restaurant restaurant = _dbContext.Restaurants.Find(entity.Restaurant.Name);
                Client client = _dbContext.Clients.Find(entity.Client.Id);

                if (restaurant is not null)
                {
                    entity.Restaurant = restaurant;
                }
                else if (restaurant is null)
                {
                    RestaurantContext restaurantContext = new RestaurantContext(_dbContext);
                    restaurantContext.Create(entity.Restaurant,true);
                }
                if (client is not null)
                {
                    entity.Client = client;
                }
                else if (client is null)
                {
                    ClientContext clientContext = new ClientContext(_dbContext);
                    clientContext.Create(entity.Client, true);
                }
            }

            _dbContext.Reservations.Add(entity);
            _dbContext.SaveChanges();
        }

        public void Delete(int key)
        {
            Reservation reservation = _dbContext.Reservations.Find(key);

            if (reservation is null)
            {
                throw new NullReferenceException("Object doesn't exist");
            }

            _dbContext.Reservations.Remove(reservation);
            _dbContext.SaveChanges();
        }

        public Reservation Read(int key, bool useNavigationalProperties, bool isReadOnlyTrue)
        {
            IQueryable<Reservation> reservations = _dbContext.Reservations;

            if (useNavigationalProperties)
            {
                reservations = reservations.Include(r => r.Restaurant).Include(r => r.Client);
            }
            if (isReadOnlyTrue)
            {
                reservations.AsNoTrackingWithIdentityResolution();
            }

            return reservations.FirstOrDefault(r => r.Id == key);
        }

        public List<Reservation> ReadAll(bool useNavigationalProperties, bool isReadOnlyTrue)
        {
            IQueryable<Reservation> reservations = _dbContext.Reservations;

            if (useNavigationalProperties)
            {
                reservations = reservations.Include(r => r.Restaurant).Include(r => r.Client);
            }
            if (isReadOnlyTrue)
            {
                reservations.AsNoTrackingWithIdentityResolution();
            }

            return reservations.ToList();
        }

        public void Update(int key, Reservation entity, bool useNavigationalProperties)
        {
            Reservation reservation = _dbContext.Reservations.Find(key);

            if (reservation is null)
            {
                throw new NullReferenceException("Object doesn't exist");
            }

            _dbContext.Reservations.Entry(reservation).CurrentValues.SetValues(entity);

            if (useNavigationalProperties)
            {
                Restaurant restaurant = _dbContext.Restaurants.Find(entity.Restaurant.Name);
                Client client = _dbContext.Clients.Find(entity.Client.Id);

                if (restaurant is not null)
                {
                    entity.Restaurant = restaurant;
                }
                else if (restaurant is null)
                {
                    RestaurantContext restaurantContext = new RestaurantContext(_dbContext);
                    restaurantContext.Create(entity.Restaurant, true);
                }
                if (client is not null)
                {
                    entity.Client = client;
                }
                else if (client is null)
                {
                    ClientContext clientContext = new ClientContext(_dbContext);
                    clientContext.Create(entity.Client, true);
                }

                reservation.Restaurant = entity.Restaurant;
                reservation.Client = entity.Client;
            }

            _dbContext.SaveChanges();
        }
    }
}
