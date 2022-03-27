﻿using System;
using BillApi.Helper;
using BillApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BillApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly JwtHelpers _jwt;
        private readonly BasicAuth _basicAuth;
        private readonly ILogger<LoginController> _logger;

        public LoginController(JwtHelpers jwt, BasicAuth basicAuth, ILogger<LoginController> logger)
        {
            _jwt = jwt;
            _basicAuth = basicAuth;
            _logger = logger;
        }

        [HttpPost()]
        public ActionResult<LoginToken> SignIn(int expireMinutes = 24 * 60)
        {
            try
            {
                var userData = _basicAuth.Parse(Request);  //this.Request.Headers["Authorization"];

                if (string.IsNullOrEmpty(userData.userId) && string.IsNullOrEmpty(userData.password))
                {
                    return BadRequest(new MessageTemplate() { BadRequestError = "Id or pwd not exist." });
                }

                if (!CheckDBResult(userData))
                {
                    return BadRequest(new MessageTemplate() { BadRequestError = "Id or pwd error." });
                }

                return Ok(new LoginToken() { Token = _jwt.GenerateToken(userData.userId, expireMinutes) });
            }
            catch (Exception e)
            {
                return BadRequest(new MessageTemplate { BadRequestError = e.Message });
            }
        }

        private bool CheckDBResult((string userId, string password) user)
        {
            return true;
        }
    }
}
