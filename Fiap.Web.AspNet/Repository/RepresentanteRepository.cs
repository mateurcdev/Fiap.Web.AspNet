using Fiap.Web.AspNet.Models;
using Oracle.ManagedDataAccess.Client;

namespace Fiap.Web.AspNet.Repository;

public class RepresentanteRepository
{

    private string? connectionString;
    private OracleConnection? connection;

    public RepresentanteRepository()
    {
        connectionString = new ConfigurationBuilder()
                                    .SetBasePath(Directory.GetCurrentDirectory())
                                    .AddJsonFile("appsettings.json")
                                    .Build().GetConnectionString("DatabaseConnection");

        connection = new OracleConnection(connectionString);
    }



    public IList<RepresentanteModel> Listar()
    {
        var lista = new List<RepresentanteModel>();

        using (connection)
        {
            var query = "SELECT REPRESENTANTEID, NOMEREPRESENTANTE, CPF FROM REPRESENTANTE";

            connection.Open();

            OracleCommand command = new OracleCommand(query, connection);
            OracleDataReader dataReader = command.ExecuteReader();

            while (dataReader.Read())
            {
                // Recupera os dados
                RepresentanteModel representante = new RepresentanteModel();
                representante.RepresentanteId = Convert.ToInt32(dataReader["REPRESENTANTEID"]);
                representante.NomeRepresentante = dataReader["NOMEREPRESENTANTE"].ToString();
                representante.Cpf = dataReader["CPF"].ToString();

                // Adiciona o modelo da lista
                lista.Add(representante);
            }

            connection.Close();

        } // Finaliza o objeto connection

        // Retorna a lista
        return lista;

    }

    public RepresentanteModel Consultar(int id)
    {
        var representante = new RepresentanteModel();

        using (connection)
        {
            var query = "SELECT REPRESENTANTEID, NOMEREPRESENTANTE, CPF FROM REPRESENTANTE WHERE REPRESENTANTEID = :ID ";

            connection.Open();

            OracleCommand command = new OracleCommand(query, connection);
            command.Parameters.Add("ID", id);

            OracleDataReader dataReader = command.ExecuteReader();

            if (dataReader.Read())
            {
                // Recupera os dados
                representante.RepresentanteId = Convert.ToInt32(dataReader["REPRESENTANTEID"]);
                representante.NomeRepresentante = dataReader["NOMEREPRESENTANTE"].ToString();
                representante.Cpf = dataReader["CPF"].ToString();
            }

            connection.Close();

        } // Finaliza o objeto connection

        // Retorna o objeto representante

        return representante;
    }

    public void Inserir(RepresentanteModel representante)
    {
        using (connection)
        {
            String query = "INSERT INTO REPRESENTANTE (REPRESENTANTEID, NOMEREPRESENTANTE, CPF ) VALUES (:id, :nome, :cpf ) ";

            connection.Open();

            OracleCommand command = new OracleCommand(query, connection);

            // Adicionando o valor ao comando
            command.Parameters.Add("id", representante.RepresentanteId);
            command.Parameters.Add("nome", representante.NomeRepresentante);
            command.Parameters.Add("cpf", representante.Cpf);

            command.ExecuteNonQuery();
            connection.Close();

        }

    }

    public void Alterar(RepresentanteModel representante)
    {
        using (connection)
        {
            String query = "UPDATE REPRESENTANTE SET NOMEREPRESENTANTE = :nome, CPF = :cpf  WHERE REPRESENTANTEID = :id ";

            connection.Open();

            OracleCommand command = new OracleCommand(query, connection);

            // Adicionando o valor ao comando
            command.Parameters.Add("nome", representante.NomeRepresentante);
            command.Parameters.Add("cpf", representante.Cpf);
            command.Parameters.Add("id", representante.RepresentanteId);


            command.ExecuteNonQuery();
            connection.Close();

        }
    }

    public void Excluir(int id)
    {
        using (connection)
        {
            String query = "DELETE REPRESENTANTE WHERE REPRESENTANTEID = :id ";

            connection.Open();

            OracleCommand command = new OracleCommand(query, connection);

            // Adicionando o valor ao comando
            command.Parameters.Add("id", id);


            command.ExecuteNonQuery();
            connection.Close();

        }
    }


}
