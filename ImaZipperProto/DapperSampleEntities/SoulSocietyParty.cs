using System;
using System.Collections.Generic;
using System.Text;

namespace DapperSample
{
	public class SoulSocietyParty
	{
		public long PartyId { get; set; } = 0;

		public string PartyName { get; set; } = string.Empty;

		public List<BleachCharacter> PartyMembers { get; } = new List<BleachCharacter>();
	}
}
