using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DataLayer
{
    public class ClientContext : IDb<Client, int>
    {
        private readonly ReservationsDbContext _dbContext;
        public ClientContext(ReservationsDbContext context)
        {
            _dbContext = context;
        }
        public void Create(Client entity, bool useNavigationalProperties)
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

            _dbContext.Clients.Add(entity);
            _dbContext.SaveChanges();
        }

        public void Delete(int key)
        {
            Client client = _dbContext.Clients.Find(key);

            if (client is null)
            {
                throw new NullReferenceException("Object doesn't exist");
            }

            _dbContext.Clients.Remove(client);
            _dbContext.SaveChanges();   
        }

        public Client Read(int key, bool useNavigationalProperties, bool isReadOnlyTrue)
        {
            IQueryable <Client> clients = _dbContext.Clients;

            if (useNavigationalProperties)
            {
                clients = clients.Include(c => c.Reservations);
            }
            if (isReadOnlyTrue)
            {
                clients.AsNoTrackingWithIdentityResolution();
            }

            return clients.FirstOrDefault(c => c.Id==key);
        }

        public List<Client> ReadAll(bool useNavigationalProperties, bool isReadOnlyTrue)
        {
            IQueryable<Client> clients = _dbContext.Clients;

            if (useNavigationalProperties)
            {
                clients = clients.Include(c => c.Reservations);
            }
            if (isReadOnlyTrue)
            {
                clients.AsNoTrackingWithIdentityResolution();
            }

            return clients.ToList();
        }

        public void Update(int key, Client entity, bool useNavigationalProperties)
        {
            Client client = _dbContext.Clients.Find(key);

            if (client is null)
            {
                throw new NullReferenceException("Object doesn't exist");
            }

            _dbContext.Clients.Entry(client).CurrentValues.SetValues(entity);

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
                client.Reservations = entity.Reservations;
            }

            _dbContext.SaveChanges();
        }
    }
}
