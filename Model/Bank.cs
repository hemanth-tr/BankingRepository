using System;
using System.ComponentModel.DataAnnotations;

namespace BankingRepository.Model
{
	/// <summary>
	/// Model to hold bank detail.
	/// </summary>
	[Serializable]
	public class Bank
	{
		/// <summary>
		/// Gets or sets the bank name.
		/// </summary>
		[Required]
		[MinLength(5, ErrorMessage = "Should be minimum of 5 characters")]
		[MaxLength(100, ErrorMessage = "Should be maximum of 100 characters")]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the bank acronym.
		/// </summary>
		[Required]
		[MinLength(3, ErrorMessage = "Should be minimum of 3 characters")]
		[MaxLength(10, ErrorMessage = "Should be maximum of 10 characters")]
		public string Acronym { get; set; }
	}
}
