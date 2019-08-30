using System.Text;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MessageApplication.Models;

namespace MessageApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly MessagesContext _context;

        public MessagesController(MessagesContext context)
        {
            _context = context;
        }
        // GET api/messages/abc
        [HttpGet("{value}")]
        public async Task<ActionResult<string>> Get(string value)
        {
            var message = _context.MessagesItems.Where(x => x.Message.Split(new char[] { ':' })[1].Equals(value));

            if (message.Count() == 0)
            {
                return NotFound();
            }

            return message.First().Message.Split(new char[] { ':' })[0];
        }

        // POST api/messages
        [HttpPost]
        public async Task<ActionResult<MessagesItem>> Post(MessagesItem item)
        {
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(item.Message));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            MessagesItem message = new MessagesItem();
            message.Message = item.Message + ":" + hash.ToString();
            _context.MessagesItems.Add(message);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { value = message.Message }, message.Message.Split(new char[] { ':' })[1]);
        }
    }
}
