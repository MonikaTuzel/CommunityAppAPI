﻿using System;

namespace Messages.DTO
{
    public class MessagesInfoDto
    {
        public int Id { get; set; }
        public string Topic { get; set; }
        public DateTime Date { get; set; }
        public string StatusName { get; set; }

    }
}