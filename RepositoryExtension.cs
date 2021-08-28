using BankingLibrary.Exceptions;
using System;
using System.Data.SqlClient;

namespace BankingRepository
{
	/// <summary>
	/// Extension for Sql repository.
	/// </summary>
	public static class RepositoryExtension
	{
		/// <summary>
		/// Returns error message for the provided exception.
		/// </summary>
		/// <param name="ex">exception detail.</param>
		/// <exception cref="Exception">
		/// Throws relevant exception based on sql exception number.
		/// </exception>
		public static void ThrowException(this SqlException ex)
		{
			string errorMessage;
			switch (ex.Number)
			{
				case 2627:
					errorMessage = "Resource already exist";
					throw new DuplicateResourceException(errorMessage);

				default:
					errorMessage = "An error occured";
					throw new Exception(errorMessage);
			}
		}
	}
}
