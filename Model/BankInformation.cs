using System;
using System.Collections.Generic;

namespace BankingRepository.Model
{
    /// <summary>
    /// Model to hold bank detail.
    /// </summary>
    public class BankInformation
	{
		/// <summary>
		/// Gets or sets the unique identifier for the bank.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Gets or sets the bank name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the status of bank.
		/// </summary>
		public Status? Status { get; set; }

		/// <summary>
		/// Gets or sets the bank acronym.
		/// </summary>
		public string Acronym {  get; set; }

		/// <summary>
		/// Gets the list of branches for the bank.
		/// </summary>
		public IEnumerable<Branch> Branches { get; set; }
	}
}
