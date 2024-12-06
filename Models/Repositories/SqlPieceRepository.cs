using Atelier_2.Context;
using Atelier_2.Models;
using Microsoft.EntityFrameworkCore;

namespace Atelier_2.Models.Repositories
{
    public class SqlPieceRepository : IRepository<Piece>
    {
        private readonly AppDbContext context;
        public SqlPieceRepository(AppDbContext context)
        {
            this.context = context;
        }
        public Piece Add(Piece P)
        {
            context.Pieces.Add(P);
            context.SaveChanges();
            return P;
        }
        public Piece Delete(int Id)
        {
            Piece P = context.Pieces.Find(Id);
            if (P != null)
            {
                context.Pieces.Remove(P);
                context.SaveChanges();
            }
            return P;
        }
        public IEnumerable<Piece> GetAll()
        {
            return context.Pieces;
        }
        public Piece Get(int Id)

        {
            return context.Pieces.Find(Id);
        }
        public Piece Update(Piece P)
        {
            var Piece =
            context.Pieces.Attach(P);
            Piece.State = EntityState.Modified;
            context.SaveChanges();
            return P;
        }
    }
}

