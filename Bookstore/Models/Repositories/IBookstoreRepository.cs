namespace Bookstore.Models.Repositories
{
    public interface IBookstoreRepository <TEntity>
    {
        IList<TEntity> List(); //GetAll ();
        TEntity Find(int id); // Get(int id);
        void Add(TEntity entity);
        void Update(int id,TEntity entity);
        void Delete(int id);
        List<TEntity> Search(string term);

    }
}
