
using Microsoft.Azure.Documents.Client;
using System.Threading.Tasks;

namespace Eauction_Seller_API.DataAccess.Utility
{
    public interface ICosmosConnection
    {
        Task<DocumentClient> InitializeAsync(string collectionId);
    }
}
