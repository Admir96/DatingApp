using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class MessagesController : BaseApiController
    {
       
       
        private readonly IUserRepository _userRepository;
        
        private readonly IMessageRepository _massageRepository;
        private readonly IMapper _mapper;
        public MessagesController(IUserRepository userRepository, 
        IMessageRepository massageRepository,  IMapper mapper)
        {          
            _mapper = mapper;
            _massageRepository = massageRepository;
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<ActionResult<MessageDTO>> CreateMessage(CreateMessageDTO createMessageDTO)
        {
           
            var username = User.GetUsername();

            if(username == createMessageDTO.RecipientUsername.ToLower()) 
            return BadRequest("You cannot send messages to yourself");

            var sender = await _userRepository.GetUserByUsernameAsync(username);
            var recipient = await _userRepository.GetUserByUsernameAsync(createMessageDTO.RecipientUsername);

            if(recipient == null) return NotFound();

            var message = new Message
            {
                Sender = sender,
                Recipient = recipient,
                SenderUsername = sender.UserName,
                RecipientUsername = recipient.UserName,
                Content = createMessageDTO.Content
            };
            _massageRepository.AddMessage(message);
            
            if(await _massageRepository.SaveAllAsync())
            return Ok(_mapper.Map<MessageDTO>(message));

            return BadRequest("Failed to send message");

        }
          [HttpGet]
            public async Task<ActionResult<IEnumerable<MessageDTO>>> GetMessagesForUser
            ([FromQuery] MessageParams messageParams)
            {
              messageParams.Username = User.GetUsername();
              var message = await _massageRepository.GetMessagesForUser(messageParams);
              Response.AddPaginationHeader(message.CurrentPage, message.PageSize,
              message.TotalCount, message.TotalPage);

               return message;
            }

            [HttpGet("thread/{username}")]
            public async Task<ActionResult<IEnumerable<MessageDTO>>> GetMessageThread(string username)
            {
                var currentUsername = User.GetUsername();
                return Ok(await _massageRepository.GetMessagesThread(currentUsername, username));
            }


            [HttpDelete("{id}")]
            public async Task<ActionResult> DeleteMessage(int id)
            {
                var username = User.GetUsername();

                var message = await _massageRepository.GetMessage(id);

                if(message.Sender.UserName != username && message.Recipient.UserName != username)
                return Unauthorized();

                if(message.Sender.UserName == username) message.SenderDeleted = true;

                if(message.Recipient.UserName == username) message.RecipientDeleted = true;

                if(message.SenderDeleted && message.RecipientDeleted)
                _massageRepository.DeleteMessage(message);

                if(await _massageRepository.SaveAllAsync()) return Ok();

                return BadRequest("Problem deleting the message");

            }

    }
}