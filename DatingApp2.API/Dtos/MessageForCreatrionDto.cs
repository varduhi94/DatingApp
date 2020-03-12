using System;

namespace DatingApp2.API.Dtos
{
    public class MessageForCreatrionDto
    {
        public int SenderId { get; set; }
        public int RecipientId { get; set; }
        public DateTime MessageSent { get; set; }
        public string Content { get; set; }
        public MessageForCreatrionDto()
        {
            MessageSent = DateTime.Now;
        }
    }
}