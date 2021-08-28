using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingRepository.Model
{
	/// <summary>
	/// Model to hold branch details.
	/// </summary>
	public class Branch
	{
		/// <summary>
		/// Gets or sets the unique identifier of a bank's branch.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Gets or sets the bank name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the bank code.
		/// </summary>
		public string Code { get; set; }

		/// <summary>
		/// Gets or sets the branch address.
		/// </summary>
		public Address Address { get; set; }
	}
}
