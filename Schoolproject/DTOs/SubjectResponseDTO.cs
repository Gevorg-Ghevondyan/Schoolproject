﻿namespace Schoolproject.DTOs
{
    public class SubjectResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<int> TeacherIds { get; set; }
    }
}