using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace SQLServerConnection
{
    public class SqlConnector
    {
        private String _connectionString;
        private SqlConnection _sqlConn;
        public SqlConnector(String connectionStr)
        {
            _connectionString = connectionStr;
            _sqlConn = new SqlConnection(connectionStr);
        }
        public void Connect()
        {
            try
            {
                _sqlConn.Open();
            }
            catch (SqlException e)
            {
                throw new Exception(e.Message);
            }
        }

        public void Disconnect()
        {
            try
            {
                _sqlConn.Close();
            }
            catch (SqlException e)
            {
                throw new Exception(e.Message);
            }
        }

        public DataSet Query(String querySqlStr, string tableName)
        {
            SqlDataAdapter sqlDatAdptr = new SqlDataAdapter(querySqlStr, _sqlConn);

            DataSet result = new DataSet();

            try
            {
                sqlDatAdptr.Fill(result, tableName);
                return result;
            }
            catch (SqlException e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                sqlDatAdptr.Dispose();
            }
        }

        public int QueryWithoutResult(String querySqlStr, String tableName)
        {
            SqlCommand sqlCmd = new SqlCommand(querySqlStr, _sqlConn);
            try
            {
                int rows = sqlCmd.ExecuteNonQuery();
                return rows;
            }
            catch (SqlException e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                sqlCmd.Dispose();
            }
        }
    }

    
}

