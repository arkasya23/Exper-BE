using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExperBE.Models.Configuration
{
    public class ExperConfiguration
    {
        public static string ConnectionString { get; set; } = "";
        public static string JwtSecret { get; set; } = "";
        public static string AllowedOrigins { get; set; } = "";
        public static bool SendEmails { get; set; } = false;
        public static string EmailHost { get; set; } = "";
        public static int EmailPort { get; set; } = 587;
        public static string EmailUserName { get; set; } = "";
        public static string EmailPassword { get; set; } = "";
    }
}
