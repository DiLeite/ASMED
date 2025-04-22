﻿namespace ASMED.Domain.Entities;

public class DoctorPatient
{
    public Guid DoctorId { get; set; }
    public Doctor Doctor { get; set; }

    public Guid PatientId { get; set; }
    public Patient Patient { get; set; }
}