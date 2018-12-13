using Bowgum;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Bowgum.Models {
    public class BooksDbContext {
        private readonly IMongoDatabase _database = null;

        public BooksDbContext(IOptions<AppSettings> settings) {
            var client = new MongoClient(settings.Value.MongoDBConnectionString);
            if (client != null) {
                _database = client.GetDatabase(settings.Value.MongoDatabaseName);
            }
        }
    }
}