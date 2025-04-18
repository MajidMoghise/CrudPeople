﻿using CrudPeople.CoreDomain.Enums;

namespace CrudPeople.ApplicationService
{
    public class CreatePersonRequestDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public PersonType PersonType { get; set; }
        public string NationalCode { get; set; }

    }
}
