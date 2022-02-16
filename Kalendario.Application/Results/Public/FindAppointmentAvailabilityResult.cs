using System;
using System.Collections.Generic;

namespace Kalendario.Application.Results.Public;

public class FindAppointmentAvailabilityResult
{
    public List<Slot> Slots { get; set; } = new();
}

public class Slot
{
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}