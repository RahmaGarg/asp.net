namespace Atelier_2.Models.Repositories
{
    public interface IClientRepository
    {
        IList<Client> GetAll();
        Client GetById(int id);
        void Add(Client c);
        void Edit(Client c);
        void Delete(Client c);
        IList<Client > FindByName(string name);
    }
}
