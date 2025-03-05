using System;
using MySql.Data.MySqlClient;

public class Class1
{

	private MySqlConnection conexion;
	private string cadenaConexion = "Server=TuServidor;Database=TuBaseDeDatos;Uid=TuUsuario;Pwd=TuContraseña;";

	public Class1()
	{

		conexion = new MySqlConnection(cadenaConexion);
	}

	public MySqlConnection ObtenerConexion()
	{
		return conexion;
	}
}
