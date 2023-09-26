using Fiap.Web.AspNet.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Globalization;

namespace Fiap.Web.AspNet.Repository
{
    public class ClienteRepository
    {

        private string? connectionString;
        private OracleConnection? connection;

        public ClienteRepository()
        {
            connectionString = new ConfigurationBuilder()
                                        .SetBasePath(Directory.GetCurrentDirectory())
                                        .AddJsonFile("appsettings.json")
                                        .Build().GetConnectionString("DatabaseConnection");

            connection = new OracleConnection(connectionString);
        }


        public IList<ClienteModel> Listar()
        {
            var lista = new List<ClienteModel>();

            using (connection)
            {
                var query = "SELECT C.CLIENTEID, C.NOME, C.OBSERVACAO, C.DATANASCIMENTO, C.REPRESENTANTEID, R.NOMEREPRESENTANTE FROM CLIENTE C INNER JOIN REPRESENTANTE R ON R.REPRESENTANTEID = C.REPRESENTANTEID";

                connection.Open();

                OracleCommand command = new OracleCommand(query, connection);
                OracleDataReader dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    // Recupera os dados
                    ClienteModel cliente = new ClienteModel();
                    cliente.ClienteId = Convert.ToInt32(dataReader["ClienteId"]);
                    cliente.Nome = dataReader["Nome"].ToString();
                    cliente.Observacao = dataReader["Observacao"].ToString();
                    cliente.DataNascimento = DateTime.ParseExact(dataReader["DataNascimento"].ToString(), "dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture); ;
                    cliente.RepresentanteId = Convert.ToInt32(dataReader["RepresentanteId"]);
                    cliente.Representante = new RepresentanteModel(Convert.ToInt32(dataReader["ClienteId"]), dataReader["NOMEREPRESENTANTE"].ToString());


                    // Adiciona o modelo da lista
                    lista.Add(cliente);
                }

                connection.Close();

            } // Finaliza o objeto connection

            // Retorna a lista
            return lista;

        }


        public ClienteModel Consultar(int id)
        {

            var cliente = new ClienteModel();

            using (connection)
            {
                var query = "SELECT C.CLIENTEID, C.NOME, C.OBSERVACAO, C.DATANASCIMENTO, C.REPRESENTANTEID, R.NOMEREPRESENTANTE FROM CLIENTE C INNER JOIN REPRESENTANTE R ON R.REPRESENTANTEID = C.REPRESENTANTEID WHERE CLIENTEID = :ID";

                connection.Open();

                OracleCommand command = new OracleCommand(query, connection);
                command.Parameters.Add("ID", id);

                OracleDataReader dataReader = command.ExecuteReader();

                if (dataReader.Read())
                {

                    cliente.ClienteId = Convert.ToInt32(dataReader["ClienteId"]);
                    cliente.Nome = dataReader["Nome"].ToString();
                    cliente.Observacao = dataReader["Observacao"].ToString();
                    cliente.DataNascimento = DateTime.ParseExact(dataReader["DataNascimento"].ToString(), "dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture);
                    cliente.RepresentanteId = Convert.ToInt32(dataReader["RepresentanteId"]);
                    cliente.Representante = new RepresentanteModel(Convert.ToInt32(dataReader["ClienteId"]), dataReader["NOMEREPRESENTANTE"].ToString());
                }

                connection.Close();

            } // Finaliza o objeto connection

            return cliente;

        }

        public void Inserir(ClienteModel cliente)
        {
            using (connection)
            {
                String query = 
                    "INSERT INTO CLIENTE ( NOME, DATANASCIMENTO, OBSERVACAO, REPRESENTANTEID )" +
                    " VALUES ( :nome, :data, :obs, :representante ) ";

                connection.Open();

                OracleCommand command = new OracleCommand(query, connection);

                // Adicionando o valor ao comando
                command.Parameters.Add("nome", cliente.Nome);
                command.Parameters.Add("data", cliente.DataNascimento);
                command.Parameters.Add("obs", cliente.Observacao);
                command.Parameters.Add("representante", Convert.ToInt32(cliente.RepresentanteId));

                command.ExecuteNonQuery();
                connection.Close();

            }

        }


        public void Alterar(ClienteModel cliente)
        {
            using (connection)
            {
                String query =
                    "UPDATE CLIENTE SET NOME = :nome , DATANASCIMENTO = :data , OBSERVACAO = :obs, REPRESENTANTEID = :representante WHERE CLIENTEID = :id ";

                connection.Open();

                OracleCommand command = new OracleCommand(query, connection);

                // Adicionando o valor ao comando
                command.Parameters.Add("nome", cliente.Nome);
                command.Parameters.Add("data", cliente.DataNascimento);
                command.Parameters.Add("obs", cliente.Observacao);
                command.Parameters.Add("representante", Convert.ToInt32(cliente.RepresentanteId));
                command.Parameters.Add("id", cliente.ClienteId);

                command.ExecuteNonQuery();
                connection.Close();

            }

        }

        public void Excluir(int id)
        {
            using (connection)
            {
                String query = "DELETE CLIENTE WHERE CLIENTE = :id ";

                connection.Open();

                OracleCommand command = new OracleCommand(query, connection);

                // Adicionando o valor ao comando
                command.Parameters.Add("id", id);


                command.ExecuteNonQuery();
                connection.Close();

            }
        }

    }
}
