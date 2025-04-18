﻿using CrudPeople.CoreDomain.Enums;

namespace CrudPeople.ApplicationService
{
    public class UpdatePersonRequestDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string NationalCode { get; set; }
        public PersonType? PersonType { get; set; }
        public byte[] RowVersion { get; set; }

    }
}
