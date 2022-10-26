using Infra.MySql;
using Infra.MySql.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace chat_message_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly DataContext dataContext;

        public TestController(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        [HttpPost]
        public IActionResult Post()
        {
            dataContext.Message.Add(new MessageModel
            {
                MyNick = "theufonseca",
                FriendNick = "guilopes",
                Sent = true,
                Date = DateTime.Now,
                Message = "Salve Gui"
            });

            dataContext.SaveChanges();

            return Ok();
        }

        [HttpGet]
        public IActionResult Get()
        {
            var messages = dataContext.Message.ToList();
            return Ok(messages);
        }
    }
}
