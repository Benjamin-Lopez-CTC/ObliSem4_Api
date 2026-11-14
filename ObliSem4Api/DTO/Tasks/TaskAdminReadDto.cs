using System;

namespace ObliSem4Api.DTO.Tasks;

public class TaskAdminReadDto : TaskReadDto
{
        public string CreatorEmail { get; set; } = string.Empty;
}
