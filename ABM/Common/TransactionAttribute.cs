using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Transactions;

namespace ABM.Common
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class TransactionAttribute : Attribute
    {
        public string Propagation { get; set; } = "Required";  // Propagación de la transacción
        public string RollbackFor { get; set; } = "Exception"; // Qué excepciones deben hacer rollback

        // Puedes agregar más propiedades, como aislamiento, etc.
    }
}
