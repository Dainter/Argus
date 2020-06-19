namespace WebApi.Controllers
{
    /// <summary/>
    public class MatchRequest
    {
        /// <summary/>
        public bool match { get; set; }

        /// <summary/>
        public string supportICSVersion { get; set; }

        /// <summary/>
        public int code { get; set; }

        /// <summary/>
        public string message { get; set; }
    }
}