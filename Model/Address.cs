using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingRepository.Model
{
	/// <summary>
	/// Model to hold address.
	/// </summary>
	public class Address
	{
		/// <summary>
		/// Gets or sets the detailed address.
		/// </summary>
		public string DetailedAddress {  get; set; }

		/// <summary>
		/// Gets or sets the city of a state.
		/// </summary>
		public string City { get; set; }

		/// <summary>
		/// Gets or sets the state of country.
		/// </summary>
		public string State { get; set; }

		/// <summary>
		/// Gets or sets the country.
		/// </summary>
		public string Country { get; set; }

		/// <summary>
		/// Gets or sets the pincode of the provided address.
		/// </summary>
		public string PinCode	{ get; set; }
	}
}
