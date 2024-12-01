using Atelier_2.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Atelier_2.Models.Repositories
{
    public class ClientRepository : IClientRepository
    {
        readonly AppDbContext context;

        public ClientRepository(AppDbContext context)
        {
            this.context = context;
        }

        // Get all clients, ordered by first name
        public IList<Client> GetAll()
        {
            return context.Clients.OrderBy(x => x.ClientFirstName).ToList();
        }

        // Get a client by ID
        public Client GetById(int id)
        {
            return context.Clients.SingleOrDefault(x => x.ClientId == id);
        }

        // Add a new client
        public void Add(Client client)
        {
            context.Clients.Add(client);
            context.SaveChanges();
        }

        // Edit an existing client
        public void Edit(Client client)
        {
            // Retrieve the existing client from the database
            var existingClient = context.Clients.Find(client.ClientId);

            // Check if the client exists
            if (existingClient != null)
            {
                // Update the properties of the existing client
                existingClient.ClientFirstName = client.ClientFirstName;
                existingClient.ClientLastName = client.ClientLastName;

                // Save the changes to the context
                context.SaveChanges();
            }
        }

        // Delete a client
        public void Delete(Client client)
        {
            var existingClient = context.Clients.Find(client.ClientId);

            if (existingClient != null)
            {
                context.Clients.Remove(existingClient);
                context.SaveChanges();
            }
        }

        // Find clients by first name
        public IList<Client> FindByName(string name)
        {
            return context.Clients
                .Where(c => EF.Functions.Like(c.ClientFirstName, $"%{name}%"))
                .ToList();
        }
    }
}
