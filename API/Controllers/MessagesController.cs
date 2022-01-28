using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Extensions;
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
    }
}