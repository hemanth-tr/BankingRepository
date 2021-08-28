using BankingRepository.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingRepository.Implementation
{
	/// <summary>
	/// Repository for handling bank's CRUD operations.
	/// </summary>
	public class BankingRepository : IBankRepository
	{
		/// <summary>
		/// Instantiates <see cref="BankRepository"/>
		/// </summary>
		/// <param name="configuration">instance of configuration manager.</param>
		/// <param name="logger">logger instance.</param>
		public BankingRepository(IConfiguration configuration, ILogger<BankingRepository> logger)
		{
			if (configuration == null) throw new ArgumentNullException(nameof(configuration));

			Logger = logger ?? throw new ArgumentNullException(nameof(logger));

			string connectionString = configuration.GetConnectionString("BankDB");

			SqlConnection = new SqlConnection(connectionString);
			Logger = logger;
		}

		/// <summary>
		/// Gets or sets the instance of <see cref="ILogger"/>.
		/// </summary>
		private ILogger<BankingRepository> Logger { get; init; }

		/// <summary>
		/// Gets or sets the instance of <see cref="SqlConnection"/> used to communicate with Sql server.
		/// </summary>
		private SqlConnection SqlConnection { get; set; }

		/// <summary>
		/// Retrieves the list of banks.
		/// </summary>
		/// <returns>lis to bank.</returns>
		public async Task<IList<BankInformation>> FetchBanksAsync()
		{
			var command = new SqlCommand("SELECT * FROM Banks", SqlConnection);

			try
			{
				SqlConnection.Open();
				var result = await command.ExecuteReaderAsync().ConfigureAwait(false);

				var mappedResult = new List<BankInformation>();
				while (await result.ReadAsync())
				{
					var id = result["Id"].ToString();
					var bankName = result["Name"].ToString();
					var acronym = result["Acronym"].ToString();
					var status = result["Status"].ToString();
					var isParsingSuccessfull = Enum.TryParse(status, out Status bankStatus);

					var bank = new BankInformation
					{
						Id = Guid.Parse(id),
						Name = bankName,
						Acronym = acronym,
						Status = isParsingSuccessfull ? bankStatus : null
					};

					mappedResult.Add(bank);
				}

				return mappedResult;
			}
			catch (SqlException ex)
			{
				ex.ThrowException();
			}
			finally
			{
				SqlConnection.Close();
			}

			return null;
		}

		/// <summary>
		/// Returns the bank information if delegate succeeds.
		/// </summary>
		/// <param name="id">unique identifier of bank.</param>
		/// <returns>bank information.</returns>
		public async Task<BankInformation> FetchBankAsync(Guid id)
		{
			var command = new SqlCommand("GetBank", SqlConnection)
			{
				CommandType = System.Data.CommandType.StoredProcedure,
			};

			command.Parameters.AddWithValue("id", id);

			try
			{
				SqlConnection.Open();
				var result = await command.ExecuteReaderAsync().ConfigureAwait(false);
				if (!result.HasRows)
				{
					return null;
				}

				_ = result.ReadAsync();

				var bankId = result["Id"].ToString();
				var bankName = result["Name"].ToString();
				var acronym = result["Acronym"].ToString();
				var status = result["Status"].ToString();
				var isParsingSuccessfull = Enum.TryParse(status, out Status bankStatus);

				var bank = new BankInformation
				{
					Id = Guid.Parse(bankId),
					Name = bankName,
					Acronym = acronym,
					Status = isParsingSuccessfull ? bankStatus : null
				};

				return bank;
			}
			catch (SqlException ex)
			{
				ex.ThrowException();
			}
			finally
			{
				SqlConnection.Close();
			}

			return null;
		}

		/// <summary>
		/// Creates a new bank.
		/// </summary>
		/// <param name="bank">Bank details.</param>
		/// <returns>Unique identifier.</returns>
		/// <exception cref="DuplicateResourceException">
		/// Thrown when resource already exists.
		/// </exception>
		/// <exception cref="ResourceCreationFailedException">
		/// Thrown when unique identifier of bank is default value,
		/// OR Thrown when resource creation failed for some reason.
		/// </exception>
		public async Task<Guid> CreateBankAsync(Bank bank)
		{
			var command = new SqlCommand("CreateBank", SqlConnection)
			{
				CommandType = System.Data.CommandType.StoredProcedure
			};

			command.Parameters.AddWithValue("@acronym", bank.Acronym);
			command.Parameters.AddWithValue("@name", bank.Name);

			Guid id = Guid.Empty;
			try
			{
				SqlConnection.Open();
				var result = await command.ExecuteReaderAsync().ConfigureAwait(false);
				result.Read();
				_ = Guid.TryParse(result["Id"].ToString(), out id);
			}
			catch (SqlException ex)
			{
				Logger.LogError(@$"SqlException: Number: {ex.Number}, Message: {ex.Message}");
				ex.ThrowException();
			}
			finally
			{
				SqlConnection.Close();
			}

			if (id == Guid.Empty)
			{
				throw new ResourceCreationFailedException($"Failed to create {bank.Acronym}");
			}

			return id;
		}

		/// <summary>
		/// Updates bank's status.
		/// </summary>
		/// <param name="id">bank id.</param>
		/// <param name="status">status to be updated.</param>
		public async Task ChangeBankStatusAsync(Guid id, Status status)
		{
			var sqlCommand = new SqlCommand("ChangeBankStatus", SqlConnection)
			{
				CommandType = System.Data.CommandType.StoredProcedure
			};

			sqlCommand.Parameters.AddWithValue("@id", id);
			sqlCommand.Parameters.AddWithValue("@status", status);

			try
			{
				SqlConnection.Open();
				await sqlCommand.ExecuteNonQueryAsync().ConfigureAwait(false);
			}
			catch (SqlException ex)
			{
				throw new ResourceUpdateFailedException($"StatusUpdateFailed. Id:{id}, Status:{status}, Message:{ex.Message}");
			}
			finally
			{
				SqlConnection.Close();
			}
		}
	}
}
