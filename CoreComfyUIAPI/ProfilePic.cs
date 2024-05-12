using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreComfyUIAPI
{
	public class ProfilePic
	{
		public string SessionID{ get; set; }
		public bool IsFemale{ get; set; }
		public bool IsPrimaryProfile { get; set; }

	}
}
