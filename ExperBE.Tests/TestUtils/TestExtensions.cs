using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExperBE.Tests.TestUtils
{
    public static class TestExtensions
    {
        public static bool IsSuccessStatusCode(this IStatusCodeActionResult objectResult)
        {
            return objectResult?.StatusCode != null 
                   && objectResult.StatusCode >= 200 
                   && objectResult.StatusCode <= 299;
        }
    }
}
