using System;

namespace Kalendario.Application.IntegrationTests.Common;

public static class Constants
{
    public static Guid CurrentUserId = new(CurrentUserIdString);

    public static Guid CurrentUserAccountId = new(CurrentUserAccountIdString);

    public static Guid RandomAccountId = new(RandomAccountIdString);
    
    public const string CurrentUserIdString = "BD45F92D-105E-4530-A265-907FA845A648";
    
    public const string CurrentUserAccountIdString = "9DAFD9B4-79B3-46BC-8C82-DB34C0477563";

    public const string RandomAccountIdString = "FD23644A-9BAE-47AA-9871-6AC59A63D4A1";
}