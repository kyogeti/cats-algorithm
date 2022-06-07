using System;
using CatAlg.Domain.Interfaces;
using CatAlg.Domain.Models;

namespace CatAlg.Domain.Repository
{
    public class CatRepository : ICatRepository
    {
        private readonly IWillSmithDatabase _database;
        public CatRepository(IWillSmithDatabase database)
        {
            _database = database;
        }
        public void SaveCatStatus(Cat cat)
        {
            _database.Save(cat);
        }
    }

    public interface IWillSmithDatabase
    {
        bool Save<T>(T entity);
    }

    public class WillSmithDatabase : IWillSmithDatabase
    {
        public bool Save<T>(T entity)
        {
            Console.WriteLine("Confia");
            return true;
        }

        /// <summary>
        /// Gera uma instância em memória do banco de dados sim, confia.
        /// </summary>
        /// <returns></returns>
        public static WillSmithDatabase GetInMemoryDatabase() => new();
    }
}