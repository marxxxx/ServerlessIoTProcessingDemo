using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanupTool
{
    public static class Program
    {
        static DocumentClient client;

        public static void Main(string[] args)
        {
            client = new DocumentClient(new Uri("https://netstammtischdemo.documents.azure.com:443/"), "YlHdXA3JtdcVfrU5qr80Jg0GFBSEBdTOTFqj8BeUdmkdIg8Cr2EwgytKVSOlGH70bFimVWziIE7OOkb7891tsQ==");

            Console.WriteLine("Deleting collection telemetry");
            DeleteAllDocumentsInCollection("testdb", "telemetry").Wait();

            Console.WriteLine("Deleting collection analytics");
            DeleteAllDocumentsInCollection("testdb", "analytics").Wait();

        }

        private static async Task DeleteAllDocumentsInCollection(string database, string collection)
        {
            var telemetry = client.CreateDocumentQuery(UriFactory.CreateDocumentCollectionUri(database, collection)).ToList();
            foreach (var t in telemetry)
            {
                await client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(database, collection, t.Id));
                Console.WriteLine($"deleted document {t.Id}");
            }
        }
    }
}
