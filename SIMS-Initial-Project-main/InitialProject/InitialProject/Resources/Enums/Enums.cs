namespace InitialProject.Resources.Enums
{
    public enum UserType
    { 
        example,
        owner,
        guest1,
        guest2,
        guide,
        superowner,
        superguest
    }

    public enum AccommodationType
    {
        Apartment,
        House,
        Hut
    }

    public enum RescheduleRequestStatus
    {
        onhold,
        approved,
        rejected
    }
    public enum RequestStatus 
    {
        pending,
        invalid,
        accepted
    }

    public enum ComplexTourStatus
    {
        pending,
        invalid,
        accepted
    }
}
