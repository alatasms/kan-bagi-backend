namespace UserService.Domain.BussinesModels
{
    /// <summary>Red blood cell compatibility: which recipients a donor's blood can be given to.</summary>
    public static class BloodCompatibility
    {
        private static readonly Dictionary<BloodType, BloodType[]> CanDonateTo = new()
        {
            [BloodType.O_Negative] = Enum.GetValues<BloodType>(),
            [BloodType.O_Positive] = [BloodType.O_Positive, BloodType.A_Positive, BloodType.B_Positive, BloodType.AB_Positive],
            [BloodType.A_Negative] = [BloodType.A_Negative, BloodType.A_Positive, BloodType.AB_Negative, BloodType.AB_Positive],
            [BloodType.A_Positive] = [BloodType.A_Positive, BloodType.AB_Positive],
            [BloodType.B_Negative] = [BloodType.B_Negative, BloodType.B_Positive, BloodType.AB_Negative, BloodType.AB_Positive],
            [BloodType.B_Positive] = [BloodType.B_Positive, BloodType.AB_Positive],
            [BloodType.AB_Negative] = [BloodType.AB_Negative, BloodType.AB_Positive],
            [BloodType.AB_Positive] = [BloodType.AB_Positive],
        };

        public static IReadOnlyList<BloodType> RecipientsOf(BloodType donor) => CanDonateTo[donor];
    }
}
